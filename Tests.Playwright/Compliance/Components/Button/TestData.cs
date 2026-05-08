using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Playwright.Compliance.Components.Button;

/// <summary>
/// Button-specific test data. Standard Color, Size, and Variant data
/// comes from <see cref="Tests.Playwright.Compliance.SharedTestData"/>.
/// </summary>
public static class TestData
{
    /// <summary>
    /// Colors that pass WCAG AAA enhanced contrast (7:1).
    /// Accent, Brand1, Brand3, Danger, Info and Success are excluded because their
    /// Designsystemet primary-button tokens do not meet the 7:1 threshold.
    /// </summary>
    public sealed class ColorAaa : TheoryData<Color>
    {
        public ColorAaa()
        {
            Add(Color.Neutral);
            Add(Color.Brand2);
            Add(Color.Warning);
        }
    }

    /// <summary>Loading state combinations across all variants.</summary>
    public sealed class Loading : TheoryData<bool, Variant?>
    {
        public Loading()
        {
            Add(true, null);
            Add(false, null);

            foreach (var variant in SharedTestData.StandardVariants)
            {
                Add(true, variant);
                Add(false, variant);
            }
        }
    }
}