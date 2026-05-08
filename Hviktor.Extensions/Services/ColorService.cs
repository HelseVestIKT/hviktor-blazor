using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Extension methods for <see cref="Color"/> enum.
/// </summary>
public class ColorService : IColorService
{
    /// <summary>
    /// Converts a <see cref="Color"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="color">The color enum value.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Color color) => GetDataAttribute(color, Color.Accent);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Color> value) => GetDataAttribute(value, Color.Accent);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Color> value, Color defaultValue)
        => value.IsRaw
            ? GetDataAttribute(GetFromString(value.RawValue!, defaultValue), defaultValue)
            : GetDataAttribute(value.EnumValueOrNull ?? defaultValue, defaultValue);

    /// <summary>
    /// Converts a <see cref="Color"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="color">The color enum value.</param>
    /// <param name="defaultValue">The default color to use if the provided color is not allowed. Defaults to <see cref="Color.Accent"/>.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Color? color, Color defaultValue)
        => (color ?? defaultValue).ToString().ToLowerInvariant();

    /// <summary>
    /// Parses a string to a <see cref="Color"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Color GetFromString(string value) => GetFromString(value, Color.Accent);

    /// <summary>
    /// Parses a string to a <see cref="Color"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public Color GetFromString(string value, Color defaultValue)
        => Enum.TryParse<Color>(value, true, out var parsedValue)
            ? parsedValue
            : defaultValue;
}