using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Unit.Components.Input;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Input")]
public class InputTests : HviktorBunitContext
{

    [Fact]
    public void Input_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Input.Input>();
        Assert.NotNull(component.Instance);
    }

    [Fact]
    public void Input_HasDsInputClass()
    {
        var component = Render<Hviktor.Components.Input.Input>();

        var input = component.Find("input");
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    public void Input_RendersAsInputElement()
    {
        var component = Render<Hviktor.Components.Input.Input>();

        var input = component.Find("input");
        Assert.Equal("INPUT", input.TagName);
    }

    [Fact]
    public void Input_HasGeneratedId()
    {
        var component = Render<Hviktor.Components.Input.Input>();

        var input = component.Find("input");
        Assert.NotNull(input.Id);
        Assert.NotEmpty(input.Id);
    }

    [Fact]
    public void Input_GeneratesUniqueIds()
    {
        var component1 = Render<Hviktor.Components.Input.Input>();
        var component2 = Render<Hviktor.Components.Input.Input>();

        var input1 = component1.Find("input");
        var input2 = component2.Find("input");

        Assert.NotEqual(input1.Id, input2.Id);
    }

    [Fact]
    public void Input_AppliesTypeAttribute()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("type", InputType.Text));

        var input = component.Find("input");
        Assert.Equal("text", input.GetAttribute("type"));
    }

    [Fact]
    public void Input_AppliesEmailType()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("type", InputType.Email));

        var input = component.Find("input");
        Assert.Equal("email", input.GetAttribute("type"));
    }

    [Fact]
    public void Input_AppliesPasswordType()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("type", InputType.Password));

        var input = component.Find("input");
        Assert.Equal("password", input.GetAttribute("type"));
    }

    [Fact]
    public void Input_AppliesNumberType()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("type", InputType.Number));

        var input = component.Find("input");
        Assert.Equal("number", input.GetAttribute("type"));
    }

    [Fact]
    public void Input_AppliesPlaceholder()
    {
        const string placeholder = "Enter your name";
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("placeholder", placeholder));

        var input = component.Find("input");
        Assert.Equal(placeholder, input.GetAttribute("placeholder"));
    }

    [Fact]
    public void Input_AppliesNameAttribute()
    {
        const string name = "username";
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("name", name));

        var input = component.Find("input");
        Assert.Equal(name, input.GetAttribute("name"));
    }

    [Fact]
    public void Input_AppliesValueAttribute()
    {
        const string value = "test value";
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("value", value));

        var input = component.Find("input");
        Assert.Equal(value, input.GetAttribute("value"));
    }

    [Fact]
    public void Input_AppliesDisabledAttribute()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("disabled", "disabled"));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("disabled"));
    }

    [Fact]
    public void Input_AppliesReadonlyAttribute()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("readonly", "readonly"));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("readonly"));
    }

    [Fact]
    public void Input_AppliesRequiredAttribute()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("required", "required"));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("required"));
    }

    [Fact]
    public void Input_AppliesMaxLengthAttribute()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("maxlength", "100"));

        var input = component.Find("input");
        Assert.Equal("100", input.GetAttribute("maxlength"));
    }

    [Fact]
    public void Input_AppliesMinLengthAttribute()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("minlength", "5"));

        var input = component.Find("input");
        Assert.Equal("5", input.GetAttribute("minlength"));
    }

    [Fact]
    public void Input_AppliesPatternAttribute()
    {
        const string pattern = "[A-Za-z]+";
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("pattern", pattern));

        var input = component.Find("input");
        Assert.Equal(pattern, input.GetAttribute("pattern"));
    }

    [Fact]
    public void Input_AppliesAriaLabel()
    {
        const string ariaLabel = "Username input";
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("aria-label", ariaLabel));

        var input = component.Find("input");
        Assert.Equal(ariaLabel, input.GetAttribute("aria-label"));
    }

    [Fact]
    public void Input_AppliesAriaDescribedby()
    {
        const string describedby = "username-description";
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("aria-describedby", describedby));

        var input = component.Find("input");
        Assert.Equal(describedby, input.GetAttribute("aria-describedby"));
    }

    [Fact]
    public void Input_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("class", "my-custom-input"));

        var input = component.Find("input");
        Assert.Contains("my-custom-input", input.ClassList);
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    public void Input_AppliesDataTestId()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("data-testid", "input-test"));

        var input = component.Find("input");
        Assert.Equal("input-test", input.GetAttribute("data-testid"));
    }

    [Fact]
    public void Input_AppliesAutocompleteAttribute()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("autocomplete", "email"));

        var input = component.Find("input");
        Assert.Equal("email", input.GetAttribute("autocomplete"));
    }

    [Fact]
    public void Input_AppliesAutofocusAttribute()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("autofocus", "autofocus"));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("autofocus"));
    }

    [Fact]
    public void Input_CombinesMultipleAttributes()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("type", InputType.Email)
            .AddUnmatched("name", "email")
            .AddUnmatched("placeholder", "Enter email")
            .AddUnmatched("required", "required")
            .AddUnmatched("aria-label", "Email address"));

        var input = component.Find("input");
        Assert.Equal("email", input.GetAttribute("type"));
        Assert.Equal("email", input.GetAttribute("name"));
        Assert.Equal("Enter email", input.GetAttribute("placeholder"));
        Assert.True(input.HasAttribute("required"));
        Assert.Equal("Email address", input.GetAttribute("aria-label"));
    }

    [Fact]
    public void Input_IsSelfClosingElement()
    {
        var component = Render<Hviktor.Components.Input.Input>();

        var input = component.Find("input");
        Assert.Empty(input.InnerHtml);
    }

    [Fact]
    public void Input_RendersOnlyOneElement()
    {
        var component = Render<Hviktor.Components.Input.Input>();

        var elements = component.FindAll("*");
        Assert.Single(elements);
    }

    [Theory]
    [InlineData(InputType.Text)]
    [InlineData(InputType.Email)]
    [InlineData(InputType.Password)]
    [InlineData(InputType.Number)]
    [InlineData(InputType.Tel)]
    [InlineData(InputType.Url)]
    [InlineData(InputType.Search)]
    [InlineData(InputType.Date)]
    [InlineData(InputType.Time)]
    [InlineData(InputType.DateTimeLocal)]
    public void Input_AppliesAllInputTypes(InputType inputType)
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("type", inputType));
        var input = component.Find("input");
        var attribute = input.GetAttribute("type");
        var inputService = component.Services.GetService<IInputTypeService>();
        Assert.Equal(inputType, inputService?.GetFromString(attribute ?? ""));
    }

    [Fact]
    public void Input_AppliesMinMaxForNumberType()
    {
        var component = Render<Hviktor.Components.Input.Input>(parameters => parameters
            .AddUnmatched("type", InputType.Number)
            .AddUnmatched("min", "0")
            .AddUnmatched("max", "100")
            .AddUnmatched("step", "5"));

        var input = component.Find("input");
        Assert.Equal("number", input.GetAttribute("type"));
        Assert.Equal("0", input.GetAttribute("min"));
        Assert.Equal("100", input.GetAttribute("max"));
        Assert.Equal("5", input.GetAttribute("step"));
    }
}