using System.Text;
using System.Text.RegularExpressions;
using Documentation.Components.Services;
using Microsoft.Extensions.Logging;
using ParameterInfo = Documentation.Components.Services.ParameterInfo;

namespace WikiGen.Services;

/// <summary>
/// Builds GitHub Wiki-compatible markdown from component registry data.
/// </summary>
public sealed partial class WikiMarkdownBuilder
{
    private readonly IComponentMetadataService metadataService;
    private readonly IDemoSourceService demoSourceService;
    private readonly ILogger<WikiMarkdownBuilder> logger;

    /// <summary>Initializes the markdown builder with required services.</summary>
    public WikiMarkdownBuilder(
        IComponentMetadataService metadataService,
        IDemoSourceService demoSourceService,
        ILogger<WikiMarkdownBuilder> logger)
    {
        this.metadataService = metadataService;
        this.demoSourceService = demoSourceService;
        this.logger = logger;
    }

    /// <summary>Builds the <c>Home.md</c> landing page listing all component groups.</summary>
    public static string BuildHomePage()
    {
        var sb = new StringBuilder();
        sb.AppendLine("<h1>");
        sb.AppendLine("  <a href=\"https://github.com/HelseVestIKT/hviktor-blazor/\" align=\"center\">");
        sb.AppendLine("    <img src=\"https://github.com/HelseVestIKT/hviktor-blazor/blob/main/logo.svg\" width=\"24\"/>  </a>");
        sb.AppendLine("  <strong>Documentation</strong>");
        sb.AppendLine("</h1>");
        sb.AppendLine();
        sb.AppendLine("This is the documentation for the Hviktor component library.  ");
        sb.AppendLine("Use this documentation to understand, integrate and extend Hviktor in your applications.");
        sb.AppendLine();
        sb.AppendLine("Hviktor is a component library based on [Hviktor](https://github.com/HelseVestIKT/Hviktor) & [Designsystemet](https://github.com/digdir/designsystemet) that is specifically designed for internal use in the [Helse Vest IKT](https://www.helse-vest-ikt.no/) organization.");
        sb.AppendLine();
        sb.AppendLine("## NuGet Packages");
        sb.AppendLine();
        sb.AppendLine("### Hviktor");
        sb.AppendLine();
        sb.AppendLine("The main component library package, containing all core components and features.  ");
        sb.AppendLine("See [GitHub NuGet Registry](https://github.com/HelseVestIKT/hviktor-blazor/pkgs/nuget/Hviktor) for the latest release.");
        sb.AppendLine();
        sb.AppendLine("```bash");
        sb.AppendLine("dotnet add package Hviktor");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("### Hviktor.Abstractions");
        sb.AppendLine();
        sb.AppendLine("The abstraction package, providing interfaces and base classes for Hviktor components.  ");
        sb.AppendLine("See [GitHub NuGet Registry](https://github.com/HelseVestIKT/hviktor-blazor/pkgs/nuget/Hviktor.Abstractions) for the latest release.");
        sb.AppendLine();
        sb.AppendLine("```bash");
        sb.AppendLine("dotnet add package Hviktor.Abstractions");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("### Hviktor.Extensions");
        sb.AppendLine();
        sb.AppendLine("Extension methods and utilities for Hviktor components.  ");
        sb.AppendLine("See [GitHub NuGet Registry](https://github.com/HelseVestIKT/hviktor-blazor/pkgs/nuget/Hviktor.Extensions) for the latest release.");
        sb.AppendLine();
        sb.AppendLine("```bash");
        sb.AppendLine("dotnet add package Hviktor.Extensions");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("### Hviktor.Icons");
        sb.AppendLine();
        sb.AppendLine("Icon components for Hviktor.  ");
        sb.AppendLine("See [GitHub NuGet Registry](https://github.com/HelseVestIKT/hviktor-blazor/pkgs/nuget/Hviktor.Icons) for the latest release.");
        sb.AppendLine();
        sb.AppendLine("```bash");
        sb.AppendLine("dotnet add package Hviktor.Icons");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("### Hviktor.Icons.Abstractions");
        sb.AppendLine();
        sb.AppendLine("Abstraction package for icon components.  ");
        sb.AppendLine("See [GitHub NuGet Registry](https://github.com/HelseVestIKT/hviktor-blazor/pkgs/nuget/Hviktor.Icons.Abstractions) for the latest release.");
        sb.AppendLine();
        sb.AppendLine("```bash");
        sb.AppendLine("dotnet add package Hviktor.Icons.Abstractions");
        sb.AppendLine("```");
        sb.AppendLine();

        return sb.ToString();
    }

