namespace Documentation.Components.Services;

/// <summary>
/// A named group of components displayed as a section in the documentation sidebar and index.
/// </summary>
/// <param name="Title">The display title for this group (e.g. "Components", "Typography").</param>
/// <param name="Items">The components belonging to this group.</param>
public sealed record ComponentGroup(string Title, IReadOnlyList<ComponentInfo> Items);