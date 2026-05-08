using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Switch;

/// <summary>
/// WCAG accessibility tests for the Switch component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class SwitchComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.State>]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Switch_State_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/switch/state", testId);
    }

    [Theory]
    [ClassData<TestData.DisabledState>]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Switch_Disabled_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/switch/disabled", testId);
    }

    [Theory]
    [ClassData<TestData.LabelVariant>]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Switch_Label_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/switch/label", testId);
    }

    [Theory]
    [ClassData<TestData.DescriptionVariant>]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Switch_Description_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/switch/description", testId);
    }

    [Theory]
    [ClassData<TestData.PositionVariant>]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Forms)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Switch_Position_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/switch/position", testId);
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Aria)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Switch_Role_WcagCompliant()
    {
        await AssertAllWcagLevelsAsync("/switch/role", "with-role");
    }
}
