using Microsoft.Playwright;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Tooltip;

/// <summary>
/// Interaction behavior tests for the Tooltip component.
/// Tests mouse and keyboard interactions:
/// - Tooltip shows on hover
/// - Tooltip hides when hover leaves
/// - Tooltip shows on focus
/// - Tooltip hides when focus leaves
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TooltipInteractionTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "tooltip";

    #region Hover Interaction Tests

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tooltip_HoverOnTrigger_ShowsTooltip()
    {
        await GoToPageAsync("interaction");

        var trigger = GetByTestId("hover-trigger");

        // Hover over the trigger
        await trigger.HoverAsync();

        // The singleton tooltip is a plain #ds-tooltip div on the body
        await WaitForTooltipOpenAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tooltip_HoverLeavesTrigger_HidesTooltip()
    {
        await GoToPageAsync("interaction");

        var trigger = GetByTestId("hover-trigger");
        var focusEnd = GetByTestId("focus-end");

        // Hover over the trigger to show tooltip
        await trigger.HoverAsync();
        await WaitForTooltipOpenAsync();

        // Move hover away from trigger
        await focusEnd.HoverAsync();

        // Tooltip should hide
        await WaitForTooltipClosedAsync();
    }

    #endregion

    #region Focus Interaction Tests

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tooltip_FocusOnTrigger_ShowsTooltip()
    {
        await GoToPageAsync("interaction");

        var trigger = GetByTestId("focus-trigger");

        // Focus the trigger
        await trigger.FocusAsync();

        // The singleton tooltip is a plain #ds-tooltip div on the body
        await WaitForTooltipOpenAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tooltip_FocusLeavesTrigger_HidesTooltip()
    {
        await GoToPageAsync("interaction");

        var trigger = GetByTestId("focus-trigger");
        var focusEnd = GetByTestId("focus-end");

        // Focus the trigger to show tooltip
        await trigger.FocusAsync();
        await WaitForTooltipOpenAsync();

        // Move focus away
        await focusEnd.FocusAsync();

        // Tooltip should hide
        await WaitForTooltipClosedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Tooltip_TabToTrigger_ShowsTooltip()
    {
        await GoToPageAsync("interaction");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();

        await PressTabAsync();

        var trigger = GetByTestId("hover-trigger");
        await Expect(trigger).ToBeFocusedAsync();

        // The singleton tooltip is a plain #ds-tooltip div on the body
        await WaitForTooltipOpenAsync();
    }

    #endregion

    #region Content Display Tests

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tooltip_WhenVisible_DisplaysCorrectContent()
    {
        await GoToPageAsync("interaction");

        var trigger = GetByTestId("hover-trigger");

        // Hover over the trigger
        await trigger.HoverAsync();

        // Wait for the singleton tooltip to open, then read its text content
        await WaitForTooltipOpenAsync();
        var content = await GetTooltipContentAsync();
        Assert.Equal("Hover tooltip content", content);
    }

    #endregion

    #region Tooltip Helpers

    /// <summary>
    /// Waits for the singleton tooltip element (<c>#ds-tooltip</c>) to be in the
    /// <c>:popover-open</c> state.
    /// </summary>
    /// <param name="timeoutMs">Maximum wait time in milliseconds. Defaults to 5000.</param>
    private Task<IJSHandle> WaitForTooltipOpenAsync(float timeoutMs = 5000) =>
        Page.WaitForFunctionAsync(
            "() => document.getElementById('ds-tooltip')?.matches(':popover-open') === true",
            null,
            new PageWaitForFunctionOptions { Timeout = timeoutMs });

    /// <summary>
    /// Waits for the singleton tooltip element (<c>#ds-tooltip</c>) to leave the
    /// <c>:popover-open</c> state.
    /// </summary>
    /// <param name="timeoutMs">Maximum wait time in milliseconds. Defaults to 5000.</param>
    private Task<IJSHandle> WaitForTooltipClosedAsync(float timeoutMs = 5000) =>
        Page.WaitForFunctionAsync(
            "() => document.getElementById('ds-tooltip')?.matches(':popover-open') !== true",
            null,
            new PageWaitForFunctionOptions { Timeout = timeoutMs });

    /// <summary>
    /// Returns the text content of the singleton tooltip element (<c>#ds-tooltip</c>),
    /// or <c>null</c> if not found.
    /// </summary>
    private Task<string?> GetTooltipContentAsync() =>
        Page.EvaluateAsync<string?>(
            "() => document.getElementById('ds-tooltip')?.textContent?.trim() ?? null");

    #endregion
}