using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Pagination;

/// <summary>
/// WCAG accessibility tests for the Pagination component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class PaginationComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Pagination_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/pagination/basic", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Variants>]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Pagination_Variants_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/pagination/variants", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Pagination_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/pagination/accessibility", testId, Tags.Wcag2a3);
    }
}