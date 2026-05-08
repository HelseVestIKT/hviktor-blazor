using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Extension methods for <see cref="Weight"/> enum.
/// </summary>
public class WeightService : IWeightService
{
    /// <summary>
    /// Converts a <see cref="Weight"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="weight">The weight enum value.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Weight weight) => GetDataAttribute(weight, Weight.Regular);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Weight> value) => GetDataAttribute(value, Weight.Regular);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Weight> value, Weight defaultValue)
        => value.IsRaw
            ? GetDataAttribute(GetFromString(value.RawValue!, defaultValue), defaultValue)
            : GetDataAttribute(value.EnumValueOrNull ?? defaultValue, defaultValue);

    /// <summary>
    /// Converts a <see cref="Weight"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="weight">The weight enum value.</param>
    /// <param name="defaultValue">The default weight to use if the provided weight is not allowed. Defaults to <see cref="Weight.Regular"/>.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Weight? weight, Weight defaultValue)
        => (weight ?? defaultValue).ToString().ToLowerInvariant();

    /// <summary>
    /// Parses a string to a <see cref="Weight"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Weight GetFromString(string value) => GetFromString(value, Weight.Regular);

    /// <summary>
    /// Parses a string to a <see cref="Weight"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public Weight GetFromString(string value, Weight defaultValue)
        => Enum.TryParse<Weight>(value, true, out var parsedValue)
            ? parsedValue
            : defaultValue;
}