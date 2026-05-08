using Bunit;
using SearchComponent = Hviktor.Components.Search.Search;

namespace Tests.Unit.Components.Search;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Search")]
public class SearchTests : HviktorBunitContext
{
    public SearchTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void Search_RendersAsDivElement()
    {
        var component = Render<SearchComponent>();

        var div = component.Find("div");
        Assert.Equal("DIV", div.TagName);
    }

    [Fact]
    public void Search_HasDsSearchClass()
    {
        var component = Render<SearchComponent>();

        var div = component.Find("div");
        Assert.Contains("ds-search", div.ClassList);
    }

    [Fact]
    public void Search_HasDefaultId()
    {
        var component = Render<SearchComponent>();

        Assert.NotNull(component.Instance.Id);
        Assert.NotEmpty(component.Instance.Id);
    }

    [Fact]
    public void Search_AcceptsCustomId()
    {
        var component = Render<SearchComponent>(parameters => parameters
            .Add(p => p.Id, "custom-search-id"));

        var div = component.Find("div");
        Assert.Equal("custom-search-id", div.Id);
    }

    [Fact]
    public void Search_RendersChildContent()
    {
        var component = Render<SearchComponent>(parameters => parameters
            .AddChildContent("<span>Search content</span>"));

        var div = component.Find("div");
        Assert.Contains("Search content", div.InnerHtml);
    }

    [Fact]
    public void Search_AppliesAdditionalAttributes()
    {
        var component = Render<SearchComponent>(parameters => parameters
            .AddUnmatched("data-testid", "search-test")
            .AddUnmatched("role", "search"));

        var div = component.Find("div");
        Assert.Equal("search-test", div.GetAttribute("data-testid"));
        Assert.Equal("search", div.GetAttribute("role"));
    }

    [Fact]
    public void Search_AppliesCustomCssClass()
    {
        var component = Render<SearchComponent>(parameters => parameters
            .AddUnmatched("class", "custom-search"));

        var div = component.Find("div");
        Assert.Contains("custom-search", div.ClassList);
        Assert.Contains("ds-search", div.ClassList);
    }

    #endregion
}