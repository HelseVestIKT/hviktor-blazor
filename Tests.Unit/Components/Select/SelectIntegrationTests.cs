using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Select;

namespace Tests.Unit.Components.Select;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Select")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class SelectIntegrationTests : HviktorBunitContext
{

    [Fact]
    public void Select_WithMultipleOptions_RendersAllOptions()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "multi-select")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Option>(0);
                builder.AddAttribute(1, "Value", "");
                builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Select...")));
                builder.CloseComponent();

                builder.OpenComponent<Option>(3);
                builder.AddAttribute(4, "Value", "opt1");
                builder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Option 1")));
                builder.CloseComponent();

                builder.OpenComponent<Option>(6);
                builder.AddAttribute(7, "Value", "opt2");
                builder.AddAttribute(8, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Option 2")));
                builder.CloseComponent();

                builder.OpenComponent<Option>(9);
                builder.AddAttribute(10, "Value", "opt3");
                builder.AddAttribute(11, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Option 3")));
                builder.CloseComponent();
            }));

        var options = component.FindAll("option");
        Assert.Equal(4, options.Count);
    }

    [Fact]
    public void Select_WithMultipleOptions_HasCorrectValues()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "value-select")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Option>(0);
                builder.AddAttribute(1, "Value", "apple");
                builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Apple")));
                builder.CloseComponent();

                builder.OpenComponent<Option>(3);
                builder.AddAttribute(4, "Value", "banana");
                builder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Banana")));
                builder.CloseComponent();

                builder.OpenComponent<Option>(6);
                builder.AddAttribute(7, "Value", "orange");
                builder.AddAttribute(8, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Orange")));
                builder.CloseComponent();
            }));

        var options = component.FindAll("option");
        var values = options.Select(o => o.GetAttribute("value")).ToList();

        Assert.Contains("apple", values);
        Assert.Contains("banana", values);
        Assert.Contains("orange", values);
    }

    [Fact]
    public void Select_WithMultipleOptions_HasCorrectText()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "text-select")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Option>(0);
                builder.AddAttribute(1, "Value", "1");
                builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "First")));
                builder.CloseComponent();

                builder.OpenComponent<Option>(3);
                builder.AddAttribute(4, "Value", "2");
                builder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Second")));
                builder.CloseComponent();

                builder.OpenComponent<Option>(6);
                builder.AddAttribute(7, "Value", "3");
                builder.AddAttribute(8, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Third")));
                builder.CloseComponent();
            }));

        var options = component.FindAll("option");
        var texts = options.Select(o => o.TextContent.Trim()).ToList();

        Assert.Contains("First", texts);
        Assert.Contains("Second", texts);
        Assert.Contains("Third", texts);
    }

    [Fact]
    public void Select_WithPlaceholderOption_HasEmptyValueFirst()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "placeholder-select")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Option>(0);
                builder.AddAttribute(1, "Value", "");
                builder.AddAttribute(2, "selected", true);
                builder.AddAttribute(3, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Please select...")));
                builder.CloseComponent();

                builder.OpenComponent<Option>(4);
                builder.AddAttribute(5, "Value", "opt1");
                builder.AddAttribute(6, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Option 1")));
                builder.CloseComponent();
            }));

        var options = component.FindAll("option");
        var firstOption = options[0];

        Assert.Equal("", firstOption.GetAttribute("value"));
        Assert.True(firstOption.HasAttribute("selected"));
        Assert.Contains("Please select", firstOption.TextContent);
    }

    [Fact]
    public void Select_ReadOnlyWithOptions_AllOptionsDisabled()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "readonly-select")
            .AddUnmatched("readOnly", true)
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Option>(0);
                builder.AddAttribute(1, "Value", "opt1");
                builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Option 1")));
                builder.CloseComponent();

                builder.OpenComponent<Option>(3);
                builder.AddAttribute(4, "Value", "opt2");
                builder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Option 2")));
                builder.CloseComponent();
            }));

        var options = component.FindAll("option");
        Assert.All(options, option => Assert.True(option.HasAttribute("disabled")));
    }

    [Fact]
    public void Select_CompleteStructure_HasCorrectAttributes()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "complete-select")
            .AddUnmatched("width", Width.Full)
            .AddUnmatched("size", Size.Medium)
            .AddUnmatched("name", "fruit-selector")
            .AddUnmatched("aria-label", "Select a fruit")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Option>(0);
                builder.AddAttribute(1, "Value", "");
                builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Choose fruit...")));
                builder.CloseComponent();

                builder.OpenComponent<Option>(3);
                builder.AddAttribute(4, "Value", "apple");
                builder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Apple")));
                builder.CloseComponent();
            }));

        var select = component.Find("select");

        Assert.Equal("complete-select", select.Id);
        Assert.Contains("ds-input", select.ClassList);
        Assert.Equal("full", select.GetAttribute("data-width"));
        Assert.Equal("md", select.GetAttribute("data-size"));
        Assert.Equal("fruit-selector", select.GetAttribute("name"));
        Assert.Equal("Select a fruit", select.GetAttribute("aria-label"));
        Assert.Equal("false", select.GetAttribute("aria-invalid"));

        var options = component.FindAll("option");
        Assert.Equal(2, options.Count);
    }
}