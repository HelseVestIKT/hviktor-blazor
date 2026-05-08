using Bunit;
using CardBlock = Card.Block;

namespace Tests.Unit.Components.Card;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Card.Block")]
public class CardBlockTests : HviktorBunitContext
{

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Block_RendersWithDefaultValues()
    {
        var component = Render<CardBlock>();
        Assert.NotNull(component.Instance);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Block_RendersAsDivElement()
    {
        var component = Render<CardBlock>();

        var block = component.Find("div");
        Assert.Equal("DIV", block.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Block_HasDsCardBlockClass()
    {
        var component = Render<CardBlock>();

        var block = component.Find("div");
        Assert.Contains("ds-card__block", block.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Block_RendersChildContent()
    {
        const string content = "Block content";
        var component = Render<CardBlock>(p => p.AddChildContent(content));

        var block = component.Find("div");
        Assert.Contains(content, block.InnerHtml);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Block_RendersComplexChildContent()
    {
        var component = Render<CardBlock>(p => p
            .AddChildContent("<h3>Title</h3><p>Description</p>"));

        var block = component.Find("div");
        Assert.Contains("<h3>Title</h3>", block.InnerHtml);
        Assert.Contains("<p>Description</p>", block.InnerHtml);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Block_RendersEmptyWhenNoChildContent()
    {
        var component = Render<CardBlock>();

        var block = component.Find("div");
        Assert.Empty(block.InnerHtml.Trim());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Block_AcceptsCustomId()
    {
        const string customId = "my-block-id";
        var component = Render<CardBlock>(p => p.Add(x => x.Id, customId));

        var block = component.Find("div");
        Assert.Equal(customId, block.Id);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Block_HasNoIdWhenEmpty()
    {
        var component = Render<CardBlock>();

        var block = component.Find("div");
        Assert.True(string.IsNullOrEmpty(block.Id));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Block_AppliesAdditionalAttributes()
    {
        var component = Render<CardBlock>(p => p
            .AddUnmatched("aria-label", "Card block")
            .AddUnmatched("data-testid", "block-test"));

        var block = component.Find("div");
        Assert.Equal("Card block", block.GetAttribute("aria-label"));
        Assert.Equal("block-test", block.GetAttribute("data-testid"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Block_AppliesCustomCssClass()
    {
        var component = Render<CardBlock>(p => p
            .AddUnmatched("class", "my-custom-class"));

        var block = component.Find("div");
        Assert.Contains("my-custom-class", block.ClassList);
        Assert.Contains("ds-card__block", block.ClassList);
    }
}