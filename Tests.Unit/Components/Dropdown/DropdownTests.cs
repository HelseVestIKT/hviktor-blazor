using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Dropdown;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Dropdown")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Popover)]
public class DropdownTests : HviktorBunitContext
{
    public DropdownTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Dropdown_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown"));

        Assert.NotNull(component.Instance);
        Assert.Null(component.Instance.Open);
    }

    [Fact]
    public void Dropdown_HasDsDropdownClass()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown"));

        var dropdown = component.Find("div");
        Assert.Contains("ds-dropdown", dropdown.ClassList);
    }

    [Fact]
    public void Dropdown_HasDsPopoverClass()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown"));

        var dropdown = component.Find("div");
        Assert.Contains("ds-popover", dropdown.ClassList);
    }

    [Fact]
    public void Dropdown_RendersAsDivElement()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown"));

        var dropdown = component.Find("div");
        Assert.Equal("DIV", dropdown.TagName);
    }

    [Fact]
    public void Dropdown_AcceptsCustomId()
    {
        const string customId = "my-custom-dropdown";
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, customId));

        Assert.Equal(customId, component.Instance.Id);
        var dropdown = component.Find("div");
        Assert.Equal(customId, dropdown.Id);
    }

    [Fact]
    public void Dropdown_HasPopoverManualAttribute()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown"));

        var dropdown = component.Find("div");
        Assert.Equal("manual", dropdown.GetAttribute("popover"));
    }

    [Fact]
    public void Dropdown_HasDefaultVariant()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown"));

        var dropdown = component.Find("div");
        Assert.Equal("primary", dropdown.GetAttribute("data-variant"));
    }

    [Fact]
    public void Dropdown_AppliesPlacement()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddUnmatched("placement", Placement.TopEnd));

        var dropdown = component.Find("div");
        Assert.Equal("top-end", dropdown.GetAttribute("data-placement"));
    }

    [Fact]
    public void Dropdown_HasNoColorAttributeWhenNotSet()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown"));

        var dropdown = component.Find("div");
        Assert.Null(dropdown.GetAttribute("data-color"));
    }

    [Fact]
    public void Dropdown_RendersChildContent()
    {
        const string content = "Dropdown content";
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent(content));

        var dropdown = component.Find("div");
        Assert.Contains(content, dropdown.InnerHtml);
    }

    [Fact]
    public void Dropdown_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddUnmatched("data-testid", "dropdown-test")
            .AddUnmatched("aria-label", "Dropdown menu"));

        var dropdown = component.Find("div");
        Assert.Equal("dropdown-test", dropdown.GetAttribute("data-testid"));
        Assert.Equal("Dropdown menu", dropdown.GetAttribute("aria-label"));
    }

    [Fact]
    public void Dropdown_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddUnmatched("class", "my-custom-dropdown"));

        var dropdown = component.Find("div");
        Assert.Contains("my-custom-dropdown", dropdown.ClassList);
        Assert.Contains("ds-dropdown", dropdown.ClassList);
    }

    [Theory]
    [InlineData(Placement.Top, "top")]
    [InlineData(Placement.TopStart, "top-start")]
    [InlineData(Placement.TopEnd, "top-end")]
    [InlineData(Placement.Bottom, "bottom")]
    [InlineData(Placement.BottomStart, "bottom-start")]
    [InlineData(Placement.BottomEnd, "bottom-end")]
    public void Dropdown_AppliesAllPlacements(Placement placement, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddUnmatched("placement", placement));

        var dropdown = component.Find("div");
        Assert.Equal(expectedDataAttribute, dropdown.GetAttribute("data-placement"));
    }

    [Fact]
    public void Dropdown_CombinesAllParameters()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "full-dropdown")
            .AddUnmatched("placement", Placement.TopEnd)
            .AddChildContent("Full dropdown"));

        var dropdown = component.Find("div");
        Assert.Equal("full-dropdown", dropdown.Id);
        Assert.Equal("top-end", dropdown.GetAttribute("data-placement"));
        Assert.Contains("Full dropdown", dropdown.InnerHtml);
    }
}