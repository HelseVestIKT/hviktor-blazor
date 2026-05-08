using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.ErrorSummary;

/// <summary>
/// Keyboard and focus behavior tests for the ErrorSummary component.
/// Tests accessibility requirements:
/// - When ErrorSummary becomes visible, focus should move to it
/// - Screen readers will then read the heading
/// - Users can navigate to each error link in the list
/// - From error links, users can navigate to the field containing the error
/// - Do NOT use live regions (aria-role="alert") for ErrorSummary
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class ErrorSummaryFocusTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "errorsummary";

    #region Focus Management

    [Fact]
    [Trait(Traits.Component, "ErrorSummary")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ErrorSummary_WhenVisible_ReceivesFocus()
    {
        await GoToPageAsync("focus");

        // Error summary should not be visible initially
        var errorSummary = GetByTestId("error-summary");
        await Expect(errorSummary).ToBeHiddenAsync();

        // Click the button to trigger validation and show error summary
        var triggerButton = GetByTestId("trigger-validation");
        await triggerButton.ClickAsync();

        // Error summary should now be visible
        await Expect(errorSummary).ToBeVisibleAsync();

        // Error summary should receive focus (recommended accessibility pattern)
        await Expect(errorSummary).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "ErrorSummary")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ErrorSummary_TabFromFocused_NavigatesToFirstLink()
    {
        await GoToPageAsync("focus");

        // Show the error summary
        var triggerButton = GetByTestId("trigger-validation");
        await triggerButton.ClickAsync();

        // Wait for error summary to be visible and focused
        var errorSummary = GetByTestId("error-summary");
        await Expect(errorSummary).ToBeVisibleAsync();
        await Expect(errorSummary).ToBeFocusedAsync();

        // Tab to navigate to first error link
        await PressTabAsync();

        // First error link should be focused
        var firstLink = GetByTestId("error-link-firstname");
        await Expect(firstLink).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "ErrorSummary")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ErrorSummary_TabThroughLinks_NavigatesAllLinks()
    {
        await GoToPageAsync("focus");

        // Show the error summary
        var triggerButton = GetByTestId("trigger-validation");
        await triggerButton.ClickAsync();

        // Wait for error summary to be visible and focused
        var errorSummary = GetByTestId("error-summary");
        await Expect(errorSummary).ToBeVisibleAsync();
        await Expect(errorSummary).ToBeFocusedAsync();

        // Tab to first link
        await PressTabAsync();
        var firstLink = GetByTestId("error-link-firstname");
        await Expect(firstLink).ToBeFocusedAsync();

        // Tab to second link
        await PressTabAsync();
        var secondLink = GetByTestId("error-link-email");
        await Expect(secondLink).ToBeFocusedAsync();
    }

    #endregion

    #region Link Navigation to Fields

    [Fact]
    [Trait(Traits.Component, "ErrorSummary")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ErrorSummary_ClickErrorLink_HasCorrectHref()
    {
        await GoToPageAsync("focus");

        // Show the error summary
        var triggerButton = GetByTestId("trigger-validation");
        await triggerButton.ClickAsync();

        // Wait for error summary to be visible
        var errorSummary = GetByTestId("error-summary");
        await Expect(errorSummary).ToBeVisibleAsync();

        // Verify the error link has the correct href pointing to the field
        var firstLink = GetByTestId("error-link-firstname");
        await Expect(firstLink).ToHaveAttributeAsync("href", "#firstname-input");
    }

    [Fact]
    [Trait(Traits.Component, "ErrorSummary")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ErrorSummary_AllErrorLinks_HaveValidHrefs()
    {
        await GoToPageAsync("focus");

        // Show the error summary
        var triggerButton = GetByTestId("trigger-validation");
        await triggerButton.ClickAsync();

        // Wait for error summary to be visible
        var errorSummary = GetByTestId("error-summary");
        await Expect(errorSummary).ToBeVisibleAsync();

        // Verify all error links have correct hrefs
        var firstLink = GetByTestId("error-link-firstname");
        await Expect(firstLink).ToHaveAttributeAsync("href", "#firstname-input");

        var secondLink = GetByTestId("error-link-email");
        await Expect(secondLink).ToHaveAttributeAsync("href", "#email-input");

        // Verify the target elements exist on the page
        var firstnameField = Page.Locator("#firstname-input");
        await Expect(firstnameField).ToBeVisibleAsync();

        var emailField = Page.Locator("#email-input");
        await Expect(emailField).ToBeVisibleAsync();
    }

    #endregion

    #region No Live Region Verification

    [Fact]
    [Trait(Traits.Component, "ErrorSummary")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task ErrorSummary_DoesNotUseLiveRegion()
    {
        await GoToPageAsync("focus");

        // Show the error summary
        var triggerButton = GetByTestId("trigger-validation");
        await triggerButton.ClickAsync();

        // Wait for error summary to be visible
        var errorSummary = GetByTestId("error-summary");
        await Expect(errorSummary).ToBeVisibleAsync();

        // Verify ErrorSummary does NOT have role="alert" or aria-live attributes
        // These are explicitly NOT recommended per the accessibility guidelines
        await Expect(errorSummary).Not.ToHaveAttributeAsync("role", "alert");
        await Expect(errorSummary).Not.ToHaveAttributeAsync("aria-live", "polite");
        await Expect(errorSummary).Not.ToHaveAttributeAsync("aria-live", "assertive");
    }

    #endregion
}