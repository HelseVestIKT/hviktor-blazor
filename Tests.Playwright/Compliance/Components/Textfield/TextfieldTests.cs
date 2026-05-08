using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Textfield;

/// <summary>
/// WCAG accessibility tests for the Textfield component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TextfieldComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.BasicData>]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textfield_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textfield/basic", testId);
    }

    [Theory]
    [ClassData<TestData.AffixesData>]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textfield_Affixes_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textfield/affixes", testId);
    }

    [Theory]
    [ClassData<TestData.ErrorData>]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textfield_Error_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textfield/error", testId);
    }

    [Theory]
    [ClassData<TestData.CounterData>]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textfield_Counter_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textfield/counter", testId);
    }

    [Theory]
    [ClassData<TestData.MultilineData>]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textfield_Multiline_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textfield/multiline", testId);
    }

    [Theory]
    [ClassData<TestData.TypesData>]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textfield_Types_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textfield/types", testId);
    }

    [Theory]
    [ClassData<TestData.AccessibilityData>]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Aria)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textfield_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textfield/accessibility", testId);
    }
}