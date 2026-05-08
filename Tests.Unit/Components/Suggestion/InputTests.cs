using Bunit;
using SuggestionComponent = Hviktor.Components.Suggestion.Suggestion;

namespace Tests.Unit.Components.Suggestion;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Suggestion.Input")]
public class InputTests : HviktorBunitContext
{
    public InputTests()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.initializeCombobox", _ => true);
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.disposeCombobox", _ => true);
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.setSelected", _ => true);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Input_RendersInputElement()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Input>());

        var input = component.Find("input");
        Assert.Equal("INPUT", input.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Cascading)]
    public void Input_ReceivesParentId()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "my-suggestion")
            .AddChildContent<global::Suggestion.Input>());

        var input = component.Find("input");
        Assert.Equal("my-suggestion-input", input.Id);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Cascading)]
    public void Input_DoesNotHaveListAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "my-suggestion")
            .AddChildContent<global::Suggestion.Input>());

        var input = component.Find("input");
        Assert.Null(input.GetAttribute("list"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_HasDsInputClass()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Input>());

        var input = component.Find("input");
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_HasTypeText()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Input>());

        var input = component.Find("input");
        Assert.Equal("text", input.GetAttribute("type"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_HasAriaAutocompleteList()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Input>());

        var input = component.Find("input");
        Assert.Equal("list", input.GetAttribute("aria-autocomplete"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_HasAutocompleteOff()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Input>());

        var input = component.Find("input");
        Assert.Equal("off", input.GetAttribute("autocomplete"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_WithReadOnly_HasReadOnlyAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("readonly", true)
            .AddChildContent<global::Suggestion.Input>());

        var input = component.Find("input");
        Assert.True(input.HasAttribute("readonly"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_WithoutReadOnly_NoReadOnlyAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Input>());

        var input = component.Find("input");
        Assert.False(input.HasAttribute("readonly"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Input_WithSelectedValue_RendersDataChip()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("selected", "test-value")
            .AddChildContent<global::Suggestion.Input>());

        var chip = component.Find("data.ds-chip");
        Assert.Equal("DATA", chip.TagName);
        Assert.Equal("test-value", chip.GetAttribute("value"));
        Assert.Equal("test-value", chip.TextContent);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Input_WithoutSelectedValue_DoesNotRenderDataChip()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddChildContent<global::Suggestion.Input>());

        Assert.Throws<ElementNotFoundException>(() => component.Find("data.ds-chip"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_DataChip_WithReadOnly_HasCorrectAriaLabel()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("selected", "my-value")
            .AddUnmatched("readonly", true)
            .AddChildContent<global::Suggestion.Input>());

        var chip = component.Find("data.ds-chip");
        Assert.Equal("my-value", chip.GetAttribute("aria-label"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_DataChip_WithoutReadOnly_HasRemovableAriaLabel()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("selected", "my-value")
            .AddChildContent<global::Suggestion.Input>());

        var chip = component.Find("data.ds-chip");
        Assert.Equal("my-value, Press to remove, 1 of 1", chip.GetAttribute("aria-label"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_DataChip_WithReadOnly_NotRemovable()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("selected", "my-value")
            .AddUnmatched("readonly", true)
            .AddChildContent<global::Suggestion.Input>());

        var chip = component.Find("data.ds-chip");
        Assert.False(chip.HasAttribute("data-removable"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_DataChip_WithoutReadOnly_IsRemovable()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("selected", "my-value")
            .AddChildContent<global::Suggestion.Input>());

        var chip = component.Find("data.ds-chip");
        Assert.True(chip.HasAttribute("data-removable"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_DataChip_HasRoleButton()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("selected", "my-value")
            .AddChildContent<global::Suggestion.Input>());

        var chip = component.Find("data.ds-chip");
        Assert.Equal("button", chip.GetAttribute("role"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Input_DataChip_IsRemovedFromTabOrder()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("selected", "my-value")
            .AddChildContent<global::Suggestion.Input>());

        var chip = component.Find("data.ds-chip");
        Assert.Equal("-1", chip.GetAttribute("tabindex"));
    }
}