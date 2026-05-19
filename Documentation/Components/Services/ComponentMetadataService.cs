using System.Collections.Frozen;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Services;

/// <summary>
/// Reads component parameter metadata via reflection, with optional XML-doc enrichment.
/// Handles generic types like <c>EnumValue&lt;T&gt;</c> and <c>CssLength</c>.
/// </summary>
public sealed partial class ComponentMetadataService : IComponentMetadataService
{
    private readonly Dictionary<Type, IReadOnlyList<ParameterInfo>> cache = new();
    private readonly Dictionary<Type, IReadOnlyList<ComponentMethodInfo>> methodCache = new();
    private readonly Dictionary<Type, string?> classSummaryCache = new();
    private readonly Dictionary<Type, ClassDocumentation?> classDocCache = new();
    private readonly Dictionary<string, XDocument> xmlDocs = new();
    private readonly Dictionary<string, Dictionary<string, XElement>> xmlMemberIndex = new();
    private readonly ILogger<ComponentMetadataService> logger;
    private bool xmlDocsLoaded;

    /// <summary>Initializes the service. XML documentation loading is deferred until first access.</summary>
    public ComponentMetadataService(ILogger<ComponentMetadataService> logger)
    {
        this.logger = logger;
    }

    /// <summary>Ensures XML documentation is loaded on first use.</summary>
    private void EnsureXmlDocsLoaded()
    {
        if (xmlDocsLoaded)
        {
            return;
        }

        xmlDocsLoaded = true;
        LoadXmlResources();
        LoadHviktorAssemblies();
    }

