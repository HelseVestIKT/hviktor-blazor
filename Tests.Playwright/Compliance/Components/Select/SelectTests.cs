using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Select;

/// <summary>
/// WCAG accessibility tests for the Select component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class SelectComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Select_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/select/basic", testId);
    }

    [Theory]
    [ClassData<TestData.State>]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Select_State_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/select/state", testId);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Select_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/select/accessibility", testId, Tags.Wcag2a3);
    }
}
