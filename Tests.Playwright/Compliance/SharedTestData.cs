using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Playwright.Compliance;

/// <summary>
/// Shared <see cref="TheoryData{T}"/> definitions for commonly reused enum value sets
/// across compliance tests. Components with restricted subsets should define their own
/// local <c>TestData</c> classes instead.
/// </summary>
public static class SharedTestData
{
    /// <summary>
    /// All standard colors supported by most components.
    /// </summary>
    public static readonly Color[] AllColors =
    [
        Color.Accent,
        Color.Neutral,
        Color.Brand1,
        Color.Brand2,
        Color.Brand3,
        Color.Success,
        Color.Danger,
        Color.Info,
        Color.Warning,
    ];

    /// <summary>
    /// All standard sizes (sm, md, lg).
    /// </summary>
    public static readonly Size[] StandardSizes =
    [
        Size.Small,
        Size.Medium,
        Size.Large,
    ];

    /// <summary>
    /// All extended sizes including extra-small and extra-large variants.
    /// </summary>
    public static readonly Size[] AllSizes =
    [
        Size.ExtraExtraSmall,
        Size.ExtraSmall,
        Size.Small,
        Size.Medium,
        Size.Large,
        Size.ExtraLarge,
        Size.ExtraExtraLarge,
    ];

    /// <summary>
    /// Standard button/component variants: primary, secondary, tertiary.
    /// </summary>
    public static readonly Variant[] StandardVariants =
    [
        Variant.Primary,
        Variant.Secondary,
        Variant.Tertiary,
    ];

    /// <summary>Theory data for all standard colors.</summary>
    public sealed class AllColorsData : TheoryData<Color>
    {
        /// <inheritdoc />
        public AllColorsData() => AddRange(AllColors);
    }

    /// <summary>Theory data for standard sizes (sm, md, lg).</summary>
    public sealed class StandardSizesData : TheoryData<Size>
    {
        /// <inheritdoc />
        public StandardSizesData() => AddRange(StandardSizes);
    }

    /// <summary>Theory data for all extended sizes.</summary>
    public sealed class AllSizesData : TheoryData<Size>
    {
        /// <inheritdoc />
        public AllSizesData() => AddRange(AllSizes);
    }

    /// <summary>Theory data for standard variants (primary, secondary, tertiary).</summary>
    public sealed class StandardVariantsData : TheoryData<Variant>
    {
        /// <inheritdoc />
        public StandardVariantsData() => AddRange(StandardVariants);
    }
}

