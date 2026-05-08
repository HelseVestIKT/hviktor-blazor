using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;
using FieldComponent = Hviktor.Components.Field.Field;

namespace Tests.Unit.Components.Field;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Field")]
public class FieldTests : HviktorBunitContext
{
    public FieldTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void Field_RendersWithDefaultValues()
    {
        var component = Render<FieldComponent>();

        Assert.NotNull(component.Instance);

        var divElement = component.Find("DIV");
        EnumValue<Position> position = divElement.GetAttribute("data-position");
        Assert.Equal(Position.Start, position);
    }

    [Fact]
    public void Field_HasDsFieldClass()
    {
        var component = Render<FieldComponent>();

        var field = component.Find("div");
        Assert.Contains("ds-field", field.ClassList);
    }

    [Fact]
    public void Field_RendersAsDivElement()
    {
        var component = Render<FieldComponent>();

        var field = component.Find("div");
        Assert.Equal("DIV", field.TagName);
    }

    [Fact]
    public void Field_HasGeneratedId()
    {
        var component = Render<FieldComponent>();

        Assert.NotNull(component.Instance.Id);
        Assert.NotEmpty(component.Instance.Id);
    }

    [Fact]
    public void Field_AcceptsCustomId()
    {
        const string customId = "my-custom-field";
        var component = Render<FieldComponent>(parameters => parameters
            .Add(p => p.Id, customId));

        Assert.Equal(customId, component.Instance.Id);
        var field = component.Find("div");
        Assert.Equal(customId, field.Id);
    }

    [Fact]
    public void Field_GeneratesUniqueIds()
    {
        var component1 = Render<FieldComponent>();
        var component2 = Render<FieldComponent>();

        Assert.NotEqual(component1.Instance.Id, component2.Instance.Id);
    }

    [Fact]
    public void Field_RendersChildContent()
    {
        const string content = "Field content";
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent(content));

