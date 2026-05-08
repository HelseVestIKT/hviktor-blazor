using Bunit;

namespace Tests.Unit.Components.Search;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Search.Input")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class SearchInputTests : HviktorBunitContext
{
    public SearchInputTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void SearchInput_RendersInputElement()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Input>());

        var input = component.Find("input");
        Assert.Equal("INPUT", input.TagName);
    }

    [Fact]
    public void SearchInput_HasDsInputClass()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Input>());

        var input = component.Find("input");
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    public void SearchInput_HasGeneratedId()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Input>());

        var input = component.Find("input");
        Assert.NotNull(input.Id);
        Assert.NotEmpty(input.Id);
    }

    [Fact]
    public void SearchInput_AppliesAriaLabel()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Input>(p => p
                .AddUnmatched("aria-label", "Search for items")));

        var input = component.Find("input");
        Assert.Equal("Search for items", input.GetAttribute("aria-label"));
    }

    [Fact]
    public void SearchInput_AppliesPlaceholder()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Input>(p => p
                .AddUnmatched("placeholder", "Type to search...")));

        var input = component.Find("input");
        Assert.Equal("Type to search...", input.GetAttribute("placeholder"));
    }

    [Fact]
    public void SearchInput_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .AddChildContent<global::Search.Input>(p => p
                .AddUnmatched("data-testid", "search-input-test")
                .AddUnmatched("autocomplete", "off")));

        var input = component.Find("input");
        Assert.Equal("search-input-test", input.GetAttribute("data-testid"));
        Assert.Equal("off", input.GetAttribute("autocomplete"));
    }
}