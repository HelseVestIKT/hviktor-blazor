using Microsoft.Playwright;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Composition;

/// <summary>
/// Composition tests for User Profile Form.
/// Tests component integration: Card, Avatar, Form fields, Alerts, Buttons.
/// </summary>
[Collection(TestCollections.Composition)]
[Trait(Traits.Collection, TestCollections.Composition)]
public class UserProfileFormTests(TestsFixture fixture) : CompositionTestBase<TestsFixture>(fixture)
{
    #region Initial Render Tests

    [Fact]
    [Trait(Traits.Component, "Heading")]
    [Trait(Traits.Component, "Avatar")]
    [Trait(Traits.Component, "Field")]
    [Trait(Traits.Component, "Input")]
    [Trait(Traits.Component, "Button")]
    public async Task Page_RendersAllComponents()
    {
        await GoToPageAsync("user-profile-form");

        await Expect(Page.GetByTestId("form-title")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-avatar")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("firstname-input")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("lastname-input")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("email-input")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("submit-button")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("cancel-button")).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Avatar")]
    public async Task Avatar_DisplaysCorrectInitials()
    {
        await GoToPageAsync("user-profile-form");

        var avatar = Page.GetByTestId("user-avatar");
        await Expect(avatar).ToHaveAttributeAsync("data-initials", "ON");
    }

    [Fact]
    [Trait(Traits.Component, "Avatar")]
    public async Task DisplayName_ShowsFullName()
    {
        await GoToPageAsync("user-profile-form");

        var name = Page.GetByTestId("avatar-name");
        await Expect(name).ToHaveTextAsync("Ola Nordmann");
    }

    #endregion

    #region Form Interaction Tests

    [Fact]
    [Trait(Traits.Component, "Button")]
    [Trait(Traits.Component, "Alert")]
    public async Task Form_SubmitWithValidData_ShowsSuccessAlert()
    {
        await GoToPageAsync("user-profile-form");

        await Page.GetByTestId("submit-button").ClickAsync();

        await Expect(Page.GetByTestId("success-alert")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("error-alert")).Not.ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Input")]
    [Trait(Traits.Component, "Button")]
    [Trait(Traits.Component, "Alert")]
    public async Task Form_SubmitWithEmptyFirstName_ShowsErrorAlert()
    {
        await GoToPageAsync("user-profile-form");

        await Page.GetByTestId("firstname-input").FillAsync("");
        await Page.GetByTestId("submit-button").ClickAsync();

        await Expect(Page.GetByTestId("error-alert")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("success-alert")).Not.ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Input")]
    [Trait(Traits.Component, "Button")]
    public async Task Form_CancelButton_ResetsToDefaultValues()
    {
        await GoToPageAsync("user-profile-form");

        // Modify values
        await Page.GetByTestId("firstname-input").FillAsync("Test");
        await Page.GetByTestId("lastname-input").FillAsync("User");

        // Cancel
        await Page.GetByTestId("cancel-button").ClickAsync();

        // Verify reset
        await Expect(Page.GetByTestId("firstname-input")).ToHaveValueAsync("Ola");
        await Expect(Page.GetByTestId("lastname-input")).ToHaveValueAsync("Nordmann");
    }

    #endregion

    #region Avatar Update Tests

    [Fact]
    [Trait(Traits.Component, "Input")]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Component, "Avatar")]
    public async Task Avatar_UpdatesInitials_WhenNameChanges()
    {
        await GoToPageAsync("user-profile-form");

        await Page.GetByTestId("firstname-input").FillAsync("Kari");
        await Page.GetByTestId("lastname-input").FillAsync("Hansen");

        // Trigger blur to update binding
        await Page.GetByTestId("bio-textarea").FocusAsync();

        var avatar = Page.GetByTestId("user-avatar");
        await Expect(avatar).ToHaveAttributeAsync("data-initials", "KH");
    }

    #endregion

    #region Switch Toggle Tests

    [Fact]
    [Trait(Traits.Component, "Switch")]
    public async Task EmailNotifications_DefaultsToChecked()
    {
        await GoToPageAsync("user-profile-form");

        var toggle = Page.GetByTestId("email-notifications-switch");
        await Expect(toggle).ToBeCheckedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    public async Task MarketingEmails_DefaultsToUnchecked()
    {
        await GoToPageAsync("user-profile-form");

        var toggle = Page.GetByTestId("marketing-emails-switch");
        await Expect(toggle).Not.ToBeCheckedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    public async Task Switch_TogglesWhenClicked()
    {
        await GoToPageAsync("user-profile-form");

        var toggle = Page.GetByTestId("marketing-emails-switch");
        await toggle.ClickAsync();

        await Expect(toggle).ToBeCheckedAsync();
    }

    #endregion

    #region Keyboard Navigation Tests

    [Fact]
    [Trait(Traits.Component, "Input")]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Form_CanBeNavigatedWithKeyboard()
    {
        await GoToPageAsync("user-profile-form");

        // Focus first input
        await Page.GetByTestId("firstname-input").FocusAsync();

        // Tab through form fields
        await Page.Keyboard.PressAsync("Tab");
        await Expect(Page.GetByTestId("lastname-input")).ToBeFocusedAsync();

        await Page.Keyboard.PressAsync("Tab");
        await Expect(Page.GetByTestId("email-input")).ToBeFocusedAsync();

        await Page.Keyboard.PressAsync("Tab");
        await Expect(Page.GetByTestId("bio-textarea")).ToBeFocusedAsync();
    }

    #endregion

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
}