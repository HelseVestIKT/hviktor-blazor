namespace Hviktor.Icons.Abstractions.Types;

/// <summary>
/// Definition of an icon used in the Hviktor icon library.<br/>
/// Each definition stores the SVG path data extracted from the
/// <c>@helsevestikt/hviktor-icons</c> package.
/// </summary>
/// <remarks>
/// Icons are rendered as inline <c>&lt;svg&gt;</c> elements with the stored
/// <see cref="PathData"/> in a <c>&lt;path /&gt;</c> child element.
/// </remarks>
public sealed class IconDefinition
{
    /// <summary>
    /// The SVG path <c>d</c> attribute data for the icon.
    /// </summary>
    public string PathData { get; }

    /// <summary>
    /// Indicates whether the icon has a valid value.
    /// </summary>
    public bool HasValue => !string.IsNullOrWhiteSpace(PathData);

    /// <summary>
    /// Creates a new <see cref="IconDefinition"/> with an identifier and SVG path data.
    /// </summary>
    /// <param name="pathData">
    /// The SVG path <c>d</c> attribute value extracted from the icon source.
    /// </param>
    public IconDefinition(string pathData)
    {
        PathData = pathData;
    }
}