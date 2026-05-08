using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Extensions.Services;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Badge;

/// <summary>
/// WCAG accessibility tests for the Badge component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class BadgeComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Variant>]
    [Trait(Traits.Component, "Badge")]
    [Trait(Traits.Category, Categories.Color)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Badge_Variant_WcagCompliant(Variant variant)
    {
        await AssertAllWcagLevelsAsync("/badge/variant", variant, VariantService.GetDataAttribute);
    }

    [Theory]
    [ClassData<TestData.Count>]
    [Trait(Traits.Component, "Badge")]
    [Trait(Traits.Category, Categories.TextAlternatives)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Badge_Count_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/badge/count", testId);
    }

    [Theory]
    [ClassData<TestData.Placement>]
    [Trait(Traits.Component, "Badge")]
    [Trait(Traits.Category, Categories.Structure)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Badge_Placement_WcagCompliant(Placement placement)
    {
        await AssertAllWcagLevelsAsync("/badge/placement", placement, PlacementService.GetDataAttribute);
    }

    [Theory]
    [ClassData<TestData.Overlap>]
    [Trait(Traits.Component, "Badge")]
    [Trait(Traits.Category, Categories.Structure)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Badge_Overlap_WcagCompliant(Variant overlap)
    {
        await AssertAllWcagLevelsAsync("/badge/overlap", overlap, VariantService.GetDataAttribute);
    }
}