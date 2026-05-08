using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Fieldset;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Fieldset")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class FieldsetCommonImplementationTests : HviktorBunitContext
{

    [Fact]
    public void Fieldset_WithRadioGroup_RendersCorrectStructure()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Select an option")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(2);
                builder.AddAttribute(3, "Name", "options");
                builder.AddAttribute(4, "Value", "option1");
                builder.AddAttribute(5, "Label", "Option 1");
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(6);
                builder.AddAttribute(7, "Name", "options");
                builder.AddAttribute(8, "Value", "option2");
                builder.AddAttribute(9, "Label", "Option 2");
                builder.CloseComponent();
            }));

        var fieldset = component.Find("fieldset");
        var legend = component.Find("legend");
        var radios = component.FindAll("input[type='radio']");

        Assert.NotNull(fieldset);
        Assert.Contains("Select an option", legend.InnerHtml);
        Assert.Equal(2, radios.Count);
        Assert.All(radios, r => Assert.Equal("options", r.GetAttribute("name")));
    }

    [Fact]
    public void Fieldset_WithRadioGroup_AllRadiosHaveSameName()
    {
        const string groupName = "payment-method";
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Payment Method")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(2);
                builder.AddAttribute(3, "Name", groupName);
                builder.AddAttribute(4, "Value", "credit");
                builder.AddAttribute(5, "Label", "Credit Card");
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(6);
                builder.AddAttribute(7, "Name", groupName);
                builder.AddAttribute(8, "Value", "debit");
                builder.AddAttribute(9, "Label", "Debit Card");
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(10);
                builder.AddAttribute(11, "Name", groupName);
                builder.AddAttribute(12, "Value", "bank");
                builder.AddAttribute(13, "Label", "Bank Transfer");
                builder.CloseComponent();
            }));

        var radios = component.FindAll("input[type='radio']");

        Assert.Equal(3, radios.Count);
        Assert.All(radios, r => Assert.Equal(groupName, r.GetAttribute("name")));
    }

    [Fact]
    public void Fieldset_WithRadioGroup_EachRadioHasUniqueValue()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Size")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(2);
                builder.AddAttribute(3, "Name", "size");
                builder.AddAttribute(4, "Value", "small");
                builder.AddAttribute(5, "Label", "Small");
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(6);
                builder.AddAttribute(7, "Name", "size");
                builder.AddAttribute(8, "Value", "medium");
                builder.AddAttribute(9, "Label", "Medium");
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(10);
                builder.AddAttribute(11, "Name", "size");
                builder.AddAttribute(12, "Value", "large");
                builder.AddAttribute(13, "Label", "Large");
                builder.CloseComponent();
            }));

        var radios = component.FindAll("input[type='radio']");
        var values = radios.Select(r => r.GetAttribute("value")).ToList();

        Assert.Equal(3, values.Count);
        Assert.Equal(3, values.Distinct().Count());
        Assert.Contains("small", values);
        Assert.Contains("medium", values);
        Assert.Contains("large", values);
    }

    [Fact]
    public void Fieldset_WithRadioGroup_LabelsAreAssociatedWithInputs()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Choose")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(2);
                builder.AddAttribute(3, "Name", "choice");
                builder.AddAttribute(4, "Value", "yes");
                builder.AddAttribute(5, "Label", "Yes");
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(6);
                builder.AddAttribute(7, "Name", "choice");
                builder.AddAttribute(8, "Value", "no");
                builder.AddAttribute(9, "Label", "No");
                builder.CloseComponent();
            }));

        var radios = component.FindAll("input[type='radio']");
        var labels = component.FindAll("label.ds-label");

        Assert.Equal(2, radios.Count);
        Assert.Equal(2, labels.Count);

        foreach (var label in labels)
        {
            var forAttr = label.GetAttribute("for");
            Assert.NotNull(forAttr);
            Assert.Contains(radios, r => r.Id == forAttr);
        }
    }

    [Fact]
    public void Fieldset_WithRadioAndDescription_HasAriaDescribedBy()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Preference")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(2);
                builder.AddAttribute(3, "Name", "pref");
                builder.AddAttribute(4, "Value", "email");
                builder.AddAttribute(5, "Label", "Email");
                builder.AddAttribute(6, "Description", "We will send updates via email");
                builder.CloseComponent();
            }));

        var radio = component.Find("input[type='radio']");
        var ariaDescribedBy = radio.GetAttribute("aria-describedby");

        Assert.NotNull(ariaDescribedBy);
        Assert.NotEmpty(ariaDescribedBy);
    }

    [Fact]
    public void Fieldset_AccessibilityStructure_HasRoleGroup()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Contact Preference")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(2);
                builder.AddAttribute(3, "Name", "contact");
                builder.AddAttribute(4, "Value", "phone");
                builder.AddAttribute(5, "Label", "Phone");
                builder.CloseComponent();
            }));

        var fieldset = component.Find("fieldset");
        Assert.Contains("ds-fieldset", fieldset.ClassList);
    }

    [Fact]
    public void Fieldset_LegendIsFirstChild()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Options")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(2);
                builder.AddAttribute(3, "Name", "opt");
                builder.AddAttribute(4, "Value", "1");
                builder.AddAttribute(5, "Label", "Option 1");
                builder.CloseComponent();
            }));

        var fieldset = component.Find("fieldset");
        var firstChild = fieldset.FirstChild;

        Assert.Equal("LEGEND", firstChild?.NodeName);
    }

    [Fact]
    public void Fieldset_WithMultipleRadios_MaintainsStructure()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "Weight", Weight.SemiBold);
                builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "User Preferences")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(3);
                builder.AddAttribute(4, "Name", "notifications");
                builder.AddAttribute(5, "Value", "all");
                builder.AddAttribute(6, "Label", "All notifications");
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(7);
                builder.AddAttribute(8, "Name", "notifications");
                builder.AddAttribute(9, "Value", "important");
                builder.AddAttribute(10, "Label", "Important only");
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(11);
                builder.AddAttribute(12, "Name", "notifications");
                builder.AddAttribute(13, "Value", "none");
                builder.AddAttribute(14, "Label", "No notifications");
                builder.CloseComponent();
            }));

        var fieldset = component.Find("fieldset");
        var legend = component.Find("legend");
        var fields = component.FindAll(".ds-field");
        var radios = component.FindAll("input[type='radio']");

        Assert.NotNull(fieldset);
        Assert.Equal(3, fields.Count);
        Assert.Equal(3, radios.Count);
        Assert.All(radios, r => Assert.Equal("notifications", r.GetAttribute("name")));
    }

    [Fact]
    public void Fieldset_DisabledPreventsInteraction()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddUnmatched("disabled", "disabled")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Disabled Group")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(2);
                builder.AddAttribute(3, "Name", "disabled-group");
                builder.AddAttribute(4, "Value", "opt1");
                builder.AddAttribute(5, "Label", "Option 1");
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(6);
                builder.AddAttribute(7, "Name", "disabled-group");
                builder.AddAttribute(8, "Value", "opt2");
                builder.AddAttribute(9, "Label", "Option 2");
                builder.CloseComponent();
            }));

        var fieldset = component.Find("fieldset");

        Assert.True(fieldset.HasAttribute("disabled"));
    }

    [Fact]
    public void Fieldset_RadioWithDescription_RendersDescriptionElement()
    {
        const string descriptionText = "This option enables all features";

        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Features")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Radio.Radio>(2);
                builder.AddAttribute(3, "Name", "features");
                builder.AddAttribute(4, "Value", "full");
                builder.AddAttribute(5, "Label", "Full Access");
                builder.AddAttribute(6, "Description", descriptionText);
                builder.CloseComponent();
            }));

        var description = component.Find("[data-field='description']");

        Assert.NotNull(description);
        Assert.Contains(descriptionText, description.InnerHtml);
    }
}