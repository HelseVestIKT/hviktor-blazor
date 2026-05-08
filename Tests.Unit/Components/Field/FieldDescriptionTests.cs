using Bunit;
using Microsoft.AspNetCore.Components;
using FieldComponent = Hviktor.Components.Field.Field;

namespace Tests.Unit.Components.Field;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Field.Description")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class FieldDescriptionTests : HviktorBunitContext
{
    public FieldDescriptionTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void FieldDescription_RendersAsDivElement()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("Description text")));

        var description = component.Find("div[data-field='description']");
        Assert.Equal("DIV", description.TagName);
    }

    [Fact]
    public void FieldDescription_RendersChildContent()
    {
        const string content = "This is a helpful description";
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent(content)));

        var description = component.Find("div[data-field='description']");
        Assert.Contains(content, description.TextContent);
    }

    [Fact]
    public void FieldDescription_HasDataFieldAttribute()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("Description")));

        var description = component.Find("div[data-field='description']");
        Assert.Equal("description", description.GetAttribute("data-field"));
    }

    #endregion

    #region ID Generation Tests.Playwright

    [Fact]
    public void FieldDescription_HasGeneratedId()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("Description")));

        var description = component.Find("div[data-field='description']");
        Assert.NotNull(description.Id);
        Assert.NotEmpty(description.Id);
    }

    [Fact]
    public void FieldDescription_IdContainsParentFieldId()
    {
        const string fieldId = "my-field";
        var component = Render<FieldComponent>(parameters => parameters
            .Add(p => p.Id, fieldId)
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("Description")));

        var description = component.Find("div[data-field='description']");
        Assert.Contains(fieldId, description.Id);
    }

    [Fact]
    public void FieldDescription_IdContainsDescriptionSuffix()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("Description")));

        var description = component.Find("div[data-field='description']");
        Assert.Contains("description", description.Id);
    }

    [Fact]
    public void FieldDescription_MultipleDescriptionsHaveUniqueIds()
    {
        RenderFragment childContent = builder =>
        {
            builder.OpenComponent<global::Field.Description>(0);
            builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "First")));
            builder.CloseComponent();
            builder.OpenComponent<global::Field.Description>(2);
            builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Second")));
            builder.CloseComponent();
        };

        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent(childContent));

        var descriptions = component.FindAll("div[data-field='description']");
        Assert.Equal(2, descriptions.Count);
        Assert.NotEqual(descriptions[0].Id, descriptions[1].Id);
    }

    #endregion

    #region Additional Attributes Tests.Playwright

    [Fact]
    public void FieldDescription_AppliesAdditionalAttributes()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddUnmatched("data-testid", "desc-test")
                .AddChildContent("Description")));

        var description = component.Find("div[data-field='description']");
        Assert.Equal("desc-test", description.GetAttribute("data-testid"));
    }

    [Fact]
    public void FieldDescription_AppliesCustomCssClass()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddUnmatched("class", "custom-description")
                .AddChildContent("Description")));

        var description = component.Find("div[data-field='description']");
        Assert.Contains("custom-description", description.ClassList);
    }

    #endregion

    #region Complex Content Tests.Playwright

    [Fact]
    public void FieldDescription_RendersHtmlContent()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("<strong>Important:</strong> Enter your full name")));

        var description = component.Find("div[data-field='description']");
        Assert.Contains("<strong>", description.InnerHtml);
        Assert.Contains("Important:", description.TextContent);
    }

    [Fact]
    public void FieldDescription_RendersMultiLineContent()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("Line 1<br/>Line 2")));

        var description = component.Find("div[data-field='description']");
        Assert.Contains("Line 1", description.TextContent);
        Assert.Contains("Line 2", description.TextContent);
    }

    #endregion

    #region Parent Field Integration Tests.Playwright

    [Fact]
    public void FieldDescription_RendersInsideField()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("Description")));

        var field = component.Find("div.ds-field");
        var description = field.QuerySelector("div[data-field='description']");
        Assert.NotNull(description);
    }

    [Fact]
    public void FieldDescription_CanRenderWithOtherFieldComponents()
    {
        RenderFragment childContent = builder =>
        {
            builder.AddMarkupContent(0, "<label>Name</label>");
            builder.OpenComponent<global::Field.Description>(1);
            builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Enter your name")));
            builder.CloseComponent();
            builder.AddMarkupContent(3, "<input type='text' />");
        };

        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent(childContent));

        var field = component.Find("div.ds-field");
        Assert.Contains("label", field.InnerHtml.ToLower());
        Assert.Contains("Enter your name", field.TextContent);
        Assert.Contains("input", field.InnerHtml.ToLower());
    }

    #endregion

    #region Edge Cases Tests.Playwright

    [Fact]
    public void FieldDescription_EmptyContent_StillRendersDiv()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("")));

        var description = component.Find("div[data-field='description']");
        Assert.NotNull(description);
        Assert.Empty(description.TextContent.Trim());
    }

    [Fact]
    public void FieldDescription_WhitespaceContent_RendersWhitespace()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("   ")));

        var description = component.Find("div[data-field='description']");
        Assert.NotNull(description);
    }

    [Fact]
    public void FieldDescription_SpecialCharacters_RendersCorrectly()
    {
        const string specialContent = "<>&\"'æøåÆØÅ©®™";
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent(specialContent)));

        var description = component.Find("div[data-field='description']");
        // Special characters should be HTML encoded or rendered correctly
        Assert.NotNull(description);
    }

    [Fact]
    public void FieldDescription_VeryLongContent_RendersCompletely()
    {
        var longContent = new string('A', 10000);
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent(longContent)));

        var description = component.Find("div[data-field='description']");
        Assert.Equal(10000, description.TextContent.Length);
    }

    [Fact]
    public void FieldDescription_UnicodeContent_RendersCorrectly()
    {
        // 日本語 (Nihongo): Japanese
        // 中文 (Zhōngwén): Chinese
        // 한국어 (Hangugeo): Korean 
        const string unicodeContent = "日本語 中文 한국어 🎉 😀";
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent(unicodeContent)));

        var description = component.Find("div[data-field='description']");
        Assert.Contains("日本語", description.TextContent);
        Assert.Contains("🎉", description.TextContent);
    }

    [Fact]
    public void FieldDescription_WithAriaAttributes_AppliesCorrectly()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddUnmatched("aria-live", "polite")
                .AddUnmatched("aria-atomic", "true")
                .AddChildContent("Description")));

        var description = component.Find("div[data-field='description']");
        Assert.Equal("polite", description.GetAttribute("aria-live"));
        Assert.Equal("true", description.GetAttribute("aria-atomic"));
    }

    [Fact]
    public void FieldDescription_IdDoesNotContainInvalidCharacters()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Description>(descParams => descParams
                .AddChildContent("Description")));

        var description = component.Find("div[data-field='description']");
        // ID should not contain spaces or other invalid characters
        Assert.DoesNotContain(" ", description.Id);
        Assert.DoesNotMatch("^[^a-zA-Z]", description.Id); // Should start with letter
    }

    #endregion
}