        var field = component.Find("div");
        Assert.Contains(content, field.InnerHtml);
    }

    #endregion

    #region Position Tests.Playwright

    [Fact]
    public void Field_DefaultPositionIsStart()
    {
        var component = Render<FieldComponent>();
        var divElement = component.Find("DIV");
        EnumValue<Position> position = divElement.GetAttribute("data-position");
        Assert.Equal(Position.Start, position);
    }

    [Fact]
    public void Field_HasDataPositionAttribute()
    {
        var component = Render<FieldComponent>();

        var field = component.Find("div");
        Assert.Equal("start", field.GetAttribute("data-position"));
    }

    [Fact]
    public void Field_PositionStart_SetsDataAttribute()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddUnmatched("position", Position.Start));

        var field = component.Find("div");
        Assert.Equal("start", field.GetAttribute("data-position"));
    }

    [Fact]
    public void Field_PositionEnd_SetsDataAttribute()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddUnmatched("position", Position.End));

        var field = component.Find("div");
        Assert.Equal("end", field.GetAttribute("data-position"));
    }

    [Theory]
    [InlineData(Position.Start, "start")]
    [InlineData(Position.End, "end")]
    public void Field_AppliesAllPositions(Position position, string expectedDataAttribute)
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddUnmatched("position", position));

        var field = component.Find("div");
        Assert.Equal(expectedDataAttribute, field.GetAttribute("data-position"));
    }

    #endregion

    #region Size Tests.Playwright

    [Fact]
    public void Field_HasNoSizeAttributeWhenNull()
    {
        var component = Render<FieldComponent>();

        var field = component.Find("div");
        Assert.Null(field.GetAttribute("data-size"));
    }

    #endregion

    #region Color Tests.Playwright

    [Fact]
    public void Field_HasNoColorAttributeWhenNull()
    {
        var component = Render<FieldComponent>();

        var field = component.Find("div");
        Assert.Null(field.GetAttribute("data-color"));
    }

    #endregion

    #region Additional Attributes Tests.Playwright

    [Fact]
    public void Field_AppliesAdditionalAttributes()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddUnmatched("data-testid", "field-test")
            .AddUnmatched("aria-label", "Test field"));

        var field = component.Find("div");
        Assert.Equal("field-test", field.GetAttribute("data-testid"));
        Assert.Equal("Test field", field.GetAttribute("aria-label"));
    }

    [Fact]
    public void Field_AppliesCustomCssClass()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddUnmatched("class", "my-custom-field"));

        var field = component.Find("div");
        Assert.Contains("my-custom-field", field.ClassList);
        Assert.Contains("ds-field", field.ClassList);
    }

    #endregion

    #region Cascading Value Tests.Playwright

    [Fact]
    public void Field_ProvidesCascadingValue()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent("<div class='nested-content'>Nested</div>"));

        var field = component.Find("div.ds-field");
        Assert.Contains("nested-content", field.InnerHtml);
    }

    [Fact]
    public void Field_RendersComplexChildContent()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent("<label>Name</label><input type='text' />"));

        var field = component.Find("div.ds-field");
        Assert.Contains("<label>", field.InnerHtml);
        Assert.Contains("<input", field.InnerHtml);
    }

    #endregion

    #region Combined Parameters Tests.Playwright

    [Fact]
    public void Field_CombinesAllParameters()
    {
        const string customId = "full-field";
        var component = Render<FieldComponent>(parameters => parameters
            .Add(p => p.Id, customId)
            .AddUnmatched("position", Position.End)
            .AddChildContent("Full field content"));

        Assert.Equal(customId, component.Instance.Id);

        var field = component.Find("div");
        Assert.Equal(customId, field.Id);

        EnumValue<Position> position = field.GetAttribute("data-position");
        Assert.Equal("end", position);
        Assert.Contains("Full field content", field.InnerHtml);
    }

    #endregion

    #region Edge Cases Tests.Playwright

    [Fact]
    public void Field_EmptyChildContent_RendersEmptyField()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent(""));

        var field = component.Find("div.ds-field");
        Assert.NotNull(field);
        Assert.Empty(field.TextContent.Trim());
    }

    [Fact]
    public void Field_NullChildContent_RendersEmptyField()
    {
        var component = Render<FieldComponent>();

        var field = component.Find("div.ds-field");
        Assert.NotNull(field);
    }

    [Fact]
    public void Field_RendersEmptyWhenNoChildContent()
    {
        var component = Render<FieldComponent>();

        var field = component.Find("div");
        Assert.Empty(field.InnerHtml.Trim());
    }

    [Fact]
    public void Field_SpecialCharactersInId_HandlesCorrectly()
    {
        const string specialId = "field-with-dashes_and_underscores123";
        var component = Render<FieldComponent>(parameters => parameters
            .Add(p => p.Id, specialId));

        var field = component.Find("div.ds-field");
        Assert.Equal(specialId, field.Id);
    }

    [Fact]
    public void Field_VeryLongContent_RendersCompletely()
    {
        var longContent = new string('X', 50000);
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent(longContent));

        var field = component.Find("div.ds-field");
        Assert.Equal(50000, field.TextContent.Length);
    }

    [Fact]
    public void Field_UnicodeContent_RendersCorrectly()
    {
        const string unicodeContent = "日本語フィールド 中文字段 한국어필드 🌍🌎🌏";
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent(unicodeContent));

        var field = component.Find("div.ds-field");
        Assert.Contains("日本語", field.TextContent);
        Assert.Contains("🌍", field.TextContent);
    }

    [Fact]
    public void Field_NestedFields_RenderCorrectly()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent("<div class='ds-field'>Nested field</div>"));

        var fields = component.FindAll("div.ds-field");
        Assert.Equal(2, fields.Count);
    }

    [Fact]
    public void Field_HtmlEntitiesInContent_RendersCorrectly()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent("&lt;script&gt;alert('test')&lt;/script&gt;"));

        var field = component.Find("div.ds-field");
        Assert.Contains("&lt;script&gt;", field.InnerHtml);
    }

    [Fact]
    public void Field_MultipleCustomClasses_AppliesAll()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddUnmatched("class", "class1 class2 class3"));

        var field = component.Find("div.ds-field");
        Assert.Contains("class1", field.ClassList);
        Assert.Contains("class2", field.ClassList);
        Assert.Contains("class3", field.ClassList);
        Assert.Contains("ds-field", field.ClassList);
    }

    [Fact]
    public void Field_DataAttributes_ApplyCorrectly()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddUnmatched("data-custom", "custom-value")
            .AddUnmatched("data-another", "another-value"));

        var field = component.Find("div.ds-field");
        Assert.Equal("custom-value", field.GetAttribute("data-custom"));
        Assert.Equal("another-value", field.GetAttribute("data-another"));
    }

    [Fact]
    public void Field_AriaAttributes_ApplyCorrectly()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddUnmatched("aria-labelledby", "label-id")
            .AddUnmatched("aria-describedby", "desc-id")
            .AddUnmatched("aria-invalid", "true"));

        var field = component.Find("div.ds-field");
        Assert.Equal("label-id", field.GetAttribute("aria-labelledby"));
        Assert.Equal("desc-id", field.GetAttribute("aria-describedby"));
        Assert.Equal("true", field.GetAttribute("aria-invalid"));
    }

    [Fact]
    public void Field_RoleAttribute_AppliesCorrectly()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddUnmatched("role", "group"));

        var field = component.Find("div.ds-field");
        Assert.Equal("group", field.GetAttribute("role"));
    }

    #endregion
}