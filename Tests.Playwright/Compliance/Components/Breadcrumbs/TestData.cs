namespace Tests.Playwright.Compliance.Components.Breadcrumbs;

/// <summary>
/// Breadcrumbs-specific test data. Standard sizes come from <see cref="Tests.Playwright.Compliance.SharedTestData"/>.
/// Breadcrumbs supports only Accent and Neutral colors.
/// </summary>
public static class TestData
{
    /// <summary>Breadcrumbs supports only Accent and Neutral colors.</summary>
    public sealed class Color : TheoryData<Hviktor.Abstractions.Enums.Attributes.Color>
    {
        public Color()
        {
            Add(Hviktor.Abstractions.Enums.Attributes.Color.Accent);
            Add(Hviktor.Abstractions.Enums.Attributes.Color.Neutral);
        }
    }
}