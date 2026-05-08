using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Checkbox;

/// <summary>
/// WCAG accessibility tests for the Checkbox component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class CheckboxComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.State>]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Checkbox_State_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/checkbox/state", testId);
    }

    [Theory]
    [ClassData<TestData.DisabledState>]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Checkbox_Disabled_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/checkbox/disabled", testId);
    }

    [Theory]
    [ClassData<TestData.ReadOnlyState>]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Checkbox_ReadOnly_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/checkbox/readonly", testId);
    }

    [Theory]
    [ClassData<TestData.LabelVariant>]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Forms)]
    [Trait(Traits.Category, Categories.NameRoleValue)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Checkbox_Label_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/checkbox/label", testId);
    }

    [Theory]
    [ClassData<TestData.DescriptionVariant>]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Checkbox_Description_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/checkbox/description", testId);
    }

    [Theory]
    [InlineData("with-error")]
    [InlineData("without-error")]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Checkbox_Error_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/checkbox/error", testId, Tags.Wcag2a3);
    }
}