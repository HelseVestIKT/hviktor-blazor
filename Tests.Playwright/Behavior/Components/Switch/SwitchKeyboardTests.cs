using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Switch;

/// <summary>
/// Keyboard behavior tests for the Switch component.
/// Tests actual keyboard navigation behavior as per accessibility requirements:
/// - Space toggles the switch on/off state
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class SwitchKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "switch";

    #region Toggle with Space Key

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Switch_SpaceKey_TogglesOnOff()
    {
        await GoToPageAsync("state");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        var switchElement = GetByTestId("unchecked");
        await Expect(switchElement).ToBeFocusedAsync();
        await Expect(switchElement).Not.ToBeCheckedAsync();

        await PressSpaceAsync();
        await Expect(switchElement).ToBeCheckedAsync();

        await PressSpaceAsync();
        await Expect(switchElement).Not.ToBeCheckedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Switch_SpaceKey_TogglesFromCheckedState()
    {
        await GoToPageAsync("state");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();
        await PressTabAsync();

        var switchElement = GetByTestId("checked");
        await Expect(switchElement).ToBeFocusedAsync();
        await Expect(switchElement).ToBeCheckedAsync();

        await PressSpaceAsync();
        await Expect(switchElement).Not.ToBeCheckedAsync();

        await PressSpaceAsync();
        await Expect(switchElement).ToBeCheckedAsync();
    }

    #endregion

    #region Disabled State Tests

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Switch_Disabled_IsSkippedByTabNavigation()
    {
        await GoToPageAsync("disabled");

        var disabledUnchecked = GetByTestId("disabled-unchecked");
        var disabledChecked = GetByTestId("disabled-checked");

        await Expect(disabledUnchecked).ToBeDisabledAsync();
        await Expect(disabledChecked).ToBeDisabledAsync();

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        await Expect(disabledUnchecked).Not.ToBeFocusedAsync();
        await Expect(disabledChecked).Not.ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Switch_Disabled_MaintainsCheckedState()
    {
        await GoToPageAsync("disabled");

        var disabledUnchecked = GetByTestId("disabled-unchecked");
        var disabledChecked = GetByTestId("disabled-checked");

        await Expect(disabledUnchecked).ToBeDisabledAsync();
        await Expect(disabledUnchecked).Not.ToBeCheckedAsync();

        await Expect(disabledChecked).ToBeDisabledAsync();
        await Expect(disabledChecked).ToBeCheckedAsync();
    }

    #endregion

    #region Tab Navigation Tests

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Switch_CanReceiveFocusViaTab()
    {
        await GoToPageAsync("state");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        var switchElement = GetByTestId("unchecked");
        await Expect(switchElement).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Switch_TabNavigatesBetweenSwitches()
    {
        await GoToPageAsync("state");

        var firstSwitch = GetByTestId("unchecked");
        var secondSwitch = GetByTestId("checked");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();
        await Expect(firstSwitch).ToBeFocusedAsync();

        await PressTabAsync();
        await Expect(secondSwitch).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Switch_ShiftTabNavigatesBackward()
    {
        await GoToPageAsync("state");

        var firstSwitch = GetByTestId("unchecked");
        var secondSwitch = GetByTestId("checked");

        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();
        await PressTabAsync();
        await Expect(secondSwitch).ToBeFocusedAsync();

        await PressShiftTabAsync();
        await Expect(firstSwitch).ToBeFocusedAsync();
    }

    #endregion
}