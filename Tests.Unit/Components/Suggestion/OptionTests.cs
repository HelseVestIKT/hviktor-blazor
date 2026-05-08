using Bunit;
using SuggestionComponent = Hviktor.Components.Suggestion.Suggestion;

namespace Tests.Unit.Components.Suggestion;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Suggestion.Option")]
public class OptionTests : HviktorBunitContext
{
    public OptionTests()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.initializeCombobox", _ => true);
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.disposeCombobox", _ => true);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Option_RendersUOptionElement()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Option>(p => p.AddChildContent("Test Option")));

        var option = component.Find("[role=option]");
        Assert.Equal("DIV", option.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Option_HasRoleOption()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Option>(p => p.AddChildContent("Test")));

        var option = component.Find("div[role=option]");
        Assert.Equal("option", option.GetAttribute("role"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Option_RendersChildContent()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Option>(p => p.AddChildContent("My Option Text")));

        var option = component.Find("div[role=option]");
        Assert.Contains("My Option Text", option.InnerHtml);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Option_WithDisabled_HasDisabledAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("disabled", true)
            .AddChildContent<global::Suggestion.Option>(p => p.AddChildContent("Test")));

        var option = component.Find("div[role=option]");
        Assert.True(option.HasAttribute("disabled"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Option_WithReadOnly_HasDisabledAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("readonly", true)
            .AddChildContent<global::Suggestion.Option>(p => p.AddChildContent("Test")));

        var option = component.Find("div[role=option]");
        Assert.True(option.HasAttribute("disabled"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Option_WithoutDisabledOrReadOnly_NoDisabledAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Option>(p => p.AddChildContent("Test")));

        var option = component.Find("div[role=option]");
        Assert.False(option.HasAttribute("disabled"));
    }
}