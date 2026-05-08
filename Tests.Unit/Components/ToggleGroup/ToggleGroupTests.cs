using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Microsoft.AspNetCore.Components;
using ToggleGroupItem = ToggleGroup.Item;

namespace Tests.Unit.Components.ToggleGroup;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "ToggleGroup")]
public class ToggleGroupTests : HviktorBunitContext
{
    public ToggleGroupTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Basic Rendering Tests

    [Fact]
    public void ToggleGroup_RendersFieldsetElement()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>();

        var fieldset = component.Find("fieldset");
        Assert.Equal("FIELDSET", fieldset.TagName);
    }

    [Fact]
    public void ToggleGroup_HasDsToggleGroupClass()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>();

        var fieldset = component.Find("fieldset");
        Assert.Contains("ds-toggle-group", fieldset.ClassList);
    }

    [Fact]
    public void ToggleGroup_RendersChildContent()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddChildContent("<span>Toggle content</span>"));

        var fieldset = component.Find("fieldset");
        Assert.Contains("Toggle content", fieldset.InnerHtml);
    }

    #endregion

    #region Name Tests

    [Fact]
    public void ToggleGroup_WithName_StoresNameParameter()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.Name, "my-toggle-group"));

        Assert.Equal("my-toggle-group", component.Instance.Name);
    }

    [Fact]
    public void ToggleGroup_WithoutName_NameIsNull()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>();

        Assert.Null(component.Instance.Name);
    }

    #endregion

    #region Value Tests

    [Fact]
    public void ToggleGroup_WithValue_ChecksMatchingItem()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.Value, "selected-value")
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<string?>(this, _ => { }))
            .AddChildContent<ToggleGroupItem>(item => item
                .AddUnmatched("value", "selected-value")
                .AddChildContent("Selected")));

        var input = component.Find("input[role='radio']");
        Assert.Equal("true", input.GetAttribute("aria-checked"));
    }

    [Fact]
    public void ToggleGroup_WithDefaultValue_ChecksMatchingItem()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.DefaultValue, "default-value")
            .AddChildContent<ToggleGroupItem>(item => item
                .AddUnmatched("value", "default-value")
                .AddChildContent("Default")));

        var input = component.Find("input[role='radio']");
        Assert.Equal("true", input.GetAttribute("aria-checked"));
    }

    [Fact]
    public void ToggleGroup_ValueOverridesDefaultValue_InControlledMode()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.DefaultValue, "default")
            .Add(p => p.Value, "actual")
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<string?>(this, _ => { }))
            .AddChildContent(builder =>
            {
                builder.OpenComponent<ToggleGroupItem>(0);
                builder.AddAttribute(1, "value", "default");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Default")));
                builder.CloseComponent();

                builder.OpenComponent<ToggleGroupItem>(3);
                builder.AddAttribute(4, "value", "actual");
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Actual")));
                builder.CloseComponent();
            }));

        var inputs = component.FindAll("input[role='radio']");
        Assert.Equal("false", inputs[0].GetAttribute("aria-checked"));
        Assert.Equal("true", inputs[1].GetAttribute("aria-checked"));
    }

    #endregion

    #region Variant Tests

    [Fact]
    public void ToggleGroup_DefaultVariantIsPrimary()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>();

        var fieldset = component.Find("fieldset");
        Assert.Equal("primary", fieldset.GetAttribute("data-variant"));
    }

    [Fact]
    public void ToggleGroup_AppliesSecondaryVariant()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddUnmatched("variant", Variant.Secondary));

        var fieldset = component.Find("fieldset");
        Assert.Equal("secondary", fieldset.GetAttribute("data-variant"));
    }

    #endregion

    #region Size Tests

    [Fact]
    public void ToggleGroup_NoSizeByDefault()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>();

        var fieldset = component.Find("fieldset");
        Assert.Null(fieldset.GetAttribute("data-size"));
    }

    [Fact]
    public void ToggleGroup_AppliesSmallSize()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddUnmatched("size", Size.Small));

        var fieldset = component.Find("fieldset");
        Assert.Equal("sm", fieldset.GetAttribute("data-size"));
    }

    [Fact]
    public void ToggleGroup_AppliesMediumSize()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddUnmatched("size", Size.Medium));

        var fieldset = component.Find("fieldset");
        Assert.Equal("md", fieldset.GetAttribute("data-size"));
    }

    [Fact]
    public void ToggleGroup_AppliesLargeSize()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddUnmatched("size", Size.Large));

        var fieldset = component.Find("fieldset");
        Assert.Equal("lg", fieldset.GetAttribute("data-size"));
    }

    #endregion

    #region Color Tests

    [Fact]
    public void ToggleGroup_NoColorByDefault()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>();

        var fieldset = component.Find("fieldset");
        Assert.Null(fieldset.GetAttribute("data-color"));
    }

    [Fact]
    public void ToggleGroup_AppliesAccentColor()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddUnmatched("color", Color.Accent));

        var fieldset = component.Find("fieldset");
        Assert.Equal("accent", fieldset.GetAttribute("data-color"));
    }

    [Fact]
    public void ToggleGroup_AppliesNeutralColor()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddUnmatched("color", Color.Neutral));

        var fieldset = component.Find("fieldset");
        Assert.Equal("neutral", fieldset.GetAttribute("data-color"));
    }

    #endregion

    #region Additional Attributes Tests

    [Fact]
    public void ToggleGroup_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddUnmatched("data-testid", "toggle-test")
            .AddUnmatched("aria-label", "Toggle options"));

        var fieldset = component.Find("fieldset");
        Assert.Equal("toggle-test", fieldset.GetAttribute("data-testid"));
        Assert.Equal("Toggle options", fieldset.GetAttribute("aria-label"));
    }

    [Fact]
    public void ToggleGroup_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .AddUnmatched("class", "custom-toggle"));

        var fieldset = component.Find("fieldset");
        Assert.Contains("custom-toggle", fieldset.ClassList);
        Assert.Contains("ds-toggle-group", fieldset.ClassList);
    }

    #endregion

    #region OnChange (ValueChanged) Tests

    [Fact]
    public void ToggleGroup_OnChange_TriggersValueChangedCallback()
    {
        string? changedValue = null;

        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.Value, "opt1")
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<string?>(this, v => changedValue = v))
            .AddChildContent<ToggleGroupItem>(itemParams => itemParams
                .AddUnmatched("value", "opt2")
                .AddChildContent("Option 2")));

        var input = component.Find("input[role='radio']");
        input.Change(new ChangeEventArgs { Value = "opt2" });

        Assert.Equal("opt2", changedValue);
    }

    [Fact]
    public void ToggleGroup_OnChange_CalledWithCorrectValue()
    {
        var callbackValues = new List<string?>();

        var component = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<string?>(this, v => callbackValues.Add(v)))
            .AddChildContent(builder =>
            {
                builder.OpenComponent<ToggleGroupItem>(0);
                builder.AddAttribute(1, "Value", "first");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "First")));
                builder.CloseComponent();

                builder.OpenComponent<ToggleGroupItem>(3);
                builder.AddAttribute(4, "Value", "second");
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Second")));
                builder.CloseComponent();

                builder.OpenComponent<ToggleGroupItem>(6);
                builder.AddAttribute(7, "Value", "third");
                builder.AddAttribute(8, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Third")));
                builder.CloseComponent();
            }));

        var inputs = component.FindAll("input[role='radio']");

        inputs[0].Change(new ChangeEventArgs { Value = "first" });
        inputs[2].Change(new ChangeEventArgs { Value = "third" });
        inputs[1].Change(new ChangeEventArgs { Value = "second" });

        Assert.Equal(3, callbackValues.Count);
        Assert.Equal("first", callbackValues[0]);
        Assert.Equal("third", callbackValues[1]);
        Assert.Equal("second", callbackValues[2]);
    }

    [Fact]
    public void ToggleGroup_OnChange_NotCalledWithoutInteraction()
    {
        var callbackCalled = false;
        _ = Render<Hviktor.Components.ToggleGroup.ToggleGroup>(parameters => parameters
            .Add(p => p.ValueChanged, EventCallback.Factory.Create<string?>(this, _ => callbackCalled = true))
            .AddChildContent<ToggleGroupItem>(itemParams => itemParams
                .AddUnmatched("value", "item1")
                .AddChildContent("Item 1")));

        Assert.False(callbackCalled);
    }

    #endregion
}

