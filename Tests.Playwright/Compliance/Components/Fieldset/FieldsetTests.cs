using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Fieldset;

/// <summary>
/// WCAG accessibility tests for the Fieldset component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class FieldsetComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Fieldset")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Fieldset_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/fieldset/basic", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Legend>]
    [Trait(Traits.Component, "Fieldset")]
    [Trait(Traits.Category, Categories.Structure)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Fieldset_Legend_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/fieldset/legend", testId);
    }

    [Theory]
    [ClassData<TestData.Description>]
    [Trait(Traits.Component, "Fieldset")]
    [Trait(Traits.Category, Categories.Aria)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Fieldset_Description_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/fieldset/description", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Fieldset")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Fieldset_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/fieldset/accessibility", testId, Tags.Wcag2a3);
    }
}