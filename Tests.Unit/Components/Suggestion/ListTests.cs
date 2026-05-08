using Bunit;
using SuggestionComponent = Hviktor.Components.Suggestion.Suggestion;

namespace Tests.Unit.Components.Suggestion;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Suggestion.List")]
public class ListTests : HviktorBunitContext
{
    public ListTests()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.initializeCombobox", _ => true);
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.disposeCombobox", _ => true);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void List_RendersDatalistElement()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.List>());

        var list = component.Find("[role=listbox]");
        Assert.Equal("DIV", list.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Cascading)]
    public void List_ReceivesParentId()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "my-suggestion")
            .AddChildContent<global::Suggestion.List>());

        var list = component.Find("div[role=listbox]");
        Assert.Equal("my-suggestion-list", list.Id);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void List_WithFilterDefault_DoesNotHaveNoFilterAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.List>());

        var list = component.Find("div[role=listbox]");
        Assert.False(list.HasAttribute("data-nofilter"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void List_WithFilterTrue_DoesNotHaveNoFilterAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("filter", true)
            .AddChildContent<global::Suggestion.List>());

        var list = component.Find("div[role=listbox]");
        Assert.False(list.HasAttribute("data-nofilter"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void List_WithFilterFalse_HasNoFilterAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("filter", false)
            .AddChildContent<global::Suggestion.List>());

        var list = component.Find("div[role=listbox]");
        Assert.True(list.HasAttribute("data-nofilter"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void List_WithDisabled_DoesNotRenderDatalist()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("disabled", true)
            .AddChildContent<global::Suggestion.List>());

        Assert.Throws<ElementNotFoundException>(() => component.Find("div[role=listbox]"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void List_WithReadOnly_DoesNotRenderDatalist()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("readonly", true)
            .AddChildContent<global::Suggestion.List>());

        Assert.Throws<ElementNotFoundException>(() => component.Find("div[role=listbox]"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void List_WithoutDisabledOrReadOnly_RendersDatalist()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.List>());

        var list = component.Find("[role=listbox]");
        Assert.Equal("DIV", list.TagName);
    }
}