using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Tabs;

/// <summary>
/// Keyboard behavior tests for the Tabs component.
/// Tests keyboard navigation as per accessibility requirements:
/// - Tab moves focus to the tab list
/// - Arrow Right (→) moves to next tab
/// - Arrow Left (←) moves to previous tab
/// - Home moves to first tab
/// - End moves to last tab
/// - Enter or Space activates the selected tab
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TabsKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "tabs";

    #region Tab Navigation to Tabs

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_TabKey_MovesToTabList()
    {
        await GoToPageAsync("keyboard");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        var homeTab = GetByTestId("home-tab");
        await Expect(homeTab).ToBeFocusedAsync();
    }

    #endregion

    #region Arrow Key Navigation

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_ArrowRight_MovesToNextTab()
    {
        await GoToPageAsync("keyboard");

        var homeTab = GetByTestId("home-tab");
        var profileTab = GetByTestId("profile-tab");

        await homeTab.FocusAsync();
        await Expect(homeTab).ToBeFocusedAsync();

        await PressArrowRightAsync();
        await Expect(profileTab).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_ArrowLeft_MovesToPreviousTab()
    {
        await GoToPageAsync("keyboard");

        var homeTab = GetByTestId("home-tab");
        var profileTab = GetByTestId("profile-tab");

        await profileTab.FocusAsync();
        await Expect(profileTab).ToBeFocusedAsync();

        await PressArrowLeftAsync();
        await Expect(homeTab).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_ArrowRight_WrapsToFirstTab()
    {
        await GoToPageAsync("keyboard");

        var homeTab = GetByTestId("home-tab");
        var helpTab = GetByTestId("help-tab");

        await helpTab.FocusAsync();
        await Expect(helpTab).ToBeFocusedAsync();

        await PressArrowRightAsync();
        await Expect(homeTab).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_ArrowLeft_WrapsToLastTab()
    {
        await GoToPageAsync("keyboard");

        var homeTab = GetByTestId("home-tab");
        var helpTab = GetByTestId("help-tab");

        await homeTab.FocusAsync();
        await Expect(homeTab).ToBeFocusedAsync();

        await PressArrowLeftAsync();
        await Expect(helpTab).ToBeFocusedAsync();
    }

    #endregion

    #region Home and End Key Navigation

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_HomeKey_MovesToFirstTab()
    {
        await GoToPageAsync("keyboard");

        var homeTab = GetByTestId("home-tab");
        var settingsTab = GetByTestId("settings-tab");

        await settingsTab.FocusAsync();
        await Expect(settingsTab).ToBeFocusedAsync();

        await PressHomeAsync();
        await Expect(homeTab).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_EndKey_MovesToLastTab()
    {
        await GoToPageAsync("keyboard");

        var homeTab = GetByTestId("home-tab");
        var helpTab = GetByTestId("help-tab");

        await homeTab.FocusAsync();
        await Expect(homeTab).ToBeFocusedAsync();

        await PressEndAsync();
        await Expect(helpTab).ToBeFocusedAsync();
    }

    #endregion

    #region Tab Activation with Enter and Space

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_EnterKey_ActivatesTab()
    {
        await GoToPageAsync("keyboard");

        var profileTab = GetByTestId("profile-tab");
        var profilePanel = GetByTestId("profile-panel");

        await profileTab.FocusAsync();
        await PressEnterAsync();

        await Expect(profileTab).ToHaveAttributeAsync("aria-selected", "true");
        await Expect(profilePanel).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_SpaceKey_ActivatesTab()
    {
        await GoToPageAsync("keyboard");

        var settingsTab = GetByTestId("settings-tab");
        var settingsPanel = GetByTestId("settings-panel");

        await settingsTab.FocusAsync();
        await PressSpaceAsync();

        await Expect(settingsTab).ToHaveAttributeAsync("aria-selected", "true");
        await Expect(settingsPanel).ToBeVisibleAsync();
    }

    #endregion

    #region Tab Switching Hides Previous Panel

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_SwitchingTabs_HidesPreviousPanel()
    {
        await GoToPageAsync("keyboard");

        var homeTab = GetByTestId("home-tab");
        var profileTab = GetByTestId("profile-tab");
        var homePanel = GetByTestId("home-panel");
        var profilePanel = GetByTestId("profile-panel");

        await Expect(homeTab).ToHaveAttributeAsync("aria-selected", "true");
        await Expect(homePanel).ToBeVisibleAsync();

        await profileTab.FocusAsync();
        await PressEnterAsync();

        await Expect(profileTab).ToHaveAttributeAsync("aria-selected", "true");
        await Expect(profilePanel).ToBeVisibleAsync();
        await Expect(homePanel).Not.ToBeVisibleAsync();
    }

    #endregion

    #region Focus After Tab Activation

    [Fact]
    [Trait(Traits.Component, "Tabs")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tabs_AfterActivation_FocusRemainsOnTab()
    {
        await GoToPageAsync("keyboard");

        var profileTab = GetByTestId("profile-tab");
        await profileTab.FocusAsync();
        await PressEnterAsync();

        await Expect(profileTab).ToBeFocusedAsync();
    }

    #endregion
}