    /// <summary>Builds the <c>_Sidebar.md</c> navigation for GitHub Wiki.</summary>
    public static string BuildSidebar(IReadOnlyList<ComponentGroup> groups)
    {
        var sb = new StringBuilder();
        sb.AppendLine("# Pages");
        sb.AppendLine();
        sb.AppendLine("- [Contributing](https://github.com/HelseVestIKT/hviktor-blazor/blob/main/CONTRIBUTING.md)  ");
        sb.AppendLine("  Guidelines for contributing to the project.");
        sb.AppendLine();
        sb.AppendLine("- [Code of Conduct](https://github.com/HelseVestIKT/hviktor-blazor/blob/main/CODE_OF_CONDUCT.md)  ");
        sb.AppendLine("  Our code of conduct for all contributors and community members.");
        sb.AppendLine();
        sb.AppendLine("- [Getting started](GettingStarted)  ");
        sb.AppendLine("  Introduction, installation, and setup instructions.");
        sb.AppendLine();
        sb.AppendLine("- [Usage](Usage)  ");
        sb.AppendLine("  How to use the components and features of the library.");
        sb.AppendLine();
        sb.AppendLine("- [Publish](Publish)  ");
        sb.AppendLine("  Instructions for publishing new versions of the library.");
        sb.AppendLine();

        foreach (var group in groups)
        {
            sb.AppendLine($"## {group.Title}  ");
            sb.AppendLine();

            foreach (var component in group.Items)
            {
                var prefix = "";
                var suffix = "";
                var tag = "";
                if (component.IsDeprecated)
                {
                    prefix = "~~";
                    suffix = "~~";
                    tag = " DEPRECATED";
                }
                else if (component.IsExperimental)
                {
                    prefix = "**";
                    suffix = "**";
                    tag = " EXPERIMENTAL";
                }

                var link = component.Title.Replace(' ', '_');
                sb.AppendLine($"- {prefix}[{component.Title}]({link}){suffix}{tag}");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }

    /// <summary>Builds the <c>_Footer.md</c> content for GitHub Wiki, which can include additional links or information.</summary>
    public static string BuildFooter()
    {
        var sb = new StringBuilder();

        sb.AppendLine("**[Hviktor](https://github.com/HelseVestIKT/hviktor-blazor/)** is a Blazor component library built on [Hviktor](https://github.com/HelseVestIKT/hviktor) and [Designsystemet](https://github.com/digdir/designsystemet), developed by [Helse Vest IKT](https://helse-vest-ikt.no/).");
        sb.AppendLine();
        sb.AppendLine("#### Hviktor");
        sb.AppendLine();
        sb.AppendLine("A component library and design system by Helse Vest IKT for digital health services.");
        sb.AppendLine();
        sb.AppendLine("- [GitHub](https://github.com/HelseVestIKT/hviktor)");
        sb.AppendLine("- [Documentation](https://helsevestikt.github.io/hviktor/)");
        sb.AppendLine();
        sb.AppendLine("#### Designsystemet");
        sb.AppendLine();
        sb.AppendLine("A design system by [Digdir](https://www.digdir.no/) for Norwegian public digital services.");
        sb.AppendLine();
        sb.AppendLine("- [GitHub](https://github.com/digdir/designsystemet)");
        sb.AppendLine("- [Documentation](https://designsystemet.no/en/components)");
        sb.AppendLine();

        return sb.ToString();
    }

    /// <summary>Builds a full markdown page for a single component.</summary>
    public string BuildComponentPage(ComponentInfo component, string groupTitle)
    {
        var sb = new StringBuilder();

        // Title and badges
        sb.AppendLine($"# {component.Title}");
        sb.AppendLine();

        AppendStatusBadges(sb, component);

        // Description
        if (!string.IsNullOrEmpty(component.Description) && component.Description != "No description available.")
        {
            sb.AppendLine(component.Description);
            sb.AppendLine();
        }

        // Designsystemet link
        if (!string.IsNullOrEmpty(component.DesignsystemetLink))
        {
            sb.AppendLine($"> [Read more about {component.Title} on designsystemet.no]({component.DesignsystemetLink})");
            sb.AppendLine();
        }

        // Use / Avoid sections
        AppendDocSection(sb, "Use", component.Documentation?.Use);
        AppendDocSection(sb, "Avoid", component.Documentation?.Avoid);

        // Parameters table
        if (component.ComponentType is not null)
        {
            var parameters = metadataService.GetParameters(component.ComponentType);
            AppendParametersTable(sb, parameters);

            var methods = metadataService.GetPublicMethods(component.ComponentType);
            AppendMethodsTable(sb, methods);
        }

        // Sub-components
        if (component.SubComponents is { Count: > 0 } subs)
        {
            foreach (var sub in subs)
            {
                sb.AppendLine($"## {sub.Title}");
                sb.AppendLine();

                var subSummary = metadataService.GetClassSummary(sub.ComponentType);
                if (!string.IsNullOrEmpty(subSummary))
                {
                    sb.AppendLine(subSummary);
                    sb.AppendLine();
                }

                var subDocs = metadataService.GetClassDocumentation(sub.ComponentType);
                AppendDocSection(sb, "Use", subDocs?.Use);
                AppendDocSection(sb, "Avoid", subDocs?.Avoid);

                var subParams = metadataService.GetParameters(sub.ComponentType);
                AppendParametersTable(sb, subParams);

                var subMethods = metadataService.GetPublicMethods(sub.ComponentType);
                AppendMethodsTable(sb, subMethods);

                // Sub-component demos
                if (sub.Demos is { Count: > 0 })
                {
                    AppendDemos(sb, sub.Demos);
                }

                AppendDocSection(sb, "Guidelines", subDocs?.Guidelines);
                AppendDocSection(sb, "Remarks", subDocs?.Remarks);
            }
        }

        // Examples (demos beyond the primary one)
        if (component.Demos.Count > 0)
        {
            sb.AppendLine("## Examples");
            sb.AppendLine();
            AppendDemos(sb, component.Demos);
        }

        // Guidelines / Remarks
        AppendDocSection(sb, "Guidelines", component.Documentation?.Guidelines);
        AppendDocSection(sb, "Remarks", component.Documentation?.Remarks);

        // Accessibility
        if (component.AccessibilityLink is not null)
        {
            sb.AppendLine("## Accessibility");
            sb.AppendLine();
            sb.AppendLine("All Hviktor components are built with **WCAG 2.1 AA** as a minimum standard.");
            sb.AppendLine();
            sb.AppendLine($"[Read about accessibility for {component.Title} on designsystemet.no]({component.AccessibilityLink})");
            sb.AppendLine();
        }

        // Footer
        sb.AppendLine($"*Category: {groupTitle}*");

        if (component.LastUpdated != default)
        {
            sb.AppendLine();
            sb.AppendLine($"*Last updated: {component.LastUpdated:yyyy-MM-dd}*");
        }

        sb.AppendLine();
        return sb.ToString();
    }

    /// <summary>Appends status badges for deprecated, experimental, or validated components.</summary>
    private static void AppendStatusBadges(StringBuilder sb, ComponentInfo component)
    {
        var badges = new List<string[]>();

        if (component.IsDeprecated)
        {
            badges.Add(["CAUTION", "Deprecated - This component will be removed in a future version."]);
        }

        if (component.IsExperimental)
        {
            badges.Add(["WARNING", "Experimental - Under active development, may change without notice."]);
        }

        if (component.IsValidated)
        {
            badges.Add(["NOTE", ":white_check_mark: **Validated**"]);
        }

        if (badges.Count > 0)
        {
            foreach (var badge in badges)
            {
                if (badge.Length > 1)
                {
                    sb.AppendLine($"> [!{badge[0]}]");
                    foreach (var s in badge.Skip(1))
                    {
                        sb.AppendLine($"> {s}");
                    }
                }
                else
                {
                    sb.AppendLine($"> {badge[0]}");
                }
            }

            sb.AppendLine();
        }
    }

    /// <summary>Appends a named documentation section if content is present.</summary>
    private static void AppendDocSection(StringBuilder sb, string heading, string? content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return;
        }

        sb.AppendLine($"### {heading}");
        sb.AppendLine();
        sb.AppendLine(content);
        sb.AppendLine();
    }

    /// <summary>Appends a markdown parameters table.</summary>
    private static void AppendParametersTable(StringBuilder sb, IReadOnlyList<ParameterInfo> parameters)
    {
        if (parameters.Count == 0)
        {
            return;
        }

        sb.AppendLine("### Parameters");
        sb.AppendLine();
        sb.AppendLine("| Parameter | Type | Default | Required | Description |");
        sb.AppendLine("|-----------|------|---------|----------|-------------|");

        foreach (var p in parameters)
        {
            var required = p.IsRequired ? "Yes" : "No";
            var defaultVal = EscapePipe(StripHtml(p.DefaultValue ?? "-"));
            var desc = EscapePipe(StripHtml(p.XmlDocSummary ?? ""));
            var type = EscapePipe($"`{p.TypeName}`");
            var allowed = p.AllowedValues.Count > 0
                ? $" Allowed: {string.Join(", ", p.AllowedValues.Select(v => $"`{v}`"))}"
                : "";

            sb.AppendLine($"| `{p.Name}` | {type} | {defaultVal} | {required} | {desc}{allowed} |");
        }

        sb.AppendLine();
    }

    /// <summary>Appends a markdown methods table.</summary>
    private static void AppendMethodsTable(StringBuilder sb, IReadOnlyList<ComponentMethodInfo> methods)
    {
        if (methods.Count == 0)
        {
            return;
        }

        sb.AppendLine("### Methods");
        sb.AppendLine();
        sb.AppendLine("| Method | Returns | Arguments | Description |");
        sb.AppendLine("|--------|---------|-----------|-------------|");

        foreach (var m in methods)
        {
            var desc = EscapePipe(StripHtml(m.XmlDocSummary ?? ""));
            var args = string.IsNullOrEmpty(m.ParameterSignature) ? "-" : EscapePipe($"`{m.ParameterSignature}`");
            sb.AppendLine($"| `{m.Name}` | `{m.ReturnType}` | {args} | {desc} |");
        }

        sb.AppendLine();
    }

    /// <summary>Appends demo code blocks for a list of demos.</summary>
    private void AppendDemos(StringBuilder sb, IReadOnlyList<DemoInfo> demos)
    {
        foreach (var demo in demos)
        {
            var title = GetDemoTitle(demo);
            if (!string.IsNullOrEmpty(title))
            {
                sb.AppendLine($"#### {title}");
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(demo.Description))
            {
                sb.AppendLine(demo.Description);
                sb.AppendLine();
            }

            try
            {
                var source = demoSourceService.GetDemoSource(demo.ResourceKey);
                sb.AppendLine("```razor");
                sb.AppendLine(source);
                sb.AppendLine("```");
                sb.AppendLine();
            }
            catch (InvalidOperationException)
            {
                LogDemoSourceNotFound(demo.DemoType.Name);
            }
        }
    }

    /// <summary>Returns a human-friendly display title for a demo.</summary>
    private static string GetDemoTitle(DemoInfo demo)
    {
        if (demo.Title is not null)
        {
            return demo.Title;
        }

        var name = demo.DemoType.Name;
        if (name.EndsWith("Demo", StringComparison.OrdinalIgnoreCase))
        {
            name = name[..^4];
        }

        return UppercaseRegex().Replace(name, " $1").Trim();
    }

    /// <summary>Strips HTML tags from a string for plain-text markdown output.</summary>
    private static string StripHtml(string html)
    {
        if (string.IsNullOrEmpty(html))
        {
            return html;
        }

        return HtmlTagRegex().Replace(html, "").Trim();
    }

    /// <summary>Escapes pipe characters for use inside markdown table cells.</summary>
    private static string EscapePipe(string text) => text.Replace("|", "\\|");

    [GeneratedRegex("<[^>]+>")]
    private static partial Regex HtmlTagRegex();

    [GeneratedRegex("([A-Z])")]
    private static partial Regex UppercaseRegex();

    [LoggerMessage(Level = LogLevel.Warning, Message = "Demo source not found for {DemoType}")]
    private partial void LogDemoSourceNotFound(string demoType);
}