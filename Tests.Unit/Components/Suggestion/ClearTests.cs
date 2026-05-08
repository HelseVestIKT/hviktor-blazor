using System.Globalization;
using Bunit;
using SuggestionComponent = Hviktor.Components.Suggestion.Suggestion;

namespace Tests.Unit.Components.Suggestion;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Suggestion.Clear")]
public class ClearTests : HviktorBunitContext
{
    public ClearTests()
    {
        CultureInfo.CurrentUICulture = new CultureInfo("nb");
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.initializeCombobox", _ => true);
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.disposeCombobox", _ => true);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Clear_RendersDelElement()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Clear>());

        var clear = component.Find("del");
        Assert.Equal("DEL", clear.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Clear_HasRoleButton()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Clear>());

        var clear = component.Find("del");
        Assert.Equal("button", clear.GetAttribute("role"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Clear_HasAriaLabel()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Clear>());

        var clear = component.Find("del");
        Assert.Equal("Tøm valg", clear.GetAttribute("aria-label"));
    }
}