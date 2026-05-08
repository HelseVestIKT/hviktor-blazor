using Bunit;

namespace Tests.Unit.Components.Fieldset;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Fieldset")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class FieldsetIntegrationTests : HviktorBunitContext
{

    [Fact]
    public void Fieldset_RendersWithLegend()
    {
        const string legendText = "Contact Details";

        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Legend>(legendParams => legendParams
                .AddChildContent(legendText)));

        var fieldset = component.Find("fieldset");
        var legend = component.Find("legend");

        Assert.NotNull(fieldset);
        Assert.NotNull(legend);
        Assert.Contains(legendText, legend.InnerHtml);
    }

    [Fact]
    public void Fieldset_RendersWithLegendAndFields()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Fieldset.Legend>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "User Info")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Field.Field>(2);
                builder.AddAttribute(3, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Field 1 content")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Field.Field>(4);
                builder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Field 2 content")));
                builder.CloseComponent();
            }));

        var fieldset = component.Find("fieldset");
        var legend = component.Find("legend");
        var fields = component.FindAll(".ds-field");

        Assert.NotNull(fieldset);
        Assert.NotNull(legend);
        Assert.Contains("User Info", legend.InnerHtml);
        Assert.Equal(2, fields.Count);
    }

    [Fact]
    public void Fieldset_AppliesDisabledToChildren()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddUnmatched("disabled", "disabled")
            .AddChildContent("<input type='text' name='test' />"));

        var fieldset = component.Find("fieldset");
        Assert.True(fieldset.HasAttribute("disabled"));
    }
}