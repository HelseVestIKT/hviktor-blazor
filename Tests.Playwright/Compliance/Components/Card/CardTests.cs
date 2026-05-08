using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Extensions.Services;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Card;

/// <summary>
/// WCAG accessibility tests for the Card component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class CardComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    #region Card Variant Tests

    [Theory]
    [ClassData<TestData.Variant>]
    [Trait(Traits.Component, "Card")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Card_Variant_WcagCompliant(Variant variant)
    {
        await AssertAllWcagLevelsAsync("/card/variant", variant, VariantService.GetDataAttribute);
    }

    #endregion

    #region Card Color Tests

    [Theory]
    [ClassData<TestData.Color>]
    [Trait(Traits.Component, "Card")]
    [Trait(Traits.Category, Categories.Color)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Card_Color_WcagCompliant(Color color)
    {
        await AssertAllWcagLevelsAsync("/card/color", color, ColorService.GetDataAttribute);
    }

    #endregion

    #region Card Size Tests

    [Theory]
    [ClassData<SharedTestData.StandardSizesData>]
    [Trait(Traits.Component, "Card")]
    [Trait(Traits.Category, Categories.SensoryAndVisualCues)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Card_Size_WcagCompliant(Size size)
    {
        await AssertAllWcagLevelsAsync("/card/size", size, SizeService.GetDataAttribute);
    }

    #endregion

    #region Card Structure Tests

    [Theory]
    [InlineData("with-block")]
    [InlineData("with-heading")]
    [InlineData("with-divider")]
    [InlineData("with-multiple-blocks")]
    [Trait(Traits.Component, "Card")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Category, Categories.Structure)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Card_Structure_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/card/structure", testId);
    }

    #endregion
}