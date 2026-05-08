using System.Diagnostics.CodeAnalysis;

namespace Documentation.Components.Services;

/// <summary>
/// Describes a single demo section for a documented component.
/// </summary>
/// <param name="DemoType">The CLR type of the demo Razor component (used by <c>DynamicComponent</c>).</param>
/// <param name="Title">Optional display title. When <see langword="null"/>, a title is auto-generated from the type name.</param>
/// <param name="Description">Optional description.</param>
public sealed record DemoInfo(
    [property: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    Type DemoType,
    string? Title = null,
    string? Description = null,
    string? Display = null,
    bool IsNotSupported = false,
    bool IsAiGenerated = false)
{
    /// <summary>
    /// Gets the resource key used to look up embedded demo source code.
    /// Derived from the type's full name by stripping the <c>Documentation.Components.Demos.</c> prefix.
    /// </summary>
    public string ResourceKey { get; } = DemoType.FullName!.Replace("Documentation.Components.Demos.", "");

    /// <summary>
    /// Gets a URL-safe section ID derived from the resource key.
    /// </summary>
    public string SectionId { get; } = DemoType.FullName!
        .Replace("Documentation.Components.Demos.", "")
        .Replace('.', '-')
        .ToLowerInvariant();
}