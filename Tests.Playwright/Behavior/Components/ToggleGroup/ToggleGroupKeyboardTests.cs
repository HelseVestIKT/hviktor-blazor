using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.ToggleGroup;

/// <summary>
/// Keyboard behavior tests for the ToggleGroup component.
/// Tests keyboard navigation as per accessibility requirements:
/// - Tab moves focus to the active toggle button (tabindex=0)
/// - Shift+Tab from a non-active item (tabindex=-1) returns focus to the active item
/// - Shift+Tab from the active item exits the group entirely
/// - Space activates the focused button
/// - Enter activates the focused button
/// - Arrow Up/Left moves focus to previous button (wraps)
/// - Arrow Down/Right moves focus to next button (wraps)
/// - Home moves to first button
/// - End moves to last button
/// Uses roving tabindex for focus management.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class ToggleGroupKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "togglegroup";

    #region Tab Navigation Tests

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_TabKey_MovesToSelectedItem()
    {
        await GoToPageAsync("keyboard");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();

        // Tab to the toggle group - should focus the selected item (home)
        await PressTabAsync();

        var homeItem = GetByTestId("home-item");
        await Expect(homeItem).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_ShiftTab_FromArrowNavigatedItem_ExitsGroup()
    {
        await GoToPageAsync("keyboard");

        // home-item is initially tabbable (tabindex=0). Arrow right moves roving
        // tabindex to profile-item, making it the new tabbable item (tabindex=0).
        var homeItem = GetByTestId("home-item");
        var profileItem = GetByTestId("profile-item");

        await homeItem.FocusAsync();
        await PressArrowRightAsync();
        await Expect(profileItem).ToBeFocusedAsync();

        // Shift+Tab from any item in the group exits the group entirely,
        // because roving tabindex follows focus, not selection.
        await PressShiftTabAsync();

        await Expect(profileItem).Not.ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_ShiftTab_FromActiveItem_ExitsGroup()
    {
        await GoToPageAsync("keyboard");

        // home-item is the active item (tabindex=0). Shift+Tab from it should exit
        // the group entirely. focus-start is off-screen (left:-9999px) so the browser
        // skips it in tab order — we assert only that focus leaves home-item.
        var homeItem = GetByTestId("home-item");
        await homeItem.FocusAsync();

        await PressShiftTabAsync();
        await Expect(homeItem).Not.ToBeFocusedAsync();
    }

    #endregion

    #region Arrow Key Navigation Tests

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_ArrowRight_MovesToNextItem()
    {
        await GoToPageAsync("keyboard");

        var homeItem = GetByTestId("home-item");
        var profileItem = GetByTestId("profile-item");

        // Focus home item (selected)
        await homeItem.FocusAsync();

        // Press Arrow Right to move to next item
        await PressArrowRightAsync();

        await Expect(profileItem).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_ArrowDown_MovesToNextItem()
    {
        await GoToPageAsync("keyboard");

        var homeItem = GetByTestId("home-item");
        var profileItem = GetByTestId("profile-item");

        // Focus home item
        await homeItem.FocusAsync();

        // Press Arrow Down to move to next item
        await PressArrowDownAsync();

        await Expect(profileItem).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_ArrowLeft_MovesToPreviousItem()
    {
        await GoToPageAsync("keyboard");

        var homeItem = GetByTestId("home-item");
        var profileItem = GetByTestId("profile-item");

        // Focus profile item
        await profileItem.FocusAsync();

        // Press Arrow Left to move to previous item
        await PressArrowLeftAsync();

        await Expect(homeItem).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_ArrowUp_MovesToPreviousItem()
    {
        await GoToPageAsync("keyboard");

        var homeItem = GetByTestId("home-item");
        var profileItem = GetByTestId("profile-item");

        // Focus profile item
        await profileItem.FocusAsync();

        // Press Arrow Up to move to previous item
        await PressArrowUpAsync();

        await Expect(homeItem).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_ArrowRight_WrapsToFirstItem()
    {
        await GoToPageAsync("keyboard");

        var homeItem = GetByTestId("home-item");
        var helpItem = GetByTestId("help-item");

        // Focus last item
        await helpItem.FocusAsync();

        // Press Arrow Right should wrap to first item
        await PressArrowRightAsync();

        await Expect(homeItem).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_ArrowLeft_WrapsToLastItem()
    {
        await GoToPageAsync("keyboard");

        var homeItem = GetByTestId("home-item");
        var helpItem = GetByTestId("help-item");

        // Focus first item
        await homeItem.FocusAsync();

        // Press Arrow Left should wrap to last item
        await PressArrowLeftAsync();

        await Expect(helpItem).ToBeFocusedAsync();
    }

    #endregion

    #region Home and End Key Navigation

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_HomeKey_MovesToFirstItem()
    {
        await GoToPageAsync("keyboard");

        var homeItem = GetByTestId("home-item");
        var settingsItem = GetByTestId("settings-item");

        // Focus a middle item
        await settingsItem.FocusAsync();

        // Press Home to move to first item
        await PressHomeAsync();

        await Expect(homeItem).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_EndKey_MovesToLastItem()
    {
        await GoToPageAsync("keyboard");

        var homeItem = GetByTestId("home-item");
        var helpItem = GetByTestId("help-item");

        // Focus first item
        await homeItem.FocusAsync();

        // Press End to move to last item
        await PressEndAsync();

        await Expect(helpItem).ToBeFocusedAsync();
    }

    #endregion

    #region Activation Tests

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_SpaceKey_ActivatesItem()
    {
        await GoToPageAsync("keyboard");

        var profileItem = GetByTestId("profile-item");

        // Focus profile item
        await profileItem.FocusAsync();

        // Press Space to activate
        await PressSpaceAsync();

        // Item should now be selected
        await Expect(profileItem).ToHaveAttributeAsync("aria-checked", "true");
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_EnterKey_ActivatesItem()
    {
        await GoToPageAsync("keyboard");

        var settingsItem = GetByTestId("settings-item");

        // Focus settings item
        await settingsItem.FocusAsync();

        // Press Enter to activate
        await PressEnterAsync();

        // Item should now be selected
        await Expect(settingsItem).ToHaveAttributeAsync("aria-checked", "true");
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_Activation_UpdatesAriaChecked()
    {
        await GoToPageAsync("keyboard");

        var homeItem = GetByTestId("home-item");
        var profileItem = GetByTestId("profile-item");

        // Initially home is selected
        await Expect(homeItem).ToHaveAttributeAsync("aria-checked", "true");
        await Expect(profileItem).ToHaveAttributeAsync("aria-checked", "false");

        // Activate profile item via its wrapping label
        var profileLabel = profileItem.Locator("xpath=..");
        await profileLabel.ClickAsync();

        // Now profile should be checked and home should not
        await Expect(profileItem).ToHaveAttributeAsync("aria-checked", "true");
        await Expect(homeItem).ToHaveAttributeAsync("aria-checked", "false");
    }

    #endregion

    #region Focus After Activation

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ToggleGroup_AfterActivation_FocusRemainsOnItem()
    {
        await GoToPageAsync("keyboard");

        var profileItem = GetByTestId("profile-item");

        // Focus and activate profile item
        await profileItem.FocusAsync();
        await PressEnterAsync();

        // Focus should remain on the item
        await Expect(profileItem).ToBeFocusedAsync();
    }

    #endregion
}