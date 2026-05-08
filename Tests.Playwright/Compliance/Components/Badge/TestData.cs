namespace Tests.Playwright.Compliance.Components.Badge;

public static class TestData
{
    private static readonly Hviktor.Abstractions.Enums.Attributes.Variant[] AllowedVariants =
    [
        Hviktor.Abstractions.Enums.Attributes.Variant.Base,
        Hviktor.Abstractions.Enums.Attributes.Variant.Tinted,
    ];

    private static readonly Hviktor.Abstractions.Enums.Attributes.Placement[] AllowedPlacements =
    [
        Hviktor.Abstractions.Enums.Attributes.Placement.TopEnd,
        Hviktor.Abstractions.Enums.Attributes.Placement.TopStart,
        Hviktor.Abstractions.Enums.Attributes.Placement.BottomEnd,
        Hviktor.Abstractions.Enums.Attributes.Placement.BottomStart,
    ];

    private static readonly Hviktor.Abstractions.Enums.Attributes.Variant[] AllowedOverlaps =
    [
        Hviktor.Abstractions.Enums.Attributes.Variant.Circle,
        Hviktor.Abstractions.Enums.Attributes.Variant.Rectangle,
    ];

    public sealed class Variant : TheoryData<Hviktor.Abstractions.Enums.Attributes.Variant>
    {
        public Variant() => AddRange(AllowedVariants);
    }

    public sealed class Placement : TheoryData<Hviktor.Abstractions.Enums.Attributes.Placement>
    {
        public Placement() => AddRange(AllowedPlacements);
    }

    public sealed class Overlap : TheoryData<Hviktor.Abstractions.Enums.Attributes.Variant>
    {
        public Overlap() => AddRange(AllowedOverlaps);
    }

    public sealed class Count : TheoryData<string>
    {
        public Count()
        {
            Add("with-count");
            Add("with-max-count");
            Add("without-count");
        }
    }
}
