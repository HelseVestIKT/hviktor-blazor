namespace Tests.Playwright.Compliance.Components.Details;

/// <summary>
/// Details-specific test data. Standard colors come from <see cref="Tests.Playwright.Compliance.SharedTestData"/>.
/// Details has custom variants (Default, Tinted) and sizes (xs, sm, md, lg).
/// </summary>
public static class TestData
{
    /// <summary>Details supports Default and Tinted variants.</summary>
    public sealed class Variant : TheoryData<Hviktor.Abstractions.Enums.Attributes.Variant>
    {
        public Variant()
        {
            Add(Hviktor.Abstractions.Enums.Attributes.Variant.Default);
            Add(Hviktor.Abstractions.Enums.Attributes.Variant.Tinted);
        }
    }

    /// <summary>Details sizes include ExtraSmall.</summary>
    public sealed class Size : TheoryData<Hviktor.Abstractions.Enums.Attributes.Size>
    {
        public Size()
        {
            Add(Hviktor.Abstractions.Enums.Attributes.Size.ExtraSmall);
            Add(Hviktor.Abstractions.Enums.Attributes.Size.Small);
            Add(Hviktor.Abstractions.Enums.Attributes.Size.Medium);
            Add(Hviktor.Abstractions.Enums.Attributes.Size.Large);
        }
    }

    public sealed class State : TheoryData<string>
    {
        public State()
        {
            Add("details-closed");
            Add("details-open");
            Add("details-uncontrolled");
        }
    }

    public sealed class Keyboard : TheoryData<string>
    {
        public Keyboard()
        {
            Add("keyboard-navigation");
            Add("keyboard-first");
            Add("keyboard-second");
        }
    }
}