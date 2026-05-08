using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Tag;

/// <summary>
/// Semantic behavior tests for the Tag component.
/// Tests proper HTML structure and accessibility features:
/// - Tag renders as span element with ds-tag class
/// - Multiple tags should be in a ul/li list for screen readers
/// - Data attributes are correctly applied for size, color, variant
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TagSemanticTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "tag";

    #region Semantic HTML Structure

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tag_RendersAsSpanElement()
    {
        await GoToPageAsync("variant");

        var tag = GetByTestId("default");

        await Expect(tag).ToBeVisibleAsync();

        // Verify it's a span element with ds-tag class
        var tagName = await tag.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
        Assert.Equal("span", tagName);
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tag_HasDsTagClass()
    {
        await GoToPageAsync("variant");

        var tag = GetByTestId("default");

        await Expect(tag).ToHaveClassAsync("ds-tag");
    }

    #endregion

    #region Variant Data Attribute Tests

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tag_DefaultVariant_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variant");

        var tag = GetByTestId("default");

        await Expect(tag).ToHaveAttributeAsync("data-variant", "default");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tag_OutlineVariant_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variant");

        var tag = GetByTestId("outline");

        await Expect(tag).ToHaveAttributeAsync("data-variant", "outline");
    }

    #endregion

    #region Color Data Attribute Tests

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task Tag_AccentColor_HasCorrectDataAttribute()
    {
        await GoToPageAsync("color");

        var tag = GetByTestId("accent");

        await Expect(tag).ToHaveAttributeAsync("data-color", "accent");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task Tag_NeutralColor_HasCorrectDataAttribute()
    {
        await GoToPageAsync("color");

        var tag = GetByTestId("neutral");

        await Expect(tag).ToHaveAttributeAsync("data-color", "neutral");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task Tag_InfoColor_HasCorrectDataAttribute()
    {
        await GoToPageAsync("color");

        var tag = GetByTestId("info");

        await Expect(tag).ToHaveAttributeAsync("data-color", "info");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task Tag_SuccessColor_HasCorrectDataAttribute()
    {
        await GoToPageAsync("color");

        var tag = GetByTestId("success");

        await Expect(tag).ToHaveAttributeAsync("data-color", "success");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task Tag_WarningColor_HasCorrectDataAttribute()
    {
        await GoToPageAsync("color");
        var tag = GetByTestId("warning");
        await Expect(tag).ToHaveAttributeAsync("data-color", "warning");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task Tag_DangerColor_HasCorrectDataAttribute()
    {
        await GoToPageAsync("color");
        var tag = GetByTestId("danger");
        await Expect(tag).ToHaveAttributeAsync("data-color", "danger");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task Tag_Brand1Color_HasCorrectDataAttribute()
    {
        await GoToPageAsync("color");
        var tag = GetByTestId("brand1");
        await Expect(tag).ToHaveAttributeAsync("data-color", "brand1");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task Tag_Brand2Color_HasCorrectDataAttribute()
    {
        await GoToPageAsync("color");
        var tag = GetByTestId("brand2");
        await Expect(tag).ToHaveAttributeAsync("data-color", "brand2");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task Tag_Brand3Color_HasCorrectDataAttribute()
    {
        await GoToPageAsync("color");
        var tag = GetByTestId("brand3");
        await Expect(tag).ToHaveAttributeAsync("data-color", "brand3");
    }

    #endregion

    #region Size Data Attribute Tests

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tag_SmallSize_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");
        var tag = GetByTestId("sm");
        await Expect(tag).ToHaveAttributeAsync("data-size", "sm");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tag_MediumSize_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");
        var tag = GetByTestId("md");
        await Expect(tag).ToHaveAttributeAsync("data-size", "md");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tag_LargeSize_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");
        var tag = GetByTestId("lg");
        await Expect(tag).ToHaveAttributeAsync("data-size", "lg");
    }

    #endregion

    #region List Structure Tests (Accessibility Best Practice)

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Structure)]
    public async Task TagList_IsUnorderedList()
    {
        await GoToPageAsync("list");
        var tagList = GetByTestId("tag-list");

        // Verify it's a ul element
        var tagName = await tagList.EvaluateAsync<string>("el => el.tagName.toLowerCase()");
        Assert.Equal("ul", tagName);
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Structure)]
    public async Task TagList_ContainsListItems()
    {
        await GoToPageAsync("list");

        var tagList = GetByTestId("tag-list");
        var listItems = tagList.Locator("li");

        await Expect(listItems).ToHaveCountAsync(3);
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Structure)]
    public async Task TagList_EachListItemContainsTag()
    {
        await GoToPageAsync("list");

        var tagList = GetByTestId("tag-list");
        var tags = tagList.Locator("li > .ds-tag");

        await Expect(tags).ToHaveCountAsync(3);
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Structure)]
    public async Task Tag_SingleTagWithoutList_IsValid()
    {
        await GoToPageAsync("list");

        var singleTag = GetByTestId("single-tag");

        await Expect(singleTag).ToBeVisibleAsync();
        await Expect(singleTag).ToHaveClassAsync("ds-tag");
    }

    #endregion

    #region Content Tests

    [Fact]
    [Trait(Traits.Component, "Tag")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Tag_DisplaysChildContent()
    {
        await GoToPageAsync("variant");
        var tag = GetByTestId("default");
        await Expect(tag).ToHaveTextAsync("Default Tag");
    }

    #endregion
}