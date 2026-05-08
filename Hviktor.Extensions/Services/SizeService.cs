using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Extension methods for <see cref="Size"/> enum.
/// </summary>
public class SizeService : ISizeService
{
    /// <summary>
    /// Converts a <see cref="Size"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="size">The size enum value.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Size size) => GetDataAttribute(size, Size.Medium);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Size> value) => GetDataAttribute(value, Size.Medium);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Size> value, Size defaultValue)
        => value.IsRaw
            ? GetDataAttribute(GetFromString(value.RawValue!, defaultValue), defaultValue)
            : GetDataAttribute(value.EnumValueOrNull ?? defaultValue, defaultValue);

    /// <summary>
    /// Converts a <see cref="Size"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="size">The size enum value.</param>
    /// <param name="defaultValue">The default size to use if the provided size is not allowed. Defaults to <see cref="Size.Medium"/>.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Size? size, Size defaultValue)
    {
        var sizeToCheck = size ?? defaultValue;
        return sizeToCheck switch
        {
            Size.ExtraExtraSmall => "2xs",
            Size.ExtraSmall => "xs",
            Size.Small => "sm",
            Size.Medium => "md",
            Size.Large => "lg",
            Size.ExtraLarge => "xl",
            Size.ExtraExtraLarge => "2xl",
            _ => ""
        };
    }

    /// <summary>
    /// Parses a string to a <see cref="Size"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Size GetFromString(string value) => GetFromString(value, Size.Medium);

    /// <summary>
    /// Parses a string to a <see cref="Size"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public Size GetFromString(string value, Size defaultValue)
    {
        if (Enum.TryParse<Size>(value, true, out var parsedValue))
        {
            return parsedValue;
        }

        var fromShortForm = value.Trim().ToLowerInvariant() switch
        {
            "2xs" => Size.ExtraExtraSmall,
            "xs" => Size.ExtraSmall,
            "sm" => Size.Small,
            "md" => Size.Medium,
            "lg" => Size.Large,
            "xl" => Size.ExtraLarge,
            "2xl" => Size.ExtraExtraLarge,
            _ => (Size?)null
        };

        return fromShortForm ?? defaultValue;
    }
}