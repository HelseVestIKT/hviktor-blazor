namespace Tests.Playwright.Compliance.Components.Link;

public static class TestData
{
    private static readonly Hviktor.Abstractions.Enums.Attributes.Size[] AllowedSizes =
    [
        Hviktor.Abstractions.Enums.Attributes.Size.Small,
        Hviktor.Abstractions.Enums.Attributes.Size.Medium,
        Hviktor.Abstractions.Enums.Attributes.Size.Large
    ];

    private static readonly Hviktor.Abstractions.Enums.Attributes.Color[] AllowedColors =
    [
        Hviktor.Abstractions.Enums.Attributes.Color.Accent,
        Hviktor.Abstractions.Enums.Attributes.Color.Neutral
    ];

    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic");
            Add("text-content");
            Add("inline-link");
        }
    }

    public sealed class Target : TheoryData<string>
    {
        public Target()
        {
            Add("target-self");
            Add("target-newtab");
            Add("external-secure");
        }
    }

    public sealed class Size : TheoryData<string>
    {
        public Size() => AddRange(AllowedSizes.Select(s => $"size-{s}".ToLower()));
    }

    public sealed class Color : TheoryData<string>
    {
        public Color() => AddRange(AllowedColors.Select(c => $"color-{c}".ToLower()));
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("accessible-text");
            Add("active-link");
            Add("keyboard-navigation");
            Add("focus-styling");
        }
    }
}