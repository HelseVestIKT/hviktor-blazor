using Bunit;

namespace Tests.Unit.Components.List;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "List.Item")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class ListItemTests : HviktorBunitContext
{

    [Fact]
    public void Item_RendersWithDefaultValues()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddChildContent<global::List.Item>());

        var item = component.Find("li");
        Assert.NotNull(item);
    }

    [Fact]
    public void Item_RendersAsLiElement()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddChildContent<global::List.Item>());

        var item = component.Find("li");
        Assert.Equal("LI", item.TagName);
    }

    [Fact]
    public void Item_RendersChildContent()
    {
        const string content = "List item content";
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddChildContent<global::List.Item>(itemParams => itemParams
                .AddChildContent(content)));

        var item = component.Find("li");
        Assert.Contains(content, item.InnerHtml);
    }

    [Fact]
    public void Item_RendersEmptyWhenNoChildContent()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddChildContent<global::List.Item>());

        var item = component.Find("li");
        Assert.Empty(item.InnerHtml.Trim());
    }

    [Fact]
    public void Item_AppliesAdditionalAttributes()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddChildContent<global::List.Item>(itemParams => itemParams
                .AddUnmatched("data-testid", "item-test")
                .AddUnmatched("aria-label", "List item")));

        var item = component.Find("li");
        Assert.Equal("item-test", item.GetAttribute("data-testid"));
        Assert.Equal("List item", item.GetAttribute("aria-label"));
    }

    [Fact]
    public void Item_AppliesCustomCssClass()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddChildContent<global::List.Item>(itemParams => itemParams
                .AddUnmatched("class", "my-custom-item")));

        var item = component.Find("li");
        Assert.Contains("my-custom-item", item.ClassList);
    }

    [Fact]
    public void Item_RendersComplexChildContent()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddChildContent<global::List.Item>(itemParams => itemParams
                .AddChildContent("<strong>Important:</strong> Read this")));

        var item = component.Find("li");
        Assert.Contains("<strong>Important:</strong>", item.InnerHtml);
        Assert.Contains("Read this", item.InnerHtml);
    }

    [Fact]
    public void Item_WorksWithOrderedList()
    {
        var component = Render<global::List.Ordered>(parameters => parameters
            .AddChildContent<global::List.Item>(itemParams => itemParams
                .AddChildContent("Ordered item")));

        var item = component.Find("li");
        Assert.Contains("Ordered item", item.InnerHtml);
    }

    [Fact]
    public void Item_AppliesValueAttribute()
    {
        var component = Render<global::List.Ordered>(parameters => parameters
            .AddChildContent<global::List.Item>(itemParams => itemParams
                .AddUnmatched("value", "10")
                .AddChildContent("Item at position 10")));

        var item = component.Find("li");
        Assert.Equal("10", item.GetAttribute("value"));
    }
}