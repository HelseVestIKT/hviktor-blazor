using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Radio;

/// <summary>
/// WCAG accessibility tests for the Radio component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class RadioComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Radio_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/radio/basic", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.State>]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Radio_State_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/radio/state", testId);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Radio_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/radio/accessibility", testId, Tags.Wcag2a3);
    }
}