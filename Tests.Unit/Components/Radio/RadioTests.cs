using Bunit;
using Microsoft.AspNetCore.Components;

namespace Tests.Unit.Components.Radio;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Radio")]
public class RadioTests : HviktorBunitContext
{

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void Radio_RendersInputElement()
    {
        var component = Render<Hviktor.Components.Radio.Radio>();

        var input = component.Find("input");
        Assert.Equal("INPUT", input.TagName);
    }

    [Fact]
    public void Radio_HasTypeRadio()
    {
        var component = Render<Hviktor.Components.Radio.Radio>();

        var input = component.Find("input");
        Assert.Equal("radio", input.GetAttribute("type"));
    }

    [Fact]
    public void Radio_HasDsInputClass()
    {
        var component = Render<Hviktor.Components.Radio.Radio>();

        var input = component.Find("input");
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    public void Radio_RendersInsideFieldComponent()
    {
        var component = Render<Hviktor.Components.Radio.Radio>();

        var field = component.Find(".ds-field");
        Assert.NotNull(field);
    }

    #endregion

    #region Id Tests.Playwright

    [Fact]
    public void Radio_HasDefaultId()
    {
        var component = Render<Hviktor.Components.Radio.Radio>();

        Assert.NotNull(component.Instance.Id);
        Assert.NotEmpty(component.Instance.Id);
    }

    [Fact]
    public void Radio_AcceptsCustomId()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .Add(p => p.Id, "custom-radio-id"));

        var input = component.Find("input");
        Assert.Equal("custom-radio-id", input.Id);
    }

    #endregion

    #region Label Tests.Playwright

    [Fact]
    public void Radio_WithLabel_RendersLabelElement()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("label", "Choose option"));

        var label = component.Find("label");
        Assert.NotNull(label);
        Assert.Contains("Choose option", label.TextContent);
    }

    [Fact]
    public void Radio_WithLabel_LabelHasForAttribute()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .Add(p => p.Id, "my-radio")
            .AddUnmatched("label", "Option label"));

        var label = component.Find("label");
        Assert.Equal("my-radio", label.GetAttribute("for"));
    }

    [Fact]
    public void Radio_WithoutLabel_DoesNotRenderLabelElement()
    {
        var component = Render<Hviktor.Components.Radio.Radio>();

        var labels = component.FindAll("label");
        Assert.Empty(labels);
    }

    [Fact]
    public void Radio_WithEmptyLabel_DoesNotRenderLabelElement()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("label", ""));

        var labels = component.FindAll("label");
        Assert.Empty(labels);
    }

    [Fact]
    public void Radio_WithWhitespaceLabel_DoesNotRenderLabelElement()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("label", "   "));

        var labels = component.FindAll("label");
        Assert.Empty(labels);
    }

    #endregion

    #region Value and Name Tests.Playwright

    [Fact]
    public void Radio_AcceptsValueParameter()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("value", "option-1"));

        var input = component.Find("input");
        Assert.Equal("option-1", input.GetAttribute("value"));
    }

    [Fact]
    public void Radio_AcceptsNameParameter()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("name", "radio-group"));

        var input = component.Find("input");
        Assert.Equal("radio-group", input.GetAttribute("name"));
    }

    [Fact]
    public void Radio_WithNameAndValue_BothAreSet()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("name", "color-picker")
            .AddUnmatched("value", "blue"));

        var input = component.Find("input");
        Assert.Equal("color-picker", input.GetAttribute("name"));
        Assert.Equal("blue", input.GetAttribute("value"));
    }

    #endregion

    #region Description Tests.Playwright

    [Fact]
    public void Radio_WithDescription_RendersDescriptionElement()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("description", "Additional info"));

        var description = component.Find("[data-field='description']");
        Assert.NotNull(description);
        Assert.Contains("Additional info", description.TextContent);
    }

    [Fact]
    public void Radio_WithDescription_HasAriaDescribedby()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .Add(p => p.Id, "my-radio")
            .AddUnmatched("description", "Help text"));

        var input = component.Find("input");
        var description = component.Find("[data-field='description']");

        Assert.Equal(description.Id, input.GetAttribute("aria-describedby"));
    }

    [Fact]
    public void Radio_WithDescription_DescriptionIdMatchesAriaDescribedby()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .Add(p => p.Id, "test-radio")
            .AddUnmatched("description", "Description text"));

        var input = component.Find("input");
        var description = component.Find("[data-field='description']");

        Assert.Equal("test-radio:description", description.Id);
        Assert.Equal("test-radio:description", input.GetAttribute("aria-describedby"));
    }

    [Fact]
    public void Radio_WithoutDescription_DoesNotRenderDescriptionElement()
    {
        var component = Render<Hviktor.Components.Radio.Radio>();

        var descriptions = component.FindAll("[data-field='description']");
        Assert.Empty(descriptions);
    }

    #endregion

    #region Additional Attributes Tests.Playwright

    [Fact]
    public void Radio_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("data-testid", "radio-test")
            .AddUnmatched("aria-label", "Test radio"));

        var input = component.Find("input");
        Assert.Equal("radio-test", input.GetAttribute("data-testid"));
        Assert.Equal("Test radio", input.GetAttribute("aria-label"));
    }

    [Fact]
    public void Radio_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("class", "custom-radio"));

        var input = component.Find("input");
        Assert.Contains("custom-radio", input.ClassList);
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    public void Radio_CanBeDisabled()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("disabled", true));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("disabled"));
    }

    [Fact]
    public void Radio_CanBeCheckedByDefault()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("checked", true));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("checked"));
    }

    [Fact]
    public void Radio_CanBeRequired()
    {
        var component = Render<Hviktor.Components.Radio.Radio>(parameters => parameters
            .AddUnmatched("required", true));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("required"));
    }

    #endregion

    #region Complete Radio Group Tests.Playwright

    [Fact]
    public void Radio_MultipleWithSameName_FormRadioGroup()
    {
        var cut = Render<RadioGroupWrapper>();

        var radios = cut.FindAll("input[type='radio']");
        Assert.Equal(3, radios.Count);
        Assert.All(radios, r => Assert.Equal("fruit-group", r.GetAttribute("name")));
    }

    [Fact]
    public void Radio_MultipleWithSameName_HaveDifferentValues()
    {
        var cut = Render<RadioGroupWrapper>();

        var radios = cut.FindAll("input[type='radio']");
        var values = radios.Select(r => r.GetAttribute("value")).ToList();

        Assert.Contains("apple", values);
        Assert.Contains("banana", values);
        Assert.Contains("orange", values);
    }

    #endregion
}

public sealed class RadioGroupWrapper : ComponentBase
{
    protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
    {
        builder.OpenComponent<Hviktor.Components.Radio.Radio>(0);
        builder.AddAttribute(1, "Name", "fruit-group");
        builder.AddAttribute(2, "Value", "apple");
        builder.AddAttribute(3, "Label", "Apple");
        builder.CloseComponent();

        builder.OpenComponent<Hviktor.Components.Radio.Radio>(4);
        builder.AddAttribute(5, "Name", "fruit-group");
        builder.AddAttribute(6, "Value", "banana");
        builder.AddAttribute(7, "Label", "Banana");
        builder.CloseComponent();

        builder.OpenComponent<Hviktor.Components.Radio.Radio>(8);
        builder.AddAttribute(9, "Name", "fruit-group");
        builder.AddAttribute(10, "Value", "orange");
        builder.AddAttribute(11, "Label", "Orange");
        builder.CloseComponent();
    }
}