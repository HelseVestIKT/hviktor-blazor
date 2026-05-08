using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Dropdown;

/// <summary>
/// Tests.Playwright for the Dropdown.Trigger component.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Dropdown.Trigger")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Trigger)]
public class DropdownTriggerTests : HviktorBunitContext
{
    public DropdownTriggerTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void DropdownTrigger_RendersButtonElement()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddChildContent("Open Menu")));

        var button = component.Find("button");
        Assert.Equal("BUTTON", button.TagName);
    }

    [Fact]
    public void DropdownTrigger_RendersChildContent()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddChildContent("Click Me")));

        var button = component.Find("button");
        Assert.Contains("Click Me", button.TextContent);
    }

    [Fact]
    public void DropdownTrigger_HasTypeButton()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddChildContent("Menu")));

        var button = component.Find("button");
        Assert.Equal("button", button.GetAttribute("type"));
    }

    [Fact]
    public void DropdownTrigger_HasPopovertargetAttribute()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "my-dropdown")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddChildContent("Open")));

        var button = component.Find("button");
        Assert.Equal("my-dropdown", button.GetAttribute("popovertarget"));
    }

    #endregion

    #region Loading Tests.Playwright

    [Fact]
    public void DropdownTrigger_LoadingFalseByDefault()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddChildContent("Menu")));

        // No spinner should be rendered when Loading is false
        var spinners = component.FindAll(".ds-spinner");
        Assert.Empty(spinners);
    }

    [Fact]
    public void DropdownTrigger_LoadingTrue_RendersSpinner()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .Add(t => t.Loading, true)
                .AddChildContent("Menu")));

        var spinner = component.Find(".ds-spinner");
        Assert.NotNull(spinner);
    }

    [Fact]
    public void DropdownTrigger_LoadingTrue_StillRendersChildContent()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .Add(t => t.Loading, true)
                .AddChildContent("Loading Menu")));

        var button = component.Find("button");
        Assert.Contains("Loading Menu", button.TextContent);
    }

    [Fact]
    public void DropdownTrigger_LoadingFalse_NoSpinner()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .Add(t => t.Loading, false)
                .AddChildContent("Menu")));

        var spinners = component.FindAll(".ds-spinner");
        Assert.Empty(spinners);
    }

    #endregion

    #region Inline Tests.Playwright

    [Fact]
    public void DropdownTrigger_WithInlineAttribute_HasDataPopoverInline()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("inline", true)
                .AddChildContent("Inline Trigger")));

        var button = component.Find("button");
        Assert.Equal("inline", button.GetAttribute("data-popover"));
    }

    [Fact]
    public void DropdownTrigger_WithInlineAttribute_NoDsButtonClass()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("inline", true)
                .AddChildContent("Inline")));

        var button = component.Find("button");
        // When inline is set, ds-button class should NOT be added
        Assert.DoesNotContain("ds-button", button.ClassList);
    }

    [Fact]
    public void DropdownTrigger_WithoutInline_HasDsButtonClass()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddChildContent("Button Trigger")));

        var button = component.Find("button");
        Assert.Contains("ds-button", button.ClassList);
    }

    #endregion

    #region Size Tests.Playwright

    [Fact]
    public void DropdownTrigger_AppliesSmallSize()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("size", Size.Small)
                .AddChildContent("Small")));

        var button = component.Find("button");
        Assert.Equal("sm", button.GetAttribute("data-size"));
    }

    [Fact]
    public void DropdownTrigger_AppliesMediumSize()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("size", Size.Medium)
                .AddChildContent("Medium")));

        var button = component.Find("button");
        Assert.Equal("md", button.GetAttribute("data-size"));
    }

    [Fact]
    public void DropdownTrigger_AppliesLargeSize()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("size", Size.Large)
                .AddChildContent("Large")));

        var button = component.Find("button");
        Assert.Equal("lg", button.GetAttribute("data-size"));
    }

    #endregion

    #region Variant Tests.Playwright

    [Fact]
    public void DropdownTrigger_AppliesPrimaryVariant()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("variant", Variant.Primary)
                .AddChildContent("Primary")));

        var button = component.Find("button");
        Assert.Equal("primary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void DropdownTrigger_AppliesSecondaryVariant()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("variant", Variant.Secondary)
                .AddChildContent("Secondary")));

        var button = component.Find("button");
        Assert.Equal("secondary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void DropdownTrigger_AppliesTertiaryVariant()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("variant", Variant.Tertiary)
                .AddChildContent("Tertiary")));

        var button = component.Find("button");
        Assert.Equal("tertiary", button.GetAttribute("data-variant"));
    }

    #endregion

    #region Color Tests.Playwright

    [Fact]
    public void DropdownTrigger_AppliesAccentColor()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("color", Color.Accent)
                .AddChildContent("Accent")));

        var button = component.Find("button");
        Assert.Equal("accent", button.GetAttribute("data-color"));
    }

    [Fact]
    public void DropdownTrigger_AppliesNeutralColor()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("color", Color.Neutral)
                .AddChildContent("Neutral")));

        var button = component.Find("button");
        Assert.Equal("neutral", button.GetAttribute("data-color"));
    }

    #endregion

    #region Combined Parameters Tests.Playwright

    [Fact]
    public void DropdownTrigger_LoadingWithVariantAndSize()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .Add(t => t.Loading, true)
                .AddUnmatched("variant", Variant.Primary)
                .AddUnmatched("size", Size.Large)
                .AddChildContent("Loading...")));

        var button = component.Find("button");
        var spinner = component.Find(".ds-spinner");

        Assert.NotNull(spinner);
        Assert.Equal("primary", button.GetAttribute("data-variant"));
        Assert.Equal("lg", button.GetAttribute("data-size"));
        Assert.Contains("Loading...", button.TextContent);
    }

    [Fact]
    public void DropdownTrigger_InlineWithColor()
    {
        var component = Render<global::Dropdown.TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-trigger")
            .AddChildContent<global::Dropdown.Trigger>(triggerParams => triggerParams
                .AddUnmatched("inline", true)
                .AddUnmatched("color", Color.Accent)
                .AddChildContent("Inline Colored")));

        var button = component.Find("button");
        Assert.Equal("inline", button.GetAttribute("data-popover"));
        Assert.Equal("accent", button.GetAttribute("data-color"));
        Assert.DoesNotContain("ds-button", button.ClassList);
    }

    #endregion
}
