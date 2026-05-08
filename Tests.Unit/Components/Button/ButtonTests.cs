using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;
using Microsoft.AspNetCore.Components.Web;

namespace Tests.Unit.Components.Button;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Button")]
public class ButtonTests : HviktorBunitContext
{

    [Fact]
    public void Button_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Button.Button>();

        Assert.NotNull(component.Instance);

        var button = component.Find("button");
        Assert.Null(button.GetAttribute("data-size"));
        Assert.Null(button.GetAttribute("data-color"));
        Assert.Equal("primary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void Button_HasDsButtonClass()
    {
        var component = Render<Hviktor.Components.Button.Button>();

        var button = component.Find("button");
        Assert.Contains("ds-button", button.ClassList);
    }

    [Fact]
    public void Button_HasGeneratedId()
    {
        var component = Render<Hviktor.Components.Button.Button>();

        var button = component.Find("button");
        var id = button.Id;
        Assert.NotNull(id);
        Assert.NotEmpty(id);
    }

    [Fact]
    public void Button_AcceptsCustomId()
    {
        const string customId = "my-custom-button";
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("id", customId));

        var attr = component.Instance.AdditionalAttributes;
        var id = attr?.GetValueOrDefault("id")?.ToString();
        Assert.Equal(customId, id);

        var button = component.Find("button");
        Assert.Equal(customId, button.Id);
    }

    [Fact]
    public void Button_RendersChildContent()
    {
        const string buttonText = "Click me";
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddChildContent(buttonText));

        var button = component.Find("button");
        Assert.Contains(buttonText, button.InnerHtml);
    }

    [Fact]
    public void Button_AppliesVariant()
    {
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("variant", Variant.Secondary));

        var attr = component.Instance.AdditionalAttributes;
        EnumValue<Variant> variant = attr?.GetValueOrDefault("variant")?.ToString();

        Assert.Equal(Variant.Secondary, variant);
        var button = component.Find("button");
        Assert.Equal("secondary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void Button_AppliesColor()
    {
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("color", Color.Accent));

        var attr = component.Instance.AdditionalAttributes;
        EnumValue<Color> color = attr?.GetValueOrDefault("color")?.ToString();
        Assert.Equal(Color.Accent, color);

        var button = component.Find("button");
        Assert.Equal("accent", button.GetAttribute("data-color"));
    }

    [Fact]
    public void Button_AppliesSize()
    {
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("size", Size.Small));

        var attr = component.Instance.AdditionalAttributes;
        EnumValue<Size> size = attr?.GetValueOrDefault("size")?.ToString();
        Assert.Equal(Size.Small, size);

        var button = component.Find("button");
        Assert.Equal("sm", button.GetAttribute("data-size"));
    }

    [Fact]
    public void Button_AppliesForAttribute()
    {
        const string forValue = "my-form-element";
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("for", forValue));

        var button = component.Find("button");
        Assert.Equal(forValue, button.GetAttribute("for"));
    }

    [Fact]
    public void Button_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("aria-label", "Test button")
            .AddUnmatched("disabled", true));

        var button = component.Find("button");
        Assert.Equal("Test button", button.GetAttribute("aria-label"));
        Assert.True(button.HasAttribute("disabled"));
    }

    [Fact]
    public void Button_HasNoSizeAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Button.Button>();

        var button = component.Find("button");
        Assert.Null(button.GetAttribute("data-size"));
    }

    [Fact]
    public void Button_HasNoColorAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Button.Button>();

        var button = component.Find("button");
        Assert.Null(button.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData(Variant.Primary, "primary")]
    [InlineData(Variant.Secondary, "secondary")]
    [InlineData(Variant.Tertiary, "tertiary")]
    public void Button_AppliesAllAllowedVariants(Variant variant, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("variant", variant));

        var button = component.Find("button");
        Assert.Equal(expectedDataAttribute, button.GetAttribute("data-variant"));
    }

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    public void Button_AppliesAllSizes(Size size, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("size", size));

        var button = component.Find("button");
        Assert.Equal(expectedDataAttribute, button.GetAttribute("data-size"));
    }

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    public void Button_AppliesAllColors(Color color, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("color", color));

        var button = component.Find("button");
        Assert.Equal(expectedDataAttribute, button.GetAttribute("data-color"));
    }

    [Fact]
    public void Button_RendersAsButtonElement()
    {
        var component = Render<Hviktor.Components.Button.Button>();

        var button = component.Find("button");
        Assert.Equal("BUTTON", button.TagName);
    }

    [Fact]
    public void Button_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var button = component.Find("button");
        Assert.Contains("my-custom-class", button.ClassList);
        Assert.Contains("ds-button", button.ClassList); // Should still have base class
    }

    [Fact]
    public void Button_GeneratesUniqueIds()
    {
        var component1 = Render<Hviktor.Components.Button.Button>();
        var component2 = Render<Hviktor.Components.Button.Button>();

        var id1 = component1.Find("button").Id;
        var id2 = component2.Find("button").Id;

        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void Button_TriggersOnClickEventOnce()
    {
        var count = 0;
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("onclick", (MouseEventArgs _) => count++));

        var button = component.Find("button");
        button.Click();

        // We use count to ensure its only triggered once per action
        Assert.Equal(1, count);
    }

    [Fact]
    public void Button_TriggersOnClickEventTwice()
    {
        var count = 0;
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("onclick", (MouseEventArgs _) => count++));

        var button = component.Find("button");
        button.Click();
        button.Click();

        Assert.Equal(2, count);
    }

    [Fact]
    public void Button_DefaultVariantIsPrimary()
    {
        var component = Render<Hviktor.Components.Button.Button>();

        var button = component.Find("button");
        Assert.Equal("primary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void Button_HasNoForAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Button.Button>();

        var button = component.Find("button");
        Assert.Null(button.GetAttribute("for"));
    }

    [Fact]
    public void Button_RendersComplexChildContent()
    {
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddChildContent("<span class=\"icon\">★</span> Save"));

        var button = component.Find("button");
        Assert.Contains("icon", button.InnerHtml);
        Assert.Contains("Save", button.InnerHtml);
    }

    [Fact]
    public void Button_AppliesMultipleUnmatchedAttributes()
    {
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("data-testid", "submit-btn")
            .AddUnmatched("title", "Submit form")
            .AddUnmatched("tabindex", "0"));

        var button = component.Find("button");
        Assert.Equal("submit-btn", button.GetAttribute("data-testid"));
        Assert.Equal("Submit form", button.GetAttribute("title"));
        Assert.Equal("0", button.GetAttribute("tabindex"));
    }

    [Fact]
    public void Button_DisabledAttributePreventsClick()
    {
        var count = 0;
        var component = Render<Hviktor.Components.Button.Button>(parameters => parameters
            .AddUnmatched("disabled", true)
            .AddUnmatched("onclick", (MouseEventArgs _) => count++));

        var button = component.Find("button");
        Assert.True(button.HasAttribute("disabled"));
        Assert.Equal(0, count);
    }
}