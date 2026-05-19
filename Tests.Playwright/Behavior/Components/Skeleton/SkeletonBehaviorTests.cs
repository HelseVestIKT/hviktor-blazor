using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Skeleton;

/// <summary>
/// Behavior and semantic tests for the Skeleton component.
/// Tests focus on:
/// - Skeleton renders with correct CSS class
/// - Skeleton has aria-hidden="true" for accessibility
/// - Skeleton variant attribute is correctly set
/// - Skeleton width and height are applied as inline styles
/// - Text variant uses data-text attribute for character width
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public partial class SkeletonBehaviorTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "skeleton";

    #region Semantic Structure

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_RendersAsSpanElement()
    {
        await GoToPageAsync("variant");

        var section = GetByTestId("rectangle");
        var skeleton = section.Locator("span.ds-skeleton");

        await Expect(skeleton).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_HasCorrectClass()
    {
        await GoToPageAsync("variant");

        var skeleton = GetByTestId("rectangle-skeleton");

        await Expect(skeleton).ToHaveClassAsync(DsSkeletonRegex());
    }

    #endregion

    #region Accessibility Attributes

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Skeleton_IsHiddenFromAccessibilityTree()
    {
        await GoToPageAsync("accessibility");

        var skeleton = GetByTestId("hidden-skeleton");
        await Expect(skeleton).ToHaveAttributeAsync("aria-hidden", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Skeleton_HasRolePresentationOrNone()
    {
        await GoToPageAsync("accessibility");

        var skeleton = GetByTestId("hidden-skeleton");

        var role = await skeleton.GetAttributeAsync("role");
        var ariaHidden = await skeleton.GetAttributeAsync("aria-hidden");

        Assert.True(role == "none" || role == "presentation" || ariaHidden == "true",
            "Skeleton should be hidden from accessibility tree");
    }

    #endregion

    #region Variant Attribute

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_RectangleVariant_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variant");
        var skeleton = GetByTestId("rectangle-skeleton");
        await Expect(skeleton).ToHaveAttributeAsync("data-variant", "rectangle");
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_CircleVariant_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variant");
        var skeleton = GetByTestId("circle-skeleton");
        await Expect(skeleton).ToHaveAttributeAsync("data-variant", "circle");
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_CircleVariantText_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variant");
        var skeleton = GetByTestId("circle-skeleton-2");
        await Expect(skeleton).ToHaveAttributeAsync("data-variant", "circle");
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_TextVariant_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variant");
        var skeleton = GetByTestId("text-skeleton");
        await Expect(skeleton).ToHaveAttributeAsync("data-variant", "text");
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_TextVariant_HasDataTextAttribute()
    {
        await GoToPageAsync("variant");

        var skeleton = GetByTestId("text-skeleton");

        var dataText = await skeleton.GetAttributeAsync("data-text");
        Assert.NotNull(dataText);
        Assert.True(dataText.Length > 0, "data-text should contain characters for width");
    }

    #endregion

    #region Width and Height Styles

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_Rectangle_HasCorrectWidth()
    {
        await GoToPageAsync("size");

        var skeleton = GetByTestId("medium-skeleton");

        var boundingBox = await skeleton.BoundingBoxAsync();
        Assert.NotNull(boundingBox);
        Assert.Equal(150, boundingBox.Width, precision: 1);
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_Rectangle_HasCorrectHeight()
    {
        await GoToPageAsync("size");

        var skeleton = GetByTestId("medium-skeleton");

        var boundingBox = await skeleton.BoundingBoxAsync();
        Assert.NotNull(boundingBox);
        Assert.Equal(50, boundingBox.Height, precision: 1);
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_Small_HasCorrectDimensions()
    {
        await GoToPageAsync("size");

        var skeleton = GetByTestId("small-skeleton");

        var boundingBox = await skeleton.BoundingBoxAsync();
        Assert.NotNull(boundingBox);
        Assert.Equal(50, boundingBox.Width, precision: 1);
        Assert.Equal(20, boundingBox.Height, precision: 1);
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_Large_HasCorrectDimensions()
    {
        await GoToPageAsync("size");

        var skeleton = GetByTestId("large-skeleton");

        var boundingBox = await skeleton.BoundingBoxAsync();
        Assert.NotNull(boundingBox);
        Assert.Equal(300, boundingBox.Width, precision: 1);
        Assert.Equal(100, boundingBox.Height, precision: 1);
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_CustomDimensions_HasCorrectDimensions()
    {
        await GoToPageAsync("size");

        var skeleton = GetByTestId("custom-skeleton");

        var boundingBox = await skeleton.BoundingBoxAsync();
        Assert.NotNull(boundingBox);
        Assert.Equal(250, boundingBox.Width, precision: 1);
        Assert.Equal(75, boundingBox.Height, precision: 1);
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_Circle_HasEqualWidthAndHeight()
    {
        await GoToPageAsync("variant");

        var skeleton = GetByTestId("circle-skeleton");

        var boundingBox = await skeleton.BoundingBoxAsync();
        Assert.NotNull(boundingBox);
        Assert.Equal(80, boundingBox.Width, precision: 1);
        Assert.Equal(80, boundingBox.Height, precision: 1);
        Assert.Equal(boundingBox.Width, boundingBox.Height, precision: 1);
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_Rectangle_HasInlineWidthStyle()
    {
        await GoToPageAsync("variant");

        var skeleton = GetByTestId("rectangle-skeleton");

        var style = await skeleton.GetAttributeAsync("style");
        Assert.NotNull(style);
        Assert.Contains("width:200px", style);
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_Rectangle_HasInlineHeightStyle()
    {
        await GoToPageAsync("variant");

        var skeleton = GetByTestId("rectangle-skeleton");

        var style = await skeleton.GetAttributeAsync("style");
        Assert.NotNull(style);
        Assert.Contains("height:100px", style);
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_TextVariant_DoesNotHaveInlineWidthHeight()
    {
        await GoToPageAsync("variant");

        var skeleton = GetByTestId("text-skeleton");

        var style = await skeleton.GetAttributeAsync("style");
        if (style != null)
        {
            Assert.DoesNotContain("width:", style);
        }
    }

    #endregion

    #region Loading Context

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Skeleton_InLoadingContext_ParentHasRoleStatus()
    {
        await GoToPageAsync("accessibility");

        var section = GetByTestId("multiple-skeletons");
        var statusContainer = section.Locator("[role='status']");

        await Expect(statusContainer).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Skeleton_InLoadingContext_ParentHasAriaLabel()
    {
        await GoToPageAsync("accessibility");

        var section = GetByTestId("multiple-skeletons");
        var statusContainer = section.Locator("[role='status']");

        await Expect(statusContainer).ToHaveAttributeAsync("aria-label", "Loading content");
    }

    #endregion

    #region Multiple Skeletons

    [Fact]
    [Trait(Traits.Component, "Skeleton")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Skeleton_MultipleInSection_AllRender()
    {
        await GoToPageAsync("accessibility");

        var section = GetByTestId("card-skeleton");
        var skeletons = section.Locator(".ds-skeleton");

        await Expect(skeletons).ToHaveCountAsync(3);
    }

    #endregion

    [System.Text.RegularExpressions.GeneratedRegex("ds-skeleton")]
    private static partial System.Text.RegularExpressions.Regex DsSkeletonRegex();
}