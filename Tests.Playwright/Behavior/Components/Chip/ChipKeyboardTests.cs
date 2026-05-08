using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Chip;

/// <summary>
/// Keyboard behavior tests for the Chip component (Radio and Checkbox variants).
/// Tests actual keyboard navigation behavior as per accessibility requirements:
/// - Space/Enter selects radio chip
/// - Space toggles checkbox chip
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class ChipKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "chip";

    #region Chip Radio - Space Key

    [Fact]
    [Trait(Traits.Component, "Chip")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ChipRadio_SpaceKey_SelectsRadio()
    {
        await GoToPageAsync("radio");

        // Find the unchecked radio chip input
        var radioInput = GetByTestId("radio-unchecked").Locator("input[type='radio']");
        await radioInput.FocusAsync();

        // Verify initially unchecked
        await Expect(radioInput).Not.ToBeCheckedAsync();

        // Press Space to select
        await PressSpaceAsync();

        // Verify now checked
        await Expect(radioInput).ToBeCheckedAsync();
    }

    #endregion

    #region Chip Checkbox - Space Key

    [Fact]
    [Trait(Traits.Component, "Chip")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task ChipCheckbox_SpaceKey_TogglesChecked()
    {
        await GoToPageAsync("checkbox");

        // Find the unchecked checkbox chip input
        var checkboxInput = GetByTestId("checkbox-unchecked").Locator("input[type='checkbox']");
        await checkboxInput.FocusAsync();

        // Verify initially unchecked
        await Expect(checkboxInput).Not.ToBeCheckedAsync();

        // Press Space to check
        await PressSpaceAsync();

        // Verify now checked
        await Expect(checkboxInput).ToBeCheckedAsync();

        // Press Space again to uncheck
        await PressSpaceAsync();

        // Verify now unchecked
        await Expect(checkboxInput).Not.ToBeCheckedAsync();
    }

    #endregion
}
