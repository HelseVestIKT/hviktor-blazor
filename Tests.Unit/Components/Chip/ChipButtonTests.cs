using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;

namespace Tests.Unit.Components.Chip;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Chip.Button")]
public class ChipButtonTests : HviktorBunitContext
{

    [Fact]
    public void ChipButton_RendersWithDefaultValues()
    {
        var component = Render<global::Chip.Button>();

        Assert.NotNull(component.Instance);

        var attr = component.Instance.AdditionalAttributes;
        Assert.Null(attr?.GetValueOrDefault("data-size"));
        Assert.Null(attr?.GetValueOrDefault("data-color"));
        Assert.Null(attr?.GetValueOrDefault("value"));
        Assert.Null(attr?.GetValueOrDefault("name"));
    }

    [Fact]
    public void ChipButton_HasDsChipClass()
    {
        var component = Render<global::Chip.Button>();

        var button = component.Find("button");
        Assert.Contains("ds-chip", button.ClassList);
    }

    [Fact]
    public void ChipButton_RendersAsButtonElement()
    {
        var component = Render<global::Chip.Button>();

        var button = component.Find("button");
        Assert.Equal("BUTTON", button.TagName);
    }

    [Fact]
    public void ChipButton_HasTypeButton()
    {
        var component = Render<global::Chip.Button>();

        var button = component.Find("button");
        Assert.Equal("button", button.GetAttribute("type"));
    }

    [Fact]
    public void ChipButton_RendersChildContent()
    {
        const string chipText = "Chip label";
        var component = Render<global::Chip.Button>(parameters => parameters
            .AddChildContent(chipText));

        var button = component.Find("button");
        Assert.Contains(chipText, button.InnerHtml);
    }

    [Fact]
    public void ChipButton_AppliesValue()
    {
        const string value = "chip-value";
        var component = Render<global::Chip.Button>(parameters => parameters
            .AddUnmatched("value", value));

        var button = component.Find("button");
        Assert.Equal(value, button.GetAttribute("value"));
    }

    [Fact]
    public void ChipButton_AppliesName()
    {
        const string name = "chip-name";
        var component = Render<global::Chip.Button>(parameters => parameters
            .AddUnmatched("name", name));

        var button = component.Find("button");
        Assert.Equal(name, button.GetAttribute("name"));
    }

    [Fact]
    public void ChipButton_AppliesColor()
    {
        var component = Render<global::Chip.Button>(parameters => parameters
            .AddUnmatched("color", Color.Accent));

        var attr = component.Instance.AdditionalAttributes;
        EnumValue<Color> color = attr?.GetValueOrDefault("color")?.ToString();
        Assert.Equal(Color.Accent, color);

        var button = component.Find("button");
        Assert.Equal("accent", button.GetAttribute("data-color"));
    }

    [Fact]
    public void ChipButton_AppliesSize()
    {
        var component = Render<global::Chip.Button>(parameters => parameters
            .AddUnmatched("size", Size.Small));

        var attr = component.Instance.AdditionalAttributes;
        EnumValue<Size> size = attr?.GetValueOrDefault("size")?.ToString();
        Assert.Equal(Size.Small, size);

        var button = component.Find("button");
        Assert.Equal("sm", button.GetAttribute("data-size"));
    }

    [Fact]
    public void ChipButton_AppliesAdditionalAttributes()
    {
        var component = Render<global::Chip.Button>(parameters => parameters
            .AddUnmatched("data-testid", "chip-test")
            .AddUnmatched("aria-label", "Test chip"));

        var button = component.Find("button");
        Assert.Equal("chip-test", button.GetAttribute("data-testid"));
        Assert.Equal("Test chip", button.GetAttribute("aria-label"));
    }

    [Fact]
    public void ChipButton_HasNoColorAttributeWhenNull()
    {
        var component = Render<global::Chip.Button>();

        var button = component.Find("button");
        Assert.Null(button.GetAttribute("data-color"));
    }

    [Fact]
    public void ChipButton_HasNoSizeAttributeWhenNull()
    {
        var component = Render<global::Chip.Button>();

        var button = component.Find("button");
        Assert.Null(button.GetAttribute("data-size"));
    }

    [Fact]
    public void ChipButton_HasNoVariantAttributeWhenNull()
    {
        var component = Render<global::Chip.Button>();

        var button = component.Find("button");
        Assert.Null(button.GetAttribute("data-variant"));
    }

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    public void ChipButton_AppliesAllSizes(Size size, string expectedDataAttribute)
    {
        var component = Render<global::Chip.Button>(parameters => parameters
            .AddUnmatched("size", size));

        var button = component.Find("button");
        Assert.Equal(expectedDataAttribute, button.GetAttribute("data-size"));
    }

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    public void ChipButton_AppliesAllColors(Color color, string expectedDataAttribute)
    {
        var component = Render<global::Chip.Button>(parameters => parameters
            .AddUnmatched("color", color));

        var button = component.Find("button");
        Assert.Equal(expectedDataAttribute, button.GetAttribute("data-color"));
    }

    [Fact]
    public void ChipButton_CombinesAllParameters()
    {
        var component = Render<global::Chip.Button>(parameters => parameters
            .AddUnmatched("value", "test-value")
            .AddUnmatched("name", "test-name")
            .AddUnmatched("color", Color.Accent)
            .AddUnmatched("size", Size.Large)
            .AddChildContent("Full chip"));

        var button = component.Find("button");
        Assert.Equal("test-value", button.GetAttribute("value"));
        Assert.Equal("test-name", button.GetAttribute("name"));
        Assert.Equal("accent", button.GetAttribute("data-color"));
        Assert.Equal("lg", button.GetAttribute("data-size"));
        Assert.Contains("Full chip", button.InnerHtml);
    }

    [Fact]
    public void ChipButton_AppliesCustomCssClass()
    {
        var component = Render<global::Chip.Button>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var button = component.Find("button");
        Assert.Contains("my-custom-class", button.ClassList);
        Assert.Contains("ds-chip", button.ClassList);
    }
}