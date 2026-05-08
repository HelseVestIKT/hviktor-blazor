using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Dialog;

/// <summary>
/// WCAG accessibility tests for the Dialog component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class DialogComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Placement>]
    [Trait(Traits.Component, "Dialog")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Dialog_Placement_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/dialog/placement", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Modal>]
    [Trait(Traits.Component, "Dialog")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Dialog_Modal_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/dialog/modal", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Closedby>]
    [Trait(Traits.Component, "Dialog")]
    [Trait(Traits.Category, Categories.Keyboard)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Dialog_Closedby_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/dialog/closedby", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.CloseButton>]
    [Trait(Traits.Component, "Dialog")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.NameRoleValue)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Dialog_CloseButton_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/dialog/closebutton", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Keyboard>]
    [Trait(Traits.Component, "Dialog")]
    [Trait(Traits.Category, Categories.Keyboard)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Dialog_Keyboard_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/dialog/keyboard", testId, Tags.Wcag2a3);
    }
}