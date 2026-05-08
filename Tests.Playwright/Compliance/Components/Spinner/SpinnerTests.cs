using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Spinner;

/// <summary>
/// WCAG accessibility tests for the Spinner component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class SpinnerComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Spinner_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/spinner/basic", testId);
    }

    [Theory]
    [ClassData<TestData.Size>]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Spinner_Size_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/spinner/size", testId);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Spinner_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/spinner/accessibility", testId);
    }
}