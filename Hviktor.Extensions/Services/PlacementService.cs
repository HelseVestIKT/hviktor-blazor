using System.Text.RegularExpressions;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Provides extension methods for the Placement enum to generate CSS classes.
/// </summary>
public class PlacementService : IPlacementService
{
    /// <summary>
    /// Converts a <see cref="Placement"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="placement">The placement enum value.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Placement placement) => GetDataAttribute(placement, Placement.BottomStart);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Placement> value) => GetDataAttribute(value, Placement.BottomStart);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Placement> value, Placement defaultValue)
        => value.IsRaw
            ? GetDataAttribute(GetFromString(value.RawValue!, defaultValue), defaultValue)
            : GetDataAttribute(value.EnumValueOrNull ?? defaultValue, defaultValue);

    /// <summary>
    /// Converts a <see cref="Placement"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="placement">The placement enum value.</param>
    /// <param name="defaultValue">The default placement to use if the provided placement is not allowed. Defaults to <see cref="Placement.BottomStart"/>.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Placement placement, Placement defaultValue)
    {
        // Split the enum name into parts based on uppercase letters
        var parts = Regex.Matches(placement.ToString(), @"[A-Z][a-z]*", RegexOptions.NonBacktracking)
            .Select(m => m.Value.ToLower())
            .ToArray();
        return string.Join("-", parts);
    }

    /// <summary>
    /// Parses a string to a <see cref="Placement"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Placement GetFromString(string value) => GetFromString(value, Placement.BottomStart);

    /// <summary>
    /// Parses a string to a <see cref="Placement"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public Placement GetFromString(string value, Placement defaultValue)
        => Enum.TryParse<Placement>(value, true, out var parsedValue)
            ? parsedValue
            : defaultValue;
}