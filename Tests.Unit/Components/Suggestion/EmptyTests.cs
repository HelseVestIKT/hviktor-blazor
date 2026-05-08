using Bunit;
using SuggestionComponent = Hviktor.Components.Suggestion.Suggestion;

namespace Tests.Unit.Components.Suggestion;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Suggestion.Empty")]
public class EmptyTests : HviktorBunitContext
{
    public EmptyTests()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.initializeCombobox", _ => true);
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.disposeCombobox", _ => true);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Empty_RendersUOptionElement()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Empty>(p => p.AddChildContent("No results")));

        var option = component.Find("[role=option]");
        Assert.Equal("DIV", option.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Empty_HasEmptyValue()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Empty>(p => p.AddChildContent("No results")));

        var option = component.Find("div[role=option]");
        Assert.Equal(option.GetAttribute("value"), string.Empty);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Empty_HasDataEmptyAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Empty>(p => p.AddChildContent("No results")));

        var option = component.Find("div[role=option]");
        Assert.True(option.HasAttribute("data-empty"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Empty_IsHidden()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Empty>(p => p.AddChildContent("No results")));

        var option = component.Find("div[role=option]");
        Assert.True(option.HasAttribute("hidden"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Empty_RendersChildContent()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Empty>(p => p.AddChildContent("No results found")));

        var option = component.Find("div[role=option]");
        Assert.Contains("No results found", option.InnerHtml);
    }
}