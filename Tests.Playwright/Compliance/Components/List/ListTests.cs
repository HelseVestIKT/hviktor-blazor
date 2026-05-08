using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.List;

/// <summary>
/// WCAG accessibility tests for the List component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class ListComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task List_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/list/basic", testId);
    }

    [Theory]
    [ClassData<TestData.Nested>]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Structure)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task List_Nested_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/list/nested", testId);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task List_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/list/accessibility", testId, Tags.Wcag2a3);
    }
}