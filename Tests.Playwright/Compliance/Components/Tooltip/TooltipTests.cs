using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Tooltip;

/// <summary>
/// WCAG accessibility tests for the Tooltip component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TooltipComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.BasicData>]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tooltip_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tooltip/basic", testId);
    }

    [Theory]
    [ClassData<TestData.PlacementData>]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tooltip_Placement_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tooltip/placement", testId);
    }

    [Theory]
    [ClassData<TestData.AriaData>]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tooltip_Aria_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tooltip/aria", testId);
    }

    [Theory]
    [ClassData<TestData.InteractionData>]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Keyboard)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tooltip_Interaction_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tooltip/interaction", testId);
    }
}