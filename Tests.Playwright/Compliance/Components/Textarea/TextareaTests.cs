using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Textarea;

/// <summary>
/// WCAG accessibility tests for the Textarea component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TextareaComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.BasicData>]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textarea_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textarea/basic", testId);
    }

    [Theory]
    [ClassData<TestData.StateData>]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Forms)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textarea_State_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textarea/state", testId);
    }

    [Theory]
    [ClassData<TestData.SizeData>]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textarea_Size_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textarea/size", testId);
    }

    [Theory]
    [ClassData<TestData.DimensionsData>]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textarea_Dimensions_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textarea/dimensions", testId);
    }

    [Theory]
    [ClassData<TestData.AccessibilityData>]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Textarea_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/textarea/accessibility", testId);
    }
}
