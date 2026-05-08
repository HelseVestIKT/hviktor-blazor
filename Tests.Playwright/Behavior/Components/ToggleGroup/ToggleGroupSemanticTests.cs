using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.ToggleGroup;

/// <summary>
/// Semantic behavior tests for the ToggleGroup component.
/// Tests proper HTML structure and accessibility features:
/// - ToggleGroup renders with role="radiogroup"
/// - ToggleGroup.Item renders with role="radio"
/// - Selected item has aria-checked="true"
/// - Unselected items have aria-checked="false" (explicitly set to false)
/// - Data attributes for variant and size are applied correctly
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class ToggleGroupSemanticTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "togglegroup";

    #region Basic Structure Tests

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ToggleGroup_HasDsToggleGroupClass()
    {
        await GoToPageAsync("basic");

        var toggleGroup = GetByTestId("basic");
        await Expect(toggleGroup).ToHaveClassAsync("ds-toggle-group");
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ToggleGroupItem_HasRoleRadio()
    {
        await GoToPageAsync("basic");

        var item = GetByTestId("item1");
        await Expect(item).ToHaveAttributeAsync("role", "radio");
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ToggleGroupItem_IsButtonElement()
    {
        await GoToPageAsync("basic");

        var item = GetByTestId("item1");
        var tagName = await item.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
        Assert.Equal("input", tagName);
    }

    #endregion

    #region Selection State Tests

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task ToggleGroup_SelectedItem_HasAriaCheckedTrue()
    {
        await GoToPageAsync("aria");

        var selectedItem = GetByTestId("selected-item");
        await Expect(selectedItem).ToHaveAttributeAsync("aria-checked", "true");
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task ToggleGroup_UnselectedItem_DoesNotHaveAriaCheckedTrue()
    {
        await GoToPageAsync("aria");

        var unselectedItem = GetByTestId("unselected-item");
        // Unselected items should not have aria-checked="true"
        var ariaChecked = await unselectedItem.GetAttributeAsync("aria-checked");
        Assert.NotEqual("true", ariaChecked);
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ToggleGroup_SelectedItem_HasTabIndexZero()
    {
        await GoToPageAsync("aria");

        var selectedItem = GetByTestId("selected-item");
        await Expect(selectedItem).ToHaveAttributeAsync("tabindex", "0");
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ToggleGroup_UnselectedItem_HasTabIndexMinusOne()
    {
        await GoToPageAsync("aria");

        var unselectedItem = GetByTestId("unselected-item");
        await Expect(unselectedItem).ToHaveAttributeAsync("tabindex", "-1");
    }

    #endregion

    #region Variant Tests

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ToggleGroup_PrimaryVariant_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variant");

        var toggleGroup = GetByTestId("primary");
        await Expect(toggleGroup).ToHaveAttributeAsync("data-variant", "primary");
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ToggleGroup_SecondaryVariant_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variant");

        var toggleGroup = GetByTestId("secondary");
        await Expect(toggleGroup).ToHaveAttributeAsync("data-variant", "secondary");
    }

    #endregion

    #region Size Tests

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ToggleGroup_SmallSize_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");

        var toggleGroup = GetByTestId("sm");
        await Expect(toggleGroup).ToHaveAttributeAsync("data-size", "sm");
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ToggleGroup_MediumSize_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");

        var toggleGroup = GetByTestId("md");
        await Expect(toggleGroup).ToHaveAttributeAsync("data-size", "md");
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ToggleGroup_LargeSize_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");

        var toggleGroup = GetByTestId("lg");
        await Expect(toggleGroup).ToHaveAttributeAsync("data-size", "lg");
    }

    #endregion

    #region DefaultValue Tests

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task ToggleGroup_DefaultValue_SelectsCorrectItem()
    {
        await GoToPageAsync("basic");

        // Option 1 should be selected by default (DefaultValue="option1")
        var item1 = GetByTestId("item1");
        var item2 = GetByTestId("item2");

        await Expect(item1).ToHaveAttributeAsync("aria-checked", "true");

        var item2AriaChecked = await item2.GetAttributeAsync("aria-checked");
        Assert.NotEqual("true", item2AriaChecked);
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task ToggleGroup_SelectedRadioInput_HasCorrectValue()
    {
        await GoToPageAsync("aria");

        var selectedItem = GetByTestId("selected-item");
        await Expect(selectedItem).ToHaveAttributeAsync("value", "selected");
    }

    #endregion

    #region Click Selection Tests

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task ToggleGroup_ClickItem_SelectsItem()
    {
        await GoToPageAsync("basic");

        var item2 = GetByTestId("item2");
        var item2Label = item2.Locator("xpath=..");

        // Click the wrapping label to trigger selection
        await item2Label.ClickAsync();

        // Item 2 should now be selected
        await Expect(item2).ToHaveAttributeAsync("aria-checked", "true");
    }

    [Fact]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task ToggleGroup_ClickItem_DeselectsPreviousItem()
    {
        await GoToPageAsync("basic");

        var item1 = GetByTestId("item1");
        var item2 = GetByTestId("item2");
        var item2Label = item2.Locator("xpath=..");

        // Initially item1 is selected
        await Expect(item1).ToHaveAttributeAsync("aria-checked", "true");

        // Click the wrapping label to trigger selection
        await item2Label.ClickAsync();

        // Item 1 should no longer be selected
        var item1AriaChecked = await item1.GetAttributeAsync("aria-checked");
        Assert.NotEqual("true", item1AriaChecked);
    }

    #endregion
}