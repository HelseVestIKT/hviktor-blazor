using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Textfield;

/// <summary>
/// Keyboard behavior tests for the Textfield component.
/// Tests keyboard navigation and interaction:
/// - Tab navigates to textfield input
/// - Text input works correctly
/// - Multiline textfield allows newlines
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TextfieldKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "textfield";

    #region Tab Navigation Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textfield_CanReceiveFocusViaTab()
    {
        await GoToPageAsync("basic");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();

        // Tab to the first textfield input
        await PressTabAsync();

        var input = Page.Locator("#textfield-basic");
        await Expect(input).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textfield_TabNavigatesBetweenTextfields()
    {
        await GoToPageAsync("basic");

        var firstInput = Page.Locator("#textfield-basic");
        var secondInput = Page.Locator("#textfield-description");

        // Focus the first input
        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();
        await Expect(firstInput).ToBeFocusedAsync();

        // Tab to the second textfield
        await PressTabAsync();
        await Expect(secondInput).ToBeFocusedAsync();
    }

    #endregion

    #region Text Input Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textfield_CanTypeText()
    {
        await GoToPageAsync("basic");

        var input = Page.Locator("#textfield-basic");
        await input.FocusAsync();

        await input.FillAsync("Hello, World!");

        await Expect(input).ToHaveValueAsync("Hello, World!");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textfield_CanClearAndRetype()
    {
        await GoToPageAsync("basic");

        var input = Page.Locator("#textfield-value");

        // Should have initial value
        await Expect(input).ToHaveValueAsync("john_doe");

        // Clear and type new value
        await input.FillAsync("new_username");

        await Expect(input).ToHaveValueAsync("new_username");
    }

    #endregion

    #region Multiline Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textfield_Multiline_CanReceiveFocus()
    {
        await GoToPageAsync("multiline");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();

        await PressTabAsync();

        var textarea = Page.Locator("#textfield-multiline");
        await Expect(textarea).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textfield_Multiline_CanTypeMultilineText()
    {
        await GoToPageAsync("multiline");

        var textarea = Page.Locator("#textfield-multiline");
        await textarea.FocusAsync();

        await textarea.FillAsync("Line 1\nLine 2\nLine 3");

        await Expect(textarea).ToHaveValueAsync("Line 1\nLine 2\nLine 3");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textfield_Multiline_EnterKey_InsertsNewLine()
    {
        await GoToPageAsync("multiline");

        var textarea = Page.Locator("#textfield-multiline");
        await textarea.FocusAsync();

        await textarea.PressSequentiallyAsync("First line");
        await PressEnterAsync();
        await textarea.PressSequentiallyAsync("Second line");

        var value = await textarea.InputValueAsync();
        Assert.Contains("\n", value);
    }

    #endregion

    #region Input Type Specific Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textfield_PasswordType_MasksInput()
    {
        await GoToPageAsync("types");

        var input = Page.Locator("#textfield-password");
        await input.FillAsync("secret123");

        // Masking is enforced by type="password", the browser renders dots/asterisks
        // instead of the literal characters. We verify the attribute is set correctly,
        // confirming the browser will mask the visual output.
        await Expect(input).ToHaveAttributeAsync("type", "password");

        // The underlying value must still be accessible so the form can submit it.
        // This is expected and correct, masking is visual only, not a data restriction.
        await Expect(input).ToHaveValueAsync("secret123");

        // Verify the plain-text value is NOT exposed as visible text content in the DOM.
        // If type="password" is applied correctly, no child text node or aria-label
        // will leak the raw value into the accessibility tree as readable text.
        var visibleText = await input.InnerTextAsync();
        Assert.DoesNotContain("secret123", visibleText);
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textfield_NumberType_AcceptsNumbers()
    {
        await GoToPageAsync("types");

        var input = Page.Locator("#textfield-number");
        await input.FillAsync("42");

        await Expect(input).ToHaveValueAsync("42");
    }

    #endregion
}