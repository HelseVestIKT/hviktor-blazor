using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Popover;

/// <summary>
/// WCAG accessibility tests for the Popover component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class PopoverComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Popover")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Popover_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/popover/basic", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Placement>]
    [Trait(Traits.Component, "Popover")]
    [Trait(Traits.Category, Categories.Structure)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Popover_Placement_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/popover/placement", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Popover")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Popover_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/popover/accessibility", testId, Tags.Wcag2a3);
    }
}