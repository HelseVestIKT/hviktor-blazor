using System.Diagnostics.CodeAnalysis;

namespace Documentation.Components.Services;

/// <summary>
/// Describes a sub-component that belongs to a parent component page (e.g. <c>Chip.Button</c> on the Chip page).
/// </summary>
/// <param name="Title">Display title shown as a section heading (e.g. <c>"Chip.Button"</c>).</param>
/// <param name="ComponentType">The CLR type of the Blazor component to inspect for parameters and methods.</param>
/// <param name="Demos">
/// Demos for this sub-component. Follows the same convention as <see cref="ComponentInfo.Demos"/>:
/// the first demo is rendered in the Overview tab, all demos appear in the Code tab.
/// </param>
public sealed record SubComponentInfo(
    string Title,
    [property: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties | DynamicallyAccessedMemberTypes.PublicMethods)]
    Type ComponentType,
    IReadOnlyList<DemoInfo>? Demos = null);