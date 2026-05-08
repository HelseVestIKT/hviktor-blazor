using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Textarea;

/// <summary>
/// Keyboard behavior tests for the Textarea component.
/// Tests keyboard navigation and interaction:
/// - Tab navigates to textarea
/// - Text input works correctly
/// - Disabled textarea cannot receive focus via Tab
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TextareaKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "textarea";

    #region Tab Navigation Tests

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textarea_CanReceiveFocusViaTab()
    {
        await GoToPageAsync("basic");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();

        // Tab to the first textarea
        await PressTabAsync();

        var textarea = GetByTestId("basic");
        await Expect(textarea).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textarea_TabNavigatesBetweenTextareas()
    {
        await GoToPageAsync("basic");

        var firstTextarea = GetByTestId("basic");
        var secondTextarea = GetByTestId("empty");

        // Focus the first textarea
        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();
        await Expect(firstTextarea).ToBeFocusedAsync();

        // Tab to the second textarea
        await PressTabAsync();
        await Expect(secondTextarea).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textarea_Disabled_IsSkippedByTabNavigation()
    {
        await GoToPageAsync("state");

        var disabledTextarea = GetByTestId("disabled");
        var readonlyTextarea = GetByTestId("readonly");

        // Verify disabled textarea is actually disabled
        await Expect(disabledTextarea).ToBeDisabledAsync();

        // Tab from focus-start
        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        // Disabled textarea should be skipped, focus goes to readonly
        await Expect(disabledTextarea).Not.ToBeFocusedAsync();
        await Expect(readonlyTextarea).ToBeFocusedAsync();
    }

    #endregion

    #region Text Input Tests

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textarea_CanTypeText()
    {
        await GoToPageAsync("basic");

        var textarea = GetByTestId("empty");
        await textarea.FocusAsync();

        await textarea.FillAsync("Hello, World!");

        await Expect(textarea).ToHaveValueAsync("Hello, World!");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textarea_CanTypeMultilineText()
    {
        await GoToPageAsync("basic");

        var textarea = GetByTestId("empty");
        await textarea.FocusAsync();

        await textarea.FillAsync("Line 1\nLine 2\nLine 3");

        await Expect(textarea).ToHaveValueAsync("Line 1\nLine 2\nLine 3");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textarea_EnterKey_InsertsNewLine()
    {
        await GoToPageAsync("basic");

        var textarea = GetByTestId("empty");
        await textarea.FocusAsync();

        await textarea.PressSequentiallyAsync("First line");
        await PressEnterAsync();
        await textarea.PressSequentiallyAsync("Second line");

        var value = await textarea.InputValueAsync();
        Assert.Contains("\n", value);
    }

    #endregion

    #region Readonly Tests

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textarea_Readonly_CanReceiveFocus()
    {
        await GoToPageAsync("state");

        var readonlyTextarea = GetByTestId("readonly");

        await readonlyTextarea.FocusAsync();
        await Expect(readonlyTextarea).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textarea_Readonly_CannotBeEdited()
    {
        await GoToPageAsync("state");

        var readonlyTextarea = GetByTestId("readonly");
        var originalValue = await readonlyTextarea.InputValueAsync();

        await readonlyTextarea.FocusAsync();
        await readonlyTextarea.PressSequentiallyAsync("New text");

        // Value should remain unchanged
        await Expect(readonlyTextarea).ToHaveValueAsync(originalValue);
    }

    #endregion

    #region Disabled Tests

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Textarea_Disabled_MaintainsValue()
    {
        await GoToPageAsync("state");

        var disabledTextarea = GetByTestId("disabled");

        // Verify disabled textarea maintains its value
        await Expect(disabledTextarea).ToBeDisabledAsync();
        await Expect(disabledTextarea).ToHaveValueAsync("This textarea is disabled");
    }

    #endregion
}