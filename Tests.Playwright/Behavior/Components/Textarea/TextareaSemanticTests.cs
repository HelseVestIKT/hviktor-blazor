using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Textarea;

/// <summary>
/// Semantic behavior tests for the Textarea component.
/// Tests proper HTML structure and accessibility features:
/// - Textarea renders as textarea element with ds-input class
/// - Data attributes are correctly applied for size
/// - Disabled, readonly, and required states work correctly
/// - Rows and cols attributes are applied correctly
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TextareaSemanticTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "textarea";

    #region Semantic HTML Structure

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textarea_RendersAsTextareaElement()
    {
        await GoToPageAsync("basic");

        var textarea = GetByTestId("basic");

        await Expect(textarea).ToBeVisibleAsync();

        // Verify it's a textarea element
        var tagName = await textarea.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
        Assert.Equal("textarea", tagName);
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textarea_HasDsInputClass()
    {
        await GoToPageAsync("basic");

        var textarea = GetByTestId("basic");

        await Expect(textarea).ToHaveClassAsync("ds-input");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textarea_HasIdAttribute()
    {
        await GoToPageAsync("basic");

        var textarea = GetByTestId("basic");

        await Expect(textarea).ToHaveAttributeAsync("id", "textarea-basic");
    }

    #endregion

    #region Size Data Attribute Tests

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textarea_SmallSize_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");

        var textarea = GetByTestId("sm");

        await Expect(textarea).ToHaveAttributeAsync("data-size", "sm");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textarea_MediumSize_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");

        var textarea = GetByTestId("md");

        await Expect(textarea).ToHaveAttributeAsync("data-size", "md");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textarea_LargeSize_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");
        var textarea = GetByTestId("lg");
        await Expect(textarea).ToHaveAttributeAsync("data-size", "lg");
    }

    #endregion

    #region State Tests

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textarea_Disabled_HasDisabledAttribute()
    {
        await GoToPageAsync("state");

        var textarea = GetByTestId("disabled");

        await Expect(textarea).ToBeDisabledAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textarea_ReadOnly_HasReadonlyAttribute()
    {
        await GoToPageAsync("state");

        var textarea = GetByTestId("readonly");

        await Expect(textarea).ToHaveAttributeAsync("readonly", "");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textarea_Required_HasRequiredAttribute()
    {
        await GoToPageAsync("state");

        var textarea = GetByTestId("required");

        await Expect(textarea).ToHaveAttributeAsync("required", "");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textarea_Required_HasAriaRequired()
    {
        await GoToPageAsync("state");

        var textarea = GetByTestId("required");

        await Expect(textarea).ToHaveAttributeAsync("aria-required", "true");
    }

    #endregion

    #region Dimensions Tests

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textarea_WithRows_HasRowsAttribute()
    {
        await GoToPageAsync("dimensions");

        var textarea = GetByTestId("with-rows");

        await Expect(textarea).ToHaveAttributeAsync("rows", "5");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textarea_WithCols_HasColsAttribute()
    {
        await GoToPageAsync("dimensions");

        var textarea = GetByTestId("with-cols");

        await Expect(textarea).ToHaveAttributeAsync("cols", "40");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textarea_WithBothDimensions_HasRowsAndColsAttributes()
    {
        await GoToPageAsync("dimensions");

        var textarea = GetByTestId("with-both");

        await Expect(textarea).ToHaveAttributeAsync("rows", "4");
        await Expect(textarea).ToHaveAttributeAsync("cols", "50");
    }

    #endregion

    #region Accessibility Attribute Tests

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Textarea_WithAriaLabel_HasAriaLabelAttribute()
    {
        await GoToPageAsync("accessibility");

        var textarea = GetByTestId("aria-labeled");

        await Expect(textarea).ToHaveAttributeAsync("aria-label", "Additional notes");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Textarea_WithAriaDescribedby_HasAriaDescribedbyAttribute()
    {
        await GoToPageAsync("accessibility");

        var textarea = GetByTestId("described");

        await Expect(textarea).ToHaveAttributeAsync("aria-describedby", "textarea-description");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textarea_WithMaxlength_HasMaxlengthAttribute()
    {
        await GoToPageAsync("accessibility");

        var textarea = GetByTestId("maxlength");

        await Expect(textarea).ToHaveAttributeAsync("maxlength", "200");
    }

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Forms)]
    public async Task Textarea_WithPlaceholder_HasPlaceholderAttribute()
    {
        await GoToPageAsync("basic");

        var textarea = GetByTestId("empty");

        await Expect(textarea).ToHaveAttributeAsync("placeholder", "Enter your comments...");
    }

    #endregion

    #region Content Tests

    [Fact]
    [Trait(Traits.Component, "Textarea")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Textarea_DisplaysChildContent()
    {
        await GoToPageAsync("basic");

        var textarea = GetByTestId("basic");

        await Expect(textarea).ToHaveValueAsync("Initial content");
    }

    #endregion
}