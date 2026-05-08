using Bunit;
using Select;

namespace Tests.Unit.Components.Select;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Select.Option")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class SelectOptionTests : HviktorBunitContext
{

    [Fact]
    public void SelectOption_RendersOptionElement()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddChildContent<Option>());

        var option = component.Find("option");
        Assert.Equal("OPTION", option.TagName);
    }

    [Fact]
    public void SelectOption_HasValueAttribute()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddChildContent<Option>(p => p
                .AddUnmatched("value", "option-1")));

        var option = component.Find("option");
        Assert.Equal("option-1", option.GetAttribute("value"));
    }

    [Fact]
    public void SelectOption_DefaultValueIsEmptyString()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddChildContent<Option>());

        var option = component.Find("option");
        Assert.Null(option.GetAttribute("value"));
    }

    [Fact]
    public void SelectOption_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddChildContent<Option>(p => p
                .AddUnmatched("value", "opt1")
                .AddChildContent("First Option")));

        var option = component.Find("option");
        Assert.Contains("First Option", option.TextContent);
    }

    [Fact]
    public void SelectOption_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddChildContent<Option>(p => p
                .AddUnmatched("value", "opt1")
                .AddUnmatched("data-testid", "option-test")));

        var option = component.Find("option");
        Assert.Equal("option-test", option.GetAttribute("data-testid"));
    }

    [Fact]
    public void SelectOption_CanBeSelected()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddChildContent<Option>(p => p
                .AddUnmatched("value", "opt1")
                .AddUnmatched("selected", true)));

        var option = component.Find("option");
        Assert.True(option.HasAttribute("selected"));
    }

    [Fact]
    public void SelectOption_CanBeDisabled()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddChildContent<Option>(p => p
                .AddUnmatched("value", "opt1")
                .AddUnmatched("disabled", true)));

        var option = component.Find("option");
        Assert.True(option.HasAttribute("disabled"));
    }

    [Fact]
    public void SelectOption_InheritsDisabledFromReadOnlySelect()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("readOnly", true)
            .AddChildContent<Option>(p => p
                .AddUnmatched("value", "opt1")
                .AddChildContent("Option 1")));

        var option = component.Find("option");
        Assert.True(option.HasAttribute("disabled"));
    }
}