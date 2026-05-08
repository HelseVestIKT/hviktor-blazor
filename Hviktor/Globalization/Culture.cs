using System.Globalization;

namespace Hviktor.Globalization;

/// <summary>
/// Provides culture-related constants and properties for date and time formatting.
/// </summary>
public static class Culture
{
    /// <summary>
    /// Gets the current culture information of the application.
    /// </summary>
    public static CultureInfo Provider => CultureInfo.CurrentCulture;

    /// <summary>
    /// The format for Calendar Date.
    /// </summary>
    public static string CalendarDateFormat => "yyyy-MM-dd";

    /// <summary>
    /// The format for Calendar Time.
    /// </summary>
    public static string CalendarTimeFormat => "HH:mm";

    /// <summary>
    /// The format for Calendar Date and Time.
    /// </summary>
    /// <remarks>
    /// This format combines the calendar <see cref="CalendarDateFormat"/> and <see cref="CalendarTimeFormat"/> formats into a single string.
    /// </remarks>
    public static string CalendarDateTimeFormat => $"{CalendarDateFormat}T{CalendarTimeFormat}";

    /// <summary>
    /// The format for Date.
    /// </summary>
    public static string DateFormat => "dd.MM.yyyy";

    /// <summary>
    /// The format for Time.
    /// </summary>
    public static string TimeFormat => "HH:mm:ss";

    /// <summary>
    /// The format for Date and Time.
    /// </summary>
    /// <remarks>
    /// This format combines the <see cref="DateFormat"/> and <see cref="TimeFormat"/> formats into a single string.
    /// </remarks>
    public static string DateTimeFormat => $"{DateFormat} {TimeFormat}";
}