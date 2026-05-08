using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Field;

/// <summary>
/// WCAG accessibility tests for the Field component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class FieldComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Field")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Field_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/field/basic", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Position>]
    [Trait(Traits.Component, "Field")]
    [Trait(Traits.Category, Categories.Structure)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Field_Position_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/field/position", testId);
    }

    [Theory]
    [ClassData<TestData.Counter>]
    [Trait(Traits.Component, "Field")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Field_Counter_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/field/counter", testId);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Field")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Field_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/field/accessibility", testId, Tags.Wcag2a3);
    }
}