using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Textfield;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Textfield")]
public class TextfieldTests : HviktorBunitContext
{

    #region Basic Rendering Tests

    [Fact]
    public void Textfield_RendersInputElement()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>();

        var input = component.Find("input");
        Assert.Equal("INPUT", input.TagName);
    }

    [Fact]
    public void Textfield_HasDsInputClass()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>();

        var input = component.Find("input");
        Assert.Contains("ds-input", input.ClassList);
    }

    [Fact]
    public void Textfield_AcceptsCustomId()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "my-textfield"));

        var input = component.Find("input");
        Assert.Equal("my-textfield", input.Id);
    }

    [Fact]
    public void Textfield_RendersInsideFieldComponent()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>();

        var field = component.Find(".ds-field");
        Assert.NotNull(field);
    }

    [Fact]
    public void Textfield_HasFieldAffixesWrapper()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>();

        var affixesWrapper = component.Find(".ds-field-affixes");
        Assert.NotNull(affixesWrapper);
    }

    #endregion

    #region Label Tests

    [Fact]
    public void Textfield_WithLabel_RendersLabelElement()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("label", "Username"));

        var label = component.Find("label");
        Assert.NotNull(label);
        Assert.Contains("Username", label.TextContent);
        Assert.Equal("medium", label.GetAttribute("data-weight"));
    }

    [Fact]
    public void Textfield_WithLabel_LabelHasMediumWeight()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("label", "Test"));

        var label = component.Find("label");
        Assert.Equal("medium", label.GetAttribute("data-weight"));
    }

    [Fact]
    public void Textfield_WithLabel_LabelHasForAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "username-field")
            .AddUnmatched("label", "Username"));

        var label = component.Find("label");
        Assert.Equal("username-field", label.GetAttribute("for"));
    }

    [Fact]
    public void Textfield_WithoutLabel_NoLabelElement()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>();

        var labels = component.FindAll("label");
        Assert.Empty(labels);
    }

    [Fact]
    public void Textfield_WithEmptyLabel_NoLabelElement()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("label", ""));

        var labels = component.FindAll("label");
        Assert.Empty(labels);
    }

    #endregion

    #region Description Tests

    [Fact]
    public void Textfield_WithDescription_RendersDescriptionElement()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "desc-field")
            .AddUnmatched("description", "Enter your username"));

        var description = component.Find("[data-field='description']");
        Assert.NotNull(description);
        Assert.Contains("Enter your username", description.TextContent);
    }

    [Fact]
    public void Textfield_WithDescription_InputHasAriaDescribedby()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "aria-field")
            .AddUnmatched("description", "Help text"));

        var input = component.Find("input");
        var ariaDescribedBy = input.GetAttribute("aria-describedby");
        Assert.NotNull(ariaDescribedBy);
        Assert.Contains("aria-field:description", ariaDescribedBy);
    }

    [Fact]
    public void Textfield_WithoutDescription_NoDescriptionElement()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "no-desc-field"));

        var descriptions = component.FindAll("[data-field='description']");
        Assert.Empty(descriptions);
    }

    #endregion

    #region Value Tests

    [Fact]
    public void Textfield_WithValue_SetsInputValue()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("value", "test value"));

        var input = component.Find("input");
        Assert.Equal("test value", input.GetAttribute("value"));
    }

    [Fact]
    public void Textfield_WithoutValue_NoValueAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>();

        var input = component.Find("input");
        Assert.Null(input.GetAttribute("value"));
    }

    #endregion

    #region Type Tests

    [Fact]
    public void Textfield_WithTextType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Text));

        var input = component.Find("input");
        Assert.Equal("text", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithEmailType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Email));

        var input = component.Find("input");
        Assert.Equal("email", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithPasswordType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Password));

        var input = component.Find("input");
        Assert.Equal("password", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithNumberType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Number));

        var input = component.Find("input");
        Assert.Equal("number", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithTelType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Tel));

        var input = component.Find("input");
        Assert.Equal("tel", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithUrlType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Url));

        var input = component.Find("input");
        Assert.Equal("url", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithoutType_NoDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>();

        var input = component.Find("input");
        Assert.Null(input.GetAttribute("type"));
    }

    #endregion

    #region MultiLine Tests

    [Fact]
    public void Textfield_MultiLineFalse_RendersInput()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("multiline", "false"));

        var inputs = component.FindAll("input");
        var textareas = component.FindAll("textarea");
        Assert.Single(inputs);
        Assert.Empty(textareas);
    }

    [Fact]
    public void Textfield_MultiLineTrue_RendersTextarea()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("multiline", "true"));

        var inputs = component.FindAll("input");
        var textareas = component.FindAll("textarea");
        Assert.Empty(inputs);
        Assert.Single(textareas);
    }

    [Fact]
    public void Textfield_MultiLineTrue_TextareaHasDsInputClass()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("multiline", "true"));

        var textarea = component.Find("textarea");
        Assert.Contains("ds-input", textarea.ClassList);
    }

    [Fact]
    public void Textfield_MultiLineTrue_TextareaHasCorrectId()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "multiline-field")
            .AddUnmatched("multiline", "true"));

        var textarea = component.Find("textarea");
        Assert.Equal("multiline-field", textarea.Id);
    }

    #endregion

    #region Prefix and Suffix Tests

    [Fact]
    public void Textfield_WithPrefix_RendersPrefix()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("prefix", "kr"));

        var prefix = component.Find(".ds-field-affix");
        Assert.NotNull(prefix);
        Assert.Contains("kr", prefix.TextContent);
    }

    [Fact]
    public void Textfield_WithPrefix_HasAriaHidden()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("prefix", "$"));

        var prefix = component.Find(".ds-field-affix");
        Assert.Equal("true", prefix.GetAttribute("aria-hidden"));
    }

    [Fact]
    public void Textfield_WithSuffix_RendersSuffix()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("suffix", "kg"));

        var suffix = component.Find(".ds-field-affix");
        Assert.NotNull(suffix);
        Assert.Contains("kg", suffix.TextContent);
    }

    [Fact]
    public void Textfield_WithPrefixAndSuffix_RendersBoth()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("prefix", "kr")
            .AddUnmatched("suffix", ",-"));

        var affixes = component.FindAll(".ds-field-affix");
        Assert.Equal(2, affixes.Count);
        Assert.Contains("kr", affixes[0].TextContent);
        Assert.Contains(",-", affixes[1].TextContent);
    }

    [Fact]
    public void Textfield_WithoutPrefixOrSuffix_NoAffixElements()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>();

        var affixes = component.FindAll(".ds-field-affix");
        Assert.Empty(affixes);
    }

    #endregion

    #region Error Tests

    [Fact]
    public void Textfield_WithError_RendersValidationMessage()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "error-field")
            .AddUnmatched("error", "This field is required"));

        var validation = component.Find(".ds-validation-message");
        Assert.NotNull(validation);
        Assert.Contains("This field is required", validation.TextContent);
    }

    [Fact]
    public void Textfield_WithError_InputHasAriaInvalid()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("error", "Invalid input"));

        var input = component.Find("input");
        Assert.Equal("true", input.GetAttribute("aria-invalid"));
    }

    [Fact]
    public void Textfield_WithError_InputHasAriaDescribedbyForValidation()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "val-field")
            .AddUnmatched("error", "Error message"));

        var input = component.Find("input");
        var ariaDescribedBy = input.GetAttribute("aria-describedby");
        Assert.NotNull(ariaDescribedBy);
        Assert.Contains("val-field:validation", ariaDescribedBy);
    }

    [Fact]
    public void Textfield_WithoutError_NoValidationMessage()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "no-error-field"));

        var validations = component.FindAll(".ds-validation-message");
        Assert.Empty(validations);
    }

    [Fact]
    public void Textfield_WithoutError_NoAriaInvalid()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>();

        var input = component.Find("input");
        Assert.Null(input.GetAttribute("aria-invalid"));
    }

    #endregion

    #region Counter Tests

    [Fact]
    public void Textfield_WithCounterAndValue_ShowsRemainingCharacters()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "counter-field")
            .AddUnmatched("counter", "100")
            .AddUnmatched("value", "Hello"));

        var counter = component.Find(".ds-field-counter");
        Assert.NotNull(counter);
        Assert.Contains("95 tegn igjen", counter.TextContent);
    }

    [Fact]
    public void Textfield_WithCounterExceeded_ShowsExcessCharacters()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "exceeded-field")
            .AddUnmatched("counter", "5")
            .AddUnmatched("value", "Hello World"));

        var counter = component.Find(".ds-field-counter");
        Assert.NotNull(counter);
        Assert.Contains("6 tegn for mye", counter.TextContent);
    }

    [Fact]
    public void Textfield_WithCounterAndNoValue_ShowsFullCountRemaining()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "no-value-counter")
            .AddUnmatched("counter", "100"));

        var counter = component.Find(".ds-field-counter");
        Assert.NotNull(counter);
        Assert.Contains("100 tegn igjen", counter.TextContent);
    }

    [Fact]
    public void Textfield_WithoutCounter_NoCounterDisplay()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("value", "Some text"));

        var counters = component.FindAll(".ds-field-counter");
        Assert.Empty(counters);
    }

    #endregion

    #region Additional Attributes Tests

    [Fact]
    public void Textfield_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("data-testid", "textfield-test")
            .AddUnmatched("name", "my-field"));

        var input = component.Find("input");
        Assert.Equal("textfield-test", input.GetAttribute("data-testid"));
        Assert.Equal("my-field", input.GetAttribute("name"));
    }

    [Fact]
    public void Textfield_AppliesPlaceholder()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("placeholder", "Enter text..."));

        var input = component.Find("input");
        Assert.Equal("Enter text...", input.GetAttribute("placeholder"));
    }

    [Fact]
    public void Textfield_AppliesRequired()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("required", true));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("required"));
    }

    [Fact]
    public void Textfield_AppliesDisabled()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("disabled", true));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("disabled"));
    }

    [Fact]
    public void Textfield_AppliesReadOnly()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("readonly", true));

        var input = component.Find("input");
        Assert.True(input.HasAttribute("readonly"));
    }

    [Fact]
    public void Textfield_AppliesMaxLength()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("maxlength", "50"));

        var input = component.Find("input");
        Assert.Equal("50", input.GetAttribute("maxlength"));
    }

    #endregion

    #region Combined Parameters Tests

    [Fact]
    public void Textfield_WithAllParameters_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "full-field")
            .AddUnmatched("label", "Email Address")
            .AddUnmatched("description", "We'll never share your email")
            .AddUnmatched("type", InputType.Email)
            .AddUnmatched("value", "test@example.com")
            .AddUnmatched("prefix", "@")
            .AddUnmatched("placeholder", "name@example.com"));

        var input = component.Find("input");
        var label = component.Find("label");
        var description = component.Find("[data-field='description']");
        var prefix = component.Find(".ds-field-affix");

        Assert.Equal("full-field", input.Id);
        Assert.Equal("email", input.GetAttribute("type"));
        Assert.Equal("test@example.com", input.GetAttribute("value"));
        Assert.Contains("Email Address", label.TextContent);
        Assert.Contains("We'll never share your email", description.TextContent);
        Assert.Contains("@", prefix.TextContent);
    }

    [Fact]
    public void Textfield_WithDescriptionAndError_BothInAriaDescribedby()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "combined-field")
            .AddUnmatched("description", "Help text")
            .AddUnmatched("error", "Error message"));

        var input = component.Find("input");
        var ariaDescribedBy = input.GetAttribute("aria-describedby");

        Assert.NotNull(ariaDescribedBy);
        Assert.Contains("combined-field:description", ariaDescribedBy);
        Assert.Contains("combined-field:validation", ariaDescribedBy);
    }

    #endregion

    #region Real-World Usage Tests

    [Fact]
    public void Textfield_LoginForm_EmailField()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "email")
            .AddUnmatched("label", "Email")
            .AddUnmatched("type", InputType.Email)
            .AddUnmatched("name", "email")
            .AddUnmatched("required", true)
            .AddUnmatched("placeholder", "name@example.com")
            .AddUnmatched("autocomplete", "email"));

        var input = component.Find("input");
        var label = component.Find("label");

        Assert.Equal("email", input.Id);
        Assert.Equal("email", input.GetAttribute("type"));
        Assert.Equal("email", input.GetAttribute("name"));
        Assert.True(input.HasAttribute("required"));
        Assert.Equal("name@example.com", input.GetAttribute("placeholder"));
        Assert.Contains("Email", label.TextContent);
    }

    [Fact]
    public void Textfield_PriceInput_WithPrefixAndSuffix()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "price")
            .AddUnmatched("label", "Price")
            .AddUnmatched("type", InputType.Number)
            .AddUnmatched("prefix", "kr")
            .AddUnmatched("suffix", ",-")
            .AddUnmatched("min", "0")
            .AddUnmatched("step", "0.01"));

        var affixes = component.FindAll(".ds-field-affix");
        var input = component.Find("input");

        Assert.Equal(2, affixes.Count);
        Assert.Equal("number", input.GetAttribute("type"));
        Assert.Equal("0", input.GetAttribute("min"));
    }

    [Fact]
    public void Textfield_CommentField_MultiLine()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "comment")
            .AddUnmatched("label", "Comment")
            .AddUnmatched("description", "Maximum 500 characters")
            .AddUnmatched("multiline", "true")
            .AddUnmatched("counter", "500")
            .AddUnmatched("value", "This is my comment")
            .AddUnmatched("rows", "5"));

        var textarea = component.Find("textarea");
        var label = component.Find("label");
        var counter = component.Find(".ds-field-counter");

        Assert.Equal("comment", textarea.Id);
        Assert.Contains("ds-input", textarea.ClassList);
        Assert.Contains("Comment", label.TextContent);
        Assert.Contains("tegn igjen", counter.TextContent);
    }

    #endregion

    #region Aria Label Tests

    [Fact]
    public void Textfield_AppliesAriaLabel()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("aria-label", "Enter your email address"));

        var input = component.Find("input");
        Assert.Equal("Enter your email address", input.GetAttribute("aria-label"));
    }

    [Fact]
    public void Textfield_AppliesAriaLabelledby()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("aria-labelledby", "external-label-id"));

        var input = component.Find("input");
        Assert.Equal("external-label-id", input.GetAttribute("aria-labelledby"));
    }

    [Fact]
    public void Textfield_WithAriaLabelAndLabel_BothApplied()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("label", "Username")
            .AddUnmatched("aria-label", "Enter your username"));

        var input = component.Find("input");
        var label = component.Find("label");

        Assert.Equal("Enter your username", input.GetAttribute("aria-label"));
        Assert.Contains("Username", label.TextContent);
    }

    #endregion

    #region Size Attribute Tests

    [Fact]
    public void Textfield_AppliesSizeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("size", "20"));

        var input = component.Find("input");
        Assert.Equal("20", input.GetAttribute("size"));
    }

    [Fact]
    public void Textfield_SizeDefinesCharacterWidth()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("size", "40"));

        var input = component.Find("input");
        Assert.Equal("40", input.GetAttribute("size"));
    }

    #endregion

    #region Additional Input Types Tests

    [Fact]
    public void Textfield_WithDateType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Date));

        var input = component.Find("input");
        Assert.Equal("date", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithDateTimeLocalType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.DateTimeLocal));

        var input = component.Find("input");
        Assert.Equal("datetime-local", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithSearchType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Search));

        var input = component.Find("input");
        Assert.Equal("search", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithColorType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Color));

        var input = component.Find("input");
        Assert.Equal("color", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithMonthType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Month));

        var input = component.Find("input");
        Assert.Equal("month", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithTimeType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Time));

        var input = component.Find("input");
        Assert.Equal("time", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithWeekType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.Week));

        var input = component.Find("input");
        Assert.Equal("week", input.GetAttribute("type"));
    }

    [Fact]
    public void Textfield_WithFileType_SetsDataTypeAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("type", InputType.File));

        var input = component.Find("input");
        Assert.Equal("file", input.GetAttribute("type"));
    }

    #endregion

    #region Wrapper Class and Style Tests

    [Fact]
    public void Textfield_FieldWrapperHasDsFormFieldClass()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>();

        var field = component.Find(".ds-field");
        Assert.NotNull(field);
    }

    #endregion

    #region Counter Edge Cases Tests

    [Fact]
    public void Textfield_WithCounterAtExactLimit_ShowsZeroRemaining()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "exact-limit-field")
            .AddUnmatched("counter", "5")
            .AddUnmatched("value", "Hello"));

        var counter = component.Find(".ds-field-counter");
        Assert.NotNull(counter);
        Assert.Contains("0 tegn igjen", counter.TextContent);
    }

    [Fact]
    public void Textfield_WithCounterOneOver_ShowsOneExcess()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "one-over-field")
            .AddUnmatched("counter", "5")
            .AddUnmatched("value", "Hello!"));

        var counter = component.Find(".ds-field-counter");
        Assert.NotNull(counter);
        Assert.Contains("1 tegn for mye", counter.TextContent);
    }

    [Fact]
    public void Textfield_WithCounterAndEmptyValue_ShowsFullCountRemaining()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "empty-value-counter")
            .AddUnmatched("counter", "100")
            .AddUnmatched("value", ""));

        var counter = component.Find(".ds-field-counter");
        Assert.NotNull(counter);
        Assert.Contains("100 tegn igjen", counter.TextContent);
    }

    #endregion

    #region Accessibility Complete Tests

    [Fact]
    public void Textfield_WithLabelDescriptionAndError_HasCompleteAccessibility()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "accessible-field")
            .AddUnmatched("label", "Email")
            .AddUnmatched("description", "Enter your work email")
            .AddUnmatched("error", "Invalid email format"));

        var input = component.Find("input");
        var label = component.Find("label");
        var ariaDescribedBy = input.GetAttribute("aria-describedby");

        Assert.Equal("accessible-field", label.GetAttribute("for"));
        Assert.NotNull(ariaDescribedBy);
        Assert.Contains("accessible-field:description:1", ariaDescribedBy);
        Assert.Contains("accessible-field:validation:1", ariaDescribedBy);
        Assert.Equal("true", input.GetAttribute("aria-invalid"));
    }

    [Fact]
    public void Textfield_WithAriaDescribedbyAndDescription_CombinesValues()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "combined-aria-field")
            .AddUnmatched("description", "Help text")
            .AddUnmatched("aria-describedby", "external-description"));

        var input = component.Find("input");
        var ariaDescribedBy = input.GetAttribute("aria-describedby");

        Assert.NotNull(ariaDescribedBy);
        Assert.Contains("external-description", ariaDescribedBy);
        Assert.Contains("combined-aria-field:description:1", ariaDescribedBy);
    }

    #endregion

    #region MultiLine with Additional Attributes Tests

    [Fact]
    public void Textfield_MultiLine_AppliesRowsAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("multiline", "true")
            .AddUnmatched("rows", "10"));

        var textarea = component.Find("textarea");
        Assert.Equal("10", textarea.GetAttribute("rows"));
    }

    [Fact]
    public void Textfield_MultiLine_AppliesColsAttribute()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("multiline", "true")
            .AddUnmatched("cols", "50"));

        var textarea = component.Find("textarea");
        Assert.Equal("50", textarea.GetAttribute("cols"));
    }

    [Fact]
    public void Textfield_MultiLine_WithDescription_HasAriaDescribedby()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("id", "multiline-desc")
            .AddUnmatched("multiline", "true")
            .AddUnmatched("description", "Enter detailed description"));

        var textarea = component.Find("textarea");
        var ariaDescribedBy = textarea.GetAttribute("aria-describedby");
        Assert.NotNull(ariaDescribedBy);
        Assert.Contains("multiline-desc:description:1", ariaDescribedBy);
    }

    [Fact]
    public void Textfield_MultiLine_WithError_HasAriaInvalid()
    {
        var component = Render<Hviktor.Components.Textfield.Textfield>(parameters => parameters
            .AddUnmatched("multiline", "true")
            .AddUnmatched("error", "Field is required"));

        var textarea = component.Find("textarea");
        Assert.Equal("true", textarea.GetAttribute("aria-invalid"));
    }

    #endregion
}