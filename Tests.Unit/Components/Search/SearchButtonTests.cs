using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Search;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Search.Button")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class SearchButtonTests : HviktorBunitContext
{
    public SearchButtonTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void SearchButton_RendersButtonElement()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Button>());

        var button = component.Find("button");
        Assert.Equal("BUTTON", button.TagName);
    }

    [Fact]
    public void SearchButton_HasTypeSubmit()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Button>());

        var button = component.Find("button");
        Assert.Equal("submit", button.GetAttribute("type"));
    }

    [Fact]
    public void SearchButton_HasAriaLabel()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Button>());

        var button = component.Find("button");
        Assert.Equal("Søk", button.GetAttribute("aria-label"));
    }

    [Fact]
    public void SearchButton_HasDefaultId()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Button>());

        var button = component.Find("button");
        Assert.NotNull(button.Id);
        Assert.NotEmpty(button.Id);
    }

    [Fact]
    public void SearchButton_AcceptsCustomId()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Button>(p => p
                .AddUnmatched("Id", "custom-search-button")));

        var button = component.Find("button");
        Assert.Equal("custom-search-button", button.Id);
    }

    [Fact]
    public void SearchButton_AppliesPrimaryVariant()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Button>(p => p
                .AddUnmatched("variant", Variant.Primary)));

        var button = component.Find("button");
        Assert.Equal("primary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void SearchButton_AppliesSecondaryVariant()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Button>(p => p
                .AddUnmatched("variant", Variant.Secondary)));

        var button = component.Find("button");
        Assert.Equal("secondary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void SearchButton_AppliesTertiaryVariant()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Button>(p => p
                .AddUnmatched("variant", Variant.Tertiary)));

        var button = component.Find("button");
        Assert.Equal("tertiary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void SearchButton_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Button>(p => p
                .AddChildContent("Search now")));

        var button = component.Find("button");
        Assert.Contains("Search now", button.InnerHtml);
    }

    [Fact]
    public void SearchButton_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Button>(p => p
                .AddUnmatched("data-testid", "search-btn-test")));

        var button = component.Find("button");
        Assert.Equal("search-btn-test", button.GetAttribute("data-testid"));
    }
}