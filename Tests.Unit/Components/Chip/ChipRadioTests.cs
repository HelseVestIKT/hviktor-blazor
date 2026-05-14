using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Chip;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Chip.Radio")]
public class ChipRadioTests : HviktorBunitContext
{
    [Fact]
    public void ChipRadio_RendersWithDefaultValues()
    {
        var component = Render<global::Chip.Radio>();

        Assert.NotNull(component.Instance);

        var input = component.Find("input");
        Assert.False(input.HasAttribute("checked"));
        Assert.Null(input.GetAttribute("value"));
        Assert.Null(input.GetAttribute("name"));
    }

    [Fact]
    public void ChipRadio_HasDsChipClass()
    {
        var component = Render<global::Chip.Radio>();

        var label = component.Find("label");
        Assert.Contains("ds-chip", label.ClassList);
    }

    [Fact]
    public void ChipRadio_RendersAsLabelWithInput()
    {
        var component = Render<global::Chip.Radio>();

        var label = component.Find("label");
        Assert.Equal("LABEL", label.TagName);

        var input = component.Find("input");
        Assert.NotNull(input);
    }

    [Fact]
    public void ChipRadio_HasTypeRadio()
    {
        var component = Render<global::Chip.Radio>();

        var input = component.Find("input");
        Assert.Equal("radio", input.GetAttribute("type"));
    }

    [Fact]
    public void ChipRadio_RendersChildContent()
    {
        const string chipText = "Radio option";
        var component = Render<global::Chip.Radio>(parameters => parameters
            .AddChildContent(chipText));

        var label = component.Find("label");
        Assert.Contains(chipText, label.InnerHtml);
    }

    [Fact]
    public void ChipRadio_AppliesCheckedState()
    {
        var component = Render<global::Chip.Radio>(parameters => parameters
            .AddUnmatched("Checked", true));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("checked"));
    }

    [Fact]
    public void ChipRadio_IsNotCheckedByDefault()
    {
        var component = Render<global::Chip.Radio>();

        var input = component.Find("input");
        Assert.False(input.HasAttribute("checked"));
    }

    [Fact]
    public void ChipRadio_AppliesValue()
    {
        const string value = "option-1";
        var component = Render<global::Chip.Radio>(parameters => parameters
            .AddUnmatched("value", value));

        var input = component.Find("input");
        Assert.Equal(value, input.GetAttribute("value"));
    }

    [Fact]
    public void ChipRadio_AppliesName()
    {
        const string name = "radio-group";
        var component = Render<global::Chip.Radio>(parameters => parameters
            .AddUnmatched("name", name));

        var input = component.Find("input");
        Assert.Equal(name, input.GetAttribute("name"));
    }

    [Fact]
    public void ChipRadio_AppliesColor()
    {
        var component = Render<global::Chip.Radio>(parameters => parameters
            .AddUnmatched("Color", Color.Accent));

        var label = component.Find("label");
        Assert.Equal("accent", label.GetAttribute("data-color"));
    }

    [Fact]
    public void ChipRadio_AppliesSize()
    {
        var component = Render<global::Chip.Radio>(parameters => parameters
            .AddUnmatched("size", Size.Small));

        var label = component.Find("label");
        Assert.Equal("sm", label.GetAttribute("data-size"));
    }

    [Fact]
    public void ChipRadio_TriggersOnChangeOnChange()
    {
        var triggered = false;
        var component = Render<global::Chip.Radio>(parameters => parameters
            .AddUnmatched("checked", false)
            .Add(p => p.OnChange, () => triggered = true));

        var input = component.Find("input");
        input.Change(true);

        Assert.True(triggered);
    }

    [Fact]
    public void ChipRadio_InputHasDsInputClass()
    {
        var component = Render<global::Chip.Radio>();

        var input = component.Find("input");
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    public void ChipRadio_CombinesAllParameters()
    {
        var component = Render<global::Chip.Radio>(parameters => parameters
            .AddUnmatched("value", "option-value")
            .AddUnmatched("name", "option-name")
            .AddUnmatched("color", Color.Neutral)
            .AddUnmatched("size", Size.Medium)
            .AddUnmatched("checked", true)
            .AddChildContent("Full radio"));

        var label = component.Find("label");
        Assert.Equal("neutral", label.GetAttribute("data-color"));
        Assert.Equal("md", label.GetAttribute("data-size"));
        Assert.Contains("Full radio", label.InnerHtml);

        var input = component.Find("input");
        Assert.Equal("option-value", input.GetAttribute("value"));
        Assert.Equal("option-name", input.GetAttribute("name"));
        Assert.True(input.HasAttribute("checked"));
    }
}