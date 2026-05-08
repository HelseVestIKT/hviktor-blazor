using Bunit;

namespace Tests.Unit.Components.Fieldset;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Fieldset")]
public class FieldsetTests : HviktorBunitContext
{

    [Fact]
    public void Fieldset_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>();

        Assert.NotNull(component.Instance);
    }

    [Fact]
    public void Fieldset_HasDsFieldsetClass()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>();

        var fieldset = component.Find("fieldset");
        Assert.Contains("ds-fieldset", fieldset.ClassList);
    }

    [Fact]
    public void Fieldset_RendersAsFieldsetElement()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>();

        var fieldset = component.Find("fieldset");
        Assert.Equal("FIELDSET", fieldset.TagName);
    }

    [Fact]
    public void Fieldset_RendersChildContent()
    {
        const string content = "Fieldset content";
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(content));

        var fieldset = component.Find("fieldset");
        Assert.Contains(content, fieldset.InnerHtml);
    }

    [Fact]
    public void Fieldset_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddUnmatched("data-testid", "fieldset-test")
            .AddUnmatched("aria-label", "Form group"));

        var fieldset = component.Find("fieldset");
        Assert.Equal("fieldset-test", fieldset.GetAttribute("data-testid"));
        Assert.Equal("Form group", fieldset.GetAttribute("aria-label"));
    }

    [Fact]
    public void Fieldset_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddUnmatched("class", "my-custom-fieldset"));

        var fieldset = component.Find("fieldset");
        Assert.Contains("my-custom-fieldset", fieldset.ClassList);
        Assert.Contains("ds-fieldset", fieldset.ClassList);
    }

    [Fact]
    public void Fieldset_RendersEmptyWhenNoChildContent()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>();

        var fieldset = component.Find("fieldset");
        Assert.Empty(fieldset.InnerHtml.Trim());
    }

    [Fact]
    public void Fieldset_RendersComplexChildContent()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent("<legend>Personal Info</legend><input type='text' name='name' />"));

        var fieldset = component.Find("fieldset");
        Assert.Contains("<legend>Personal Info</legend>", fieldset.InnerHtml);
        Assert.Contains("<input type=\"text\" name=\"name\">", fieldset.InnerHtml);
    }

    [Fact]
    public void Fieldset_AppliesDisabledAttribute()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddUnmatched("disabled", "disabled"));

        var fieldset = component.Find("fieldset");
        Assert.True(fieldset.HasAttribute("disabled"));
    }

    [Fact]
    public void Fieldset_AppliesNameAttribute()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddUnmatched("name", "personal-info"));

        var fieldset = component.Find("fieldset");
        Assert.Equal("personal-info", fieldset.GetAttribute("name"));
    }
}