namespace Tests.Playwright.Compliance.Components.Skeleton;

public static class TestData
{
    private static readonly Hviktor.Abstractions.Enums.Attributes.Variant[] AllowedVariants =
    [
        Hviktor.Abstractions.Enums.Attributes.Variant.Rectangle,
        Hviktor.Abstractions.Enums.Attributes.Variant.Circle,
        Hviktor.Abstractions.Enums.Attributes.Variant.Text,
    ];

    public sealed class Variant : TheoryData<Hviktor.Abstractions.Enums.Attributes.Variant>
    {
        public Variant() => AddRange(AllowedVariants);
    }

    public sealed class Size : TheoryData<string>
    {
        public Size()
        {
            Add("size-small");
            Add("size-medium");
            Add("size-large");
            Add("custom-dimensions");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("aria-hidden");
            Add("multiple-skeletons");
            Add("with-context");
            Add("card-skeleton");
        }
    }
}
