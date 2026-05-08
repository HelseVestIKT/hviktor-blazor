using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.ErrorSummary;

/// <summary>
/// WCAG accessibility tests for the ErrorSummary component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class ErrorSummaryComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "ErrorSummary")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ErrorSummary_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/errorsummary/basic", testId);
    }

    [Theory]
    [ClassData<TestData.Heading>]
    [Trait(Traits.Component, "ErrorSummary")]
    [Trait(Traits.Category, Categories.Structure)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ErrorSummary_Heading_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/errorsummary/heading", testId);
    }

    [Theory]
    [ClassData<TestData.Links>]
    [Trait(Traits.Component, "ErrorSummary")]
    [Trait(Traits.Category, Categories.NameRoleValue)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ErrorSummary_Links_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/errorsummary/links", testId);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "ErrorSummary")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ErrorSummary_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/errorsummary/accessibility", testId);
    }
}
