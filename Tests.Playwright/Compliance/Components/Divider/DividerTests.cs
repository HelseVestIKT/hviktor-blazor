using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Divider;

/// <summary>
/// WCAG accessibility tests for the Divider component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class DividerComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Divider")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Divider_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/divider/basic", testId);
    }

    [Theory]
    [ClassData<TestData.Sections>]
    [Trait(Traits.Component, "Divider")]
    [Trait(Traits.Category, Categories.Structure)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Divider_Sections_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/divider/sections", testId);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Divider")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Divider_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/divider/accessibility", testId);
    }
}
