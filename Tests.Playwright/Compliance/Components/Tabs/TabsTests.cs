using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Tabs;

/// <summary>
/// WCAG accessibility tests for the Tabs component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TabsComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.BasicTabs>]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tabs_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tabs/basic", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.DefaultValueVariant>]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tabs_DefaultValue_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tabs/default-value", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.AriaVariant>]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tabs_Aria_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tabs/aria", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.InteractiveVariant>]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tabs_InteractiveContent_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tabs/interactive-content", testId, Tags.Wcag2a3);
    }
}