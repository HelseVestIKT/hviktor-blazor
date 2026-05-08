using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.AvatarStack;

/// <summary>
/// WCAG accessibility tests for the AvatarStack component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class AvatarStackComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.ColorData>]
    [Trait(Traits.Component, "AvatarStack")]
    [Trait(Traits.Category, Categories.Color)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task AvatarStack_Color_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/avatar-stack/color", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.SizeData>]
    [Trait(Traits.Component, "AvatarStack")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task AvatarStack_Size_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/avatar-stack/size", testId, Tags.Wcag2a3);
    }
}