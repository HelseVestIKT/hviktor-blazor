using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Tabs;

/// <summary>
/// Semantic behavior tests for the Tabs component.
/// Tests proper ARIA attributes and semantic structure:
/// - TabList has role="tablist"
/// - Each Tab has role="tab" and aria-selected
/// - Each Tab has aria-controls pointing to its panel
/// - Each Panel has role="tabpanel" and aria-labelledby pointing to its tab
/// - Only selected panel is visible
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TabsSemanticTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "tabs";

    #region TabList Role

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task TabsList_HasRoleTablist()
    {
        await GoToPageAsync("aria");
        var tablist = GetByTestId("aria-tablist");
        await Expect(tablist).ToHaveAttributeAsync("role", "tablist");
    }

    #endregion

    #region Tab Role and Attributes

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Tab_HasRoleTab()
    {
        await GoToPageAsync("aria");
        var tab = GetByTestId("overview-tab");
        await Expect(tab).ToHaveAttributeAsync("role", "tab");
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Tab_SelectedTab_HasAriaSelectedTrue()
    {
        await GoToPageAsync("aria");
        var selectedTab = GetByTestId("overview-tab");
        await Expect(selectedTab).ToHaveAttributeAsync("aria-selected", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Tab_UnselectedTab_HasAriaSelectedFalse()
    {
        await GoToPageAsync("aria");
        var unselectedTab = GetByTestId("details-tab");
        await Expect(unselectedTab).ToHaveAttributeAsync("aria-selected", "false");
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Tab_HasAriaControlsAttribute()
    {
        await GoToPageAsync("aria");
        var tab = GetByTestId("overview-tab");

        var ariaControls = await tab.GetAttributeAsync("aria-controls");
        Assert.NotNull(ariaControls);
        Assert.NotEmpty(ariaControls);
    }

    #endregion

    #region Panel Role and Attributes

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Panel_HasRoleTabpanel()
    {
        await GoToPageAsync("aria");
        var panel = GetByTestId("overview-panel");
        await Expect(panel).ToHaveAttributeAsync("role", "tabpanel");
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Panel_HasAriaLabelledbyAttribute()
    {
        await GoToPageAsync("aria");
        var panel = GetByTestId("overview-panel");

        var ariaLabelledby = await panel.GetAttributeAsync("aria-labelledby");
        Assert.NotNull(ariaLabelledby);
        Assert.NotEmpty(ariaLabelledby);
    }

    #endregion

    #region Panel Visibility

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Panel_SelectedPanel_IsVisible()
    {
        await GoToPageAsync("aria");
        var selectedPanel = GetByTestId("overview-panel");
        await Expect(selectedPanel).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Panel_UnselectedPanel_IsNotVisible()
    {
        await GoToPageAsync("aria");
        var unselectedPanel = GetByTestId("details-panel");
        await Expect(unselectedPanel).Not.ToBeVisibleAsync();
    }

    #endregion

    #region Default Value Selection

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tabs_DefaultValue_SelectsCorrectTab()
    {
        await GoToPageAsync("default-value");

        var firstTab = GetByTestId("first-tab");
        var firstPanel = GetByTestId("first-panel");

        await Expect(firstTab).ToHaveAttributeAsync("aria-selected", "true");
        await Expect(firstPanel).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tabs_DefaultValueSecond_SelectsSecondTab()
    {
        await GoToPageAsync("default-value");

        var secondTab = GetByTestId("second-second-tab");
        var secondPanel = GetByTestId("second-second-panel");

        await Expect(secondTab).ToHaveAttributeAsync("aria-selected", "true");
        await Expect(secondPanel).ToBeVisibleAsync();
    }

    #endregion

    #region Tab Click Activation

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tab_Click_ActivatesTab()
    {
        await GoToPageAsync("basic");

        var tab2 = GetByTestId("tab2");
        var panel2 = GetByTestId("panel2");
        var panel1 = GetByTestId("panel1");

        await tab2.ClickAsync();

        await Expect(tab2).ToHaveAttributeAsync("aria-selected", "true");
        await Expect(panel2).ToBeVisibleAsync();
        await Expect(panel1).Not.ToBeVisibleAsync();
    }

    #endregion

    #region Unique Tab Titles

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tabs_EachTab_HasUniqueValue()
    {
        await GoToPageAsync("basic");

        var tab1 = GetByTestId("tab1");
        var tab2 = GetByTestId("tab2");
        var tab3 = GetByTestId("tab3");

        var value1 = await tab1.GetAttributeAsync("data-value");
        var value2 = await tab2.GetAttributeAsync("data-value");
        var value3 = await tab3.GetAttributeAsync("data-value");

        Assert.NotEqual(value1, value2);
        Assert.NotEqual(value2, value3);
        Assert.NotEqual(value1, value3);
    }

    #endregion
}