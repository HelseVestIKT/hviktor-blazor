using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Extension methods for <see cref="Width"/> enum.
/// </summary>
public class WidthService : IWidthService
{
    /// <summary>
    /// Converts a <see cref="Width"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="width">The width enum value.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Width width) => GetDataAttribute(width, Width.Full);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Width> value) => GetDataAttribute(value, Width.Full);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Width> value, Width defaultValue)
        => value.IsRaw
            ? GetDataAttribute(GetFromString(value.RawValue!, defaultValue), defaultValue)
            : GetDataAttribute(value.EnumValueOrNull ?? defaultValue, defaultValue);

    /// <summary>
    /// Converts a <see cref="Width"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="width">The width enum value.</param>
    /// <param name="defaultValue">The default width to use if the provided width is not allowed. Defaults to <see cref="Width.Full"/>.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Width? width, Width defaultValue)
        => (width ?? defaultValue).ToString().ToLowerInvariant();

    /// <summary>
    /// Parses a string to a <see cref="Width"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Width GetFromString(string value) => GetFromString(value, Width.Full);

    /// <summary>
    /// Parses a string to a <see cref="Width"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public Width GetFromString(string value, Width defaultValue)
        => Enum.TryParse<Width>(value, true, out var parsedValue)
            ? parsedValue
            : defaultValue;
}