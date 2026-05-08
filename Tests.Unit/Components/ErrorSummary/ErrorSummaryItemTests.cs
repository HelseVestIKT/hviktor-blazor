using Bunit;

namespace Tests.Unit.Components.ErrorSummary;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "ErrorSummary.Item")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class ErrorSummaryItemTests : HviktorBunitContext
{

    [Fact]
    public void Item_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Item>());

        var item = component.Find("li");
        Assert.NotNull(item);
    }

    [Fact]
    public void Item_RendersAsLiElement()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Item>());

        var item = component.Find("li");
        Assert.Equal("LI", item.TagName);
    }

    [Fact]
    public void Item_RendersChildContent()
    {
        const string itemContent = "Error message";
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Item>(itemParams => itemParams
                .AddChildContent(itemContent)));

        var item = component.Find("li");
        Assert.Contains(itemContent, item.InnerHtml);
    }

    [Fact]
    public void Item_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Item>(itemParams => itemParams
                .AddUnmatched("data-testid", "error-item-test")));

        var item = component.Find("li");
        Assert.Equal("error-item-test", item.GetAttribute("data-testid"));
    }

    [Fact]
    public void Item_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Item>(itemParams => itemParams
                .AddUnmatched("class", "my-error-item")));

        var item = component.Find("li");
        Assert.Contains("my-error-item", item.ClassList);
    }
}