using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Extensions.Services;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Breadcrumbs;

/// <summary>
/// WCAG accessibility tests for the Breadcrumbs component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class BreadcrumbsComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    #region Breadcrumbs Color Tests

    [Theory]
    [ClassData<TestData.Color>]
    [Trait(Traits.Component, "Breadcrumbs")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Breadcrumbs_Color_WcagCompliant(Color color)
    {
        await AssertAllWcagLevelsAsync("/breadcrumbs/color", color, ColorService.GetDataAttribute, Tags.Wcag2a3);
    }

    #endregion

    #region Breadcrumbs Size Tests

    [Theory]
    [ClassData<SharedTestData.StandardSizesData>]
    [Trait(Traits.Component, "Breadcrumbs")]
    [Trait(Traits.Category, Categories.SensoryAndVisualCues)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Breadcrumbs_Size_WcagCompliant(Size size)
    {
        await AssertAllWcagLevelsAsync("/breadcrumbs/size", size, SizeService.GetDataAttribute, Tags.Wcag2a3);
    }

    #endregion

    #region Breadcrumbs Structure Tests

    [Theory]
    [InlineData("default")]
    [InlineData("with-aria-current")]
    [Trait(Traits.Component, "Breadcrumbs")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Breadcrumbs_Structure_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/breadcrumbs/structure", testId, Tags.Wcag2a3);
    }

    #endregion
}