using Microsoft.Playwright;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Dialog;

/// <summary>
/// Keyboard behavior tests for the Dialog component.
/// Tests actual keyboard navigation behavior as per accessibility requirements:
/// - Esc closes the dialog
/// - Focus trap in modal dialogs
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class DialogKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "dialog";

    #region Close with Escape Key

    [Fact]
    [Trait(Traits.Component, "Dialog")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Dialog_EscapeKey_ClosesDialog()
    {
        await GoToPageAsync("keyboard");

        // Click button to open dialog
        var openButton = GetByTestId("open-keyboard-dialog");
        await openButton.ClickAsync();

        // Wait for the native dialog.open property to become true before asserting visibility
        await WaitForDialogOpenAsync("keyboard-dialog");
        var dialog = Page.GetByTestId("keyboard-dialog");
        await Expect(dialog).ToBeVisibleAsync();

        // Press Escape to close
        await PressEscapeAsync();

        // Wait for the native dialog.open property to become false before asserting hidden
        await WaitForDialogClosedAsync("keyboard-dialog");

        // Verify dialog is closed
        await Expect(dialog).ToBeHiddenAsync();
    }

    #endregion

    #region Dialog Helpers

    /// <summary>
    /// Waits for the native <c>&lt;dialog&gt;</c> element identified by <paramref name="dialogTestId"/>
    /// to enter the open state (<c>dialog.open === true</c>).
    /// </summary>
    /// <param name="dialogTestId">The <c>data-testid</c> value of the dialog element.</param>
    /// <param name="timeoutMs">Maximum wait time in milliseconds. Defaults to 5000.</param>
    private Task<IJSHandle> WaitForDialogOpenAsync(string dialogTestId, float timeoutMs = 5000) =>
        Page.WaitForFunctionAsync(
            "testId => document.querySelector(`[data-testid='${testId}']`)?.open === true",
            dialogTestId,
            new PageWaitForFunctionOptions { Timeout = timeoutMs });

    /// <summary>
    /// Waits for the native <c>&lt;dialog&gt;</c> element identified by <paramref name="dialogTestId"/>
    /// to leave the open state (<c>dialog.open !== true</c>).
    /// </summary>
    /// <param name="dialogTestId">The <c>data-testid</c> value of the dialog element.</param>
    /// <param name="timeoutMs">Maximum wait time in milliseconds. Defaults to 5000.</param>
    private Task<IJSHandle> WaitForDialogClosedAsync(string dialogTestId, float timeoutMs = 5000) =>
        Page.WaitForFunctionAsync(
            "testId => document.querySelector(`[data-testid='${testId}']`)?.open !== true",
            dialogTestId,
            new PageWaitForFunctionOptions { Timeout = timeoutMs });

    #endregion
}