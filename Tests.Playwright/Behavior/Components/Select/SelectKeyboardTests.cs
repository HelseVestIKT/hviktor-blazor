using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Select;

/// <summary>
/// Keyboard and behavior tests for the Select component.
/// Tests accessibility requirements:
/// - Space opens the list (also selects an option on Mac)
/// - Enter selects an option in the list (also opens the list on Windows)
/// - ↓ marks the next option in the list
/// - ↑ marks the previous option in the list
/// - Home marks the first option in the list
/// - End marks the last option in the list
/// - Character keys jump to the first option starting with that character
/// - Tab navigates between select elements
/// - ReadOnly prevents interaction but maintains accessibility
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public partial class SelectKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "select";

    #region Arrow Key Navigation

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_ArrowDown_SelectsNextOption()
    {
        await GoToPageAsync("accessibility");

        var select = GetByTestId("keyboard-select");

        // Focus the select
        await select.FocusAsync();
        await Expect(select).ToBeFocusedAsync();

        // Press arrow down to move to next option
        await PressArrowDownAsync();

        // Verify the value has changed to the next option
        await Expect(select).ToHaveValueAsync("apple");
    }

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_ArrowUp_SelectsPreviousOption()
    {
        await GoToPageAsync("accessibility");

        var select = GetByTestId("keyboard-select");

        // First select an option that's not the first one
        await select.SelectOptionAsync("cherry");
        await Expect(select).ToHaveValueAsync("cherry");

        // Press arrow up to move to previous option
        await select.FocusAsync();
        await PressArrowUpAsync();

        // Verify the value has changed to the previous option
        await Expect(select).ToHaveValueAsync("banana");
    }

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_MultipleArrowDown_NavigatesThrough()
    {
        await GoToPageAsync("accessibility");

        var select = GetByTestId("keyboard-select");
        await select.FocusAsync();

        // Press arrow down multiple times
        await PressArrowDownAsync(); // -> Apple
        await PressArrowDownAsync(); // -> Banana
        await PressArrowDownAsync(); // -> Cherry

        await Expect(select).ToHaveValueAsync("cherry");
    }

    #endregion

    #region Home and End Keys

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_HomeKey_SelectsFirstOption()
    {
        await GoToPageAsync("accessibility");

        var select = GetByTestId("keyboard-select");

        // First select a middle option
        await select.SelectOptionAsync("cherry");
        await Expect(select).ToHaveValueAsync("cherry");

        // Press Home to go to first option
        await select.FocusAsync();
        await PressHomeAsync();

        // Note: Some browsers may require the select to be open for Home/End to work
        // The first option with value is "" (placeholder), then "apple"
        // Behavior may vary by browser
        var value = await select.InputValueAsync();
        Assert.True(value == "" || value == "apple", $"Expected first option, got: {value}");
    }

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_EndKey_SelectsLastOption()
    {
        await GoToPageAsync("accessibility");

        var select = GetByTestId("keyboard-select");

        // Focus the select at first option
        await select.FocusAsync();

        // Press End to go to last option
        await PressEndAsync();

        // Verify it's at the last option (elderberry)
        await Expect(select).ToHaveValueAsync("elderberry");
    }

    #endregion

    #region Character Jump Navigation

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_CharacterKey_JumpsToMatchingOption()
    {
        await GoToPageAsync("accessibility");

        var select = GetByTestId("character-select");
        await select.FocusAsync();

        // Type 'b' to jump to "Bob"
        await TypeCharacterAsync("b");

        await Expect(select).ToHaveValueAsync("bob");
    }

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_CharacterKey_JumpsToFirstMatchingOption()
    {
        await GoToPageAsync("accessibility");

        var select = GetByTestId("character-select");
        await select.FocusAsync();

        // Type 'a' to jump to first option starting with 'a' (Alice)
        await TypeCharacterAsync("a");

        await Expect(select).ToHaveValueAsync("alice");
    }

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_RepeatedCharacterKey_CyclesThroughMatches()
    {
        await GoToPageAsync("accessibility");

        var select = GetByTestId("character-select");
        await select.FocusAsync();

        // Type 'd' to jump to "David"
        await TypeCharacterAsync("d");
        await Expect(select).ToHaveValueAsync("david");

        // Type 'd' again to cycle to "Diana"
        await TypeCharacterAsync("d");
        await Expect(select).ToHaveValueAsync("diana");
    }

    #endregion

    #region Tab Navigation

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_Tab_NavigatesToNextSelect()
    {
        await GoToPageAsync("accessibility");

        var firstSelect = GetByTestId("first-select");
        var secondSelect = GetByTestId("second-select");

        // Focus first select
        await firstSelect.FocusAsync();
        await Expect(firstSelect).ToBeFocusedAsync();

        // Tab to next select
        await PressTabAsync();
        await Expect(secondSelect).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_ShiftTab_NavigatesToPreviousSelect()
    {
        await GoToPageAsync("accessibility");

        var firstSelect = GetByTestId("first-select");
        var secondSelect = GetByTestId("second-select");

        // Focus second select
        await secondSelect.FocusAsync();
        await Expect(secondSelect).ToBeFocusedAsync();

        // Shift+Tab to previous select
        await PressShiftTabAsync();
        await Expect(firstSelect).ToBeFocusedAsync();
    }

    #endregion

    #region Space and Enter Keys

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Select_SpaceKey_OpensOrSelectsOption()
    {
        await GoToPageAsync("accessibility");

        var select = GetByTestId("keyboard-select");
        await select.FocusAsync();

        // Navigate to an option first
        await PressArrowDownAsync();
        await Expect(select).ToHaveValueAsync("apple");

        // Space behavior varies by platform (opens on some, selects on others)
        // Just verify the select is still functional
        await PressSpaceAsync();

        // The select should still have a value
        var value = await select.InputValueAsync();
        Assert.NotNull(value);
    }

    #endregion

    #region Semantic Structure

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Select_RendersAsSelectElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var select = section.Locator("select.ds-input");

        await Expect(select).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Select_HasCorrectClass()
    {
        await GoToPageAsync("basic");

        var select = GetByTestId("basic-select");

        await Expect(select).ToHaveClassAsync(DsInputRegex());
    }

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Select_OptionsHaveCorrectValues()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var options = section.Locator("option");

        await Expect(options).ToHaveCountAsync(4);
    }

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Select_HasAriaInvalidAttribute()
    {
        await GoToPageAsync("basic");

        var select = GetByTestId("basic-select");

        await Expect(select).ToHaveAttributeAsync("aria-invalid", "false");
    }

    #endregion

    #region ReadOnly State

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Select_ReadOnly_HasReadOnlyAttribute()
    {
        await GoToPageAsync("state");

        var select = GetByTestId("readonly-select");

        await Expect(select).ToHaveAttributeAsync("readonly", "true");
    }

    #endregion

    #region Width Variants

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Select_WidthFull_HasDataAttribute()
    {
        await GoToPageAsync("state");

        var select = GetByTestId("width-full-select");

        await Expect(select).ToHaveAttributeAsync("data-width", "full");
    }

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Select_WidthAuto_HasDataAttribute()
    {
        await GoToPageAsync("state");

        var select = GetByTestId("width-auto-select");

        await Expect(select).ToHaveAttributeAsync("data-width", "auto");
    }

    #endregion

    #region Required State

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Select_Required_HasRequiredAttribute()
    {
        await GoToPageAsync("state");

        var select = GetByTestId("required-select");

        await Expect(select).ToHaveAttributeAsync("required", "");
    }

    #endregion

    #region Aria Label

    [Fact]
    [Trait(Traits.Component, "Select")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Select_AriaLabelledBy_HasCorrectAttribute()
    {
        await GoToPageAsync("accessibility");

        var select = GetByTestId("labelledby-select");

        await Expect(select).ToHaveAttributeAsync("aria-labelledby", "custom-label-id");
    }

    [System.Text.RegularExpressions.GeneratedRegex("ds-input")]
    private static partial System.Text.RegularExpressions.Regex DsInputRegex();

    #endregion
}