using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;

namespace Tests.Unit.Components.Card;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Card")]
public class CardTests : HviktorBunitContext
{

    [Fact]
    public void Card_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Card.Card>();
        Assert.NotNull(component.Instance);

        var card = component.Find("div");
        Assert.Null(card.GetAttribute("data-size"));
        Assert.Null(card.GetAttribute("data-color"));
        Assert.Equal("default", card.GetAttribute("data-variant"));
    }

    [Fact]
    public void Card_HasDsCardClass()
    {
        var component = Render<Hviktor.Components.Card.Card>();

        var card = component.Find("div");
        Assert.Contains("ds-card", card.ClassList);
    }

    [Fact]
    public void Card_HasGeneratedId()
    {
        var component = Render<Hviktor.Components.Card.Card>();

        var card = component.Find("div");
        var id = card.Id;
        Assert.NotNull(id);
        Assert.NotEmpty(id);
    }

    [Fact]
    public void Card_AcceptsCustomId()
    {
        const string customId = "my-custom-card";
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("Id", customId));

        var attr = component.Instance.AdditionalAttributes;
        var id = attr?.GetValueOrDefault("id")?.ToString();
        Assert.Equal(customId, id);

        var card = component.Find("div");
        Assert.Equal(customId, card.Id);
    }

    [Fact]
    public void Card_RendersChildContent()
    {
        const string cardText = "Card content";
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddChildContent(cardText));

        var card = component.Find("div");
        Assert.Contains(cardText, card.InnerHtml);
    }

    [Fact]
    public void Card_AppliesVariant()
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("variant", Variant.Tinted));

        var attr = component.Instance.AdditionalAttributes;
        EnumValue<Variant> variant = attr?.GetValueOrDefault("variant")?.ToString();
        Assert.Equal(Variant.Tinted, variant);

        var card = component.Find("div");
        Assert.Equal("tinted", card.GetAttribute("data-variant"));
    }

    [Fact]
    public void Card_AppliesColor()
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("color", Color.Accent));

        var attr = component.Instance.AdditionalAttributes;
        EnumValue<Color> color = attr?.GetValueOrDefault("color")?.ToString();
        Assert.Equal(Color.Accent, color);

        var card = component.Find("div");
        Assert.Equal("accent", card.GetAttribute("data-color"));
    }

    [Fact]
    public void Card_AppliesSize()
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("size", Size.Small));

        var attr = component.Instance.AdditionalAttributes;
        EnumValue<Size> size = attr?.GetValueOrDefault("size")?.ToString();
        Assert.Equal(Size.Small, size);

        var card = component.Find("div");
        Assert.Equal("sm", card.GetAttribute("data-size"));
    }

    [Fact]
    public void Card_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("aria-label", "Test card")
            .AddUnmatched("data-testid", "card-test"));

        var card = component.Find("div");
        Assert.Equal("Test card", card.GetAttribute("aria-label"));
        Assert.Equal("card-test", card.GetAttribute("data-testid"));
    }

    [Fact]
    public void Card_HasNoSizeAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Card.Card>();

        var card = component.Find("div");
        Assert.Null(card.GetAttribute("data-size"));
    }

    [Fact]
    public void Card_HasNoColorAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Card.Card>();

        var card = component.Find("div");
        Assert.Null(card.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData(Variant.Default, "default")]
    [InlineData(Variant.Tinted, "tinted")]
    public void Card_AppliesAllAllowedVariants(Variant variant, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("variant", variant));

        var card = component.Find("div");
        Assert.Equal(expectedDataAttribute, card.GetAttribute("data-variant"));
    }

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    public void Card_AppliesAllSizes(Size size, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("Size", size));

        var card = component.Find("div");
        Assert.Equal(expectedDataAttribute, card.GetAttribute("data-size"));
    }

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    public void Card_AppliesAllColors(Color color, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("Color", color));

        var card = component.Find("div");
        Assert.Equal(expectedDataAttribute, card.GetAttribute("data-color"));
    }

    [Fact]
    public void Card_RendersAsDivElement()
    {
        var component = Render<Hviktor.Components.Card.Card>();

        var card = component.Find("div");
        Assert.Equal("DIV", card.TagName);
    }

    [Fact]
    public void Card_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var card = component.Find("div");
        Assert.Contains("my-custom-class", card.ClassList);
        Assert.Contains("ds-card", card.ClassList);
    }

    [Fact]
    public void Card_GeneratesUniqueIds()
    {
        var component1 = Render<Hviktor.Components.Card.Card>();
        var component2 = Render<Hviktor.Components.Card.Card>();

        var id1 = component1.Find("div").Id;
        var id2 = component2.Find("div").Id;

        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void Card_DefaultVariantIsDefault()
    {
        var component = Render<Hviktor.Components.Card.Card>();

        var card = component.Find("div");
        Assert.Equal("default", card.GetAttribute("data-variant"));
    }

    [Fact]
    public void Card_RendersComplexChildContent()
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddChildContent("<h2>Title</h2><p>Description</p>"));

        var card = component.Find("div");
        Assert.Contains("<h2>Title</h2>", card.InnerHtml);
        Assert.Contains("<p>Description</p>", card.InnerHtml);
    }

    [Fact]
    public void Card_AppliesMultipleUnmatchedAttributes()
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("data-testid", "card-container")
            .AddUnmatched("title", "Card title")
            .AddUnmatched("tabindex", "0"));

        var card = component.Find("div");
        Assert.Equal("card-container", card.GetAttribute("data-testid"));
        Assert.Equal("Card title", card.GetAttribute("title"));
        Assert.Equal("0", card.GetAttribute("tabindex"));
    }

    [Fact]
    public void Card_HasTabIndex()
    {
        var component = Render<Hviktor.Components.Card.Card>();

        var card = component.Find("div");
        Assert.Equal("0", card.GetAttribute("tabindex"));
    }

    [Fact]
    public void Card_RendersEmptyWhenNoChildContent()
    {
        var component = Render<Hviktor.Components.Card.Card>();

        var card = component.Find("div");
        Assert.Empty(card.InnerHtml.Trim());
    }

    [Fact]
    public void Card_CombinesVariantAndColor()
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("Variant", Variant.Tinted)
            .AddUnmatched("Color", Color.Accent));

        var card = component.Find("div");
        Assert.Equal("tinted", card.GetAttribute("data-variant"));
        Assert.Equal("accent", card.GetAttribute("data-color"));
    }

    [Fact]
    public void Card_CombinesAllParameters()
    {
        var component = Render<Hviktor.Components.Card.Card>(parameters => parameters
            .AddUnmatched("Variant", Variant.Tinted)
            .AddUnmatched("Color", Color.Neutral)
            .AddUnmatched("Size", Size.Large)
            .AddChildContent("Full card"));

        var card = component.Find("div");
        Assert.Equal("tinted", card.GetAttribute("data-variant"));
        Assert.Equal("neutral", card.GetAttribute("data-color"));
        Assert.Equal("lg", card.GetAttribute("data-size"));
        Assert.Contains("Full card", card.InnerHtml);
    }
}