using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Extensions.Services;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Button;

/// <summary>
/// WCAG accessibility tests for the Button component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class ButtonComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    #region Button Variant Tests

    [Theory]
    [ClassData<SharedTestData.StandardVariantsData>]
    [Trait(Traits.Component, "Button")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Button_Variant_WcagCompliant(Variant variant)
    {
        await AssertAllWcagLevelsAsync("/button/variant", variant, VariantService.GetDataAttribute, Tags.Wcag2a3);
    }

    #endregion

    #region Button Size Tests

    [Theory]
    [ClassData<SharedTestData.AllSizesData>]
    [Trait(Traits.Component, "Button")]
    [Trait(Traits.Category, Categories.SensoryAndVisualCues)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Button_Size_WcagCompliant(Size size)
    {
        await AssertAllWcagLevelsAsync("/button/size", size, SizeService.GetDataAttribute, Tags.Wcag2a3);
    }

    #endregion

    #region Button Color Tests

    [Theory]
    [ClassData<SharedTestData.AllColorsData>]
    [Trait(Traits.Component, "Button")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Button_Color_WcagCompliant(Color color)
    {
        await AssertAllWcagLevelsAsync("/button/color", color, ColorService.GetDataAttribute, Tags.Wcag2a3);
    }

    #endregion

    #region Button Loading Tests

    [Theory]
    [ClassData<TestData.Loading>]
    [Trait(Traits.Component, "Button")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.NameRoleValue)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Button_Loading_WcagCompliant(bool isLoading, Variant? variant)
    {
        var testId = GetLoadingTestId(isLoading, variant);
        await AssertAllWcagLevelsAsync("/button/loading", testId, Tags.Wcag2a3);
    }

    private static string GetLoadingTestId(bool isLoading, Variant? variant)
    {
        var variantAsString = variant is not null
            ? VariantService.GetDataAttribute(variant.Value)
            : "default";

        return $"{isLoading}-{variantAsString}".ToLower();
    }

    #endregion
}