using System.Diagnostics.CodeAnalysis;

namespace Documentation.Components.Services;

/// <summary>
/// Describes a component available in the documentation.
/// </summary>
/// <param name="Slug">The URL slug used in routing (e.g. "button").</param>
/// <param name="Title">The display title (e.g. "Button").</param>
/// <param name="LastUpdated">The date the component was last updated.</param>
/// <param name="ComponentType">The Blazor component type to inspect for parameters.</param>
/// <param name="Demos">One or more demo components to render and display source code for.</param>
/// <param name="DesignsystemetLink">Optional link to the Designsystemet documentation page.</param>
/// <param name="XmlDocSummary">Class-level XML doc summary extracted from the component, or <see langword="null"/>.</param>
/// <param name="SubComponents">
/// Optional list of sub-components (e.g. <c>Chip.Button</c>, <c>Chip.Checkbox</c>) whose parameters
/// and methods are displayed in dedicated sections on the same documentation page.
/// </param>
/// <param name="IsDeprecated">Whether the component is deprecated and will be removed in a future version.</param>
/// <param name="IsExperimental">Whether the component is experimental and subject to change.</param>
/// <param name="Documentation">
/// Full XML documentation extracted from the component class, including summary, remarks, use, avoid, and guidelines.
/// </param>
public sealed record ComponentInfo(
    string Slug,
    string Title,
    [property: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)]
    Type? ComponentType,
    IReadOnlyList<DemoInfo> Demos,
    string? DesignsystemetLink = null,
    string? XmlDocSummary = null,
    IReadOnlyList<SubComponentInfo>? SubComponents = null,
    bool IsDeprecated = false,
    bool IsExperimental = false,
    bool IsValidated = false,
    ClassDocumentation? Documentation = null,
    DateTime LastUpdated = default)
{
    /// <summary>
    /// Gets the accessibility link for this component on designsystemet.no,
    /// or <see langword="null"/> if no Designsystemet link is set.
    /// </summary>
    public string? AccessibilityLink => DesignsystemetLink?.Replace("/overview", "/accessibility");

    /// <summary>
    /// Gets the component description, falling back from <see cref="Documentation"/> summary
    /// to <see cref="XmlDocSummary"/>, then to a default message.
    /// </summary>
    public string Description => Documentation?.Summary ?? XmlDocSummary ?? "No description available.";

    public override string ToString() => $"{Title} ({Slug})";
}