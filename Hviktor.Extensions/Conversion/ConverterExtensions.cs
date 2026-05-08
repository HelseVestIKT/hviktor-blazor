using System.Globalization;

namespace Hviktor.Extensions.Conversion;

/// <h3>Converter</h3>
/// <summary>
/// The <b>Converter</b> class provides static methods to convert objects to various types such as <b>int</b>, <b>long</b>, <b>double</b>, and <b>decimal</b>.<br/>
/// It includes methods to handle null values and exceptions, providing fallback values when conversions fail.<br/>
/// </summary>
public static class Converter<T>
{
    /// <summary>
    /// Converts an object to the specified type <b>T</b>.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <returns>The converted value of type <b>T</b>.</returns>
    public static T ToValue(object value)
    {
        return (T)value;
    }

    /// <summary>
    /// Converts an object to a string.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <returns>The converted string value.</returns>
    public static string ToString(object? value)
    {
        return ToString(value, string.Empty);
    }

    /// <summary>
    /// Converts an object to a string with a fallback value.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <param name="fallback">The fallback value if the conversion fails.</param>
    /// <returns>The converted string value or the fallback value.</returns>
    private static string ToString(object? value, string fallback)
    {
        return value is null ? fallback : value.ToString() ?? fallback;
    }

    /// <summary>
    /// Converts an object to an integer.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <returns>The converted integer value.</returns>
    public static int ToInt(object? value)
    {
        return ToInt(value, 0);
    }

    /// <summary>
    /// Converts an object to an integer with a fallback value.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <param name="fallback">The fallback value if the conversion fails.</param>
    /// <returns>The converted integer value or the fallback value.</returns>
    public static int ToInt(object? value, int fallback)
    {
        try
        {
            return value is null ? fallback : Convert.ToInt32(value, CultureInfo.CurrentCulture);
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            return fallback;
        }
    }

    /// <summary>
    /// Converts an object to a long integer.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <returns>The converted long integer value.</returns>
    public static long ToLong(object? value)
    {
        return ToLong(value, 0L);
    }

    /// <summary>
    /// Converts an object to a long integer with a fallback value.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <param name="fallback">The fallback value if the conversion fails.</param>
    /// <returns>The converted long integer value or the fallback value.</returns>
    public static long ToLong(object? value, long fallback)
    {
        try
        {
            return value is null ? fallback : Convert.ToInt64(value, CultureInfo.CurrentCulture);
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            return fallback;
        }
    }

    /// <summary>
    /// Converts an object to a double.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <returns>The converted double value.</returns>
    public static double ToDouble(object? value)
    {
        return ToDouble(value, 0D);
    }

    /// <summary>
    /// Converts an object to a double with a fallback value.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <param name="fallback">The fallback value if the conversion fails.</param>
    /// <returns>The converted double value or the fallback value.</returns>
    public static double ToDouble(object? value, double fallback)
    {
        try
        {
            return value is null ? fallback : Convert.ToDouble(value, CultureInfo.CurrentCulture);
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            return fallback;
        }
    }

    /// <summary>
    /// Converts an object to a decimal.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <returns>The converted decimal value.</returns>
    public static decimal ToDecimal(object? value)
    {
        return ToDecimal(value, 0M);
    }

    /// <summary>
    /// Converts an object to a decimal with a fallback value.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <param name="fallback">The fallback value if the conversion fails.</param>
    /// <returns>The converted decimal value or the fallback value.</returns>
    public static decimal ToDecimal(object? value, decimal fallback)
    {
        try
        {
            return value is null ? fallback : Convert.ToDecimal(value, CultureInfo.CurrentCulture);
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            return fallback;
        }
    }

    /// <summary>
    /// Converts an object to a boolean value.
    /// </summary>
    /// <param name="value"> The object to convert.</param>
    /// <param name="fallback"> The fallback value if the conversion fails or if the value is null.</param>
    /// <returns> The converted boolean value or the fallback value if the conversion fails or if the value is null.</returns>
    public static bool ToBool(object? value, bool fallback = false)
    {
        try
        {
            return value is null ? fallback : Convert.ToBoolean(value, CultureInfo.CurrentCulture);
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            return fallback;
        }
    }

    /// <summary>
    /// Converts an object to a DateTime value.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <returns>The converted DateTime value.</returns>
    public static DateTime ToDateTime(object? value)
    {
        return ToDateTime(value, DateTime.Now);
    }

    /// <summary>
    /// Converts an object to a DateTime value with a fallback value.
    /// </summary>
    /// <param name="value">The object to convert.</param>
    /// <param name="fallback">The fallback value if the conversion fails.</param>
    /// <returns>The converted DateTime value or the fallback value.</returns>
    public static DateTime ToDateTime(object? value, DateTime fallback)
    {
        try
        {
            return DateTime.TryParse(value?.ToString(), CultureInfo.CurrentCulture, out var dateTime)
                ? dateTime
                : fallback;
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException or ArgumentOutOfRangeException)
        {
            return fallback;
        }
    }
}