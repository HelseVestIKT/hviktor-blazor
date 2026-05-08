using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Tooltip;

/// <summary>
/// Semantic behavior tests for the Tooltip component.
/// Tests proper HTML structure and accessibility features:
/// - Trigger element has data-tooltip attribute
/// - Trigger element has data-placement attribute
/// - ARIA attributes are applied by JS based on data-tooltip-type
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TooltipSemanticTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "tooltip";

    #region Basic Structure Tests

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tooltip_TriggerHasDataTooltipAttribute()
    {
        await GoToPageAsync("basic");
        var trigger = GetByTestId("basic-trigger");
        await Expect(trigger).ToHaveAttributeAsync("data-tooltip", "This is a tooltip");
    }

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tooltip_TriggerButton_IsVisible()
    {
        await GoToPageAsync("basic");
        var trigger = GetByTestId("basic-trigger");
        await Expect(trigger).ToBeVisibleAsync();
        await Expect(trigger).ToHaveTextAsync("Hover me");
    }

    #endregion

    #region Placement Tests

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tooltip_TopPlacement_HasCorrectAttribute()
    {
        await GoToPageAsync("placement");
        var trigger = GetByTestId("top-trigger");
        await Expect(trigger).ToHaveAttributeAsync("data-placement", "top");
    }

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tooltip_RightPlacement_HasCorrectAttribute()
    {
        await GoToPageAsync("placement");
        var trigger = GetByTestId("right-trigger");
        await Expect(trigger).ToHaveAttributeAsync("data-placement", "right");
    }

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tooltip_BottomPlacement_HasCorrectAttribute()
    {
        await GoToPageAsync("placement");
        var trigger = GetByTestId("bottom-trigger");
        await Expect(trigger).ToHaveAttributeAsync("data-placement", "bottom");
    }

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tooltip_LeftPlacement_HasCorrectAttribute()
    {
        await GoToPageAsync("placement");
        var trigger = GetByTestId("left-trigger");
        await Expect(trigger).ToHaveAttributeAsync("data-placement", "left");
    }

    #endregion

    #region ARIA Type Tests

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Tooltip_DefaultType_HasAriaLabelledby()
    {
        await GoToPageAsync("aria");
        var trigger = GetByTestId("describedby-trigger");
        await Expect(trigger).ToHaveAttributeAsync("aria-labelledby", "ds-tooltip");
    }

    [Fact]
    [Trait(Traits.Component, "Tooltip")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Tooltip_LabelledByType_HasAriaDescribedby()
    {
        await GoToPageAsync("aria");
        var trigger = GetByTestId("labelledby-trigger");
        await Expect(trigger).ToHaveAttributeAsync("aria-describedby", "ds-tooltip");
    }

    #endregion
}