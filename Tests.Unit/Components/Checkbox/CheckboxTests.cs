using Bunit;
using Microsoft.AspNetCore.Components;

namespace Tests.Unit.Components.Checkbox;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Checkbox")]
public class CheckboxTests : HviktorBunitContext
{
    public CheckboxTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Checkbox_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>();

        Assert.NotNull(component.Instance);

        var attr = component.Instance.AdditionalAttributes;
        var isChecked = attr?.GetValueOrDefault("checked");
        Assert.Null(isChecked);

        var readOnly = attr?.GetValueOrDefault("readOnly");
        Assert.False(readOnly is false);

        Assert.Null(component.Instance.Label);
        Assert.Null(component.Instance.Description);
    }

    [Fact]
    public void Checkbox_HasDsInputClass()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>();

        var input = component.Find("input");
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    public void Checkbox_HasGeneratedId()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>();

        Assert.NotNull(component.Instance.Id);
        Assert.NotEmpty(component.Instance.Id);
    }

    [Fact]
    public void Checkbox_AcceptsCustomId()
    {
        const string customId = "my-custom-checkbox";
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Id, customId));

        Assert.Equal(customId, component.Instance.Id);
        var input = component.Find("input");
        Assert.Equal(customId, input.Id);
    }

    [Fact]
    public void Checkbox_HasTypeCheckbox()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>();

        var input = component.Find("input");
        Assert.Equal("checkbox", input.GetAttribute("type"));
    }

    [Fact]
    public void Checkbox_RendersLabel()
    {
        const string labelText = "Accept terms";
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Label, labelText));

        var label = component.Find("label");
        Assert.Contains(labelText, label.InnerHtml);
    }

    [Fact]
    public void Checkbox_DoesNotRenderLabelWhenNull()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>();

        Assert.Throws<ElementNotFoundException>(() => component.Find("label"));
    }

    [Fact]
    public void Checkbox_RendersDescription()
    {
        const string descriptionText = "Please read and accept our terms";
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Description, descriptionText));

        var description = component.Find("[data-field='description']");
        Assert.Contains(descriptionText, description.InnerHtml);
    }

    [Fact]
    public void Checkbox_DoesNotRenderDescriptionWhenNull()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>();

        Assert.Throws<ElementNotFoundException>(() => component.Find("[data-field='description']"));
    }

    [Fact]
    public void Checkbox_LabelHasForAttribute()
    {
        const string customId = "my-checkbox";
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Id, customId)
            .Add(p => p.Label, "Test label"));

        var label = component.Find("label");
        Assert.Equal(customId, label.GetAttribute("for"));
    }

    [Fact]
    public void Checkbox_IsCheckedWhenValueTrue()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("checked", true));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("checked"));
    }

    [Fact]
    public void Checkbox_IsNotCheckedWhenValueFalse()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("checked", false));

        var input = component.Find("input");
        Assert.False(input.HasAttribute("checked"));
    }

    [Fact]
    public void Checkbox_IsNotCheckedWhenValueNull()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("checked"));

        var input = component.Find("input");
        Assert.False(input.HasAttribute("checked"));
    }

    [Fact]
    public void Checkbox_HasReadOnlyAttribute()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("readOnly", true));

        var attr = component.Instance.AdditionalAttributes;
        var readOnly = attr?.GetValueOrDefault("readOnly");
        Assert.True(readOnly is true);
    }

    [Fact]
    public void Checkbox_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("data-testid", "checkbox-test")
            .AddUnmatched("aria-label", "Custom aria label"));

        var input = component.Find("input");
        Assert.Equal("checkbox-test", input.GetAttribute("data-testid"));
        Assert.Equal("Custom aria label", input.GetAttribute("aria-label"));
    }

    [Fact]
    public void Checkbox_GeneratesUniqueIds()
    {
        var component1 = Render<Hviktor.Components.Checkbox.Checkbox>();
        var component2 = Render<Hviktor.Components.Checkbox.Checkbox>();

        Assert.NotEqual(component1.Instance.Id, component2.Instance.Id);
    }

    [Fact]
    public void Checkbox_RendersInsideFieldComponent()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>();
        var field = component.Find(".ds-field");
        Assert.NotNull(field);
    }

    [Fact]
    public void Checkbox_HasAriaDescribedBy()
    {
        const string customId = "my-checkbox";
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Id, customId)
            .Add(p => p.Description, "Some description"));

        var input = component.Find("input");
        Assert.Equal($"{customId}:description", input.GetAttribute("aria-describedby"));
    }

    [Fact]
    public void Checkbox_LabelHasCorrectId()
    {
        const string customId = "my-checkbox";
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Id, customId)
            .Add(p => p.Label, "Test label"));

        var label = component.Find("label");
        Assert.Equal($"{customId}:label", label.Id);
    }

    [Fact]
    public void Checkbox_DescriptionHasCorrectId()
    {
        const string customId = "my-checkbox";
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Id, customId)
            .Add(p => p.Description, "Test description"));

        var description = component.Find("[data-field='description']");
        Assert.Equal($"{customId}:description", description.Id);
    }

    [Fact]
    public void Checkbox_TriggersCheckedChangedOnClick()
    {
        var checkedValue = false;
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("checked", false)
            .AddUnmatched("onchange", (ChangeEventArgs args) => checkedValue = args.Value is true or "true" or "True"));

        var input = component.Find("input");
        input.Change(true);

        Assert.True(checkedValue);
    }

    [Fact]
    public void Checkbox_RendersWithLabelAndDescription()
    {
        const string label = "Remember me";
        const string description = "Keep me logged in";

        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Label, label)
            .Add(p => p.Description, description));

        var labelElement = component.Find("label");
        var descriptionElement = component.Find("[data-field='description']");

        Assert.Contains(label, labelElement.InnerHtml);
        Assert.Contains(description, descriptionElement.InnerHtml);
    }

    [Fact]
    public void Checkbox_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var input = component.Find("input");
        Assert.Contains("my-custom-class", input.ClassList);
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    public void Checkbox_AcceptsKeyParameter()
    {
        const string key = "terms-checkbox";
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("key", key));

        var attr = component.Instance.AdditionalAttributes;
        var attrKey = attr?.GetValueOrDefault("key");
        Assert.Equal(key, attrKey);
    }

    [Fact]
    public void Checkbox_LabelHasDataFieldAttribute()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Label, "Test label"));

        var label = component.Find("label");
        Assert.Equal("label", label.GetAttribute("data-field"));
    }

    [Fact]
    public void Checkbox_LabelHasRegularWeight()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Label, "Test label"));

        var label = component.Find("label");
        Assert.Equal("regular", label.GetAttribute("data-weight"));
    }

    [Fact]
    public void Checkbox_CombinesAllParameters()
    {
        const string customId = "full-checkbox";
        const string label = "Full checkbox";
        const string description = "With all parameters";

        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .Add(p => p.Id, customId)
            .Add(p => p.Label, label)
            .Add(p => p.Description, description)
            .AddUnmatched("checked", true)
            .AddUnmatched("readOnly", false));

        var input = component.Find("input");
        Assert.Equal(customId, input.Id);
        Assert.True(input.HasAttribute("checked"));

        var labelElement = component.Find("label");
        Assert.Contains(label, labelElement.InnerHtml);

        var descriptionElement = component.Find("[data-field='description']");
        Assert.Contains(description, descriptionElement.InnerHtml);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Checkbox_ReflectsCheckedState(bool isChecked)
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("Checked", isChecked));

        var input = component.Find("input");
        Assert.Equal(isChecked, input.HasAttribute("checked"));
    }

    [Fact]
    public void Checkbox_RendersInputElement()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>();

        var input = component.Find("input");
        Assert.Equal("INPUT", input.TagName);
    }

    #region Re-render stability (ElementReferenceCapture sequence fix)

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Checkbox_ReRendersWithoutError_WhenCheckedStateChanges()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("checked", false));

        component.Render(parameters => parameters
            .AddUnmatched("checked", true));

        var reRenderedInput = component.Find("input");
        Assert.NotNull(reRenderedInput);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Checkbox_ReRendersWithoutError_WhenCheckedToggledMultipleTimes()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("checked", false));

        for (var i = 0; i < 5; i++)
        {
            var mod2 = i % 2;
            component.Render(parameters => parameters
                .AddUnmatched("checked", mod2 == 0));
            var input = component.Find("input");
            Assert.NotNull(input);
        }
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Checkbox_ReRendersWithoutError_WhenAttributeCountChanges()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("checked", true)
            .AddUnmatched("data-testid", "re-render-test"));

        component.Render(parameters => parameters
            .AddUnmatched("checked", true)
            .AddUnmatched("data-testid", "re-render-test")
            .AddUnmatched("aria-label", "New attribute added")
            .AddUnmatched("data-custom", "extra"));

        var input = component.Find("input");
        Assert.NotNull(input);
        Assert.Equal("New attribute added", input.GetAttribute("aria-label"));
        Assert.Equal("extra", input.GetAttribute("data-custom"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Checkbox_ReRendersWithoutError_WhenIndeterminateStateChanges()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters => parameters
            .AddUnmatched("allowIndeterminate", true)
            .AddUnmatched("checked", true));

        component.Render(parameters => parameters
            .AddUnmatched("allowIndeterminate", true));

        var input = component.Find("input");
        Assert.NotNull(input);
        Assert.False(input.HasAttribute("checked"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Checkbox_ReRendersWithoutError_WhenAttributeCountExceedsReservedSequence()
    {
        var component = Render<Hviktor.Components.Checkbox.Checkbox>(parameters =>
        {
            parameters.AddUnmatched("checked", true);
            for (var i = 0; i < 10_000; i++)
            {
                parameters.AddUnmatched($"data-test-attr-{i}", i);
            }
        });

        component.Render(parameters => parameters
            .AddUnmatched("checked", true));

        var input = component.Find("input");
        Assert.NotNull(input);
    }

    #endregion
}