    private void LoadXmlResources()
    {
        var hostAssembly = typeof(ComponentMetadataService).Assembly;
        var hostResources = hostAssembly.GetManifestResourceNames();

        // Resource names follow the pattern "Documentation.HviktorXmlDocs.{AssemblyName}.xml".
        // Extract the assembly name and load each XML doc directly.
        const string prefix = "Documentation.HviktorXmlDocs.";
        const string suffix = ".xml";

        var xmlResources = hostResources
            .Where(r => r.StartsWith(prefix, StringComparison.Ordinal)
                        && r.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            .ToArray();

        LogXmlDocsDiscovery(0, xmlResources.Length, string.Join(", ", xmlResources));

        foreach (var resourceName in xmlResources)
        {
            // Extract assembly name: "Documentation.HviktorXmlDocs.Hviktor.Abstractions.xml" -> "Hviktor.Abstractions"
            var assemblyName = resourceName[prefix.Length..^suffix.Length];

            using var stream = hostAssembly.GetManifestResourceStream(resourceName);
            if (stream is null)
            {
                LogXmlDocsNotFound(assemblyName);
                continue;
            }

            try
            {
                xmlDocs[assemblyName] = XDocument.Load(stream);
                IndexXmlDoc(assemblyName, xmlDocs[assemblyName]);
            }
            catch (Exception)
            {
                LogCouldNotLoadXmlDocsFromPath(resourceName);
            }
        }
    }

    /// <summary>Builds a member-name-to-element index for fast O(1) lookups.</summary>
    private void IndexXmlDoc(string assemblyName, XDocument doc)
    {
        var index = new Dictionary<string, XElement>(StringComparer.Ordinal);
        foreach (var member in doc.Descendants("member"))
        {
            var name = member.Attribute("name")?.Value;
            if (name is not null)
            {
                index[name] = member;
            }
        }

        xmlMemberIndex[assemblyName] = index;
    }

    private void LoadHviktorAssemblies()
    {
        var hviktorAssemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => a.GetName().Name?.StartsWith("Hviktor", StringComparison.OrdinalIgnoreCase) == true);

        var canLoadFromDisk = !OperatingSystem.IsBrowser();
        foreach (var assembly in hviktorAssemblies)
        {
            var name = assembly.GetName().Name;
            if (name is null || xmlDocs.ContainsKey(name))
            {
                continue;
            }

            // Try self-embedded XML docs first (Release/NuGet builds)
            var embeddedXmlName = $"{name}.{name}.xml";
            using var embeddedStream = assembly.GetManifestResourceStream(embeddedXmlName);
            if (embeddedStream is not null)
            {
                try
                {
                    xmlDocs[name] = XDocument.Load(embeddedStream);
                    IndexXmlDoc(name, xmlDocs[name]);
                    continue;
                }
                catch (Exception)
                {
                    LogCouldNotLoadXmlDocsFromPath(embeddedXmlName);
                }
            }

            // Fall back to file-based loading (server scenario)
            if (!canLoadFromDisk)
            {
                continue;
            }

            var location = assembly.Location;
            if (string.IsNullOrEmpty(location))
            {
                continue;
            }

            var xmlPath = Path.ChangeExtension(location, ".xml");
            if (!File.Exists(xmlPath))
            {
                continue;
            }

            try
            {
                var doc = XDocument.Load(xmlPath);
                xmlDocs[name] = doc;
                IndexXmlDoc(name, doc);
            }
            catch (Exception)
            {
                LogCouldNotLoadXmlDocsFromPath(xmlPath);
            }
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<ParameterInfo> GetParameters([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type componentType)
    {
        if (cache.TryGetValue(componentType, out var cached))
        {
            return cached;
        }

        EnsureXmlDocsLoaded();

        var result = new List<ParameterInfo>();

        var props = componentType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<ParameterAttribute>() is not null)
            .OrderBy(p => p.Name);

        foreach (var prop in props)
        {
            var paramAttr = prop.GetCustomAttribute<ParameterAttribute>()!;

            // Skip CaptureUnmatchedValues - not a typed parameter consumers set directly
            if (paramAttr.CaptureUnmatchedValues)
            {
                continue;
            }

            var typeName = BuildTypeName(prop.PropertyType);
            var typeHtml = BuildTypeHtml(prop.PropertyType);
            var defaultValue = GetDefaultValue(prop);
            var allowedValues = GetAllowedValues(prop);
            var isRequired = prop.GetCustomAttribute<EditorRequiredAttribute>() is not null;
            (var summary, var summaryHtml) = GetXmlDocSummaryPair(componentType, prop);

            result.Add(new ParameterInfo(prop.Name, typeName, defaultValue, allowedValues, isRequired, summary, summaryHtml, typeHtml));
        }

        var list = result.AsReadOnly();
        cache[componentType] = list;
        return list;
    }

    private static string BuildTypeName(Type type)
    {
        if (type.IsGenericType)
        {
            var generic = type.GetGenericTypeDefinition();
            var args = type.GetGenericArguments();

            // Nullable<T> → "T?"
            if (generic == typeof(Nullable<>))
            {
                return BuildTypeName(args[0]) + "?";
            }

            // EnumValue<T> → "EnumValue<T>"
            var baseName = generic.Name[..generic.Name.IndexOf('`')];
            var argNames = string.Join(", ", args.Select(BuildTypeName));
            return $"{baseName}<{argNames}>";
        }

        return type switch
        {
            { } t when t == typeof(string) => "string",
            { } t when t == typeof(bool) => "bool",
            { } t when t == typeof(int) => "int",
            { } t when t == typeof(long) => "long",
            { } t when t == typeof(double) => "double",
            { } t when t == typeof(float) => "float",
            { } t when t == typeof(decimal) => "decimal",
            { } t when t == typeof(object) => "object",
            { } t when t == typeof(RenderFragment) => "RenderFragment",
            { } t when t == typeof(ElementReference) => "ElementReference",
            { } t when t.IsEnum => t.Name,
            _ => type.Name
        };
    }

    /// <summary>
    /// Builds an HTML representation of a CLR type with popover buttons for known types,
    /// matching the rendering used by the structured XML doc tables.
    /// </summary>
    private string BuildTypeHtml(Type type)
    {
        if (type.IsGenericType)
        {
            var generic = type.GetGenericTypeDefinition();
            var args = type.GetGenericArguments();

            // Nullable<T> → "T?"
            if (generic == typeof(Nullable<>))
            {
                return BuildTypeHtml(args[0]) + "?";
            }

            var baseName = generic.Name[..generic.Name.IndexOf('`')];
            var argHtmls = string.Join(", ", args.Select(BuildTypeHtml));
            return $"{WebUtility.HtmlEncode(baseName)}&lt;{argHtmls}&gt;";
        }

        // Try to resolve a cref and render with popover
        var crefId = GetCrefForType(type);
        if (crefId is not null)
        {
            var displayName = BuildTypeName(type);
            return RenderTypePopover(crefId, displayName);
        }

        return $"<code>{WebUtility.HtmlEncode(BuildTypeName(type))}</code>";
    }

    /// <summary>Returns the XML doc cref member ID for a CLR type, or <see langword="null"/> if not resolvable.</summary>
    private static string? GetCrefForType(Type type) => type.FullName is null ? null : $"T:{type.FullName}";

    /// <summary>
    /// Renders a type reference as an HTML popover button with documentation,
    /// using the same pattern as <see cref="RenderSeeAsHtml"/>.
    /// </summary>
    private string RenderTypePopover(string cref, string displayName)
    {
        var summaryHtml = GetCrefSummaryHtml(cref);
        return BuildPopoverHtml(WebUtility.HtmlEncode(displayName), summaryHtml);
    }

    /// <summary>
    /// Builds an inline popover button+panel HTML fragment for a type or member reference.
    /// </summary>
    private static string BuildPopoverHtml(string encodedName, string? summaryHtml)
    {
        var id = "see-ref-" + Cryptography.GenerateId();
        var sb = new System.Text.StringBuilder();
        sb.Append("<button type=\"button\" data-popover=\"inline\" popovertarget=\"");
        sb.Append(id);
        sb.Append("\">");
        sb.Append(encodedName);
        sb.Append("</button>");
        sb.Append("<div id=\"");
        sb.Append(id);
        sb.Append("\" class=\"ds-popover\" popover=\"manual\" data-placement=\"top\" data-variant=\"primary\">");
        sb.Append("<p class=\"ds-paragraph\"><strong class=\"block\">");
        sb.Append(encodedName);
        sb.Append("</strong>");
        sb.Append(summaryHtml ?? "<em>No documentation available.</em>");
        sb.Append("</p></div>");
        return sb.ToString();
    }

    private string? GetDefaultValue(PropertyInfo prop)
    {
        // Check [DefaultValue] attribute first
        var dvAttr = prop.GetCustomAttribute<DefaultValueAttribute>();
        if (dvAttr?.Value is not null)
        {
            return FormatValue(dvAttr.Value);
        }

        // Infer from property type
        var t = prop.PropertyType;
        if (t == typeof(bool))
        {
            return RenderTypePopover("T:System.Boolean", "false");
        }

        return RenderTypePopover("T:System.Nullable", "null");
    }

    private static string FormatValue(object value)
    {
        return value switch
        {
            bool b => b ? "true" : "false",
            string s => $"\"{s}\"",
            Enum e => $"{e.GetType().Name}.{e}",
            _ => value.ToString() ?? string.Empty
        };
    }

    private static ReadOnlyCollection<string> GetAllowedValues(PropertyInfo prop)
    {
        var attr = prop.GetCustomAttribute<AllowedValuesAttribute>();
        if (attr is null)
        {
            return [];
        }

        return Array.AsReadOnly(attr.Values
            .OfType<object>()
            .Select(v => v is Enum e ? $"{e.GetType().Name}.{e}" : v.ToString() ?? string.Empty)
            .ToArray());
    }

    /// <inheritdoc/>
    public string? GetClassSummary(Type componentType)
    {
        if (classSummaryCache.TryGetValue(componentType, out var cached))
        {
            return cached;
        }

        var result = GetClassSummaryCore(componentType);
        classSummaryCache[componentType] = result;
        return result;
    }

    private string? GetClassSummaryCore(Type componentType)
    {
        EnsureXmlDocsLoaded();
        var member = FindTypeMember(componentType);
        if (member is null)
        {
            return null;
        }

        try
        {
            var summary = member.Element("summary");
            return summary is null ? null : ExtractSummaryContent(summary);
        }
        catch (Exception)
        {
            LogCouldNotReadXmlClassDocForType(componentType.Name);
            return null;
        }
    }

    /// <inheritdoc/>
    public ClassDocumentation? GetClassDocumentation(Type componentType)
    {
        if (classDocCache.TryGetValue(componentType, out var cached))
        {
            return cached;
        }

        var result = GetClassDocumentationCore(componentType);
        classDocCache[componentType] = result;
        return result;
    }

    private ClassDocumentation? GetClassDocumentationCore(Type componentType)
    {
        EnsureXmlDocsLoaded();
        var member = FindTypeMember(componentType);
        if (member is null)
        {
            return null;
        }

        try
        {
            var summary = ExtractOptionalElement(member, "summary");
            var parameters = ExtractOptionalElement(member, "parameters");
            var remarks = ExtractOptionalElement(member, "remarks");
            var use = ExtractOptionalElement(member, "use");
            var avoid = ExtractOptionalElement(member, "avoid");
            var guidelines = ExtractOptionalElement(member, "guidelines");

            if (summary is null && parameters is null && remarks is null && use is null && avoid is null && guidelines is null)
            {
                return null;
            }

            var summaryHtml = ExtractOptionalElementAsHtml(member, "summary");
            var remarksHtml = ExtractOptionalElementAsHtml(member, "remarks");
            var useHtml = ExtractOptionalElementAsHtml(member, "use");
            var avoidHtml = ExtractOptionalElementAsHtml(member, "avoid");
            var guidelinesHtml = ExtractOptionalElementAsHtml(member, "guidelines");

            return new ClassDocumentation(summary, parameters, remarks, use, avoid, guidelines,
                summaryHtml, remarksHtml, useHtml, avoidHtml, guidelinesHtml);
        }
        catch (Exception)
        {
            LogCouldNotReadXmlClassDocForType(componentType.Name);
            return null;
        }
    }

    /// <summary>Finds the XML doc member element for a CLR type, or <see langword="null"/> if not indexed.</summary>
    private XElement? FindTypeMember(Type componentType)
    {
        var assemblyName = componentType.Assembly.GetName().Name;
        if (assemblyName is null || !xmlMemberIndex.TryGetValue(assemblyName, out var index))
        {
            return null;
        }

        var memberId = $"T:{componentType.FullName}";
        return index.GetValueOrDefault(memberId);
    }

    /// <summary>Extracts and converts an optional XML doc element to Markdown, or returns <see langword="null"/>.</summary>
    private string? ExtractOptionalElement(XElement member, string elementName)
    {
        var element = member.Element(elementName);
        return element is null ? null : ExtractSummaryContent(element);
    }

    /// <summary>Extracts and converts an optional XML doc element to HTML with popover support, or returns <see langword="null"/>.</summary>
    private string? ExtractOptionalElementAsHtml(XElement member, string elementName)
    {
        var element = member.Element(elementName);
        return element is null ? null : ExtractSummaryContentAsHtml(element);
    }

    private (string? Markdown, string? Html) GetXmlDocSummaryPair(Type declaringType, PropertyInfo prop)
    {
        var assemblyName = declaringType.Assembly.GetName().Name;
        if (assemblyName is null || !xmlMemberIndex.TryGetValue(assemblyName, out var index))
            return (null, null);

        try
        {
            var memberId = $"P:{declaringType.FullName}.{prop.Name}";
            if (!index.TryGetValue(memberId, out var member))
            {
                return (null, null);
            }

            var summary = member.Element("summary");
            return summary is null
                ? (null, null)
                : (ExtractSummaryContent(summary), ExtractSummaryContentAsHtml(summary));
        }
        catch (Exception)
        {
            LogCouldNotReadXmlDocForTypeProp(declaringType.Name, prop.Name);
            return (null, null);
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<ComponentMethodInfo> GetPublicMethods([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] Type componentType)
    {
        if (methodCache.TryGetValue(componentType, out var cached))
        {
            return cached;
        }

        var methods = componentType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(m => !m.IsSpecialName) // skip property getters/setters, event add/remove
            .Where(m => !ExcludedMethods.Contains(m.Name))
            .Where(m => !m.Name.StartsWith("get_", StringComparison.Ordinal))
            .Where(m => !m.Name.StartsWith("set_", StringComparison.Ordinal))
            .OrderBy(m => m.Name)
            .ToList();

        if (methods.Count == 0)
        {
            methodCache[componentType] = [];
            return [];
        }

        var result = new List<ComponentMethodInfo>();

        foreach (var method in methods)
        {
            var returnType = BuildTypeName(method.ReturnType);
            var returnTypeHtml = BuildTypeHtml(method.ReturnType);
            var parameters = method.GetParameters();
            var paramSignature = parameters.Length == 0
                ? string.Empty
                : string.Join(", ", parameters.Select(p => $"{BuildTypeName(p.ParameterType)} {p.Name}"));
            var paramSignatureHtml = parameters.Length == 0
                ? string.Empty
                : string.Join(", ", parameters.Select(p => $"{BuildTypeHtml(p.ParameterType)} {p.Name}"));
            (var summary, var summaryHtml) = GetXmlDocMethodSummaryPair(componentType, method);

            result.Add(new ComponentMethodInfo(method.Name, returnType, paramSignature, summary, summaryHtml, returnTypeHtml, paramSignatureHtml));
        }

        var list = result.AsReadOnly();
        methodCache[componentType] = list;
        return list;
    }

    private (string? Markdown, string? Html) GetXmlDocMethodSummaryPair(Type declaringType, MethodInfo method)
    {
        var assemblyName = declaringType.Assembly.GetName().Name;
        if (assemblyName is null || !xmlMemberIndex.TryGetValue(assemblyName, out var index))
        {
            return (null, null);
        }

        try
        {
            var parameters = method.GetParameters();
            var memberId = parameters.Length == 0
                ? $"M:{declaringType.FullName}.{method.Name}"
                : $"M:{declaringType.FullName}.{method.Name}({string.Join(",", parameters.Select(p => p.ParameterType.FullName))})";

            if (!index.TryGetValue(memberId, out var member))
            {
                return (null, null);
            }

            var summary = member.Element("summary");
            return summary is null
                ? (null, null)
                : (ExtractSummaryContent(summary), ExtractSummaryContentAsHtml(summary));
        }
        catch (Exception)
        {
            LogCouldNotReadXmlDocForTypeProp(declaringType.Name, method.Name);
            return (null, null);
        }
    }

    /// <summary>
    /// Converts the inner content of an XML doc <c>&lt;summary&gt;</c> element to a Markdown-friendly string.
    /// Preserves <c>&lt;br/&gt;</c> as line breaks and converts <c>&lt;c&gt;</c> to backtick code,
    /// <c>&lt;see&gt;</c> to type name references, and <c>&lt;para&gt;</c> to paragraph breaks.
    /// </summary>
    private string ExtractSummaryContent(XElement summary)
    {
        var sb = new System.Text.StringBuilder();
        foreach (var node in summary.Nodes())
        {
            switch (node)
            {
                case XText text:
                    sb.Append(text.Value);
                    break;
                case XElement el:
                    switch (el.Name.LocalName)
                    {
                        case "br":
                            sb.Append('\n');
                            break;
                        case "c":
                            sb.Append('`');
                            sb.Append(el.Value);
                            sb.Append('`');
                            break;
                        case "see" or "seealso":
                            var cref = el.Attribute("cref")?.Value;
                            if (cref is not null)
                            {
                                sb.Append('`');
                                var stripped = cref.Contains('.')
                                    ? cref[(cref.LastIndexOf('.') + 1)..]
                                    : StripTypePrefix(cref);
                                sb.Append(stripped);
                                sb.Append('`');
                            }

                            var langword = el.Attribute("langword")?.Value;
                            if (langword is not null)
                            {
                                sb.Append('`');
                                sb.Append(langword);
                                sb.Append('`');
                            }

                            var seeHref = el.Attribute("href")?.Value;
                            if (seeHref is not null)
                            {
                                var linkText = string.IsNullOrWhiteSpace(el.Value) ? seeHref : el.Value;
                                sb.Append('[');
                                sb.Append(linkText);
                                sb.Append("](");
                                sb.Append(seeHref);
                                sb.Append(')');
                            }

                            break;
                        case "para":
                            sb.Append("\n\n");
                            sb.Append(ExtractSummaryContent(el));
                            sb.Append("\n\n");
                            break;
                        case "list":
                            sb.Append(ConvertListToMarkdown(el));
                            break;
                        case "item":
                            // Standalone <item> outside a <list> context
                            sb.Append("- ");
                            sb.Append(ExtractSummaryContent(el));
                            sb.Append('\n');
                            break;
                        case "b" or "strong":
                            sb.Append("**");
                            sb.Append(ExtractSummaryContent(el));
                            sb.Append("**");
                            break;
                        case "i" or "em":
                            sb.Append('*');
                            sb.Append(ExtractSummaryContent(el));
                            sb.Append('*');
                            break;
                        default:
                            // For any other element, just include its text content
                            sb.Append(el.Value);
                            break;
                    }

                    break;
            }
        }

        // Collapse runs of spaces/tabs within each line, but preserve newlines
        var lines = sb.ToString().Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            lines[i] = WhitespaceLineRegex().Replace(lines[i].Trim(), " ");
        }

        return string.Join('\n', lines).Trim();
    }

    /// <summary>
    /// Converts an XML doc <c>&lt;list&gt;</c> element to Markdown.
    /// Supports <c>type="bullet"</c> (unordered list), <c>type="number"</c> (ordered list),
    /// and <c>type="table"</c> (raw HTML table, since Markdown pipe tables cannot handle
    /// multi-line cells or pipe characters in values).
    /// </summary>
    private string ConvertListToMarkdown(XElement list)
    {
        var sb = new System.Text.StringBuilder();
        var listType = list.Attribute("type")?.Value ?? "bullet";

        if (listType == "table")
        {
            sb.Append(ConvertListTableToMarkdown(list));
        }
        else
        {
            // Render as bullet or numbered list
            sb.Append('\n');
            var index = 1;
            foreach (var item in list.Elements("item"))
            {
                var prefix = listType == "number" ? $"{index++}. " : "- ";

                // If the item has <term> and <description>, format as "**term**: description"
                var term = item.Element("term");
                var desc = item.Element("description");
                if (term is not null && desc is not null)
                {
                    sb.Append(prefix);
                    sb.Append("**");
                    sb.Append(ExtractSummaryContent(term));
                    sb.Append("**: ");
                    sb.Append(ExtractSummaryContent(desc));
                    sb.Append('\n');
                }
                else
                {
                    sb.Append(prefix);
                    sb.Append(ExtractSummaryContent(item));
                    sb.Append('\n');
                }
            }

            sb.Append('\n');
        }

        return sb.ToString();
    }

    private System.Text.StringBuilder ConvertListTableToMarkdown(XElement list)
    {
        var sb = new System.Text.StringBuilder();

        // Try to parse items as structured attribute documentation.
        // If any item matches the structured convention, render a multi-column table.
        var items = list.Elements("item").ToList();
        var parsed = items.Select(ParseStructuredItem).ToList();
        var isStructured = parsed.Any(p => p is not null);

        if (isStructured)
        {
            sb.Append("\n\n<table class=\"ds-table my-4 w-full text-sm bg-(--ds-color-neutral-surface-default)\" data-border=\"true\" data-color=\"neutral\"><thead><tr>");
            sb.Append("<th>Parameter</th><th>Type</th><th>Default</th><th>Allowed</th><th>Description</th><th></th>");
            sb.Append("</tr></thead><tbody>");

            for (var idx = 0; idx < items.Count; idx++)
            {
                var p = parsed[idx];
                if (p is not null)
                {
                    var tagColor = p.IsRequired ? "warning" : "info";
                    var tagText = p.IsRequired ? "Må fylles ut" : "Valgfritt";

                    sb.Append("<tr>");
                    sb.Append($"<td>{WebUtility.HtmlEncode(p.Name)}</td>");
                    sb.Append($"<td>{p.TypeHtml}</td>");
                    sb.Append($"<td>{p.DefaultHtml}</td>");
                    sb.Append($"<td>{p.AllowedHtml}</td>");
                    sb.Append($"<td>{p.DescriptionHtml}</td>");
                    sb.Append($"<td><span class=\"ds-tag\" data-size=\"sm\" data-color=\"{tagColor}\">{tagText}</span></td>");
                    sb.Append("</tr>");
                }
                else
                {
                    // Fallback row for non-structured items within a structured table
                    var termEl = items[idx].Element("term") ?? new XElement("term");
                    var descEl = items[idx].Element("description") ?? new XElement("description");
                    sb.Append("<tr>");
                    sb.Append($"<td colspan=\"2\">{ExtractSummaryContentAsHtml(termEl)}</td>");
                    sb.Append("<td></td><td></td><td></td>");
                    sb.Append($"<td>{ExtractSummaryContentAsHtml(descEl)}</td>");
                    sb.Append("</tr>");
                }
            }

            sb.Append("</tbody></table>\n\n");
        }
        else
        {
            // No structured items detected - render a simple 2-column table
            sb.Append("\n\n<table class=\"ds-table my-4 w-full text-sm bg-(--ds-color-neutral-surface-default)\" data-border=\"true\" data-color=\"neutral\"><thead><tr>");
            var header = list.Element("listheader");
            if (header is not null)
            {
                var term = ExtractSummaryContentAsHtml(header.Element("term") ?? new XElement("term"));
                var desc = ExtractSummaryContentAsHtml(header.Element("description") ?? new XElement("description"));
                sb.Append($"<th>{term}</th><th>{desc}</th>");
            }

            sb.Append("</tr></thead><tbody>");

            foreach (var item in items)
            {
                var term = ExtractSummaryContentAsHtml(item.Element("term") ?? new XElement("term"));
                var desc = ExtractSummaryContentAsHtml(item.Element("description") ?? new XElement("description"));
                sb.Append($"<tr><td>{term}</td><td>{desc}</td></tr>");
            }

            sb.Append("</tbody></table>\n\n");
        }

        return sb;
    }

    [GeneratedRegex(@"[ \t]+")]
    private static partial Regex WhitespaceLineRegex();

    /// <summary>
    /// Converts the inner content of an XML doc element to an HTML fragment.
    /// Used for table cells emitted as raw HTML, where Markdown syntax would be
    /// re-encoded by the renderer.
    /// </summary>
    private string ExtractSummaryContentAsHtml(XElement element) => RenderNodesAsHtml(element.Nodes().ToList());

    /// <summary>
    /// Converts an XML doc <c>&lt;list&gt;</c> element to an HTML fragment for nested lists inside table cells.
    /// </summary>
    private string ConvertListToHtml(XElement list)
    {
        var sb = new System.Text.StringBuilder();
        var listType = list.Attribute("type")?.Value ?? "bullet";

        if (listType == "number")
        {
            sb.Append("<ol class=\"ds-list\">");
        }
        else
        {
            sb.Append("<ul class=\"ds-list\">");
        }

        foreach (var item in list.Elements("item"))
        {
            sb.Append("<li>");
            var term = item.Element("term");
            var desc = item.Element("description");
            if (term is not null && desc is not null)
            {
                sb.Append("<strong>");
                sb.Append(ExtractSummaryContentAsHtml(term));
                sb.Append("</strong>: ");
                sb.Append(ExtractSummaryContentAsHtml(desc));
            }
            else
            {
                sb.Append(ExtractSummaryContentAsHtml(item));
            }

            sb.Append("</li>");
        }

        sb.Append(listType == "number" ? "</ol>" : "</ul>");
        return sb.ToString();
    }

    /// <summary>
    /// Holds parsed data from a structured <c>&lt;list type="table"&gt;</c> item
    /// that follows the convention for attribute documentation.
    /// </summary>
    private sealed record StructuredItemData(
        string Name,
        string TypeHtml,
        bool IsRequired,
        string DefaultHtml,
        string AllowedHtml,
        string DescriptionHtml);

    /// <summary>
    /// Attempts to parse a <c>&lt;list type="table"&gt;</c> item as a structured attribute entry.
    /// Returns <see langword="null"/> if the item does not follow the structured convention.
    /// </summary>
    /// <remarks>
    /// Expected term format: <c>&lt;b&gt;name&lt;/b&gt;: type&lt;br/&gt;&lt;i&gt;(optional|required)&lt;/i&gt;</c>.
    /// Expected description format: labeled fields (<c>&lt;b&gt;Default&lt;/b&gt;:</c>,
    /// <c>&lt;b&gt;Allowed&lt;/b&gt;:</c>, <c>&lt;b&gt;Description&lt;/b&gt;:</c>) separated by <c>&lt;br/&gt;</c>.
    /// </remarks>
    private StructuredItemData? ParseStructuredItem(XElement item)
    {
        var termEl = item.Element("term");
        var descEl = item.Element("description");
        if (termEl is null)
        {
            return null;
        }

        // Detect <i>(optional|required)</i> in the term
        var italicEl = termEl.Elements("i")
            .FirstOrDefault(e =>
            {
                var text = e.Value.Trim().Trim('(', ')').ToLowerInvariant();
                return text is "optional" or "required";
            });

        if (italicEl is null)
        {
            return null;
        }

        var isRequired = italicEl.Value.Trim().Trim('(', ')').Equals("required", StringComparison.InvariantCultureIgnoreCase);

        // Extract name from the first <b> element in the term
        var nameEl = termEl.Element("b") ?? termEl.Element("strong");
        var name = nameEl?.Value.Trim() ?? string.Empty;

        // Extract type: everything between the name/colon and the <br/> before <i>,
        // excluding the <b> name element and the <i> element
        var nodes = termEl.Nodes().ToList();
        var italicIndex = nodes.IndexOf(italicEl);
        var nameIndex = nameEl is not null ? nodes.IndexOf(nameEl) : -1;

        // Find the <br/> preceding the italic (skipping whitespace text nodes)
        var brBeforeItalicIndex = -1;
        for (var j = italicIndex - 1; j >= 0; j--)
        {
            if (nodes[j] is XText t && string.IsNullOrWhiteSpace(t.Value))
            {
                continue;
            }

            if (nodes[j] is XElement br && br.Name.LocalName == "br")
            {
                brBeforeItalicIndex = j;
            }

            break;
        }

        // Build the type HTML from nodes between <b>name</b> and the <br/> before <i>
        var typeSb = new System.Text.StringBuilder();
        var startAfter = nameIndex >= 0 ? nameIndex + 1 : 0;
        var endBefore = brBeforeItalicIndex >= 0 ? brBeforeItalicIndex : italicIndex;
        for (var i = startAfter; i < endBefore; i++)
        {
            switch (nodes[i])
            {
                case XText text:
                    // Strip the leading ": " separator
                    var val = text.Value;
                    if (i == startAfter)
                    {
                        val = val.TrimStart().TrimStart(':').TrimStart();
                    }

                    typeSb.Append(WebUtility.HtmlEncode(val));
                    break;
                case XElement el:
                    switch (el.Name.LocalName)
                    {
                        case "see" or "seealso":
                            typeSb.Append(RenderSeeAsHtml(el));
                            break;
                        case "c":
                            typeSb.Append("<code>");
                            typeSb.Append(WebUtility.HtmlEncode(el.Value));
                            typeSb.Append("</code>");
                            break;
                        case "i" or "em":
                            typeSb.Append(WebUtility.HtmlEncode(el.Value));
                            break;
                        default:
                            typeSb.Append(WebUtility.HtmlEncode(el.Value));
                            break;
                    }

                    break;
            }
        }

        var typeHtml = WhitespaceLineRegex().Replace(typeSb.ToString().Trim(), " ");

        // Parse description fields
        var defaultHtml = string.Empty;
        var allowedHtml = string.Empty;
        var descriptionHtml = string.Empty;

        if (descEl is not null)
        {
            var segments = SplitByBreaks(descEl);
            string? lastKnownLabel = null;

            foreach (var segment in segments)
            {
                var (label, content) = ExtractLabeledField(segment);
                if (label is not null)
                {
                    switch (label.ToLowerInvariant())
                    {
                        case "default":
                            defaultHtml = content ?? string.Empty;
                            lastKnownLabel = "default";
                            break;
                        case "allowed":
                            allowedHtml = content ?? string.Empty;
                            lastKnownLabel = "allowed";
                            break;
                        case "description":
                            descriptionHtml = content ?? string.Empty;
                            lastKnownLabel = "description";
                            break;
                        default:
                            // Unknown label - append to description as labeled content
                            var labeledHtml = $"<strong>{WebUtility.HtmlEncode(label)}</strong>: {content}";
                            descriptionHtml = string.IsNullOrEmpty(descriptionHtml)
                                ? labeledHtml
                                : descriptionHtml + "<br/>" + labeledHtml;
                            lastKnownLabel = "description";
                            break;
                    }
                }
                else
                {
                    // Unlabeled content - merge with the last known field, or use as description
                    var plainHtml = RenderNodesAsHtml(segment);
                    if (string.IsNullOrWhiteSpace(plainHtml))
                    {
                        continue;
                    }

                    switch (lastKnownLabel)
                    {
                        case "default":
                            defaultHtml += "<br/>" + plainHtml;
                            break;
                        case "allowed":
                            allowedHtml += "<br/>" + plainHtml;
                            break;
                        default:
                            descriptionHtml = string.IsNullOrEmpty(descriptionHtml)
                                ? plainHtml
                                : descriptionHtml + "<br/>" + plainHtml;
                            lastKnownLabel = "description";
                            break;
                    }
                }
            }

            // If no labeled fields were found, use the entire description as the description column
            if (string.IsNullOrEmpty(defaultHtml) && string.IsNullOrEmpty(allowedHtml) && string.IsNullOrEmpty(descriptionHtml))
            {
                descriptionHtml = ExtractSummaryContentAsHtml(descEl);
            }
        }

        return new StructuredItemData(name, typeHtml, isRequired, defaultHtml, allowedHtml, descriptionHtml);
    }

    /// <summary>
    /// Splits an element's child nodes into groups delimited by <c>&lt;br/&gt;</c> elements.
    /// </summary>
    private static List<List<XNode>> SplitByBreaks(XElement element)
    {
        var result = new List<List<XNode>>();
        var current = new List<XNode>();
        foreach (var node in element.Nodes())
        {
            if (node is XElement el && el.Name.LocalName == "br")
            {
                if (current.Count > 0)
                {
                    result.Add(current);
                    current = [];
                }
            }
            else
            {
                current.Add(node);
            }
        }

        if (current.Count > 0)
        {
            result.Add(current);
        }

        return result;
    }

    /// <summary>
    /// Extracts a labeled field from a segment of nodes if it starts with <c>&lt;b&gt;Label&lt;/b&gt;: content</c>.
    /// Returns the label and the remaining content as HTML strings.
    /// </summary>
    private (string? Label, string? Content) ExtractLabeledField(List<XNode> nodes)
    {
        if (nodes.Count == 0)
        {
            return (null, null);
        }

        // Find the first significant node (skip whitespace-only text nodes)
        var firstIndex = -1;
        for (var i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] is XText t && string.IsNullOrWhiteSpace(t.Value))
            {
                continue;
            }

            firstIndex = i;
            break;
        }

        if (firstIndex < 0)
        {
            return (null, null);
        }

        // Check if the first significant node is a <b> or <strong> element
        if (nodes[firstIndex] is not XElement boldEl || boldEl.Name.LocalName is not ("b" or "strong"))
        {
            return (null, null);
        }

        var label = boldEl.Value.Trim().TrimEnd(':');

        // The content is everything after the <b> element
        var contentNodes = nodes.Skip(firstIndex + 1).ToList();

        // Strip leading colon and whitespace from the first text node
        if (contentNodes.Count > 0 && contentNodes[0] is XText firstText)
        {
            var trimmed = firstText.Value.TrimStart().TrimStart(':').TrimStart();
            contentNodes[0] = new XText(trimmed);
        }

        var content = RenderNodesAsHtml(contentNodes);
        return (label, content);
    }

    /// <summary>
    /// Renders a list of XML nodes as an HTML string fragment.
    /// </summary>
    private string RenderNodesAsHtml(List<XNode> nodes)
    {
        var sb = new System.Text.StringBuilder();
        foreach (var node in nodes)
        {
            switch (node)
            {
                case XText text:
                    sb.Append(WebUtility.HtmlEncode(text.Value));
                    break;
                case XElement el:
                    switch (el.Name.LocalName)
                    {
                        case "br":
                            sb.Append("<br/>");
                            break;
                        case "c":
                            sb.Append("<code>");
                            sb.Append(WebUtility.HtmlEncode(el.Value));
                            sb.Append("</code>");
                            break;
                        case "see" or "seealso":
                            sb.Append(RenderSeeAsHtml(el));
                            break;
                        case "para":
                            sb.Append("<br/><br/>");
                            sb.Append(RenderNodesAsHtml(el.Nodes().ToList()));
                            break;
                        case "b" or "strong":
                            sb.Append("<strong>");
                            sb.Append(RenderNodesAsHtml(el.Nodes().ToList()));
                            sb.Append("</strong>");
                            break;
                        case "i" or "em":
                            sb.Append("<em>");
                            sb.Append(RenderNodesAsHtml(el.Nodes().ToList()));
                            sb.Append("</em>");
                            break;
                        case "list":
                            sb.Append(ConvertListToHtml(el));
                            break;
                        default:
                            sb.Append(WebUtility.HtmlEncode(el.Value));
                            break;
                    }

                    break;
            }
        }

        return WhitespaceLineRegex().Replace(sb.ToString().Trim(), " ");
    }

    /// <summary>
    /// Maps C# langword keywords to their Microsoft documentation URLs.
    /// Covers all reserved keywords and contextual keywords from the C# language reference.
    /// </summary>
    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/"/>
    private static readonly FrozenDictionary<string, string> LangwordTypeLinks = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        // Reserved keywords
        ["abstract"] = "language-reference/keywords/abstract",
        ["as"] = "language-reference/operators/type-testing-and-cast#the-as-operator",
        ["base"] = "language-reference/keywords/base",
        ["bool"] = "language-reference/builtin-types/bool",
        ["break"] = "language-reference/statements/jump-statements#the-break-statement",
        ["byte"] = "language-reference/builtin-types/integral-numeric-types",
        ["case"] = "language-reference/statements/selection-statements#the-switch-statement",
        ["catch"] = "language-reference/statements/exception-handling-statements#the-try-catch-statement",
        ["char"] = "language-reference/builtin-types/char",
        ["checked"] = "language-reference/statements/checked-and-unchecked",
        ["class"] = "language-reference/keywords/class",
        ["const"] = "language-reference/keywords/const",
        ["continue"] = "language-reference/statements/jump-statements#the-continue-statement",
        ["decimal"] = "language-reference/builtin-types/floating-point-numeric-types",
        ["default"] = "language-reference/keywords/default",
        ["delegate"] = "language-reference/builtin-types/reference-types",
        ["do"] = "language-reference/statements/iteration-statements#the-do-statement",
        ["double"] = "language-reference/builtin-types/floating-point-numeric-types",
        ["else"] = "language-reference/statements/selection-statements#the-if-statement",
        ["enum"] = "language-reference/builtin-types/enum",
        ["event"] = "language-reference/keywords/event",
        ["explicit"] = "language-reference/operators/user-defined-conversion-operators",
        ["extern"] = "language-reference/keywords/extern",
        ["false"] = "language-reference/builtin-types/bool",
        ["finally"] = "language-reference/statements/exception-handling-statements#the-try-finally-statement",
        ["fixed"] = "language-reference/statements/fixed",
        ["float"] = "language-reference/builtin-types/floating-point-numeric-types",
        ["for"] = "language-reference/statements/iteration-statements#the-for-statement",
        ["foreach"] = "language-reference/statements/iteration-statements#the-foreach-statement",
        ["goto"] = "language-reference/statements/jump-statements#the-goto-statement",
        ["if"] = "language-reference/statements/selection-statements#the-if-statement",
        ["implicit"] = "language-reference/operators/user-defined-conversion-operators",
        ["in"] = "language-reference/keywords/in",
        ["int"] = "language-reference/builtin-types/integral-numeric-types",
        ["interface"] = "language-reference/keywords/interface",
        ["internal"] = "language-reference/keywords/internal",
        ["is"] = "language-reference/operators/is",
        ["lock"] = "language-reference/statements/lock",
        ["long"] = "language-reference/builtin-types/integral-numeric-types",
        ["namespace"] = "language-reference/keywords/namespace",
        ["new"] = "language-reference/keywords/new",
        ["null"] = "language-reference/keywords/null",
        ["object"] = "language-reference/builtin-types/reference-types",
        ["operator"] = "language-reference/operators/operator-overloading",
        ["out"] = "language-reference/keywords/out",
        ["override"] = "language-reference/keywords/override",
        ["params"] = "language-reference/keywords/method-parameters#params-modifier",
        ["private"] = "language-reference/keywords/private",
        ["protected"] = "language-reference/keywords/protected",
        ["public"] = "language-reference/keywords/public",
        ["readonly"] = "language-reference/keywords/readonly",
        ["ref"] = "language-reference/keywords/ref",
        ["return"] = "language-reference/statements/jump-statements#the-return-statement",
        ["sbyte"] = "language-reference/builtin-types/integral-numeric-types",
        ["sealed"] = "language-reference/keywords/sealed",
        ["short"] = "language-reference/builtin-types/integral-numeric-types",
        ["sizeof"] = "language-reference/operators/sizeof",
        ["stackalloc"] = "language-reference/operators/stackalloc",
        ["static"] = "language-reference/keywords/static",
        ["string"] = "language-reference/builtin-types/reference-types",
        ["struct"] = "language-reference/builtin-types/struct",
        ["switch"] = "language-reference/operators/switch-expression",
        ["this"] = "language-reference/keywords/this",
        ["throw"] = "language-reference/statements/exception-handling-statements#the-throw-statement",
        ["true"] = "language-reference/builtin-types/bool",
        ["try"] = "language-reference/statements/exception-handling-statements#the-try-statement",
        ["typeof"] = "language-reference/operators/type-testing-and-cast#the-typeof-operator",
        ["uint"] = "language-reference/builtin-types/integral-numeric-types",
        ["ulong"] = "language-reference/builtin-types/integral-numeric-types",
        ["unchecked"] = "language-reference/statements/checked-and-unchecked",
        ["unsafe"] = "language-reference/keywords/unsafe",
        ["ushort"] = "language-reference/builtin-types/integral-numeric-types",
        ["using"] = "language-reference/keywords/using",
        ["virtual"] = "language-reference/keywords/virtual",
        ["void"] = "language-reference/builtin-types/void",
        ["volatile"] = "language-reference/keywords/volatile",
        ["while"] = "language-reference/statements/iteration-statements#the-while-statement",

        // Contextual keywords
        ["add"] = "language-reference/keywords/add",
        ["allows"] = "language-reference/keywords/where-generic-type-constraint",
        ["alias"] = "language-reference/keywords/extern-alias",
        ["and"] = "language-reference/operators/patterns#logical-patterns",
        ["ascending"] = "language-reference/keywords/ascending",
        ["args"] = "fundamentals/program-structure/top-level-statements#args",
        ["async"] = "language-reference/keywords/async",
        ["await"] = "language-reference/operators/await",
        ["by"] = "language-reference/keywords/by",
        ["descending"] = "language-reference/keywords/descending",
        ["dynamic"] = "language-reference/builtin-types/reference-types",
        ["equals"] = "language-reference/keywords/equals",
        ["extension"] = "language-reference/keywords/extension",
        ["field"] = "language-reference/keywords/field",
        ["file"] = "language-reference/keywords/file",
        ["from"] = "language-reference/keywords/from-clause",
        ["get"] = "language-reference/keywords/get",
        ["global"] = "language-reference/operators/namespace-alias-qualifier",
        ["group"] = "language-reference/keywords/group-clause",
        ["init"] = "language-reference/keywords/init",
        ["into"] = "language-reference/keywords/into",
        ["join"] = "language-reference/keywords/join-clause",
        ["let"] = "language-reference/keywords/let-clause",
        ["managed"] = "language-reference/unsafe-code#function-pointers",
        ["nameof"] = "language-reference/operators/nameof",
        ["nint"] = "language-reference/builtin-types/integral-numeric-types",
        ["not"] = "language-reference/operators/patterns#logical-patterns",
        ["notnull"] = "programming-guide/generics/constraints-on-type-parameters#notnull-constraint",
        ["nuint"] = "language-reference/builtin-types/integral-numeric-types",
        ["on"] = "language-reference/keywords/on",
        ["or"] = "language-reference/operators/patterns#logical-patterns",
        ["orderby"] = "language-reference/keywords/orderby-clause",
        ["partial"] = "language-reference/keywords/partial-type",
        ["record"] = "fundamentals/types/records",
        ["remove"] = "language-reference/keywords/remove",
        ["required"] = "language-reference/keywords/required",
        ["scoped"] = "language-reference/statements/declarations#scoped-ref",
        ["select"] = "language-reference/keywords/select-clause",
        ["set"] = "language-reference/keywords/set",
        ["unmanaged"] = "language-reference/unsafe-code#function-pointers",
        ["value"] = "language-reference/keywords/value",
        ["var"] = "language-reference/statements/declarations#implicitly-typed-local-variables",
        ["when"] = "language-reference/keywords/when",
        ["where"] = "language-reference/keywords/where-generic-type-constraint",
        ["with"] = "language-reference/keywords/with",
        ["yield"] = "language-reference/statements/yield",
    }.ToFrozenDictionary(StringComparer.Ordinal);

