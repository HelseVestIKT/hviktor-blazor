namespace Hviktor.Abstractions.Enums.Attributes;

/// <summary>
/// Represents the visual variant/style of a component.
/// </summary>
public enum Variant
{
    /// <summary>
    /// Default variant - standard appearance.
    /// </summary>
    Default,

    /// <summary>
    /// Base variant - minimal styling, neutral appearance.
    /// </summary>
    Base,

    /// <summary>
    /// Primary variant - filled background, high emphasis
    /// </summary>
    Primary,

    /// <summary>
    /// Secondary variant - outlined style, medium emphasis
    /// </summary>
    Secondary,

    /// <summary>
    /// Tertiary variant - text style, low emphasis
    /// </summary>
    Tertiary,

    /// <summary>
    /// Tinted variant - colored background, high emphasis
    /// </summary>
    Tinted,

    /// <summary>
    /// Long variant - extended length, more content
    /// </summary>
    /// <remarks>Used for Typography.</remarks>
    Long,

    /// <summary>
    /// Short variant - shortened length, less content
    /// </summary>
    /// <remarks>Used for Typography.</remarks>
    Short,

    /// <summary>
    /// Square variant - square shape, uniform dimensions
    /// </summary>
    /// <remarks>Used for avatars.</remarks>
    Square,

    /// <summary>
    /// Rectangle variant - rectangle shape, variable dimensions
    /// </summary>
    /// <remarks>Used for loaders.</remarks>
    Rectangle,

    /// <summary>
    /// Circle variant - circular shape, variable dimensions
    /// </summary>
    /// <remarks>Used for avatars and loaders.</remarks>
    Circle,

    /// <summary>
    /// Text variant - text-only style, no background
    /// </summary>
    /// <remarks>Used for loaders.</remarks>
    Text,
    
    /// <summary>
    /// Outline variant - outlined style
    /// </summary>
    /// <remarks>Used for tags.</remarks>
    Outline
}