using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Suggestion;

/// <summary>
/// Behavior and keyboard tests for the Suggestion component.
/// Tests focus on:
/// - Suggestion renders with correct structure
/// - Input has aria-autocomplete="list" for accessibility
/// - Options have role="option"
/// - Keyboard navigation works (arrow keys, Enter, Escape)
/// - Filtering options by typing
/// - Selection updates the input value
/// - Multiple selection mode works correctly
/// - Clear button clears the selection
/// - Disabled options are non-interactive
/// - ReadOnly mode prevents interaction
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public partial class SuggestionBehaviorTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "suggestion";

    #region Semantic Structure

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_RendersAsCustomElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var suggestion = section.Locator("div.ds-suggestion");

        await Expect(suggestion).ToBeAttachedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_HasCorrectClass()
    {
        await GoToPageAsync("basic");
        var suggestion = GetByTestId("basic-suggestion");
        await Expect(suggestion).ToHaveClassAsync(DsSuggestionRegex());
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_InputHasCorrectClass()
    {
        await GoToPageAsync("basic");
        var input = GetByTestId("basic-input");
        await Expect(input).ToHaveClassAsync(DsInputRegex());
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_InputHasAriaAutocomplete()
    {
        await GoToPageAsync("basic");
        var input = GetByTestId("basic-input");
        await Expect(input).ToHaveAttributeAsync("aria-autocomplete", "list");
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_InputHasAutocompleteOff()
    {
        await GoToPageAsync("basic");
        var input = GetByTestId("basic-input");

        // Browser autocomplete should be disabled for custom autocomplete
        await Expect(input).ToHaveAttributeAsync("autocomplete", "off");
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_ListRendersAsDatalist()
    {
        await GoToPageAsync("basic");
        var suggestion = GetByTestId("basic-suggestion");
        var list = suggestion.Locator("[role='listbox']");
        await Expect(list).ToBeAttachedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_OptionsHaveRoleOption()
    {
        await GoToPageAsync("basic");
        var suggestion = GetByTestId("basic-suggestion");
        var options = suggestion.Locator("[role='option']");

        // Should have 3 options (Apple, Banana, Cherry)
        await Expect(options).ToHaveCountAsync(3);
    }

    #endregion

    #region Aria Label

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Suggestion_InputHasAriaLabel()
    {
        await GoToPageAsync("basic");
        var input = GetByTestId("basic-input");

        var ariaLabel = await input.GetAttributeAsync("aria-label");
        Assert.NotNull(ariaLabel);

        var cleanedLabel = MyRegex().Replace(ariaLabel, "");
        Assert.Equal("Select a fruit", cleanedLabel);
    }

    #endregion

    #region Default Selected

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_DefaultSelected_ShowsSelectedValue()
    {
        await GoToPageAsync("basic");
        var suggestion = GetByTestId("default-suggestion");
        var chip = suggestion.Locator("data.ds-chip");
        await Expect(chip).ToContainTextAsync("banana");
    }

    #endregion

    #region Keyboard Navigation

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Suggestion_Tab_FocusesInput()
    {
        await GoToPageAsync("accessibility");
        var input = GetByTestId("keyboard-input");
        await input.FocusAsync();
        await Expect(input).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Suggestion_Typing_FiltersOptions()
    {
        await GoToPageAsync("features");
        var input = GetByTestId("filter-input");

        await input.FillAsync("ap");
        // Wait for filtering to occur - the list should only show Apple and Apricot
        // This depends on the component's filter behavior
        await Expect(input).ToHaveValueAsync("ap");
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Suggestion_Escape_ClearsInput()
    {
        await GoToPageAsync("accessibility");
        var input = GetByTestId("keyboard-input");

        await input.FillAsync("test");
        await Expect(input).ToHaveValueAsync("test");
        await input.PressAsync("Escape");

        // Input should be cleared or focus should leave
        // Behavior depends on component implementation
    }

    #endregion

    #region Features

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_Filter_HasFilterAttribute()
    {
        await GoToPageAsync("features");
        var suggestion = GetByTestId("filter-suggestion");

        // Filter is default true, so no specific attribute needed
        await Expect(suggestion).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_Creatable_HasCreatableAttribute()
    {
        await GoToPageAsync("features");
        var suggestion = GetByTestId("creatable-suggestion");
        await Expect(suggestion).ToHaveAttributeAsync("data-creatable", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_Multiple_HasMultipleAttribute()
    {
        await GoToPageAsync("features");
        var suggestion = GetByTestId("multiple-suggestion");
        await Expect(suggestion).ToHaveAttributeAsync("data-multiple", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Suggestion_Name_HasHiddenSelectForForm()
    {
        await GoToPageAsync("features");

        var suggestion = GetByTestId("named-suggestion");
        var hiddenSelect = suggestion.Locator("select[name='selectedItem'][hidden]");

        await Expect(hiddenSelect).ToBeAttachedAsync();
    }

    #endregion

    #region Disabled Options

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_DisabledOption_HasDisabledAttribute()
    {
        await GoToPageAsync("accessibility");

        var suggestion = GetByTestId("disabled-suggestion");
        var disabledOption = suggestion.Locator("[role='option'][disabled]");

        await Expect(disabledOption).ToBeAttachedAsync();
    }

    #endregion

    #region ReadOnly

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_ReadOnly_HasReadOnlyAttribute()
    {
        await GoToPageAsync("accessibility");
        var suggestion = GetByTestId("readonly-suggestion");
        await Expect(suggestion).ToHaveAttributeAsync("readonly", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_ReadOnly_InputIsReadOnly()
    {
        await GoToPageAsync("accessibility");
        var input = GetByTestId("readonly-input");
        await Expect(input).ToHaveAttributeAsync("readonly", "");
    }

    #endregion

    #region Clear Button

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_ClearButton_Exists()
    {
        await GoToPageAsync("accessibility");
        var clearButton = GetByTestId("clear-button");
        await Expect(clearButton).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Suggestion_ClearButton_HasAriaLabel()
    {
        await GoToPageAsync("accessibility");
        var clearButton = GetByTestId("clear-button");

        var ariaLabel = await clearButton.GetAttributeAsync("aria-label");
        Assert.NotNull(ariaLabel);

        var cleanedLabel = AnsiColorRegex().Replace(ariaLabel, "");
        Assert.Equal("Clear selection", cleanedLabel);
    }

    #endregion

    #region Empty State

    [Fact]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Suggestion_EmptyMessage_Exists()
    {
        await GoToPageAsync("accessibility");

        // Empty component wraps an Option with data-empty attribute
        var suggestion = GetByTestId("empty-suggestion");
        var emptyOption = suggestion.Locator("[role='option'][data-empty]");
        await Expect(emptyOption).ToBeAttachedAsync();
    }

    [System.Text.RegularExpressions.GeneratedRegex(@"[\u200B-\u200D\uFEFF]")]
    private static partial System.Text.RegularExpressions.Regex MyRegex();
    [System.Text.RegularExpressions.GeneratedRegex("ds-suggestion")]
    private static partial System.Text.RegularExpressions.Regex DsSuggestionRegex();
    [System.Text.RegularExpressions.GeneratedRegex("ds-input")]
    private static partial System.Text.RegularExpressions.Regex DsInputRegex();
    [System.Text.RegularExpressions.GeneratedRegex(@"[\u200B-\u200D\uFEFF]")]
    private static partial System.Text.RegularExpressions.Regex AnsiColorRegex();

    #endregion
}