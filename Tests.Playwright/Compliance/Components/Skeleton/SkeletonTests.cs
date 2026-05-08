using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Extensions.Services;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Skeleton;

/// <summary>
/// WCAG accessibility tests for the Skeleton component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class SkeletonComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Variant>]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Skeleton_Variant_WcagCompliant(Variant variant)
    {
        await AssertAllWcagLevelsAsync("/skeleton/variant", variant, VariantService.GetDataAttribute);
    }

    [Theory]
    [ClassData<TestData.Size>]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Skeleton_Size_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/skeleton/size", testId);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Skeleton_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/skeleton/accessibility", testId);
    }
}
