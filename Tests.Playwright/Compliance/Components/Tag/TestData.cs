namespace Tests.Playwright.Compliance.Components.Tag;

public static class TestData
{
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

    private static readonly Hviktor.Abstractions.Enums.Attributes.Variant[] AllowedVariants =
    [
        Hviktor.Abstractions.Enums.Attributes.Variant.Default,
        Hviktor.Abstractions.Enums.Attributes.Variant.Outline
    ];

    private static readonly string[] AllowedSizes = ["sm", "md", "lg"];

    public sealed class VariantData : TheoryData<string>
    {
        public VariantData() => AddRange(AllowedVariants.Select(v => v.ToString().ToLowerInvariant()));
    }

    public sealed class ColorData : TheoryData<string>
    {
        public ColorData() => AddRange(AllowedColors.Select(c => c.ToString().ToLowerInvariant()));
    }

    public sealed class SizeData : TheoryData<string>
    {
        public SizeData() => AddRange(AllowedSizes);
    }

    public sealed class ListData : TheoryData<string>
    {
        public ListData()
        {
            Add("tag-list");
            Add("single-tag");
        }
    }
}