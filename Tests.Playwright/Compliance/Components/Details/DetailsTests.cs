using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Extensions.Services;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Details;

/// <summary>
/// WCAG accessibility tests for the Details component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class DetailsComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    #region Details State Tests

    [Theory]
    [ClassData<TestData.State>]
    [Trait(Traits.Component, "Details")]
    [Trait(Traits.Category, Categories.Forms)]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Details_State_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/details/state", testId);
    }

    #endregion

    #region Details Variant Tests

    [Theory]
    [ClassData<TestData.Variant>]
    [Trait(Traits.Component, "Details")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Details_Variant_WcagCompliant(Variant variant)
    {
        await AssertAllWcagLevelsAsync("/details/variant", variant, VariantService.GetDataAttribute);
    }

    #endregion

    #region Details Size Tests

    [Theory]
    [ClassData<TestData.Size>]
    [Trait(Traits.Component, "Details")]
    [Trait(Traits.Category, Categories.SensoryAndVisualCues)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Details_Size_WcagCompliant(Size size)
    {
        await AssertAllWcagLevelsAsync("/details/size", size, SizeService.GetDataAttribute);
    }

    #endregion

    #region Details Color Tests

    [Theory]
    [ClassData<SharedTestData.AllColorsData>]
    [Trait(Traits.Component, "Details")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Details_Color_WcagCompliant(Color color)
    {
        await AssertAllWcagLevelsAsync("/details/color", color, ColorService.GetDataAttribute);
    }

    #endregion

    #region Details Keyboard Navigation Tests

    [Theory]
    [ClassData<TestData.Keyboard>]
    [Trait(Traits.Component, "Details")]
    [Trait(Traits.Category, Categories.Keyboard)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Details_Keyboard_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/details/keyboard", testId);
    }

    #endregion
}