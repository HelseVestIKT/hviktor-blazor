using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Popover;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Popover)]
[Trait(TestCollections.Traits.Component, "Popover")]
public class PopoverTests : HviktorBunitContext
{
    public PopoverTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Popover_RendersAsDivElement()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover"));

        var div = component.Find("div");
        Assert.Equal("DIV", div.TagName);
    }

    [Fact]
    public void Popover_HasDsPopoverClass()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover"));

        var div = component.Find("div");
        Assert.Contains("ds-popover", div.ClassList);
    }

    [Fact]
    public void Popover_HasPopoverManualAttribute()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover"));

        var div = component.Find("div");
        Assert.Equal("manual", div.GetAttribute("popover"));
    }

    [Fact]
    public void Popover_HasIdAttribute()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "my-popover"));

        var div = component.Find("div");
        Assert.Equal("my-popover", div.Id);
    }

    [Fact]
    public void Popover_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover")
            .AddChildContent("<span>Popover content</span>"));

        var div = component.Find("div");
        Assert.Contains("Popover content", div.InnerHtml);
    }

    [Fact]
    public void Popover_HasDefaultPlacement()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover"));

        var div = component.Find("div");
        Assert.Equal("bottom-start", div.GetAttribute("data-placement"));
    }

    [Fact]
    public void Popover_AppliesCustomPlacement()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover")
            .AddUnmatched("placement", Placement.TopEnd));

        var div = component.Find("div");
        Assert.Equal("top-end", div.GetAttribute("data-placement"));
    }

    [Fact]
    public void Popover_HasDefaultVariant()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover"));

        var div = component.Find("div");
        Assert.Equal("primary", div.GetAttribute("data-variant"));
    }

    [Fact]
    public void Popover_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover")
            .AddUnmatched("data-testid", "popover-test")
            .AddUnmatched("aria-label", "Test popover"));

        var div = component.Find("div");
        Assert.Equal("popover-test", div.GetAttribute("data-testid"));
        Assert.Equal("Test popover", div.GetAttribute("aria-label"));
    }

    [Fact]
    public void Popover_OpenParameterIsSettable()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover")
            .Add(p => p.Open, true));

        Assert.True(component.Instance.Open);
    }

    [Fact]
    public void Popover_OpenParameterDefaultsToNull()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover"));

        Assert.Null(component.Instance.Open);
    }

    [Fact]
    public void Popover_OnCloseCallbackIsSettable()
    {
        var component = Render<Hviktor.Components.Popover.Popover>(parameters => parameters
            .Add(p => p.Id, "test-popover")
            .Add(p => p.OnClose, () => { }));

        Assert.NotNull(component.Instance);
    }
}