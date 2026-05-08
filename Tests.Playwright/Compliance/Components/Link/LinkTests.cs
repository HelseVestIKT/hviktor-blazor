using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Link;

/// <summary>
/// WCAG accessibility tests for the Link component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class LinkComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Link_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/link/basic", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Target>]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Link_Target_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/link/target", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Size>]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Structure)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Link_Size_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/link/size", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Color>]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Color)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Link_Color_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/link/color", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Link_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/link/accessibility", testId, Tags.Wcag2a3);
    }
}
