using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Extension methods for <see cref="Variant"/> enum.
/// </summary>
public class VariantService : IVariantService
{
    /// <summary>
    /// Converts a <see cref="Variant"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="variant">The variant enum value.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Variant variant) => GetDataAttribute(variant, Variant.Default);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Variant> value)
        => GetDataAttribute(value, Variant.Default);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Variant> value, Variant defaultValue)
        => value.IsRaw
            ? GetDataAttribute(GetFromString(value.RawValue!, defaultValue), defaultValue)
            : GetDataAttribute(value.EnumValueOrNull ?? defaultValue, defaultValue);

    /// <summary>
    /// Converts a <see cref="Variant"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="variant">The variant enum value.</param>
    /// <param name="defaultValue">The default variant to use if the provided variant is not allowed. Defaults to <see cref="Variant.Default"/>.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Variant? variant, Variant defaultValue)
        => (variant ?? defaultValue).ToString().ToLowerInvariant();

    /// <summary>
    /// Parses a string to a <see cref="Variant"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value">The string representation of the variant.</param>
    /// <returns>The parsed variant enum value.</returns>
    public Variant GetFromString(string value) => GetFromString(value, Variant.Default);

    /// <summary>
    /// Parses a string to a <see cref="Variant"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value">The string representation of the variant.</param>
    /// <param name="defaultValue">The default variant to use if the provided variant is not allowed. Defaults to <see cref="Variant.Default"/>.</param>
    /// <returns>The parsed variant enum value.</returns>
    public Variant GetFromString(string value, Variant defaultValue)
        => Enum.TryParse<Variant>(value, true, out var parsedValue)
            ? parsedValue
            : defaultValue;
}