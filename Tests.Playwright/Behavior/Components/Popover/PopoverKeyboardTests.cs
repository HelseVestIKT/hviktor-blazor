using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Popover;

/// <summary>
/// Keyboard and behavior tests for the Popover component.
/// Tests accessibility requirements:
/// - Enter or Space opens/closes the popover
/// - Escape closes the popover
/// - Built-in accessibility via native Popover API
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class PopoverKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "popover";

    #region Enter/Space Opens Popover

    [Fact]
    [Trait(Traits.Component, "Popover")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Popover_EnterKey_OpensPopover()
    {
        await GoToPageAsync("accessibility");

        var focusStart = GetByTestId("focus-start");
        var trigger = GetByTestId("keyboard-trigger");
        var popover = GetByTestId("keyboard-popover");

        // Tab into the trigger from focus-start anchor
        await focusStart.FocusAsync();
        await PressTabAsync();
        await Expect(trigger).ToBeFocusedAsync();

        // Press Enter to open
        await PressEnterAsync();
        await Expect(popover).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Popover")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Popover_SpaceKey_OpensPopover()
    {
        await GoToPageAsync("accessibility");

        var focusStart = GetByTestId("focus-start");
        var trigger = GetByTestId("keyboard-trigger");
        var popover = GetByTestId("keyboard-popover");

        // Tab into the trigger from focus-start anchor
        await focusStart.FocusAsync();
        await PressTabAsync();
        await Expect(trigger).ToBeFocusedAsync();

        // Press Space to open
        await PressSpaceAsync();
        await Expect(popover).ToBeVisibleAsync();
    }

    #endregion

    #region Escape Closes Popover

    [Fact]
    [Trait(Traits.Component, "Popover")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Popover_EscapeKey_ClosesPopover()
    {
        await GoToPageAsync("accessibility");

        var trigger = GetByTestId("keyboard-trigger");
        var popover = GetByTestId("keyboard-popover");

        // Open the popover first
        await trigger.ClickAsync();
        await Expect(popover).ToBeVisibleAsync();

        // Press Escape to close
        await PressEscapeAsync();
        await Expect(popover).ToBeHiddenAsync();
    }

    #endregion

    #region Toggle Behavior

    [Fact]
    [Trait(Traits.Component, "Popover")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Popover_EnterKey_TogglesPopover()
    {
        await GoToPageAsync("accessibility");

        var focusStart = GetByTestId("focus-start");
        var trigger = GetByTestId("keyboard-trigger");
        var popover = GetByTestId("keyboard-popover");

        // Tab into the trigger from focus-start anchor
        await focusStart.FocusAsync();
        await PressTabAsync();
        await Expect(trigger).ToBeFocusedAsync();

        // First Enter opens
        await PressEnterAsync();
        await Expect(popover).ToBeVisibleAsync();

        // Second Enter closes
        await PressEnterAsync();
        await Expect(popover).ToBeHiddenAsync();
    }

    #endregion

    #region Tab Navigation

    [Fact]
    [Trait(Traits.Component, "Popover")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Popover_TabNavigation_NavigatesBetweenTriggers()
    {
        await GoToPageAsync("accessibility");

        var trigger1 = GetByTestId("popover-trigger-1");
        var trigger2 = GetByTestId("popover-trigger-2");

        // Tab into the first trigger from focus-start
        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();

        // Tab past the keyboard-trigger, then to trigger-1
        await PressTabAsync();
        await PressTabAsync();
        await Expect(trigger1).ToBeFocusedAsync();

        // Tab to second trigger
        await PressTabAsync();
        await Expect(trigger2).ToBeFocusedAsync();
    }

    #endregion
}