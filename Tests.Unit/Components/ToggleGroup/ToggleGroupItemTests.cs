using Bunit;
using ToggleGroup;

namespace Tests.Unit.Components.ToggleGroup;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "ToggleGroup.Item")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class ToggleGroupItemTests : HviktorBunitContext
{
    public ToggleGroupItemTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void ToggleGroupItem_DefaultTypeIsRadio()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.Equal("radio", input.GetAttribute("type"));
    }

    [Fact]
    public void ToggleGroupItem_RendersLabelWithInput()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var label = component.Find("label");
        Assert.Equal("LABEL", label.TagName);

        var input = component.Find("input");
        Assert.Equal("INPUT", input.TagName);
    }

    [Fact]
    public void ToggleGroupItem_HasRoleRadio()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.Equal("radio", input.GetAttribute("role"));
    }

    [Fact]
    public void ToggleGroupItem_HasTypeRadio()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.Equal("radio", input.GetAttribute("type"));
    }

    [Fact]
    public void ToggleGroupItem_HasDefaultId()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.NotNull(input.Id);
        Assert.NotEmpty(input.Id);
    }

    [Fact]
    public void ToggleGroupItem_AcceptsCustomId()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent<Item>(itemParams => itemParams
                .Add(i => i.Id, "custom-item-id")
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.Equal("custom-item-id", input.Id);
    }

    [Fact]
    public void ToggleGroupItem_HasValueAttribute()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "my-value")
                .AddChildContent("Item")));

        var input = component.Find("input");
        Assert.Equal("my-value", input.GetAttribute("value"));
    }

    [Fact]
    public void ToggleGroupItem_RendersChildContent()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Toggle Option")));

        var label = component.Find("label");
        Assert.Contains("Toggle Option", label.TextContent);
    }

    [Fact]
    public void ToggleGroupItem_InheritsNameFromParent()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.Name, "toggle-group-name")
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.Equal("toggle-group-name", input.GetAttribute("name"));
    }

    [Fact]
    public void ToggleGroupItem_HasRovingTabindexItemAttribute()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("data-roving-tabindex-item"));
    }

    [Fact]
    public void ToggleGroupItem_UnselectedHasTertiaryVariant()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.DefaultValue, "other-value")
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var label = component.Find("label");
        Assert.Equal("tertiary", label.GetAttribute("data-variant"));
    }

    [Fact]
    public void ToggleGroupItem_SelectedHasPrimaryVariant()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.DefaultValue, "item1")
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var label = component.Find("label");
        Assert.Equal("primary", label.GetAttribute("data-variant"));
    }

    [Fact]
    public void ToggleGroupItem_SelectedHasAriaChecked()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.DefaultValue, "item1")
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.Equal("true", input.GetAttribute("aria-checked"));
    }

    [Fact]
    public void ToggleGroupItem_SelectedIsChecked()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.DefaultValue, "item1")
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("checked"));
    }

    [Fact]
    public void ToggleGroupItem_UnselectedHasAriaCheckedFalse()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.DefaultValue, "other")
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.Equal("false", input.GetAttribute("aria-checked"));
    }

    [Fact]
    public void ToggleGroupItem_HasRovingTabindexRemoval()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent<Item>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        var input = component.Find("input");
        Assert.Equal("-1", input.GetAttribute("tabindex"));
    }
}