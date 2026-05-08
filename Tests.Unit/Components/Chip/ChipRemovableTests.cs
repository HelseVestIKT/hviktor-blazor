using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Chip;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Chip.Removable")]
public class ChipRemovableTests : HviktorBunitContext
{

    [Fact]
    public void ChipRemovable_RendersWithDefaultValues()
    {
        var component = Render<global::Chip.Removable>();

        Assert.NotNull(component.Instance);
    }

    [Fact]
    public void ChipRemovable_HasDsChipClass()
    {
        var component = Render<global::Chip.Removable>();

        var button = component.Find("button");
        Assert.Contains("ds-chip", button.ClassList);
    }

    [Fact]
    public void ChipRemovable_RendersAsButtonElement()
    {
        var component = Render<global::Chip.Removable>();

        var button = component.Find("button");
        Assert.Equal("BUTTON", button.TagName);
    }

    [Fact]
    public void ChipRemovable_HasRemovableDataAttribute()
    {
        var component = Render<global::Chip.Removable>();

        var button = component.Find("button");
        Assert.Equal("true", button.GetAttribute("data-removable"));
    }

    [Fact]
    public void ChipRemovable_RendersChildContent()
    {
        const string chipText = "Removable chip";
        var component = Render<global::Chip.Removable>(parameters => parameters
            .AddChildContent(chipText));

        var button = component.Find("button");
        Assert.Contains(chipText, button.InnerHtml);
    }

    [Fact]
    public void ChipRemovable_AppliesColor()
    {
        var component = Render<global::Chip.Removable>(parameters => parameters
            .AddUnmatched("color", Color.Accent));

        var button = component.Find("button");
        Assert.Equal("accent", button.GetAttribute("data-color"));
    }

    [Fact]
    public void ChipRemovable_AppliesSize()
    {
        var component = Render<global::Chip.Removable>(parameters => parameters
            .AddUnmatched("size", Size.Small));

        var button = component.Find("button");
        Assert.Equal("sm", button.GetAttribute("data-size"));
    }

    [Fact]
    public void ChipRemovable_AppliesValue()
    {
        const string value = "removable-value";
        var component = Render<global::Chip.Removable>(parameters => parameters
            .AddUnmatched("value", value));

        var button = component.Find("button");
        Assert.Equal(value, button.GetAttribute("value"));
    }

    [Fact]
    public void ChipRemovable_AppliesName()
    {
        const string name = "removable-name";
        var component = Render<global::Chip.Removable>(parameters => parameters
            .AddUnmatched("name", name));

        var button = component.Find("button");
        Assert.Equal(name, button.GetAttribute("name"));
    }

    [Fact]
    public void ChipRemovable_HasTypeButton()
    {
        var component = Render<global::Chip.Removable>();

        var button = component.Find("button");
        Assert.Equal("button", button.GetAttribute("type"));
    }

    [Fact]
    public void ChipRemovable_CombinesAllParameters()
    {
        var component = Render<global::Chip.Removable>(parameters => parameters
            .AddUnmatched("value", "remove-value")
            .AddUnmatched("name", "remove-name")
            .AddUnmatched("color", Color.Neutral)
            .AddUnmatched("size", Size.Medium)
            .AddChildContent("Full removable"));

        var button = component.Find("button");
        Assert.Equal("remove-value", button.GetAttribute("value"));
        Assert.Equal("remove-name", button.GetAttribute("name"));
        Assert.Equal("neutral", button.GetAttribute("data-color"));
        Assert.Equal("md", button.GetAttribute("data-size"));
        Assert.Equal("true", button.GetAttribute("data-removable"));
        Assert.Contains("Full removable", button.InnerHtml);
    }

    [Fact]
    public void ChipRemovable_AppliesCustomCssClass()
    {
        var component = Render<global::Chip.Removable>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var button = component.Find("button");
        Assert.Contains("my-custom-class", button.ClassList);
        Assert.Contains("ds-chip", button.ClassList);
    }

    [Fact]
    public void ChipRemovable_AppliesAdditionalAttributes()
    {
        var component = Render<global::Chip.Removable>(parameters => parameters
            .AddUnmatched("data-testid", "removable-test")
            .AddUnmatched("aria-label", "Remove item"));

        var button = component.Find("button");
        Assert.Equal("removable-test", button.GetAttribute("data-testid"));
        Assert.Equal("Remove item", button.GetAttribute("aria-label"));
    }
}