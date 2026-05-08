using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Search;

namespace Tests.Unit.Components.Search;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Search")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class SearchIntegrationTests : HviktorBunitContext
{
    public SearchIntegrationTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Search_WithInputAndClear_RendersBothComponents()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "integration-search")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Search.Input>(0);
                builder.AddAttribute(1, "placeholder", "Search...");
                builder.CloseComponent();

                builder.OpenComponent<Clear>(2);
                builder.CloseComponent();
            }));

        var input = component.Find("input");
        var clearButton = component.Find("button[type='reset']");

        Assert.NotNull(input);
        Assert.NotNull(clearButton);
        Assert.Equal("Search...", input.GetAttribute("placeholder"));
    }

    [Fact]
    public void Search_WithInputClearAndButton_RendersAllComponents()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "full-search")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Search.Input>(0);
                builder.AddAttribute(1, "aria-label", "Search input");
                builder.CloseComponent();

                builder.OpenComponent<Clear>(2);
                builder.CloseComponent();

                builder.OpenComponent<global::Search.Button>(3);
                builder.AddAttribute(4, "Variant", Variant.Secondary);
                builder.CloseComponent();
            }));

        var input = component.Find("input");
        var clearButton = component.Find("button[type='reset']");
        var submitButton = component.Find("button[type='submit']");

        Assert.NotNull(input);
        Assert.NotNull(clearButton);
        Assert.NotNull(submitButton);
    }

    [Fact]
    public void Search_ClearButtonIdLinksToParentSearch()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "linked-search")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Search.Input>(0);
                builder.CloseComponent();

                builder.OpenComponent<Clear>(1);
                builder.CloseComponent();
            }));

        var searchContainer = component.Find(".ds-search");
        var clearButton = component.Find("button[type='reset']");

        Assert.Equal("linked-search", searchContainer.Id);
        Assert.Equal("linked-search-clear", clearButton.Id);
    }

    [Fact]
    public void Search_AllComponentsHaveCorrectStructure()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "structured-search")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Search.Input>(0);
                builder.AddAttribute(1, "placeholder", "Enter search term");
                builder.CloseComponent();

                builder.OpenComponent<Clear>(2);
                builder.CloseComponent();

                builder.OpenComponent<global::Search.Button>(3);
                builder.AddAttribute(4, "Variant", Variant.Primary);
                builder.CloseComponent();
            }));

        // Check search container
        var searchContainer = component.Find(".ds-search");
        Assert.Equal("structured-search", searchContainer.Id);

        // Check input
        var input = component.Find("input");
        Assert.Contains("ds-input", input.ClassList);

        // Check clear button
        var clearButton = component.Find("button[type='reset']");
        Assert.Equal("Tøm", clearButton.GetAttribute("aria-label"));

        // Check submit button  
        var submitButton = component.Find("button[type='submit']");
        Assert.Equal("Søk", submitButton.GetAttribute("aria-label"));
        Assert.Equal("primary", submitButton.GetAttribute("data-variant"));
    }
}