using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Dropdown;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Dropdown.Button")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class DropdownButtonTests : HviktorBunitContext
{
    public DropdownButtonTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Button_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>());

        var button = component.Find("button");
        Assert.NotNull(button);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Button_RendersAsButtonElement()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>());

        var button = component.Find("button");
        Assert.Equal("BUTTON", button.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Button_DefaultVariantIsTertiary()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>());

        var button = component.Find("button");
        Assert.Equal("tertiary", button.GetAttribute("data-variant"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Button_RendersChildContent()
    {
        const string content = "Click me";
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>(btnParams => btnParams
                .AddChildContent(content)));

        var button = component.Find("button");
        Assert.Contains(content, button.InnerHtml);
    }

    [Theory]
    [InlineData(Variant.Primary, "primary")]
    [InlineData(Variant.Secondary, "secondary")]
    [InlineData(Variant.Tertiary, "tertiary")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Button_AppliesVariant(Variant variant, string expected)
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>(btnParams => btnParams
                .AddUnmatched("variant", variant)));

        var button = component.Find("button");
        Assert.Equal(expected, button.GetAttribute("data-variant"));
    }

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Button_AppliesSize(Size size, string expected)
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>(btnParams => btnParams
                .AddUnmatched("size", size)));

        var button = component.Find("button");
        Assert.Equal(expected, button.GetAttribute("data-size"));
    }

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Button_AppliesColor(Color color, string expected)
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>(btnParams => btnParams
                .AddUnmatched("color", color)));

        var button = component.Find("button");
        Assert.Equal(expected, button.GetAttribute("data-color"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Button_HasNoSizeWhenNotSet()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>());

        var button = component.Find("button");
        Assert.Null(button.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Button_HasNoColorWhenNotSet()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>());

        var button = component.Find("button");
        Assert.Null(button.GetAttribute("data-color"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Button_AppliesIconDataAttribute()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>(btnParams => btnParams
                .AddUnmatched("icon", "true")));

        var button = component.Find("button");
        Assert.NotNull(button.GetAttribute("data-icon"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Button_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>(btnParams => btnParams
                .AddUnmatched("aria-label", "Dropdown action")
                .AddUnmatched("data-testid", "btn-test")));

        var button = component.Find("button");
        Assert.Equal("Dropdown action", button.GetAttribute("aria-label"));
        Assert.Equal("btn-test", button.GetAttribute("data-testid"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Button_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Button>(btnParams => btnParams
                .AddUnmatched("class", "my-custom-class")));

        var button = component.Find("button");
        Assert.Contains("my-custom-class", button.ClassList);
    }
}