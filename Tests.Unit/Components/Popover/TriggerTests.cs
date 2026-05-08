using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Popover;

namespace Tests.Unit.Components.Popover;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Popover.Trigger")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Trigger)]
public class TriggerTests : HviktorBunitContext
{
    public TriggerTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Trigger_RendersButtonElement()
    {
        var component = Render<Trigger>(parameters => parameters
            .AddChildContent("Click me"));

        var button = component.Find("button");
        Assert.Equal("BUTTON", button.TagName);
    }

    [Fact]
    public void Trigger_HasDsButtonClass()
    {
        var component = Render<Trigger>(parameters => parameters
            .AddChildContent("Click me"));

        var button = component.Find("button");
        Assert.Contains("ds-button", button.ClassList);
    }

    [Fact]
    public void Trigger_HasTypeButton()
    {
        var component = Render<Trigger>(parameters => parameters
            .AddChildContent("Click me"));

        var button = component.Find("button");
        Assert.Equal("button", button.GetAttribute("type"));
    }

    [Fact]
    public void Trigger_RendersChildContent()
    {
        var component = Render<Trigger>(parameters => parameters
            .AddChildContent("Open popover"));

        var button = component.Find("button");
        Assert.Contains("Open popover", button.InnerHtml);
    }

    [Fact]
    public void Trigger_AppliesVariant()
    {
        var component = Render<Trigger>(parameters => parameters
            .AddUnmatched("variant", Variant.Primary)
            .AddChildContent("Click"));

        var button = component.Find("button");
        Assert.Equal("primary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void Trigger_AppliesSize()
    {
        var component = Render<Trigger>(parameters => parameters
            .AddUnmatched("size", Size.Large)
            .AddChildContent("Click"));

        var button = component.Find("button");
        Assert.Equal("lg", button.GetAttribute("data-size"));
    }

    [Fact]
    public void Trigger_AppliesColor()
    {
        var component = Render<Trigger>(parameters => parameters
            .AddUnmatched("color", Color.Danger)
            .AddChildContent("Click"));

        var button = component.Find("button");
        Assert.Equal("danger", button.GetAttribute("data-color"));
    }

    [Fact]
    public void Trigger_AppliesAdditionalAttributes()
    {
        var component = Render<Trigger>(parameters => parameters
            .AddUnmatched("aria-label", "Open menu")
            .AddUnmatched("data-testid", "trigger-test")
            .AddChildContent("Click"));

        var button = component.Find("button");
        Assert.Equal("Open menu", button.GetAttribute("aria-label"));
        Assert.Equal("trigger-test", button.GetAttribute("data-testid"));
    }

    [Fact]
    public void Trigger_DefaultPlacementIsBottom()
    {
        var component = Render<Trigger>(parameters => parameters
            .AddChildContent("Click"));

        var button = component.Find("button");
        Assert.Equal("bottom", button.GetAttribute("data-placement"));
    }

    [Fact]
    public void Trigger_AcceptsCustomPlacement()
    {
        var component = Render<Trigger>(parameters => parameters
            .AddUnmatched("placement", Placement.Top)
            .AddChildContent("Click"));

        var button = component.Find("button");
        Assert.Equal("top", button.GetAttribute("data-placement"));
    }
}