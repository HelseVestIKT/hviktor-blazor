using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Microsoft.AspNetCore.Components;

namespace Tests.Unit.Components.Fieldset;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Fieldset.Description")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class FieldsetDescriptionTests : HviktorBunitContext
{

    #region Parent Requirement Tests.Playwright

    [Fact]
    public void Description_ThrowsWhenNoParentFieldset()
    {
        Assert.Throws<InvalidOperationException>(() =>
            Render<global::Fieldset.Description>(parameters => parameters
                .AddChildContent("Test description")));
    }

    #endregion

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void Description_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddChildContent("Test description")));

        var description = component.Find("p");
        Assert.NotNull(description);
    }

    [Fact]
    public void Description_HasDsParagraphClass()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddChildContent("Test description")));

        var paragraph = component.Find("p");
        Assert.Contains("ds-paragraph", paragraph.ClassList);
    }

    [Fact]
    public void Description_RendersAsParagraphElement()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddChildContent("Test description")));

        var paragraph = component.Find("p");
        Assert.Equal("P", paragraph.TagName);
    }

    [Fact]
    public void Description_RendersChildContent()
    {
        const string content = "This is a description text";
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddChildContent(content)));

        var paragraph = component.Find("p");
        Assert.Contains(content, paragraph.InnerHtml);
    }

    [Fact]
    public void Description_RendersEmptyParagraphWhenNoChildContent()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>());

        var paragraph = component.Find("p");
        Assert.NotNull(paragraph);
        Assert.Empty(paragraph.InnerHtml.Trim());
    }

    #endregion

    #region Variant Tests.Playwright

    [Fact]
    public void Description_DefaultVariantIsDefault()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddChildContent("Test")));

        var paragraph = component.Find("p");
        Assert.Equal("default", paragraph.GetAttribute("data-variant"));
    }

    [Theory]
    [InlineData(Variant.Short, "short")]
    [InlineData(Variant.Default, "default")]
    [InlineData(Variant.Long, "long")]
    public void Description_AppliesVariant(Variant variant, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddUnmatched("variant", variant)
                .AddChildContent("Test")));

        var paragraph = component.Find("p");
        Assert.Equal(expectedDataAttribute, paragraph.GetAttribute("data-variant"));
    }

    #endregion

    #region Attributes Tests.Playwright

    [Fact]
    public void Description_AcceptsCustomId()
    {
        const string customId = "my-description-id";
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddUnmatched("id", customId)
                .AddChildContent("Test description")));

        var paragraph = component.Find("p");
        Assert.Equal(customId, paragraph.Id);
    }

    [Fact]
    public void Description_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddUnmatched("data-testid", "description-test")
                .AddUnmatched("aria-label", "Description text")
                .AddChildContent("Test description")));

        var paragraph = component.Find("p");
        Assert.Equal("description-test", paragraph.GetAttribute("data-testid"));
        Assert.Equal("Description text", paragraph.GetAttribute("aria-label"));
    }

    [Fact]
    public void Description_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddUnmatched("class", "my-custom-description")
                .AddChildContent("Test description")));

        var paragraph = component.Find("p");
        Assert.Contains("my-custom-description", paragraph.ClassList);
        Assert.Contains("ds-paragraph", paragraph.ClassList);
    }

    [Fact]
    public void Description_CombinesIdAndAttributes()
    {
        const string customId = "desc-1";
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddUnmatched("id", customId)
                .AddUnmatched("data-field", "description")
                .AddChildContent("Help text")));

        var paragraph = component.Find("p");
        Assert.Equal(customId, paragraph.Id);
        Assert.Equal("description", paragraph.GetAttribute("data-field"));
        Assert.Contains("Help text", paragraph.InnerHtml);
    }

    #endregion

    #region Complex Content Tests.Playwright

    [Fact]
    public void Description_RendersComplexChildContent()
    {
        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent<global::Fieldset.Description>(descParams => descParams
                .AddChildContent("<strong>Important:</strong> Please read carefully")));

        var paragraph = component.Find("p");
        Assert.Contains("<strong>Important:</strong>", paragraph.InnerHtml);
        Assert.Contains("Please read carefully", paragraph.InnerHtml);
    }

    #endregion

    #region Multiple Descriptions Tests.Playwright

    [Fact]
    public void Description_MultipleDescriptions_RenderIndependently()
    {
        RenderFragment childContent = builder =>
        {
            builder.OpenComponent<global::Fieldset.Description>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "First description")));
            builder.AddAttribute(2, "id", "desc-1");
            builder.CloseComponent();

            builder.OpenComponent<global::Fieldset.Description>(3);
            builder.AddAttribute(4, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Second description")));
            builder.AddAttribute(5, "id", "desc-2");
            builder.CloseComponent();
        };

        var component = Render<Hviktor.Components.Fieldset.Fieldset>(parameters => parameters
            .AddChildContent(childContent));

        var descriptions = component.FindAll("p.ds-paragraph");

        Assert.Equal(2, descriptions.Count);
        Assert.Equal("desc-1", descriptions[0].Id);
        Assert.Equal("desc-2", descriptions[1].Id);
        Assert.Contains("First description", descriptions[0].InnerHtml);
        Assert.Contains("Second description", descriptions[1].InnerHtml);
    }

    #endregion
}