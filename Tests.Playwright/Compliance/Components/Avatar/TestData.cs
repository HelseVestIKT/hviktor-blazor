namespace Tests.Playwright.Compliance.Components.Avatar;

public static class TestData
{
    private static readonly Hviktor.Abstractions.Enums.Attributes.Variant[] AllowedVariants =
    [
        Hviktor.Abstractions.Enums.Attributes.Variant.Circle,
        Hviktor.Abstractions.Enums.Attributes.Variant.Square,
    ];

    private static readonly Hviktor.Abstractions.Enums.Attributes.Size[] AllowedSizes =
    [
        Hviktor.Abstractions.Enums.Attributes.Size.ExtraSmall,
        Hviktor.Abstractions.Enums.Attributes.Size.Small,
        Hviktor.Abstractions.Enums.Attributes.Size.Medium,
        Hviktor.Abstractions.Enums.Attributes.Size.Large,
    ];

    public sealed class Variant : TheoryData<Hviktor.Abstractions.Enums.Attributes.Variant>
    {
        public Variant() => AddRange(AllowedVariants);
    }

    public sealed class Size : TheoryData<Hviktor.Abstractions.Enums.Attributes.Size>
    {
        public Size() => AddRange(AllowedSizes);
    }

    public sealed class Initials : TheoryData<string>
    {
        public Initials()
        {
            Add("with-initials");
            Add("without-initials");
        }
    }

    public sealed class AsChild : TheoryData<string>
    {
        public AsChild()
        {
            Add("as-anchor");
            Add("as-link");
        }
    }

    /// <summary>
    /// AsChild variants that pass WCAG AAA (7:1 contrast).
    /// </summary>
    public sealed class AsChildAaa : TheoryData<string>
    {
        public AsChildAaa()
        {
            Add("as-anchor");
        }
    }
}