    /// <summary>
    /// Tracks the current recursion depth for <see cref="RenderSeeAsHtml"/> to prevent
    /// infinite recursion when a referenced member's summary itself contains <c>&lt;see&gt;</c> elements.
    /// </summary>
    private int seeRecursionDepth;

    /// <summary>
    /// Renders a <c>&lt;see&gt;</c> element as an HTML fragment.
    /// For <c>cref</c> references, emits an inline trigger button and a matching popover
    /// using the native HTML Popover API (<c>popovertarget</c> + <c>popover</c>).
    /// The popover body is populated with the XML doc summary for the referenced member.
    /// For <c>langword</c> references, emits a link to the Microsoft documentation.
    /// </summary>
    private string RenderSeeAsHtml(XElement seeEl)
    {
        var sb = new System.Text.StringBuilder();
        var cref = seeEl.Attribute("cref")?.Value;
        if (cref is not null)
        {
            var name = cref;
            if (cref.Contains('.'))
            {
                name = cref[(cref.LastIndexOf('.') + 1)..];
            }
            else if (cref.Length > 2 && cref[1] == ':')
            {
                name = cref[2..];
            }

            var encodedName = WebUtility.HtmlEncode(name);

            // Prevent infinite recursion: only expand popover at the top level(s)
            if (seeRecursionDepth > 1)
            {
                sb.Append("<code>");
                sb.Append(encodedName);
                sb.Append("</code>");
                return sb.ToString();
            }

            seeRecursionDepth++;
            string? summaryHtml;
            try
            {
                summaryHtml = GetCrefSummaryHtml(cref);
            }
            finally
            {
                seeRecursionDepth--;
            }

            sb.Append(BuildPopoverHtml(encodedName, summaryHtml));
        }

        var langword = seeEl.Attribute("langword")?.Value;
        if (langword is not null)
        {
            // Type aliases link to MS docs; keywords render as inline code
            if (LangwordTypeLinks.TryGetValue(langword, out var langHref))
            {
                var fullHref = $"https://learn.microsoft.com/en-us/dotnet/csharp/{langHref}";
                sb.Append("<a class=\"ds-link\" data-color=\"info\" href=\"");
                sb.Append(fullHref);
                sb.Append("\" target=\"_blank\" rel=\"noopener noreferrer\">");
                sb.Append(WebUtility.HtmlEncode(langword));
                sb.Append("</a>");
            }
            else
            {
                sb.Append("<code>");
                sb.Append(WebUtility.HtmlEncode(langword));
                sb.Append("</code>");
            }
        }

        var href = seeEl.Attribute("href")?.Value;
        if (href is not null)
        {
            var linkText = string.IsNullOrWhiteSpace(seeEl.Value) ? href : WebUtility.HtmlEncode(seeEl.Value);
            sb.Append("<a class=\"ds-link\" data-color=\"info\" href=\"");
            sb.Append(WebUtility.HtmlEncode(href));
            sb.Append("\" target=\"_blank\" rel=\"noopener noreferrer\">");
            sb.Append(linkText);
            sb.Append("</a>");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Looks up the XML doc summary for a cref member ID across all loaded XML doc files.
    /// Falls back to descriptions for well-known .NET types.
    /// Returns the summary as an HTML string, or <see langword="null"/> if not found.
    /// </summary>
    private string? GetCrefSummaryHtml(string cref)
    {
        // Search indexed members across all loaded XML doc files (O(1) per doc)
        foreach ((_, var index) in xmlMemberIndex)
        {
            if (index.TryGetValue(cref, out var member))
            {
                var summary = member.Element("summary");
                if (summary is not null)
                {
                    return ExtractSummaryContentAsHtml(summary);
                }
            }
        }

        var stripped = StripTypePrefix(cref);
        if (stripped.StartsWith("Hviktor", StringComparison.Ordinal))
        {
            return null;
        }

        var sb = new System.Text.StringBuilder();
        var description = WellKnownTypeDescriptions.GetValueOrDefault(cref);
        if (description is not null)
        {
            sb.Append(description);
            sb.Append("<br/>");
        }

        sb.Append("<a class=\"ds-link\" data-color=\"info\" href=\"https://learn.microsoft.com/en-us/dotnet/api/");
        sb.Append(stripped);
        sb.Append("?view=net-10.0\" target=\"_blank\" rel=\"noopener noreferrer\">Learn more (Microsoft)</a>");

        return sb.ToString();
    }

    /// <summary>
    /// Strip prefix like "T:", "P:", "M:" and namespace
    /// </summary>
    /// <param name="cref"></param>
    /// <returns></returns>
    private static string StripTypePrefix(string cref) => cref.Length > 2 && cref[1] == ':' ? cref[2..] : cref;

    /// <summary>
    /// Descriptions for common .NET types that are frequently referenced in XML docs
    /// but not present in the project's own XML documentation files.
    /// </summary>
    private static readonly FrozenDictionary<string, string> WellKnownTypeDescriptions = new Dictionary<string, string>(StringComparer.Ordinal)
    {
        ["T:System.Boolean"] = "Represents a Boolean (true or false) value.",
        ["T:System.String"] = "Represents text as a sequence of UTF-16 code units.",
        ["T:System.Int32"] = "Represents a 32-bit signed integer.",
        ["T:System.Int64"] = "Represents a 64-bit signed integer.",
        ["T:System.Double"] = "Represents a double-precision floating-point number.",
        ["T:System.Single"] = "Represents a single-precision floating-point number.",
        ["T:System.Decimal"] = "Represents a decimal floating-point number.",
        ["T:System.Char"] = "Represents a character as a UTF-16 code unit.",
        ["T:System.Byte"] = "Represents an 8-bit unsigned integer.",
        ["T:System.Object"] = "Supports all classes in the .NET class hierarchy.",
        ["T:System.Void"] = "Specifies a return value type for a method that does not return a value.",
        ["T:System.Nullable`1"] = "Represents a value type that can be assigned null.",
        ["T:System.String[]"] = "An array of strings.",
        ["T:System.EventArgs"] = "Represents the base class for classes that contain event data.",
        ["T:System.Exception"] = "Represents errors that occur during application execution.",
        ["T:System.Type"] = "Represents type declarations: class types, interface types, array types, value types, and enumeration types.",
        ["T:System.Guid"] = "Represents a globally unique identifier (GUID).",
        ["T:System.DateTime"] = "Represents an instant in time, typically expressed as a date and time of day.",
        ["T:System.TimeSpan"] = "Represents a time interval.",
        ["T:System.Uri"] = "Provides an object representation of a uniform resource identifier (URI).",
        ["T:System.Threading.Tasks.Task"] = "Represents an asynchronous operation.",
        ["T:System.Threading.Tasks.ValueTask"] = "Provides a value type that wraps a Task and a TResult, only one of which is used.",
        ["T:Microsoft.AspNetCore.Components.RenderFragment"] = "Represents a segment of UI content, implemented as a delegate that writes the content to a RenderTreeBuilder.",
        ["T:Microsoft.AspNetCore.Components.EventCallback"] = "A bound event handler delegate.",
        ["T:Microsoft.AspNetCore.Components.ElementReference"] = "Represents a reference to a rendered element.",
    }.ToFrozenDictionary(StringComparer.Ordinal);

    /// <summary>Excluded method names that are framework/lifecycle internals.</summary>
    private static readonly FrozenSet<string> ExcludedMethods = new HashSet<string>(StringComparer.Ordinal)
    {
        "SetParametersAsync", "StateHasChanged", // ComponentBase lifecycle
        "GetType", "ToString", "Equals", "GetHashCode", // Object
        "Dispose", "DisposeAsync", // IDisposable / IAsyncDisposable
        "Attach", "BuildRenderTree", // Common Blazor internals
    }.ToFrozenSet(StringComparer.Ordinal);


    /// <inheritdoc/>
    public string? BuildParametersHtml(IReadOnlyList<ParameterInfo> parameters)
    {
        if (parameters.Count == 0)
        {
            return null;
        }

        var sb = new System.Text.StringBuilder();
        sb.Append("<table class=\"ds-table my-4 w-full text-sm bg-(--ds-color-neutral-surface-default)\" data-border=\"true\" data-color=\"neutral\"><thead><tr>");
        sb.Append("<th>Parameter</th><th>Type</th><th>Default</th><th>Allowed</th><th>Description</th><th></th>");
        sb.Append("</tr></thead><tbody>");

        foreach (var p in parameters)
        {
            var name = WebUtility.HtmlEncode(p.Name);
            var type = p.TypeHtml ?? $"<code>{WebUtility.HtmlEncode(p.TypeName)}</code>";
            var def = p.DefaultValue ?? string.Empty;
            var allowed = p.AllowedValues.Count > 0
                ? string.Join(" ", p.AllowedValues.Select(v => $"<code>{WebUtility.HtmlEncode(v)}</code>"))
                : string.Empty;
            var desc = !string.IsNullOrWhiteSpace(p.XmlDocSummaryHtml)
                ? p.XmlDocSummaryHtml
                : string.Empty;
            var tagColor = p.IsRequired ? "warning" : "info";
            var tagText = p.IsRequired ? "Må fylles ut" : "Valgfritt";

            sb.Append("<tr>");
            sb.Append($"<td>{name}</td>");
            sb.Append($"<td>{type}</td>");
            sb.Append($"<td>{def}</td>");
            sb.Append($"<td>{allowed}</td>");
            sb.Append($"<td>{desc}</td>");
            sb.Append($"<td><span class=\"ds-tag\" data-size=\"sm\" data-color=\"{tagColor}\">{tagText}</span></td>");
            sb.Append("</tr>");
        }

        sb.Append("</tbody></table>");
        return sb.ToString();
    }

    /// <inheritdoc/>
    public string? BuildMethodsHtml(IReadOnlyList<ComponentMethodInfo> methods)
    {
        if (methods.Count == 0)
        {
            return null;
        }

        var sb = new System.Text.StringBuilder();
        sb.Append("<table class=\"ds-table my-4 w-full text-sm bg-(--ds-color-neutral-surface-default)\" data-border=\"true\" data-color=\"neutral\"><thead><tr>");
        sb.Append("<th>Method</th><th>Returns</th><th>Arguments</th><th>Description</th>");
        sb.Append("</tr></thead><tbody>");

        foreach (var methodInfo in methods)
        {
            var name = WebUtility.HtmlEncode(methodInfo.Name);
            var type = methodInfo.ReturnTypeHtml ?? $"<code>{WebUtility.HtmlEncode(methodInfo.ReturnType)}</code>";
            var args = methodInfo.ParameterSignatureHtml ?? (string.IsNullOrEmpty(methodInfo.ParameterSignature)
                ? string.Empty
                : string.Join(" ", methodInfo.ParameterSignature.Split(", ").Select(v => $"<code>{WebUtility.HtmlEncode(v)}</code>")));
            var desc = !string.IsNullOrWhiteSpace(methodInfo.XmlDocSummaryHtml)
                ? methodInfo.XmlDocSummaryHtml
                : string.Empty;

            sb.Append("<tr>");
            sb.Append($"<td>{name}</td>");
            sb.Append($"<td>{type}</td>");
            sb.Append($"<td>{args}</td>");
            sb.Append($"<td>{desc}</td>");
            sb.Append("</tr>");
        }

        sb.Append("</tbody></table>");
        return sb.ToString();
    }

    [LoggerMessage(LogLevel.Warning, "Could not load XML docs from {path}")]
    partial void LogCouldNotLoadXmlDocsFromPath(string path);

    [LoggerMessage(LogLevel.Debug, "XML docs discovery: {assemblyCount} Hviktor assemblies, {resourceCount} XML resources in host: [{resources}]")]
    partial void LogXmlDocsDiscovery(int assemblyCount, int resourceCount, string resources);

    [LoggerMessage(LogLevel.Warning, "XML docs not found for {assemblyName}")]
    partial void LogXmlDocsNotFound(string assemblyName);

    [LoggerMessage(LogLevel.Debug, "Could not read XML class doc for {type}")]
    partial void LogCouldNotReadXmlClassDocForType(string type);

    [LoggerMessage(LogLevel.Debug, "Could not read XML doc for {type}.{prop}")]
    partial void LogCouldNotReadXmlDocForTypeProp(string type, string prop);
}