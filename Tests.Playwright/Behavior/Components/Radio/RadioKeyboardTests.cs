using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Radio;

/// <summary>
/// Keyboard and behavior tests for the Radio component.
/// Tests accessibility requirements:
/// - Arrow keys (↑ ↓ ← →) navigate between options in a group
/// - Tab navigates to the whole radio group, not individual options
/// - Space selects first option when no option is pre-selected
/// - Radio buttons should be in a fieldset with legend
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class RadioKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "radio";

    #region Arrow Key Navigation

    [Fact]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Radio_ArrowDown_SelectsNextOption()
    {
        await GoToPageAsync("accessibility");

        var smallRadio = GetByTestId("size-small");
        var mediumRadio = GetByTestId("size-medium");

        // Focus and select first radio
        await smallRadio.FocusAsync();
        await Expect(smallRadio).ToBeFocusedAsync();

        // Press arrow down to move to next
        await PressArrowDownAsync();
        await Expect(mediumRadio).ToBeFocusedAsync();
        await Expect(mediumRadio).ToBeCheckedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Radio_ArrowUp_SelectsPreviousOption()
    {
        await GoToPageAsync("accessibility");

        var mediumRadio = GetByTestId("size-medium");
        var largeRadio = GetByTestId("size-large");

        // Focus and select last radio
        await largeRadio.ClickAsync();
        await Expect(largeRadio).ToBeCheckedAsync();

        // Press arrow up to move to previous
        await PressArrowUpAsync();
        await Expect(mediumRadio).ToBeFocusedAsync();
        await Expect(mediumRadio).ToBeCheckedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Radio_ArrowRight_SelectsNextOption()
    {
        await GoToPageAsync("accessibility");

        var smallRadio = GetByTestId("size-small");
        var mediumRadio = GetByTestId("size-medium");

        // Focus and select first radio
        await smallRadio.FocusAsync();
        await Expect(smallRadio).ToBeFocusedAsync();

        // Press arrow right to move to next
        await PressArrowRightAsync();
        await Expect(mediumRadio).ToBeFocusedAsync();
        await Expect(mediumRadio).ToBeCheckedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Radio_ArrowLeft_SelectsPreviousOption()
    {
        await GoToPageAsync("accessibility");

        var mediumRadio = GetByTestId("size-medium");
        var largeRadio = GetByTestId("size-large");

        // Focus and select last radio
        await largeRadio.ClickAsync();
        await Expect(largeRadio).ToBeCheckedAsync();

        // Press arrow left to move to previous
        await PressArrowLeftAsync();
        await Expect(mediumRadio).ToBeFocusedAsync();
        await Expect(mediumRadio).ToBeCheckedAsync();
    }

    #endregion

    #region Tab Navigation

    [Fact]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Radio_Tab_NavigatesBetweenGroups()
    {
        await GoToPageAsync("accessibility");

        var group1Radio = GetByTestId("group1-a");
        var group2Radio = GetByTestId("group2-x");

        // Focus first group's first radio
        await group1Radio.FocusAsync();
        await Expect(group1Radio).ToBeFocusedAsync();

        // Tab should move to next group, not next radio in same group
        await PressTabAsync();
        await Expect(group2Radio).ToBeFocusedAsync();
    }

    #endregion

    #region Semantic Structure

    [Fact]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Radio_RendersAsInputTypeRadio()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic-group");
        var radios = section.Locator("input[type='radio']");

        await Expect(radios).ToHaveCountAsync(3);
    }

    [Fact]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Radio_HasAssociatedLabel()
    {
        await GoToPageAsync("basic");

        var radio = GetByTestId("radio-1");
        var radioId = await radio.GetAttributeAsync("id");

        Assert.NotNull(radioId);

        // Verify label exists and points to this radio
        var label = Locator($"label[for='{radioId}']");
        await Expect(label).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Radio_GroupedInFieldset()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic-group");
        var fieldset = section.Locator("fieldset");
        var legend = fieldset.Locator("legend");

        await Expect(fieldset).ToBeVisibleAsync();
        await Expect(legend).ToBeVisibleAsync();
        await Expect(legend).ToHaveTextAsync("Choose an option");
    }

    [Fact]
    [Trait(Traits.Component, "Radio")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Radio_WithDescription_HasAriaDescribedby()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("with-description");
        var radio = section.Locator("input[type='radio']").First;
        var ariaDescribedby = await radio.GetAttributeAsync("aria-describedby");

        Assert.NotNull(ariaDescribedby);
    }

    #endregion
}