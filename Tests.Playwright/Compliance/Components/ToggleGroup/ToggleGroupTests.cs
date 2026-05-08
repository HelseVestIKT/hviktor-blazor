using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.ToggleGroup;

/// <summary>
/// WCAG accessibility tests for the ToggleGroup component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class ToggleGroupComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.BasicData>]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Forms)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ToggleGroup_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/togglegroup/basic", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.VariantData>]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ToggleGroup_Variant_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/togglegroup/variant", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.SizeData>]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ToggleGroup_Size_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/togglegroup/size", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.AriaData>]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task ToggleGroup_Aria_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/togglegroup/aria", testId, Tags.Wcag2a3);
    }
}