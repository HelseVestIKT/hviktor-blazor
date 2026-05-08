using System.Collections.Frozen;
using System.Collections.ObjectModel;
using Documentation.Components.Layout;
using Documentation.Components.Services;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Icons.Abstractions.Types;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ParameterInfo = Documentation.Components.Services.ParameterInfo;

namespace Documentation.Components.Pages;

/// <summary>
/// Per-component documentation page with Overview, Code, and Accessibility tabs
/// and a persistent sidebar listing all components.
/// </summary>
public sealed partial class ComponentPage : ComponentBase, IAsyncDisposable
{
    [Inject] private ComponentRegistry Registry { get; set; } = null!;
    [Inject] private IComponentMetadataService MetadataService { get; set; } = null!;
    [Inject] private IDemoSourceService DemoSourceService { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private IThemeService ThemeService { get; set; } = null!;
    [Inject] private IJsRuntimeService JsRuntimeService { get; set; } = null!;

    private const string Light = "light";
    private const string Dark = "dark";

    private const string Overview = "overview";
    private const string Code = "code";
    private const string Accessibility = "accessibility";

    /// <summary>Component URL slug from the route (e.g. "button").</summary>
    [Parameter]
    public string Slug { get; set; } = string.Empty;

    /// <summary>Active tab from the route: "overview", "code" or "accessibility".</summary>
    [Parameter]
    public string? Tab { get; set; } = Overview;

    private ComponentInfo? ComponentInfo { get; set; }
    private IReadOnlyList<ParameterInfo> Parameters { get; set; } = [];
    private IReadOnlyList<ComponentMethodInfo> Methods { get; set; } = [];

    /// <summary>
    /// Resolved parameters and methods for each sub-component declared in <see cref="ComponentInfo.SubComponents"/>.
    /// Keyed by the sub-component's display title.
    /// </summary>
    private IReadOnlyList<ResolvedSubComponent> SubComponents { get; set; } = [];

    /// <summary>Tracks which demo sections have their code block expanded (by <see cref="DemoInfo.SectionId"/>).</summary>
    private HashSet<string> ExpandedDemos { get; } = [];

    /// <summary>The global color scheme as reported by <see cref="IThemeService"/>.</summary>
    private string GlobalColorScheme => ThemeService.CurrentScheme;

    /// <summary>Per-demo color scheme overrides (keyed by <see cref="DemoInfo.SectionId"/>).</summary>
    private Dictionary<string, string> DemoColorSchemes { get; } = new(StringComparer.Ordinal);

    /// <summary>Tracks the previous slug to detect component changes and reset transient state.</summary>
    private string previousSlug = string.Empty;

    /// <summary>Whether metadata is still loading after a slug change.</summary>
    private bool isLoadingMetadata;

    /// <summary>Whether JS interop needs to run after the next render.</summary>
    private bool needsPopoverInit;

    /// <summary>Cached JS module reference for this component's collocated script.</summary>
    private IJSObjectReference? componentPageModule;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        ThemeService.OnSchemeChanged -= OnGlobalSchemeChanged;
        ThemeService.OnSchemeChanged += OnGlobalSchemeChanged;

        Tab ??= Overview;

        var slugChanged = !string.Equals(Slug, previousSlug, StringComparison.Ordinal);

        // Reset transient UI state when navigating to a different component
        if (slugChanged)
        {
            ExpandedDemos.Clear();
            DemoColorSchemes.Clear();
            previousSlug = Slug;

            ComponentInfo = Registry.Get(Slug);

            // Clear metadata so the page shell renders immediately
            Parameters = [];
            Methods = [];
            SubComponents = [];
            isLoadingMetadata = ComponentInfo is not null;
        }

        OutlineEntries = BuildOutlineEntries();
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        if (!isLoadingMetadata)
        {
            return;
        }

        isLoadingMetadata = false;

        // Yield to let the page shell render before doing heavy metadata work
        await Task.Yield();

        if (ComponentInfo?.ComponentType is { } t)
        {
            Parameters = MetadataService.GetParameters(t);
            Methods = MetadataService.GetPublicMethods(t);
        }

        if (ComponentInfo?.SubComponents is { Count: > 0 } subs)
        {
            SubComponents = subs
                .Select(sub =>
                {
                    var parameters = MetadataService.GetParameters(sub.ComponentType);
                    var methods = MetadataService.GetPublicMethods(sub.ComponentType);
                    var summary = MetadataService.GetClassSummary(sub.ComponentType);
                    var docs = MetadataService.GetClassDocumentation(sub.ComponentType);
                    var demos = sub.Demos ?? [];
                    return new ResolvedSubComponent(sub.Title, demos, parameters, methods, summary, docs);
                })
                .ToList()
                .AsReadOnly();
        }

        OutlineEntries = BuildOutlineEntries();
        needsPopoverInit = true;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Only initialize popovers when content changed (slug navigation)
        if (needsPopoverInit)
        {
            needsPopoverInit = false;
            try
            {
                componentPageModule ??= await JsRuntimeService.ImportAsync<ComponentPage>();
                await componentPageModule.InvokeVoidAsync("initializeSeeRefPopovers");
            }
            catch (JSDisconnectedException)
            {
                // Circuit disconnected during prerender or navigation
            }
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>Toggles the color scheme for a specific demo between light and dark.</summary>
    private void ToggleDemoColorScheme(DemoInfo demo)
    {
        if (DemoColorSchemes.TryGetValue(demo.SectionId, out var current))
        {
            DemoColorSchemes[demo.SectionId] = current == Light ? Dark : Light;
        }
        else
        {
            // First toggle: flip from the current global scheme
            DemoColorSchemes[demo.SectionId] = GlobalColorScheme == Light ? Dark : Light;
        }
    }

    /// <summary>Gets the manually set color scheme for a demo, or <see langword="null"/> to inherit the global scheme.</summary>
    private string? GetDemoColorScheme(DemoInfo demo) => DemoColorSchemes.GetValueOrDefault(demo.SectionId);

    /// <summary>Returns the effective color scheme for a demo, resolving the global fallback.</summary>
    private string GetEffectiveDemoColorScheme(DemoInfo demo) => GetDemoColorScheme(demo) ?? GlobalColorScheme;

    /// <summary>Returns the icon definition for the demo's current color scheme toggle button.</summary>
    private IconDefinition GetDemoColorSchemeIcon(DemoInfo demo) => GetEffectiveDemoColorScheme(demo) == Light ? DocumentationIconSet.Moon : DocumentationIconSet.Sun;

    /// <summary>Returns the aria-label for the demo's color scheme toggle button.</summary>
    private string GetDemoToggleAriaLabel(DemoInfo demo) => $"Toggle {(GetEffectiveDemoColorScheme(demo) == Light ? "dark" : "light")} mode";

    /// <summary>Toggles code visibility for a demo section.</summary>
    private void ToggleCode(DemoInfo demo)
    {
        if (!ExpandedDemos.Remove(demo.SectionId))
        {
            ExpandedDemos.Add(demo.SectionId);
        }
    }

    /// <summary>Returns whether the code block for the given demo is currently visible.</summary>
    private bool IsCodeVisible(DemoInfo demo) => ExpandedDemos.Contains(demo.SectionId);

    /// <summary>Returns a human-friendly display title for a demo.</summary>
    private static string GetDemoTitle(DemoInfo demo)
    {
        if (demo.Title is not null)
        {
            return demo.Title;
        }

        // Auto-generate from type name: "ButtonColorDemo" → "Button Color"
        var name = demo.DemoType.Name;
        if (name.EndsWith("Demo", StringComparison.OrdinalIgnoreCase))
        {
            name = name[..^4];
        }

        return UppercaseRegex().Replace(name, " $1").Trim();
    }

    /// <summary>Returns the demo source code for the given demo, or null on error.</summary>
    private string? GetDemoSource(DemoInfo demo)
    {
        try
        {
            return DemoSourceService.GetDemoSource(demo.ResourceKey);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    /// <summary>Valid tab values for the component documentation pages.</summary>
    private static readonly FrozenSet<string> ValidTabs = FrozenSet.ToFrozenSet([Overview, Accessibility, Code], StringComparer.OrdinalIgnoreCase);

    /// <summary>Navigates to the given tab while preserving the current slug.</summary>
    private void NavigateTo(string tab)
    {
        if (!ValidTabs.Contains(tab))
        {
            tab = Overview;
        }

        NavigationManager.NavigateTo($"/components/{Slug}/{tab}");
    }

    [System.Text.RegularExpressions.GeneratedRegex("([A-Z])")]
    private static partial System.Text.RegularExpressions.Regex UppercaseRegex();

    /// <summary>Converts a display title to a URL-safe slug for use as an HTML id attribute.</summary>
    private static string Slugify(string title) =>
        title.ToLowerInvariant().Replace(' ', '-').Replace(".", "-");

    /// <summary>
    /// Builds the outline entries for the right-side "on this page" navigation
    /// based on the active tab and component configuration.
    /// </summary>
    private IReadOnlyList<OnThisPage.OnThisPageEntry> OutlineEntries { get; set; } = [];

    /// <summary>Builds the outline entries for the current tab.</summary>
    private ReadOnlyCollection<OnThisPage.OnThisPageEntry> BuildOutlineEntries()
    {
        if (ComponentInfo is null)
        {
            return [];
        }

        var isAccessibilityTab = string.Equals(Tab, Accessibility, StringComparison.OrdinalIgnoreCase);
        if (isAccessibilityTab)
        {
            return new ReadOnlyCollection<OnThisPage.OnThisPageEntry>(
                new List<OnThisPage.OnThisPageEntry>
                {
                    new(Accessibility, "Accessibility")
                }
            );
        }

        var isCodeTab = string.Equals(Tab, Code, StringComparison.OrdinalIgnoreCase);
        var entries = new List<OnThisPage.OnThisPageEntry>
        {
            new(Slugify(ComponentInfo.Title), ComponentInfo.Title)
        };

        if (isCodeTab)
        {
            if (Parameters.Count > 0 || !string.IsNullOrEmpty(ComponentInfo.Documentation?.Parameters))
            {
                entries.Add(new OnThisPage.OnThisPageEntry("code-parameters", "Parameters", NestLevel: 1));
            }

            if (Methods.Count > 0)
            {
                entries.Add(new OnThisPage.OnThisPageEntry("code-methods", "Methods", NestLevel: 1));
            }
        }
        else
        {
            AddDocSectionEntry(entries, ComponentInfo.Documentation?.Use, "use", "Use", nestLevel: 1);
            AddDocSectionEntry(entries, ComponentInfo.Documentation?.Avoid, "avoid", "Avoid", nestLevel: 1);
        }

        BuildSubComponents(entries, isCodeTab);

        // Examples header and main demos (skip first)
        var hasExamples = ComponentInfo.Demos.Count > 1;
        if (hasExamples)
        {
            entries.Add(new OnThisPage.OnThisPageEntry(isCodeTab ? "code-examples" : "examples", "Examples"));
            AddDemoEntries(entries, ComponentInfo.Demos.Skip(1), isCodeTab);
        }

        if (!isCodeTab)
        {
            AddDocSectionEntry(entries, ComponentInfo.Documentation?.Guidelines, "guidelines", "Guidelines", 0);
            AddDocSectionEntry(entries, ComponentInfo.Documentation?.Remarks, "remarks", "Remarks", 0);
        }

        return entries.AsReadOnly();
    }

    private void BuildSubComponents(List<OnThisPage.OnThisPageEntry> entries, bool isCodeTab)
    {
        foreach (var sub in SubComponents)
        {
            var subSlug = isCodeTab ? $"code-{Slugify(sub.Title)}" : Slugify(sub.Title);
            if (!string.IsNullOrWhiteSpace(sub.Title))
            {
                entries.Add(new OnThisPage.OnThisPageEntry(subSlug, sub.Title));
            }

            if (isCodeTab)
            {
                if (sub.Demos.Count > 0)
                {
                    AddDemoEntries(entries, sub.Demos.Take(1), isCodeTab);
                }

                if (sub.Parameters.Count > 0 || !string.IsNullOrEmpty(sub.Documentation?.Parameters))
                {
                    entries.Add(new OnThisPage.OnThisPageEntry($"{subSlug}-parameters", "Parameters", NestLevel: 1));
                }

                if (sub.Methods.Count > 0)
                {
                    entries.Add(new OnThisPage.OnThisPageEntry($"{subSlug}-methods", "Methods", NestLevel: 1));
                }

                AddDemoEntries(entries, sub.Demos.Skip(1), isCodeTab);
            }
            else
            {
                AddDocSectionEntry(entries, sub.Documentation?.Use, $"{subSlug}-use", "Use");
                AddDocSectionEntry(entries, sub.Documentation?.Avoid, $"{subSlug}-avoid", "Avoid");
                AddDocSectionEntry(entries, sub.Documentation?.Guidelines, $"{subSlug}-guidelines", "Guidelines");
                AddDocSectionEntry(entries, sub.Documentation?.Remarks, $"{subSlug}-remarks", "Remarks");
            }
        }
    }

    /// <summary>Adds nested demo entries for each demo with a non-empty title.</summary>
    private static void AddDemoEntries(List<OnThisPage.OnThisPageEntry> entries, IEnumerable<DemoInfo> demos, bool prefixCode = false)
    {
        foreach (var demo in demos)
        {
            var title = GetDemoTitle(demo);
            if (!string.IsNullOrWhiteSpace(title))
            {
                var id = prefixCode ? $"code-{demo.SectionId}" : demo.SectionId;
                entries.Add(new OnThisPage.OnThisPageEntry(id, title, NestLevel: 1));
            }
        }
    }

    /// <summary>Adds an outline entry when the documentation content is not empty.</summary>
    private static void AddDocSectionEntry(List<OnThisPage.OnThisPageEntry> entries, string? content, string id, string label, int nestLevel = 1)
    {
        if (!string.IsNullOrEmpty(content))
        {
            entries.Add(new OnThisPage.OnThisPageEntry(id, label, NestLevel: nestLevel));
        }
    }

    /// <summary>
    /// Builds a single HTML table combining reflected parameters and any implicit parameters documentation.
    /// Delegates row rendering to <see cref="IComponentMetadataService.BuildParametersHtml"/> for consistent
    /// HTML output (popovers, encoding, tags). When implicit parameters are present, their
    /// <c>&lt;tbody&gt;</c> rows are extracted and appended to the reflected parameters table.
    /// Returns <see langword="null"/> when there are no parameters to display.
    /// </summary>
    private string? BuildParametersHtml(IReadOnlyList<ParameterInfo> parameters, string? implicitParametersHtml)
    {
        if (parameters.Count == 0 && string.IsNullOrEmpty(implicitParametersHtml))
        {
            return null;
        }

        // If only implicit parameters exist, return as-is
        if (parameters.Count == 0)
        {
            return implicitParametersHtml;
        }

        var baseTable = MetadataService.BuildParametersHtml(parameters);
        if (string.IsNullOrEmpty(baseTable))
        {
            return implicitParametersHtml;
        }

        // No implicit parameters to merge, return the base table
        if (string.IsNullOrEmpty(implicitParametersHtml))
        {
            return baseTable;
        }

        // Extract <tbody> rows from implicit parameters HTML and merge into the base table
        var implicitRows = ExtractTbodyContent(implicitParametersHtml);
        if (string.IsNullOrEmpty(implicitRows))
        {
            return baseTable;
        }

        // Insert implicit rows before the closing </tbody>
        const string tbodyClose = "</tbody>";
        var insertPos = baseTable.LastIndexOf(tbodyClose, StringComparison.OrdinalIgnoreCase);
        if (insertPos < 0)
        {
            return baseTable;
        }

        return string.Concat(baseTable.AsSpan(0, insertPos), implicitRows, tbodyClose, baseTable.AsSpan(insertPos + tbodyClose.Length));
    }

    private string? BuildMethodsHtml(IReadOnlyList<ComponentMethodInfo> methods) => methods.Count == 0 ? null : MetadataService.BuildMethodsHtml(methods);

    /// <summary>Extracts the content between <c>&lt;tbody&gt;</c> and <c>&lt;/tbody&gt;</c> from an HTML string.</summary>
    private static string? ExtractTbodyContent(string html)
    {
        const string tbodyOpen = "<tbody>";
        const string tbodyClose = "</tbody>";
        var start = html.IndexOf(tbodyOpen, StringComparison.OrdinalIgnoreCase);
        var end = html.IndexOf(tbodyClose, StringComparison.OrdinalIgnoreCase);
        if (start < 0 || end < 0 || end <= start)
        {
            return null;
        }

        return html[(start + tbodyOpen.Length)..end];
    }

    private static string GetCssDemoContainerDisplayClass(string? display)
    {
        var displayClass = display is not null ? $"__{display}" : "";
        return $"component-page__demo-container{displayClass}";
    }

    /// <summary>Resolved metadata for a single sub-component displayed on the parent component's page.</summary>
    /// <param name="Title">Display title, e.g. <c>"Chip.Button"</c>.</param>
    /// <param name="Demos">Demo components for this sub-component (first = overview, all = code tab).</param>
    /// <param name="Parameters">Reflected and merged parameters for the sub-component.</param>
    /// <param name="Methods">Public methods exposed by the sub-component.</param>
    /// <param name="XmlDocSummary">Class-level XML doc summary, or <see langword="null"/>.</param>
    /// <param name="Documentation">Full XML documentation for the sub-component, or <see langword="null"/>.</param>
    private sealed record ResolvedSubComponent(
        string Title,
        IReadOnlyList<DemoInfo> Demos,
        IReadOnlyList<ParameterInfo> Parameters,
        IReadOnlyList<ComponentMethodInfo> Methods,
        string? XmlDocSummary,
        ClassDocumentation? Documentation);

    /// <summary>Handles global theme changes by clearing per-demo overrides and re-rendering.</summary>
    private void OnGlobalSchemeChanged()
    {
        DemoColorSchemes.Clear();
        InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        ThemeService.OnSchemeChanged -= OnGlobalSchemeChanged;
        if (componentPageModule is not null)
        {
            try
            {
                await componentPageModule.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Circuit disconnected
            }
        }
    }
}