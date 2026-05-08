using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Switch;

/// <summary>
/// Semantic behavior tests for the Switch component.
/// Tests verify that the Switch component has correct ARIA role and attributes
/// for screen reader compatibility.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class SwitchSemanticTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "switch";

    #region Role Attribute Tests

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Switch_HasRoleSwitchAttribute()
    {
        await GoToPageAsync("role");
        var switchElement = GetByTestId("with-role");
        await Expect(switchElement).ToHaveAttributeAsync("role", "switch");
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Switch_UncheckedState_HasCorrectSemantics()
    {
        await GoToPageAsync("state");
        var switchElement = GetByTestId("unchecked");
        await Expect(switchElement).ToHaveAttributeAsync("role", "switch");
        await Expect(switchElement).Not.ToBeCheckedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Switch_CheckedState_HasCorrectSemantics()
    {
        await GoToPageAsync("state");
        var switchElement = GetByTestId("checked");
        await Expect(switchElement).ToHaveAttributeAsync("role", "switch");
        await Expect(switchElement).ToBeCheckedAsync();
    }

    #endregion

    #region Label Association Tests

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Switch_WithLabel_IsAccessibleByLabel()
    {
        await GoToPageAsync("label");
        var switchElement = GetByTestId("with-label");
        await Expect(switchElement).ToHaveAttributeAsync("role", "switch");
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Switch_WithAriaLabel_HasAriaLabelAttribute()
    {
        await GoToPageAsync("label");
        var switchElement = GetByTestId("with-aria-label");
        await Expect(switchElement).ToHaveAttributeAsync("aria-label", "Switch with aria-label");
        await Expect(switchElement).ToHaveAttributeAsync("role", "switch");
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Switch_WithAriaLabelledBy_HasAriaLabelledByAttribute()
    {
        await GoToPageAsync("label");
        var switchElement = GetByTestId("with-aria-labelledby");
        await Expect(switchElement).ToHaveAttributeAsync("aria-labelledby", "external-label");
        await Expect(switchElement).ToHaveAttributeAsync("role", "switch");
    }

    #endregion

    #region Position Tests

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Switch_PositionStart_HasCorrectRole()
    {
        await GoToPageAsync("position");
        var switchElement = GetByTestId("position-start");
        await Expect(switchElement).ToHaveAttributeAsync("role", "switch");
    }

    [Fact]
    [Trait(Traits.Component, "Switch")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Switch_PositionEnd_HasCorrectRole()
    {
        await GoToPageAsync("position");
        var switchElement = GetByTestId("position-end");
        await Expect(switchElement).ToHaveAttributeAsync("role", "switch");
    }

    #endregion
}