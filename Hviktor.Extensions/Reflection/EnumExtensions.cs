using System.ComponentModel;

namespace Hviktor.Extensions.Reflection;

/// <summary>
/// Provides extension methods for working with enums, including retrieving descriptions and enum values based on descriptions.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Gets the description of an enum value, if available. If no description is found, returns the enum value's name.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum? value)
    {
        if (value is null)
        {
            return "";
        }

        var field = value.GetType().GetField(value.ToString());
        if (field is null)
        {
            return value.ToString();
        }

        var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute?.Description ?? value.ToString();
    }

    /// <summary>
    /// Gets the enum value based on its description. If no matching description is found, returns default for the enum type.
    /// </summary>
    /// <param name="description"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T? GetEnumValue<T>(this string? description)
    {
        var type = typeof(T);
        if (!type.IsEnum)
        {
            Console.Error.WriteLine("Type must be an enum");
            return default;
        }

        foreach (var field in type.GetFields())
        {
            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute && attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
            {
                return field.GetValue(null) is T value ? value : default;
            }
        }

        Console.Error.WriteLine("Description not found");
        return default;
    }
}