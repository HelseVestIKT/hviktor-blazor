using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Textfield;

/// <summary>
/// Semantic behavior tests for the Textfield component.
/// Tests proper HTML structure and accessibility features:
/// - Textfield renders input or textarea based on MultiLine parameter
/// - Label is associated with input via for attribute
/// - Description and error are linked via aria-describedby
/// - Prefix and suffix have aria-hidden for screen readers
/// - Counter displays remaining characters
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public partial class TextfieldSemanticTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "textfield";

    #region Basic Structure Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_RendersInputElement()
    {
        await GoToPageAsync("basic");

        var input = Page.Locator("#textfield-basic");
        await Expect(input).ToBeVisibleAsync();

        var tagName = await input.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
        Assert.Equal("input", tagName);
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_HasDsInputClass()
    {
        await GoToPageAsync("basic");
        var input = Page.Locator("#textfield-basic");
        await Expect(input).ToHaveClassAsync(DsInputClassRegex());
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_LabelIsAssociatedWithInput()
    {
        await GoToPageAsync("basic");

        var label = Page.Locator("label[for='textfield-basic']");
        await Expect(label).ToBeVisibleAsync();
        await Expect(label).ToHaveTextAsync("Name");
    }

    #endregion

    #region Description Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Textfield_WithDescription_HasAriaDescribedby()
    {
        await GoToPageAsync("basic");

        var input = Page.Locator("#textfield-description");
        var ariaDescribedby = await input.GetAttributeAsync("aria-describedby");
        Assert.NotNull(ariaDescribedby);
        Assert.Contains("textfield-description:description", ariaDescribedby);
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_WithDescription_DisplaysDescriptionText()
    {
        await GoToPageAsync("basic");

        var description = Page.Locator("#textfield-description\\:description\\:1");
        await Expect(description).ToBeVisibleAsync();
        await Expect(description).ToContainTextAsync("We'll never share your email");
    }

    #endregion

    #region Prefix and Suffix Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_Prefix_IsVisible()
    {
        await GoToPageAsync("affixes");
        var prefix = Page.Locator("//div[contains(@class, 'ds-field-affixes') and .//input[@id='textfield-prefix']]//span[@class='ds-field-affix']").First;
        await Expect(prefix).ToBeVisibleAsync();
        await Expect(prefix).ToHaveTextAsync("https://");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Textfield_Prefix_HasAriaHidden()
    {
        await GoToPageAsync("affixes");
        var prefix = Page.Locator("//div[contains(@class, 'ds-field-affixes') and .//input[@id='textfield-prefix']]//span[@class='ds-field-affix']").First;
        await Expect(prefix).ToHaveAttributeAsync("aria-hidden", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_Suffix_IsVisible()
    {
        await GoToPageAsync("affixes");
        var suffix = Page.Locator("//div[contains(@class, 'ds-field-affixes') and .//input[@id='textfield-suffix']]//span[@class='ds-field-affix']").First;
        await Expect(suffix).ToBeVisibleAsync();
        await Expect(suffix).ToContainTextAsync("@example.com");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Textfield_Suffix_HasAriaHidden()
    {
        await GoToPageAsync("affixes");
        var suffix = Page.Locator("//div[contains(@class, 'ds-field-affixes') and .//input[@id='textfield-suffix']]//span[@class='ds-field-affix']").First;
        await Expect(suffix).ToHaveAttributeAsync("aria-hidden", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_WithBothAffixes_DisplaysBothCorrectly()
    {
        await GoToPageAsync("affixes");

        var affixes = Page.Locator("//div[contains(@class, 'ds-field-affixes') and .//input[@id='textfield-both']]//span[@class='ds-field-affix']");
        await Expect(affixes).ToHaveCountAsync(2);

        var prefix = affixes.First;
        var suffix = affixes.Last;

        await Expect(prefix).ToHaveTextAsync("$");
        await Expect(suffix).ToHaveTextAsync(".00");
    }

    #endregion

    #region Error Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Textfield_WithError_HasAriaInvalid()
    {
        await GoToPageAsync("error");

        var input = Page.Locator("#textfield-error");
        await Expect(input).ToHaveAttributeAsync("aria-invalid", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Textfield_WithError_HasAriaDescribedbyForValidation()
    {
        await GoToPageAsync("error");

        var input = Page.Locator("#textfield-error");
        var ariaDescribedby = await input.GetAttributeAsync("aria-describedby");
        Assert.NotNull(ariaDescribedby);
        Assert.Contains("textfield-error:validation", ariaDescribedby);
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_WithError_DisplaysErrorMessage()
    {
        await GoToPageAsync("error");

        var errorMessage = Page.Locator("#textfield-error\\:validation\\:1");
        await Expect(errorMessage).ToBeVisibleAsync();
        await Expect(errorMessage).ToContainTextAsync("Please enter a valid email address");
    }

    #endregion

    #region Counter Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_Counter_DisplaysRemainingCharacters()
    {
        await GoToPageAsync("counter");

        var counter = Page.Locator("#textfield-counter\\:validation\\:2");
        await Expect(counter).ToBeVisibleAsync();
        await Expect(counter).ToContainTextAsync("95 tegn igjen");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_Counter_AtLimit_ShowsZeroRemaining()
    {
        await GoToPageAsync("counter");

        var counter = Page.Locator("#textfield-counter-at\\:validation\\:2");
        await Expect(counter).ToBeVisibleAsync();
        await Expect(counter).ToContainTextAsync("0 tegn igjen");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_Counter_OverLimit_ShowsExcessCharacters()
    {
        await GoToPageAsync("counter");

        var counter = Page.Locator("#textfield-counter-over\\:validation\\:2");
        await Expect(counter).ToBeVisibleAsync();
        await Expect(counter).ToContainTextAsync("tegn for mye");
    }

    #endregion

    #region Multiline Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_Multiline_RendersTextareaElement()
    {
        await GoToPageAsync("multiline");

        var textarea = Page.Locator("#textfield-multiline");
        await Expect(textarea).ToBeVisibleAsync();

        var tagName = await textarea.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
        Assert.Equal("textarea", tagName);
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textfield_Multiline_HasDsInputClass()
    {
        await GoToPageAsync("multiline");
        var textarea = Page.Locator("#textfield-multiline");
        await Expect(textarea).ToHaveClassAsync(DsInputClassRegex());
    }

    #endregion

    #region Input Type Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textfield_EmailType_HasCorrectTypeAttribute()
    {
        await GoToPageAsync("types");
        var input = Page.Locator("#textfield-email");
        await Expect(input).ToHaveAttributeAsync("type", "email");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textfield_PasswordType_HasCorrectTypeAttribute()
    {
        await GoToPageAsync("types");
        var input = Page.Locator("#textfield-password");
        await Expect(input).ToHaveAttributeAsync("type", "password");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textfield_NumberType_HasCorrectTypeAttribute()
    {
        await GoToPageAsync("types");
        var input = Page.Locator("#textfield-number");
        await Expect(input).ToHaveAttributeAsync("type", "number");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textfield_TelType_HasCorrectTypeAttribute()
    {
        await GoToPageAsync("types");
        var input = Page.Locator("#textfield-tel");
        await Expect(input).ToHaveAttributeAsync("type", "tel");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textfield_UrlType_HasCorrectTypeAttribute()
    {
        await GoToPageAsync("types");
        var input = Page.Locator("#textfield-url");
        await Expect(input).ToHaveAttributeAsync("type", "url");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textfield_SearchType_HasCorrectTypeAttribute()
    {
        await GoToPageAsync("types");
        var input = Page.Locator("#textfield-search");
        await Expect(input).ToHaveAttributeAsync("type", "search");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textfield_DateType_HasCorrectTypeAttribute()
    {
        await GoToPageAsync("types");
        var input = Page.Locator("#textfield-date");
        await Expect(input).ToHaveAttributeAsync("type", "date");
    }

    #endregion

    #region ARIA Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Textfield_AriaLabel_IsAppliedToInput()
    {
        await GoToPageAsync("accessibility");
        var input = Page.Locator("#textfield-aria");
        await Expect(input).ToHaveAttributeAsync("aria-label", "Search products");
    }

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Textfield_AriaLabelledby_IsAppliedToInput()
    {
        await GoToPageAsync("accessibility");
        var input = Page.Locator("#textfield-labelledby");
        await Expect(input).ToHaveAttributeAsync("aria-labelledby", "custom-label");
    }

    #endregion

    #region Value Tests

    [Fact]
    [Trait(Traits.Component, "Textfield")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textfield_WithValue_DisplaysValue()
    {
        await GoToPageAsync("basic");
        var input = Page.Locator("#textfield-value");
        await Expect(input).ToHaveValueAsync("john_doe");
    }

    [System.Text.RegularExpressions.GeneratedRegex("ds-input")]
    private static partial System.Text.RegularExpressions.Regex DsInputClassRegex();

    #endregion
}