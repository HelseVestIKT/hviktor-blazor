using Microsoft.Playwright;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Dropdown;

/// <summary>
/// Keyboard behavior tests for the Dropdown component.
/// Tests actual keyboard navigation behavior as per accessibility requirements:
/// - Enter or Space opens/closes the menu
/// - Enter or Space selects marked element
/// - Esc closes the menu
/// - Tab navigates focusable elements in Dropdown.List
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class DropdownKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "dropdown";

    #region Open/Close with Enter Key

    [Fact]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Dropdown_EnterKey_OpensMenu()
    {
        await GoToPageAsync("keyboard");

        // Find and focus the trigger button
        var trigger = GetByTestId("keyboard-trigger");
        await trigger.FocusAsync();

        // Press Enter to open
        await PressEnterAsync();

        // Wait for the popover to enter :popover-open state before asserting visibility
        await WaitForPopoverOpenAsync("keyboard-dropdown");

        // Verify dropdown is visible (popover is open)
        var dropdown = GetByTestId("keyboard-dropdown");
        await Expect(dropdown).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Dropdown_EnterKey_ClosesOpenMenu()
    {
        await GoToPageAsync("keyboard");

        var trigger = GetByTestId("keyboard-trigger");
        var dropdown = GetByTestId("keyboard-dropdown");

        // Open the dropdown via locator.PressAsync for reliable focus+key in one step
        await trigger.PressAsync("Enter");
        await WaitForPopoverOpenAsync("keyboard-dropdown");
        await Expect(dropdown).ToBeVisibleAsync();

        // Press Enter again on the focused trigger to close
        await trigger.PressAsync("Enter");
        await WaitForPopoverClosedAsync("keyboard-dropdown");

        // Verify dropdown is hidden
        await Expect(dropdown).ToBeHiddenAsync();
    }

    #endregion

    #region Open/Close with Space Key

    [Fact]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Dropdown_SpaceKey_OpensMenu()
    {
        await GoToPageAsync("keyboard");

        var trigger = GetByTestId("keyboard-trigger");
        await trigger.FocusAsync();

        // Press Space to open
        await PressSpaceAsync();

        // Wait for the popover to enter :popover-open state before asserting visibility
        await WaitForPopoverOpenAsync("keyboard-dropdown");

        var dropdown = GetByTestId("keyboard-dropdown");
        await Expect(dropdown).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Dropdown_SpaceKey_ClosesOpenMenu()
    {
        await GoToPageAsync("keyboard");

        var trigger = GetByTestId("keyboard-trigger");
        var dropdown = GetByTestId("keyboard-dropdown");

        // Open the dropdown via locator.PressAsync for reliable focus+key in one step
        await trigger.PressAsync("Space");
        await WaitForPopoverOpenAsync("keyboard-dropdown");
        await Expect(dropdown).ToBeVisibleAsync();

        // Press Space again on the focused trigger to close
        await trigger.PressAsync("Space");
        await WaitForPopoverClosedAsync("keyboard-dropdown");

        await Expect(dropdown).ToBeHiddenAsync();
    }

    #endregion

    #region Close with Escape Key

    [Fact]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Dropdown_EscapeKey_ClosesMenu()
    {
        await GoToPageAsync("keyboard");

        var trigger = GetByTestId("keyboard-trigger");
        var dropdown = GetByTestId("keyboard-dropdown");

        // Open the dropdown
        await trigger.PressAsync("Enter");
        await WaitForPopoverOpenAsync("keyboard-dropdown");
        await Expect(dropdown).ToBeVisibleAsync();

        // Press Escape to close
        await PressEscapeAsync();
        await WaitForPopoverClosedAsync("keyboard-dropdown");

        await Expect(dropdown).ToBeHiddenAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Dropdown_EscapeKey_ReturnsFocusToTrigger()
    {
        await GoToPageAsync("keyboard");

        var trigger = GetByTestId("keyboard-trigger");

        // Open the dropdown
        await trigger.PressAsync("Enter");
        await WaitForPopoverOpenAsync("keyboard-dropdown");

        // Tab into the dropdown items
        await PressTabAsync();

        // Press Escape to close
        await PressEscapeAsync();
        await WaitForPopoverClosedAsync("keyboard-dropdown");

        // Verify focus returns to trigger
        await Expect(trigger).ToBeFocusedAsync();
    }

    #endregion

    #region Tab Navigation in Dropdown

    [Fact]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Dropdown_TabKey_NavigatesFocusableElements()
    {
        await GoToPageAsync("keyboard");

        var trigger = GetByTestId("keyboard-trigger");

        // Open the dropdown
        await trigger.PressAsync("Enter");
        await WaitForPopoverOpenAsync("keyboard-dropdown");

        // Tab to first item
        await PressTabAsync();
        var firstButton = GetByTestId("keyboard-item-1");
        await Expect(firstButton).ToBeFocusedAsync();

        // Tab to second item
        await PressTabAsync();
        var secondButton = GetByTestId("keyboard-item-2");
        await Expect(secondButton).ToBeFocusedAsync();

        // Tab to third item
        await PressTabAsync();
        var thirdButton = GetByTestId("keyboard-item-3");
        await Expect(thirdButton).ToBeFocusedAsync();
    }

    #endregion

    #region Select Item with Enter/Space

    [Fact]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Dropdown_EnterOnItem_ActivatesButton()
    {
        await GoToPageAsync("keyboard");

        var trigger = GetByTestId("keyboard-trigger");

        // Open the dropdown
        await trigger.PressAsync("Enter");
        await WaitForPopoverOpenAsync("keyboard-dropdown");

        // Tab to first item
        await PressTabAsync();
        var firstButton = GetByTestId("keyboard-item-1");
        await Expect(firstButton).ToBeFocusedAsync();

        // Press Enter to activate the button - this triggers click event
        // Note: Dropdown does not have built-in close-on-select behavior per design system docs
        await PressEnterAsync();

        // The button should still be focusable (was activated via keyboard)
        await Expect(firstButton).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Dropdown_SpaceOnItem_ActivatesButton()
    {
        await GoToPageAsync("keyboard");

        var trigger = GetByTestId("keyboard-trigger");

        // Open the dropdown
        await trigger.PressAsync("Space");
        await WaitForPopoverOpenAsync("keyboard-dropdown");

        // Tab to first item
        await PressTabAsync();
        var firstButton = GetByTestId("keyboard-item-1");
        await Expect(firstButton).ToBeFocusedAsync();

        // Press Space to activate the button - this triggers click event
        // Note: Dropdown does not have built-in close-on-select behavior per design system docs
        await PressSpaceAsync();

        // The button should still be focusable (was activated via keyboard)
        await Expect(firstButton).ToBeFocusedAsync();
    }

    #endregion

    #region Helper Methods

    #region Popover State Helpers

    /// <summary>
    /// Waits for the popover identified by the given <c>data-testid</c> to enter
    /// the <c>:popover-open</c> state.
    /// </summary>
    /// <remarks>
    /// Uses JavaScript to poll the <c>:popover-open</c> pseudo-class directly, which is
    /// more reliable than CSS visibility assertions when <c>popover="manual"</c> elements
    /// transition in headless Chromium with <c>slowMo: 0</c>.
    /// </remarks>
    /// <param name="popoverTestId">The <c>data-testid</c> value of the popover element.</param>
    /// <param name="timeoutMs">Maximum wait time in milliseconds. Defaults to 5000.</param>
    private Task<IJSHandle> WaitForPopoverOpenAsync(string popoverTestId, float timeoutMs = 5000) =>
        Page.WaitForFunctionAsync(
            "testId => document.querySelector(`[data-testid='${testId}']`)?.matches(':popover-open') === true",
            popoverTestId,
            new PageWaitForFunctionOptions { Timeout = timeoutMs });

    /// <summary>
    /// Waits for the popover identified by the given <c>data-testid</c> to leave
    /// the <c>:popover-open</c> state.
    /// </summary>
    /// <param name="popoverTestId">The <c>data-testid</c> value of the popover element.</param>
    /// <param name="timeoutMs">Maximum wait time in milliseconds. Defaults to 5000.</param>
    private Task<IJSHandle> WaitForPopoverClosedAsync(string popoverTestId, float timeoutMs = 5000) =>
        Page.WaitForFunctionAsync(
            "testId => document.querySelector(`[data-testid='${testId}']`)?.matches(':popover-open') !== true",
            popoverTestId,
            new PageWaitForFunctionOptions { Timeout = timeoutMs });

    #endregion

    #endregion
}