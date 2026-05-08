namespace Tests.Playwright.Compliance.Components.AvatarStack;

/// <summary>
/// Test data for AvatarStack WCAG compliance tests.
/// </summary>
public static class TestData
{
    private static readonly string[] AllowedColors =
    [
        "accent",
        "neutral",
        "info",
        "success",
        "warning",
        "danger"
    ];

    private static readonly string[] AllowedSizes = ["sm", "md", "lg"];

    /// <summary>
    /// Enumerates all allowed color values for compliance testing.
    /// </summary>
    public sealed class ColorData : TheoryData<string>
    {
        public ColorData() => AddRange(AllowedColors);
    }

    /// <summary>
    /// Enumerates all allowed size values for compliance testing.
    /// </summary>
    public sealed class SizeData : TheoryData<string>
    {
        public SizeData() => AddRange(AllowedSizes);
    }
}