using System.Text.RegularExpressions;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Provides extension methods for the Position enum to generate CSS classes.
/// </summary>
public class PositionService : IPositionService
{
    /// <summary>
    /// Converts a <see cref="Position"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="position">The position enum value.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Position position) => GetDataAttribute(position, Position.BottomLeft);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Position> value) => GetDataAttribute(value, Position.BottomLeft);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<Position> value, Position defaultValue)
        => value.IsRaw
            ? GetDataAttribute(GetFromString(value.RawValue!, defaultValue), defaultValue)
            : GetDataAttribute(value.EnumValueOrNull ?? defaultValue, defaultValue);

    /// <summary>
    /// Converts a <see cref="Position"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="position">The position enum value.</param>
    /// <param name="defaultValue">The default position to use if the provided position is not allowed. Defaults to <see cref="Position.BottomLeft"/>.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(Position position, Position defaultValue)
    {
        // Split the enum name into parts based on uppercase letters
        var parts = Regex.Matches(position.ToString(), @"[A-Z][a-z]*", RegexOptions.NonBacktracking)
            .Select(m => m.Value.ToLower())
            .ToArray();
        return string.Join("-", parts);
    }

    /// <summary>
    /// Parses a string to a <see cref="Position"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Position GetFromString(string value) => GetFromString(value, Position.BottomLeft);

    /// <summary>
    /// Parses a string to a <see cref="Position"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public Position GetFromString(string value, Position defaultValue)
        => Enum.TryParse<Position>(value, true, out var parsedValue)
            ? parsedValue
            : defaultValue;
}