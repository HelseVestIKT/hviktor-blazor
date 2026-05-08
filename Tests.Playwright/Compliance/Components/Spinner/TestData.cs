namespace Tests.Playwright.Compliance.Components.Spinner;

public static class TestData
{
    private static readonly Hviktor.Abstractions.Enums.Attributes.Size[] AllowedSizes =
    [
        Hviktor.Abstractions.Enums.Attributes.Size.ExtraExtraSmall,
        Hviktor.Abstractions.Enums.Attributes.Size.ExtraSmall,
        Hviktor.Abstractions.Enums.Attributes.Size.Small,
        Hviktor.Abstractions.Enums.Attributes.Size.Medium,
        Hviktor.Abstractions.Enums.Attributes.Size.Large,
        Hviktor.Abstractions.Enums.Attributes.Size.ExtraLarge
    ];

    private static readonly Hviktor.Abstractions.Enums.Attributes.Color[] AllowedColors =
    [
        Hviktor.Abstractions.Enums.Attributes.Color.Accent,
        Hviktor.Abstractions.Enums.Attributes.Color.Neutral,
        Hviktor.Abstractions.Enums.Attributes.Color.Brand1,
        Hviktor.Abstractions.Enums.Attributes.Color.Brand2,
        Hviktor.Abstractions.Enums.Attributes.Color.Brand3,
        Hviktor.Abstractions.Enums.Attributes.Color.Success,
        Hviktor.Abstractions.Enums.Attributes.Color.Danger,
        Hviktor.Abstractions.Enums.Attributes.Color.Info,
        Hviktor.Abstractions.Enums.Attributes.Color.Warning,
    ];

    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic");
            Add("custom-label");
            Add("with-text");
        }
    }

    public sealed class Size : TheoryData<string>
    {
        public Size() => AddRange(AllowedSizes.Select(s => $"size-{s.ToString().ToLowerInvariant()}"));
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("aria-label");
            Add("aria-hidden");
            Add("status-region");
            Add("role-img");
            Add("multiple-spinners");
        }
    }
}