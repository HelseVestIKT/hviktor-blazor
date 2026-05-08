namespace Tests.Playwright.Compliance.Components.Card;

/// <summary>
/// Card-specific test data. Standard sizes come from <see cref="Tests.Playwright.Compliance.SharedTestData"/>.
/// Card has restricted color and variant subsets.
/// </summary>
public static class TestData
{
    /// <summary>Card supports only Default and Tinted variants.</summary>
    public sealed class Variant : TheoryData<Hviktor.Abstractions.Enums.Attributes.Variant>
    {
        public Variant()
        {
            Add(Hviktor.Abstractions.Enums.Attributes.Variant.Default);
            Add(Hviktor.Abstractions.Enums.Attributes.Variant.Tinted);
        }
    }

    /// <summary>Card supports only Accent and Neutral colors.</summary>
    public sealed class Color : TheoryData<Hviktor.Abstractions.Enums.Attributes.Color>
    {
        public Color()
        {
            Add(Hviktor.Abstractions.Enums.Attributes.Color.Accent);
            Add(Hviktor.Abstractions.Enums.Attributes.Color.Neutral);
        }
    }
}
