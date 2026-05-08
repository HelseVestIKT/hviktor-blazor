using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Checkbox;

/// <summary>
/// Keyboard behavior tests for the Checkbox component.
/// Tests actual keyboard navigation behavior as per accessibility requirements:
/// - Space toggles the checkbox checked state
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class CheckboxKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "checkbox";

    #region Toggle with Space Key

    [Fact]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Checkbox_SpaceKey_TogglesChecked()
    {
        await GoToPageAsync("state");

        var checkbox = GetByTestId("unchecked");
        await checkbox.FocusAsync();

        // Verify initially unchecked
        await Expect(checkbox).Not.ToBeCheckedAsync();

        // Press Space to check
        await PressSpaceAsync();

        // Verify now checked
        await Expect(checkbox).ToBeCheckedAsync();

        // Press Space again to uncheck
        await PressSpaceAsync();

        // Verify now unchecked
        await Expect(checkbox).Not.ToBeCheckedAsync();
    }

    #endregion
}