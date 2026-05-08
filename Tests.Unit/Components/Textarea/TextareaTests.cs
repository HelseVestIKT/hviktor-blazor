using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Textarea;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Textarea")]
public class TextareaTests : HviktorBunitContext
{

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void Textarea_RendersTextareaElement()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>();

        var textarea = component.Find("textarea");
        Assert.Equal("TEXTAREA", textarea.TagName);
    }

    [Fact]
    public void Textarea_HasDsInputClass()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>();

        var textarea = component.Find("textarea");
        Assert.Contains("ds-input", textarea.ClassList);
    }

    [Fact]
    public void Textarea_AcceptsCustomId()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("id", "my-textarea"));

        var textarea = component.Find("textarea");
        Assert.Equal("my-textarea", textarea.Id);
    }

    #endregion

    #region Disabled Tests.Playwright

    [Fact]
    public void Textarea_NotDisabledByDefault()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>();

        var textarea = component.Find("textarea");
        Assert.False(textarea.HasAttribute("disabled"));
    }

    [Fact]
    public void Textarea_CanBeDisabled()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("disabled", true));

        var textarea = component.Find("textarea");
        Assert.True(textarea.HasAttribute("disabled"));
    }

    [Fact]
    public void Textarea_DisabledFalse_NoAttribute()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("disabled", false));

        var textarea = component.Find("textarea");
        Assert.False(textarea.HasAttribute("disabled"));
    }

    #endregion

    #region ReadOnly Tests.Playwright

    [Fact]
    public void Textarea_NotReadOnlyByDefault()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>();

        var textarea = component.Find("textarea");
        Assert.False(textarea.HasAttribute("readonly"));
    }

    [Fact]
    public void Textarea_CanBeReadOnly()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("readonly", true));

        var textarea = component.Find("textarea");
        Assert.True(textarea.HasAttribute("readonly"));
    }

    [Fact]
    public void Textarea_ReadOnlyFalse_NoAttribute()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("readonly", false));

        var textarea = component.Find("textarea");
        Assert.False(textarea.HasAttribute("readonly"));
    }

    #endregion

    #region Cols and Rows Tests.Playwright

    [Fact]
    public void Textarea_NoColsAttributeByDefault()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>();

        var textarea = component.Find("textarea");
        Assert.Null(textarea.GetAttribute("cols"));
    }

    [Fact]
    public void Textarea_NoRowsAttributeByDefault()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>();

        var textarea = component.Find("textarea");
        Assert.Null(textarea.GetAttribute("rows"));
    }

    [Fact]
    public void Textarea_AppliesCols()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("cols", 40));

        var textarea = component.Find("textarea");
        Assert.Equal("40", textarea.GetAttribute("cols"));
    }

    [Fact]
    public void Textarea_AppliesRows()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("rows", 10));

        var textarea = component.Find("textarea");
        Assert.Equal("10", textarea.GetAttribute("rows"));
    }

    [Fact]
    public void Textarea_AppliesColsAndRows()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("cols", 50)
            .AddUnmatched("rows", 5));

        var textarea = component.Find("textarea");
        Assert.Equal("50", textarea.GetAttribute("cols"));
        Assert.Equal("5", textarea.GetAttribute("rows"));
    }

    [Fact]
    public void Textarea_ZeroCols_AppliesAttribute()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("cols", 0));

        var textarea = component.Find("textarea");
        Assert.Equal("0", textarea.GetAttribute("cols"));
    }

    [Fact]
    public void Textarea_ZeroRows_AppliesAttribute()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("rows", 0));

        var textarea = component.Find("textarea");
        Assert.Equal("0", textarea.GetAttribute("rows"));
    }

    #endregion

    #region Size Tests.Playwright

    [Fact]
    public void Textarea_NoSizeAttributeByDefault()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>();

        var textarea = component.Find("textarea");
        Assert.Null(textarea.GetAttribute("data-size"));
    }

    [Fact]
    public void Textarea_AppliesSmallSize()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("size", Size.Small));

        var textarea = component.Find("textarea");
        Assert.Equal("sm", textarea.GetAttribute("data-size"));
    }

    [Fact]
    public void Textarea_AppliesMediumSize()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("size", Size.Medium));

        var textarea = component.Find("textarea");
        Assert.Equal("md", textarea.GetAttribute("data-size"));
    }

    [Fact]
    public void Textarea_AppliesLargeSize()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("size", Size.Large));

        var textarea = component.Find("textarea");
        Assert.Equal("lg", textarea.GetAttribute("data-size"));
    }

    #endregion

    #region Additional Attributes Tests.Playwright

    [Fact]
    public void Textarea_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("data-testid", "textarea-test")
            .AddUnmatched("name", "my-textarea"));

        var textarea = component.Find("textarea");
        Assert.Equal("textarea-test", textarea.GetAttribute("data-testid"));
        Assert.Equal("my-textarea", textarea.GetAttribute("name"));
    }

    [Fact]
    public void Textarea_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("class", "custom-textarea"));

        var textarea = component.Find("textarea");
        Assert.Contains("custom-textarea", textarea.ClassList);
        Assert.Contains("ds-input", textarea.ClassList);
    }

    [Fact]
    public void Textarea_AppliesPlaceholder()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("placeholder", "Enter your message..."));

        var textarea = component.Find("textarea");
        Assert.Equal("Enter your message...", textarea.GetAttribute("placeholder"));
    }

    [Fact]
    public void Textarea_AppliesAriaLabel()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("aria-label", "Message input"));

        var textarea = component.Find("textarea");
        Assert.Equal("Message input", textarea.GetAttribute("aria-label"));
    }

    [Fact]
    public void Textarea_AppliesRequired()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("required", true));

        var textarea = component.Find("textarea");
        Assert.True(textarea.HasAttribute("required"));
    }

    [Fact]
    public void Textarea_AppliesMaxLength()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("maxlength", "500"));

        var textarea = component.Find("textarea");
        Assert.Equal("500", textarea.GetAttribute("maxlength"));
    }

    [Fact]
    public void Textarea_AppliesMinLength()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("minlength", "10"));

        var textarea = component.Find("textarea");
        Assert.Equal("10", textarea.GetAttribute("minlength"));
    }

    #endregion

    #region Combined Parameters Tests.Playwright

    [Fact]
    public void Textarea_WithAllParameters_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("id", "full-textarea")
            .AddUnmatched("cols", 60)
            .AddUnmatched("rows", 8)
            .AddUnmatched("size", Size.Medium)
            .AddUnmatched("disabled", false)
            .AddUnmatched("readonly", false));

        var textarea = component.Find("textarea");
        Assert.Equal("full-textarea", textarea.Id);
        Assert.Contains("ds-input", textarea.ClassList);
        Assert.Equal("60", textarea.GetAttribute("cols"));
        Assert.Equal("8", textarea.GetAttribute("rows"));
        Assert.Equal("md", textarea.GetAttribute("data-size"));
        Assert.False(textarea.HasAttribute("disabled"));
        Assert.False(textarea.HasAttribute("readonly"));
    }

    [Fact]
    public void Textarea_DisabledAndReadOnly_AppliesBoth()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("disabled", true)
            .AddUnmatched("readonly", true));

        var textarea = component.Find("textarea");
        Assert.True(textarea.HasAttribute("disabled"));
        Assert.True(textarea.HasAttribute("readonly"));
    }

    #endregion

    #region Real-World Usage Tests.Playwright

    [Fact]
    public void Textarea_CommentBox_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("id", "comment-box")
            .AddUnmatched("rows", 4)
            .AddUnmatched("cols", 50)
            .AddUnmatched("placeholder", "Write your comment...")
            .AddUnmatched("name", "comment")
            .AddUnmatched("aria-label", "Comment"));

        var textarea = component.Find("textarea");
        Assert.Equal("comment-box", textarea.Id);
        Assert.Equal("4", textarea.GetAttribute("rows"));
        Assert.Equal("50", textarea.GetAttribute("cols"));
        Assert.Equal("Write your comment...", textarea.GetAttribute("placeholder"));
        Assert.Equal("comment", textarea.GetAttribute("name"));
        Assert.Equal("Comment", textarea.GetAttribute("aria-label"));
    }

    [Fact]
    public void Textarea_ReadOnlyDisplay_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("readonly", true)
            .AddUnmatched("rows", 6)
            .AddUnmatched("aria-label", "Read-only content"));

        var textarea = component.Find("textarea");
        Assert.True(textarea.HasAttribute("readonly"));
        Assert.Equal("6", textarea.GetAttribute("rows"));
    }

    [Fact]
    public void Textarea_FormField_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Textarea.Textarea>(parameters => parameters
            .AddUnmatched("id", "description")
            .AddUnmatched("size", Size.Large)
            .AddUnmatched("rows", 10)
            .AddUnmatched("name", "description")
            .AddUnmatched("required", true)
            .AddUnmatched("maxlength", "1000")
            .AddUnmatched("placeholder", "Enter description..."));

        var textarea = component.Find("textarea");
        Assert.Equal("description", textarea.Id);
        Assert.Equal("lg", textarea.GetAttribute("data-size"));
        Assert.Equal("10", textarea.GetAttribute("rows"));
        Assert.Equal("description", textarea.GetAttribute("name"));
        Assert.True(textarea.HasAttribute("required"));
        Assert.Equal("1000", textarea.GetAttribute("maxlength"));
        Assert.Equal("Enter description...", textarea.GetAttribute("placeholder"));
    }

    #endregion
}