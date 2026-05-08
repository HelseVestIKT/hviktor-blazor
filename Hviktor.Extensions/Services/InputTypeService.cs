using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Provides extension methods for the InputType enum to generate CSS classes.
/// </summary>
public class InputTypeService : IInputTypeService
{
    /// <summary>
    /// Converts a <see cref="InputType"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="type">The type enum value.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(InputType type) => GetDataAttribute(type, InputType.DateTimeLocal);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<InputType> value) => GetDataAttribute(value, InputType.DateTimeLocal);

    /// <inheritdoc/>
    public string GetDataAttribute(EnumValue<InputType> value, InputType defaultValue)
        => value.IsRaw
            ? GetDataAttribute(GetFromString(value.RawValue!, defaultValue), defaultValue)
            : GetDataAttribute(value.EnumValueOrNull ?? defaultValue, defaultValue);

    /// <summary>
    /// Converts a <see cref="InputType"/> enum value to its corresponding data attribute value.
    /// </summary>
    /// <param name="type">The type enum value.</param>
    /// <param name="defaultValue">The default type to use if the provided type is not allowed. Defaults to <see cref="InputType.DateTimeLocal"/>.</param>
    /// <returns>The lowercase string representation for use in HTML data attributes.</returns>
    public static string GetDataAttribute(InputType type, InputType defaultValue)
    {
        return type == InputType.DateTimeLocal
            ? "datetime-local"
            : type.ToString().ToLower();
    }

    /// <summary>
    /// Parses a string to a <see cref="InputType"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public InputType GetFromString(string value) => GetFromString(value, InputType.DateTimeLocal);

    /// <summary>
    /// Parses a string to a <see cref="InputType"/> enum value, returning a default value if parsing fails or the value is not allowed.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public InputType GetFromString(string value, InputType defaultValue)
        => Enum.TryParse<InputType>(value, true, out var parsedValue)
            ? parsedValue
            : defaultValue;
}