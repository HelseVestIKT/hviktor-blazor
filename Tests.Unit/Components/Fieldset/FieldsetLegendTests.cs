using Bunit;

namespace Tests.Unit.Components.Fieldset;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Fieldset.Legend")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class LegendTests : HviktorBunitContext
{

    [Fact]
    public void Legend_RendersWithDefaultValues()
    {
        var component = Render<global::Fieldset.Legend>();
        Assert.NotNull(component.Instance);
    }

    [Fact]
    public void Legend_HasDsLegendClass()
    {
        var component = Render<global::Fieldset.Legend>();

        var legend = component.Find("legend");
        Assert.Contains("ds-label", legend.ClassList);
    }

    [Fact]
    public void Legend_RendersAsLegendElement()
    {
        var component = Render<global::Fieldset.Legend>();

        var legend = component.Find("legend");
        Assert.Equal("LEGEND", legend.TagName);
    }

    [Fact]
    public void Legend_RendersChildContent()
    {
        const string content = "Personal Information";
        var component = Render<global::Fieldset.Legend>(parameters => parameters
            .AddChildContent(content));

        var legend = component.Find("legend");
        Assert.Contains(content, legend.InnerHtml);
    }

    [Fact]
    public void Legend_AppliesAdditionalAttributes()
    {
        var component = Render<global::Fieldset.Legend>(parameters => parameters
            .AddUnmatched("data-testid", "legend-test")
            .AddUnmatched("id", "legend-id"));

        var legend = component.Find("legend");
        Assert.Equal("legend-test", legend.GetAttribute("data-testid"));
        Assert.Equal("legend-id", legend.GetAttribute("id"));
    }

    [Fact]
    public void Legend_AppliesCustomCssClass()
    {
        var component = Render<global::Fieldset.Legend>(parameters => parameters
            .AddUnmatched("class", "my-custom-legend"));

        var legend = component.Find("legend");
        Assert.Contains("my-custom-legend", legend.ClassList);
        Assert.Contains("ds-label", legend.ClassList);
    }

    [Fact]
    public void Legend_RendersEmptyWhenNoChildContent()
    {
        var component = Render<global::Fieldset.Legend>();

        var legend = component.Find("legend");
        Assert.Empty(legend.InnerHtml.Trim());
    }
}