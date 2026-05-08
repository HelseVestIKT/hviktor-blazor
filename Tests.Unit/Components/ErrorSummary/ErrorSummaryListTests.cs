using Bunit;

namespace Tests.Unit.Components.ErrorSummary;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "ErrorSummary.List")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class ErrorSummaryListTests : HviktorBunitContext
{

    [Fact]
    public void List_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.List>());

        var list = component.Find("ul");
        Assert.NotNull(list);
    }

    [Fact]
    public void List_RendersAsUlElement()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.List>());

        var list = component.Find("ul");
        Assert.Equal("UL", list.TagName);
    }

    [Fact]
    public void List_RendersChildContent()
    {
        const string listContent = "List item content";
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.List>(listParams => listParams
                .AddChildContent(listContent)));

        var list = component.Find("ul");
        Assert.Contains(listContent, list.InnerHtml);
    }

    [Fact]
    public void List_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.List>(listParams => listParams
                .AddUnmatched("data-testid", "error-list-test")
                .AddUnmatched("aria-label", "Error list")));

        var list = component.Find("ul");
        Assert.Equal("error-list-test", list.GetAttribute("data-testid"));
        Assert.Equal("Error list", list.GetAttribute("aria-label"));
    }

    [Fact]
    public void List_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.List>(listParams => listParams
                .AddUnmatched("class", "my-error-list")));

        var list = component.Find("ul");
        Assert.Contains("my-error-list", list.ClassList);
    }
}