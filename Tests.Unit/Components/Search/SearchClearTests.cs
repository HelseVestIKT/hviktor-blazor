using Bunit;
using Search;

namespace Tests.Unit.Components.Search;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Search.Clear")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class SearchClearTests : HviktorBunitContext
{
    public SearchClearTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void SearchClear_RendersButtonElement()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "test-search")
            .AddChildContent<Clear>());

        var button = component.Find("button");
        Assert.Equal("BUTTON", button.TagName);
    }

    [Fact]
    public void SearchClear_HasTypeReset()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "test-search")
            .AddChildContent<Clear>());

        var button = component.Find("button");
        Assert.Equal("reset", button.GetAttribute("type"));
    }

    [Fact]
    public void SearchClear_HasAriaLabel()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "test-search")
            .AddChildContent<Clear>());

        var button = component.Find("button");
        Assert.Equal("Tøm", button.GetAttribute("aria-label"));
    }

    [Fact]
    public void SearchClear_HasDataIconAttribute()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "test-search")
            .AddChildContent<Clear>());

        var button = component.Find("button");
        Assert.Equal("true", button.GetAttribute("data-icon"));
    }

    [Fact]
    public void SearchClear_HasIdBasedOnParent()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "my-search")
            .AddChildContent<Clear>());

        var button = component.Find("button");
        Assert.Equal("my-search-clear", button.Id);
    }

    [Fact]
    public void SearchClear_HasTertiaryVariant()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "test-search")
            .AddChildContent<Clear>());

        var button = component.Find("button");
        Assert.Equal("tertiary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void SearchClear_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Search.Search>(parameters => parameters
            .Add(p => p.Id, "test-search")
            .AddChildContent<Clear>(p => p
                .AddChildContent("Clear search")));

        var button = component.Find("button");
        Assert.Contains("Clear search", button.InnerHtml);
    }
}