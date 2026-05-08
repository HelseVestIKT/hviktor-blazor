using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Chip;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Chip.Checkbox")]
public class ChipCheckboxTests : HviktorBunitContext
{

    [Fact]
    public void ChipCheckbox_RendersWithDefaultValues()
    {
        var component = Render<global::Chip.Checkbox>();

        Assert.NotNull(component.Instance);

        var input = component.Find("input");
        Assert.False(input.HasAttribute("checked"));
    }

    [Fact]
    public void ChipCheckbox_HasDsChipClass()
    {
        var component = Render<global::Chip.Checkbox>();

        var label = component.Find("label");
        Assert.Contains("ds-chip", label.ClassList);
    }

    [Fact]
    public void ChipCheckbox_HasTypeCheckbox()
    {
        var component = Render<global::Chip.Checkbox>();

        var input = component.Find("input");
        Assert.Equal("checkbox", input.GetAttribute("type"));
    }

    [Fact]
    public void ChipCheckbox_RendersChildContent()
    {
        const string chipText = "Checkbox option";
        var component = Render<global::Chip.Checkbox>(parameters => parameters
            .AddChildContent(chipText));

        var label = component.Find("label");
        Assert.Contains(chipText, label.InnerHtml);
    }

    [Fact]
    public void ChipCheckbox_AppliesCheckedState()
    {
        var component = Render<global::Chip.Checkbox>(parameters => parameters
            .AddUnmatched("checked", true));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("checked"));
    }

    [Fact]
    public void ChipCheckbox_IsNotCheckedByDefault()
    {
        var component = Render<global::Chip.Checkbox>();

        var input = component.Find("input");
        Assert.False(input.HasAttribute("checked"));
    }

    [Fact]
    public void ChipCheckbox_TriggersCheckedChangedOnChange()
    {
        var checkedValue = false;
        var component = Render<global::Chip.Checkbox>(parameters => parameters
            .AddUnmatched("checked", false)
            .Add(p => p.CheckedChanged, value => checkedValue = value));

        var input = component.Find("input");
        input.Change(true);

        Assert.True(checkedValue);
    }

    [Fact]
    public void ChipCheckbox_CanBeUnchecked()
    {
        var checkedValue = true;
        var component = Render<global::Chip.Checkbox>(parameters => parameters
            .AddUnmatched("checked", true)
            .Add(p => p.CheckedChanged, value => checkedValue = value));

        var input = component.Find("input");
        input.Change(false);

        Assert.False(checkedValue);
    }

    [Fact]
    public void ChipCheckbox_AppliesColor()
    {
        var component = Render<global::Chip.Checkbox>(parameters => parameters
            .AddUnmatched("Color", Color.Accent));

        var label = component.Find("label");
        Assert.Equal("accent", label.GetAttribute("data-color"));
    }

    [Fact]
    public void ChipCheckbox_AppliesSize()
    {
        var component = Render<global::Chip.Checkbox>(parameters => parameters
            .AddUnmatched("Size", Size.Large));

        var label = component.Find("label");
        Assert.Equal("lg", label.GetAttribute("data-size"));
    }

    [Fact]
    public void ChipCheckbox_InputHasDsInputClass()
    {
        var component = Render<global::Chip.Checkbox>();

        var input = component.Find("input");
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    public void ChipCheckbox_CombinesAllParameters()
    {
        var component = Render<global::Chip.Checkbox>(parameters => parameters
            .AddUnmatched("value", "check-value")
            .AddUnmatched("name", "check-name")
            .AddUnmatched("Color", Color.Accent)
            .AddUnmatched("Size", Size.Small)
            .AddUnmatched("checked", true)
            .AddChildContent("Full checkbox"));

        var label = component.Find("label");
        Assert.Equal("accent", label.GetAttribute("data-color"));
        Assert.Equal("sm", label.GetAttribute("data-size"));
        Assert.Contains("Full checkbox", label.InnerHtml);

        var input = component.Find("input");
        Assert.Equal("check-value", input.GetAttribute("value"));
        Assert.Equal("check-name", input.GetAttribute("name"));
        Assert.True(input.HasAttribute("checked"));
    }
}