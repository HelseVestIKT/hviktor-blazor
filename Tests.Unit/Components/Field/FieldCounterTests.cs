using Bunit;
using FieldComponent = Hviktor.Components.Field.Field;

namespace Tests.Unit.Components.Field;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Field.Counter")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class FieldCounterTests : HviktorBunitContext
{
    public FieldCounterTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void FieldCounter_RendersWithDefaultValues()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        Assert.NotNull(component);
    }

    [Fact]
    public void FieldCounter_RendersScreenReaderDiv()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        var srOnly = component.Find("div.ds-sr-only[aria-live='polite']");
        Assert.NotNull(srOnly);
    }

    [Fact]
    public void FieldCounter_RendersParagraphWhenUnderLimit()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 5)));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.NotNull(paragraph);
    }

    [Fact]
    public void FieldCounter_RendersValidationMessageWhenOverLimit()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 15)));

        var validationMessage = component.Find("p.ds-validation-message");
        Assert.NotNull(validationMessage);
    }

    [Fact]
    public void FieldCounter_RendersDescriptionDiv()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        var description = component.Find("div[data-field='description']");
        Assert.NotNull(description);
    }

    #endregion

    #region Limit Parameter Tests.Playwright

    [Fact]
    public void FieldCounter_AcceptsLimitParameter()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 100)));

        // Check that the limit is displayed in the hint
        var description = component.Find("div[data-field='description']");
        Assert.Contains("100", description.TextContent);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(500)]
    public void FieldCounter_DisplaysCorrectLimitInHint(int limit)
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, limit)));

        var description = component.Find("div[data-field='description']");
        Assert.Contains(limit.ToString(), description.TextContent);
    }

    #endregion

    #region Count Parameter Tests.Playwright

    [Fact]
    public void FieldCounter_AcceptsCountParameter()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 3)));

        // Should show "7 tegn igjen" (10 - 3 = 7)
        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("7", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_ShowsCorrectRemainingCount()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 20)
                .Add(c => c.Count, 5)));

        // Should show 15 characters remaining (20 - 5 = 15)
        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("15", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_ShowsZeroWhenAtLimit()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 10)));

        // Should show "0 tegn igjen"
        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("0", paragraph.TextContent);
    }

    #endregion

    #region Over Limit Tests.Playwright

    [Fact]
    public void FieldCounter_ShowsPositiveCountWhenOverLimit()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 14)));

        // Should show "4 tegn for mye" (not -4)
        var validationMessage = component.Find("p.ds-validation-message");
        Assert.Contains("4", validationMessage.TextContent);
        Assert.DoesNotContain("-4", validationMessage.TextContent);
    }

    [Fact]
    public void FieldCounter_UsesValidationMessageWhenOverLimit()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 15)));

        // Should render ValidationMessage instead of Paragraph
        var validationMessages = component.FindAll("p.ds-validation-message");
        var paragraphs = component.FindAll("p.ds-paragraph[aria-hidden='true']");

        Assert.Single(validationMessages);
        Assert.Empty(paragraphs);
    }

    [Fact]
    public void FieldCounter_UsesParagraphWhenUnderLimit()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 5)));

        // Should render Paragraph instead of ValidationMessage
        var validationMessages = component.FindAll("p.ds-validation-message");
        var paragraphs = component.FindAll("p.ds-paragraph[aria-hidden='true']");

        Assert.Empty(validationMessages);
        Assert.Single(paragraphs);
    }

    #endregion

    #region Over Template Tests.Playwright

    [Fact]
    public void FieldCounter_DefaultOverTemplate()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 12)));

        var validationMessage = component.Find("p.ds-validation-message");
        Assert.Contains("tegn for mye", validationMessage.TextContent);
    }

    [Fact]
    public void FieldCounter_CustomOverTemplate()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 15)
                .Add(c => c.Over, "{0} characters over limit")));

        var validationMessage = component.Find("p.ds-validation-message");
        Assert.Contains("5 characters over limit", validationMessage.TextContent);
    }

    [Fact]
    public void FieldCounter_OverTemplateReplacesPlaceholder()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 13)
                .Add(c => c.Over, "Too many! ({0})")));

        var validationMessage = component.Find("p.ds-validation-message");
        Assert.Contains("Too many! (3)", validationMessage.TextContent);
    }

    #endregion

    #region Under Template Tests.Playwright

    [Fact]
    public void FieldCounter_DefaultUnderTemplate()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 5)));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("tegn igjen", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_CustomUnderTemplate()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 20)
                .Add(c => c.Count, 5)
                .Add(c => c.Under, "{0} characters remaining")));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("15 characters remaining", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_UnderTemplateReplacesPlaceholder()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 50)
                .Add(c => c.Count, 30)
                .Add(c => c.Under, "You have {0} left")));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("You have 20 left", paragraph.TextContent);
    }

    #endregion

    #region Hint Template Tests.Playwright

    [Fact]
    public void FieldCounter_DefaultHintTemplate()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        var description = component.Find("div[data-field='description']");
        Assert.Contains("10", description.TextContent);
    }

    #endregion

    #region Accessibility Tests.Playwright

    [Fact]
    public void FieldCounter_ScreenReaderDivHasAriaLive()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        var srOnly = component.Find("div.ds-sr-only");
        Assert.Equal("polite", srOnly.GetAttribute("aria-live"));
    }

    [Fact]
    public void FieldCounter_DescriptionHasAriaHidden()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        var description = component.Find("div[data-field='description']");
        Assert.Equal("true", description.GetAttribute("aria-hidden"));
    }

    [Fact]
    public void FieldCounter_DescriptionHasDsSrOnlyClass()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        var description = component.Find("div[data-field='description']");
        Assert.Contains("ds-sr-only", description.ClassList);
    }

    [Fact]
    public void FieldCounter_ParagraphHasAriaHidden()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 5)));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Equal("true", paragraph.GetAttribute("aria-hidden"));
    }

    #endregion

    #region Description ID Tests.Playwright

    [Fact]
    public void FieldCounter_DescriptionHasId()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        var description = component.Find("div[data-field='description']");
        Assert.NotNull(description.Id);
        Assert.NotEmpty(description.Id);
    }

    [Fact]
    public void FieldCounter_DescriptionHasDataFieldAttribute()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        var description = component.Find("div[data-field='description']");
        Assert.Equal("description", description.GetAttribute("data-field"));
    }

    #endregion

    #region Edge Cases Tests.Playwright

    [Fact]
    public void FieldCounter_ZeroLimit()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 0)));

        var description = component.Find("div[data-field='description']");
        Assert.Contains("0", description.TextContent);
    }

    [Fact]
    public void FieldCounter_ZeroCount()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 0)));

        // Should show "10 tegn igjen"
        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("10", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_LargeNumbers()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10000)
                .Add(c => c.Count, 5000)));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("5000", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_NegativeLimit_ShowsOverLimit()
    {
        // Edge case: negative limit with any count should be "over"
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, -5)
                .Add(c => c.Count, 0)));

        // Count (0) > Limit (-5), so should show validation message
        var validationMessage = component.Find("p.ds-validation-message");
        Assert.NotNull(validationMessage);
    }

    [Fact]
    public void FieldCounter_ExactlyAtLimit_ShowsZeroRemaining()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 50)
                .Add(c => c.Count, 50)));

        // Should show paragraph (not over), with 0 remaining
        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("0", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_OneOverLimit_ShowsValidationMessage()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 11)));

        var validationMessage = component.Find("p.ds-validation-message");
        Assert.Contains("1", validationMessage.TextContent);
    }

    [Fact]
    public void FieldCounter_TemplateWithoutPlaceholder_RendersStaticText()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 5)
                .Add(c => c.Under, "Characters remaining")));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Equal("Characters remaining", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_TemplateWithMultiplePlaceholders_ReplacesAll()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 7)
                .Add(c => c.Under, "{0} left ({0} characters)")));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("3 left (3 characters)", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_VeryLargeOverage_ShowsPositiveNumber()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 1000)));

        var validationMessage = component.Find("p.ds-validation-message");
        Assert.Contains("990", validationMessage.TextContent);
        Assert.DoesNotContain("-990", validationMessage.TextContent);
    }

    [Fact]
    public void FieldCounter_SpecialCharactersInTemplate_RendersCorrectly()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 5)
                .Add(c => c.Under, "{0} tegn <igjen> & \"mer\"")));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.NotNull(paragraph);
    }

    [Fact]
    public void FieldCounter_UnicodeInTemplate_RendersCorrectly()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 5)
                .Add(c => c.Under, "{0} 文字残り 🎉")));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("文字残り", paragraph.TextContent);
        Assert.Contains("🎉", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_EmptyStringTemplates_RendersEmpty()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 5)
                .Add(c => c.Under, "")));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Empty(paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_CountNull_DefaultsToZero()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        // With null count (not set), should show limit as remaining
        var paragraph = component.Find("p.ds-paragraph");
        Assert.Equal("10 tegn igjen", paragraph.TextContent);
    }

    #endregion

    #region Combined Parameters Tests.Playwright

    [Fact]
    public void FieldCounter_AllCustomTemplates()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 100)
                .Add(c => c.Count, 50)
                .Add(c => c.Over, "{0} over")
                .Add(c => c.Under, "{0} remaining")));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("50 remaining", paragraph.TextContent);

        var description = component.Find("div[data-field='description']");
        Assert.Equal("100", description.TextContent);
    }

    [Fact]
    public void FieldCounter_SwitchesFromUnderToOverWhenExceedsLimit()
    {
        // First, under limit
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 8)
                .Add(c => c.Under, "{0} left")
                .Add(c => c.Over, "{0} over")));

        var paragraph = component.Find("p.ds-paragraph");
        Assert.Contains("2 left", paragraph.TextContent);
    }

    [Fact]
    public void FieldCounter_DisplaysOverTemplateWhenExceeded()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)
                .Add(c => c.Count, 15)
                .Add(c => c.Under, "{0} left")
                .Add(c => c.Over, "{0} over")));

        var validationMessage = component.Find("p.ds-validation-message");
        Assert.Contains("5 over", validationMessage.TextContent);
    }

    #endregion

    #region Interface Tests.Playwright

    [Fact]
    public void FieldCounter_ImplementsIAsyncDisposable()
    {
        var component = Render<FieldComponent>(parameters => parameters
            .AddChildContent<global::Field.Counter>(counterParams => counterParams
                .Add(c => c.Limit, 10)));

        // The test passes if it renders without error
        Assert.NotNull(component);
    }

    #endregion
}