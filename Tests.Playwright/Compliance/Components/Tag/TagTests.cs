using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Tag;

/// <summary>
/// WCAG accessibility tests for the Tag component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TagComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.VariantData>]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tag_Variant_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tag/variant", testId);
    }

    [Theory]
    [ClassData<TestData.ColorData>]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tag_Color_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tag/color", testId);
    }

    [Theory]
    [ClassData<TestData.SizeData>]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tag_Size_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tag/size", testId);
    }

    [Theory]
    [ClassData<TestData.ListData>]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Structure)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Tag_List_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/tag/list", testId);
    }
}