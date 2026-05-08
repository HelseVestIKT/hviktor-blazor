using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Microsoft.AspNetCore.Components;
using ToggleGroupItem = ToggleGroup.Item;

namespace Tests.Unit.Components.ToggleGroup;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "ToggleGroup")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class ToggleGroupIntegrationTests : HviktorBunitContext
{
    public ToggleGroupIntegrationTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void ToggleGroup_WithMultipleItems_RendersAllItems()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<ToggleGroupItem>(0);
                builder.AddAttribute(1, "Value", "opt1");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 1")));
                builder.CloseComponent();

                builder.OpenComponent<ToggleGroupItem>(3);
                builder.AddAttribute(4, "Value", "opt2");
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 2")));
                builder.CloseComponent();

                builder.OpenComponent<ToggleGroupItem>(6);
                builder.AddAttribute(7, "Value", "opt3");
                builder.AddAttribute(8, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 3")));
                builder.CloseComponent();
            }));

        var inputs = component.FindAll("input[role='radio']");
        Assert.Equal(3, inputs.Count);
    }

    [Fact]
    public void ToggleGroup_WithDefaultValue_CorrectItemSelected()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.DefaultValue, "opt2")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<ToggleGroupItem>(0);
                builder.AddAttribute(1, "Value", "opt1");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 1")));
                builder.CloseComponent();

                builder.OpenComponent<ToggleGroupItem>(3);
                builder.AddAttribute(4, "Value", "opt2");
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 2")));
                builder.CloseComponent();

                builder.OpenComponent<ToggleGroupItem>(6);
                builder.AddAttribute(7, "Value", "opt3");
                builder.AddAttribute(8, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 3")));
                builder.CloseComponent();
            }));

        var labels = component.FindAll("label.ds-button");
        var inputs = component.FindAll("input[role='radio']");

        Assert.Equal("tertiary", labels[0].GetAttribute("data-variant"));
        Assert.Equal("primary", labels[1].GetAttribute("data-variant"));
        Assert.Equal("tertiary", labels[2].GetAttribute("data-variant"));

        Assert.Equal("true", inputs[1].GetAttribute("aria-checked"));
    }

    [Fact]
    public void ToggleGroup_ClickItem_TriggersValueChanged()
    {
        string? changedValue = null;

        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.Value, "opt1")
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<string?>(this, v => changedValue = v))
            .AddChildContent(builder =>
            {
                builder.OpenComponent<ToggleGroupItem>(0);
                builder.AddAttribute(1, "Value", "opt1");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 1")));
                builder.CloseComponent();

                builder.OpenComponent<ToggleGroupItem>(3);
                builder.AddAttribute(4, "Value", "opt2");
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 2")));
                builder.CloseComponent();
            }));

        var inputs = component.FindAll("input[role='radio']");
        inputs[1].Change(new ChangeEventArgs { Value = "opt2" });

        Assert.Equal("opt2", changedValue);
    }

    [Fact]
    public void ToggleGroup_AllItemsShareSameName()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.Name, "shared-name")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<ToggleGroupItem>(0);
                builder.AddAttribute(1, "Value", "opt1");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 1")));
                builder.CloseComponent();

                builder.OpenComponent<ToggleGroupItem>(3);
                builder.AddAttribute(4, "Value", "opt2");
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Option 2")));
                builder.CloseComponent();
            }));

        var inputs = component.FindAll("input[role='radio']");
        Assert.All(inputs, input => Assert.Equal("shared-name", input.GetAttribute("name")));
    }

    [Fact]
    public void ToggleGroup_CompleteStructure_HasCorrectAccessibility()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.Name, "view-mode")
            .Add(p => p.DefaultValue, "list")
            .AddUnmatched("aria-label", "View mode")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<ToggleGroupItem>(0);
                builder.AddAttribute(1, "Value", "grid");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Grid")));
                builder.CloseComponent();

                builder.OpenComponent<ToggleGroupItem>(3);
                builder.AddAttribute(4, "Value", "list");
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "List")));
                builder.CloseComponent();
            }));

        var container = component.Find("fieldset.ds-toggle-group");
        var inputs = component.FindAll("input[role='radio']");
        var labels = component.FindAll("label.ds-button");

        Assert.Equal("View mode", container.GetAttribute("aria-label"));

        Assert.All(inputs, input => Assert.Equal("radio", input.GetAttribute("role")));
        Assert.All(inputs, input => Assert.Equal("view-mode", input.GetAttribute("name")));

        Assert.Equal("tertiary", labels[0].GetAttribute("data-variant"));
        Assert.Equal("primary", labels[1].GetAttribute("data-variant"));
        Assert.Equal("true", inputs[1].GetAttribute("aria-checked"));
    }

    [Fact]
    public void ToggleGroup_WithSizeAndColor_AppliesStyles()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddUnmatched("size", Size.Large)
            .AddUnmatched("color", Color.Accent)
            .AddUnmatched("variant", Variant.Secondary)
            .AddChildContent<ToggleGroupItem>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item")));

        var container = component.Find("fieldset.ds-toggle-group");

        Assert.Equal("lg", container.GetAttribute("data-size"));
        Assert.Equal("accent", container.GetAttribute("data-color"));
        Assert.Equal("secondary", container.GetAttribute("data-variant"));
    }
}

