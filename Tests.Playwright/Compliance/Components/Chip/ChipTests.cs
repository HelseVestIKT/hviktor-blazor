using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Extensions.Services;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Chip;

/// <summary>
/// WCAG accessibility tests for the Chip component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at A, AA, AAA, and best practice levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class ChipComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    #region Chip Button Tests

    [Theory]
    [ClassData<TestData.ButtonType>]
    [Trait(Traits.Component, "Chip")]
    [Trait(Traits.Category, Categories.Forms)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ChipButton_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/chip/button", testId);
    }

    #endregion

    #region Chip Radio Tests

    [Theory]
    [ClassData<TestData.RadioState>]
    [Trait(Traits.Component, "Chip")]
    [Trait(Traits.Category, Categories.Forms)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ChipRadio_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/chip/radio", testId, Tags.Wcag2a3);
    }

    #endregion

    #region Chip Checkbox Tests

    [Theory]
    [ClassData<TestData.CheckboxState>]
    [Trait(Traits.Component, "Chip")]
    [Trait(Traits.Category, Categories.Forms)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ChipCheckbox_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/chip/checkbox", testId, Tags.Wcag2a3);
    }

    #endregion

    #region Chip Removable Tests

    [Theory]
    [ClassData<TestData.RemovableType>]
    [Trait(Traits.Component, "Chip")]
    [Trait(Traits.Category, Categories.Forms)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ChipRemovable_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/chip/removable", testId, Tags.Wcag2a3);
    }

    #endregion

    #region Chip Variant Tests

    [Theory]
    [ClassData<SharedTestData.StandardVariantsData>]
    [Trait(Traits.Component, "Chip")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ChipVariant_WcagCompliant(Variant variant)
    {
        await AssertAllWcagLevelsAsync("/chip/variant", variant, VariantService.GetDataAttribute);
    }

    #endregion

    #region Chip Size Tests

    [Theory]
    [ClassData<TestData.Size>]
    [Trait(Traits.Component, "Chip")]
    [Trait(Traits.Category, Categories.SensoryAndVisualCues)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ChipSize_WcagCompliant(Size size)
    {
        await AssertAllWcagLevelsAsync("/chip/size", size, SizeService.GetDataAttribute);
    }

    #endregion

    #region Chip Color Tests

    [Theory]
    [ClassData<SharedTestData.AllColorsData>]
    [Trait(Traits.Component, "Chip")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ChipColor_WcagCompliant(Color color)
    {
        await AssertAllWcagLevelsAsync("/chip/color", color, ColorService.GetDataAttribute);
    }

    #endregion
}