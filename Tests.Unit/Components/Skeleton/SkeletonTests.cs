using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Skeleton;

/// <summary>
/// Unit tests for the Skeleton component.
/// Tests cover rendering, variants, width/height, child content, and accessibility attributes.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Skeleton")]
public class SkeletonTests : HviktorBunitContext
{

    #region Basic Rendering Tests

    [Fact]
    public void Skeleton_RendersSpanElement()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>();

        var span = component.Find("span");
        Assert.Equal("SPAN", span.TagName);
    }

    [Fact]
    public void Skeleton_HasDsSkeletonClass()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>();

        var span = component.Find("span");
        Assert.Contains("ds-skeleton", span.ClassList);
    }

    [Fact]
    public void Skeleton_HasAriaHiddenAttribute()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>();

        var span = component.Find("span");
        Assert.Equal("true", span.GetAttribute("aria-hidden"));
    }

    [Fact]
    public void Skeleton_DefaultVariantIsRectangle()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>();

        var span = component.Find("span");
        Assert.Equal("rectangle", span.GetAttribute("data-variant"));
    }

    #endregion

    #region Variant Tests

    [Fact]
    public void Skeleton_RectangleVariant_HasCorrectDataAttribute()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Rectangle));

        var span = component.Find("span");
        Assert.Equal("rectangle", span.GetAttribute("data-variant"));
    }

    [Fact]
    public void Skeleton_CircleVariant_HasCorrectDataAttribute()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Circle));

        var span = component.Find("span");
        Assert.Equal("circle", span.GetAttribute("data-variant"));
    }

    [Fact]
    public void Skeleton_TextVariant_HasCorrectDataAttribute()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Text));

        var span = component.Find("span");
        Assert.Equal("text", span.GetAttribute("data-variant"));
    }

    #endregion

    #region Width and Height Tests

    [Fact]
    public void Skeleton_Rectangle_AppliesWidthStyle()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Rectangle)
            .AddUnmatched("width", 200));

        var span = component.Find("span");
        Assert.Contains("width:200px", span.GetAttribute("style"));
    }

    [Fact]
    public void Skeleton_Rectangle_AppliesHeightStyle()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Rectangle)
            .AddUnmatched("height", 150));

        var span = component.Find("span");
        Assert.Contains("height:150px", span.GetAttribute("style"));
    }

    [Fact]
    public void Skeleton_Circle_AppliesWidthAndHeightStyles()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Circle)
            .AddUnmatched("width", 40)
            .AddUnmatched("height", 40));

        var span = component.Find("span");
        var style = span.GetAttribute("style");
        Assert.Contains("width:40px", style);
        Assert.Contains("height:40px", style);
    }

    [Fact]
    public void Skeleton_TextVariant_DoesNotApplyWidthHeightStyles()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Text)
            .AddUnmatched("width", 100)
            .AddUnmatched("height", 50));

        var span = component.Find("span");
        var style = span.GetAttribute("style");

        // Text variant should not have width/height styles
        Assert.True(style is null || !style.Contains("width:"));
        Assert.True(style is null || !style.Contains("height:"));
    }

    #endregion

    #region Text Variant Data-Text Attribute Tests

    [Fact]
    public void Skeleton_TextVariant_WithWidth_HasDataTextAttribute()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Text)
            .AddUnmatched("width", 10));

        var span = component.Find("span");
        var dataText = span.GetAttribute("data-text");

        Assert.NotNull(dataText);
        Assert.Equal(10, dataText.Length);
        Assert.Equal("----------", dataText);
    }

    [Fact]
    public void Skeleton_TextVariant_WithWidth_RenderContentIsEmpty()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Text)
            .AddUnmatched("width", 5));

        var span = component.Find("span");
        Assert.Equal("", span.TextContent);
    }

    [Fact]
    public void Skeleton_TextVariant_WithZeroWidth_RendersEmptyContent()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Text)
            .AddUnmatched("width", 0));

        var span = component.Find("span");
        Assert.Empty(span.TextContent);
    }

    [Fact]
    public void Skeleton_RectangleVariant_DoesNotHaveDataTextAttribute()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Rectangle)
            .AddUnmatched("width", 100));

        var span = component.Find("span");
        Assert.Null(span.GetAttribute("data-text"));
    }

    [Fact]
    public void Skeleton_CircleVariant_DoesNotHaveDataTextAttribute()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Circle)
            .AddUnmatched("width", 50));

        var span = component.Find("span");
        Assert.Null(span.GetAttribute("data-text"));
    }

    #endregion

    #region Child Content Tests

    [Fact]
    public void Skeleton_TextVariant_WithChildContent_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Text)
            .AddChildContent("Loading text"));

        var span = component.Find("span");
        Assert.Contains("Loading text", span.TextContent);
    }

    [Fact]
    public void Skeleton_TextVariant_WithChildContent_HasSingleDashDataText()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Text)
            .AddChildContent("Some placeholder text"));

        var span = component.Find("span");
        Assert.Equal("-", span.GetAttribute("data-text"));
    }

    [Fact]
    public void Skeleton_TextVariant_WithChildContent_IgnoresWidthAttribute()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Text)
            .AddUnmatched("width", 100)
            .AddChildContent("Child content"));

        var span = component.Find("span");
        // When there's child content, data-text should be "-" not 100 dashes
        Assert.Equal("-", span.GetAttribute("data-text"));
        Assert.Contains("Child content", span.TextContent);
    }

    [Fact]
    public void Skeleton_RectangleVariant_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Rectangle)
            .AddChildContent("Content inside rectangle"));

        var span = component.Find("span");
        Assert.Contains("Content inside rectangle", span.TextContent);
    }

    #endregion

    #region Additional Attributes Tests

    [Fact]
    public void Skeleton_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("data-testid", "my-skeleton"));

        var span = component.Find("span");
        Assert.Equal("my-skeleton", span.GetAttribute("data-testid"));
    }

    [Fact]
    public void Skeleton_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("class", "custom-class"));

        var span = component.Find("span");
        Assert.Contains("ds-skeleton", span.ClassList);
        Assert.Contains("custom-class", span.ClassList);
    }

    [Fact]
    public void Skeleton_AppliesMultipleAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("data-testid", "skeleton-1")
            .AddUnmatched("title", "Loading..."));

        var span = component.Find("span");
        Assert.Equal("skeleton-1", span.GetAttribute("data-testid"));
        Assert.Equal("Loading...", span.GetAttribute("title"));
    }

    #endregion

    #region Default Values Tests

    [Fact]
    public void Skeleton_DefaultChildContent_IsNull()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>();

        Assert.Null(component.Instance.ChildContent);
    }

    [Fact]
    public void Skeleton_DefaultAdditionalAttributes_IsNull()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>();

        Assert.Null(component.Instance.AdditionalAttributes);
    }

    [Fact]
    public void Skeleton_Default_DoesNotHaveInlineStyles()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>();
        var span = component.Find("span");
        Assert.Null(span.GetAttribute("style"));
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void Skeleton_TextVariant_CompleteConfiguration_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Text)
            .AddChildContent("Username placeholder")
            .AddUnmatched("data-testid", "user-skeleton"));

        var span = component.Find("span");

        Assert.Contains("ds-skeleton", span.ClassList);
        Assert.Equal("text", span.GetAttribute("data-variant"));
        Assert.Equal("-", span.GetAttribute("data-text"));
        Assert.Equal("true", span.GetAttribute("aria-hidden"));
        Assert.Equal("user-skeleton", span.GetAttribute("data-testid"));
        Assert.Contains("Username placeholder", span.TextContent);
    }

    [Fact]
    public void Skeleton_RectangleVariant_CompleteConfiguration_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Rectangle)
            .AddUnmatched("width", 200)
            .AddUnmatched("height", 150)
            .AddUnmatched("data-testid", "image-skeleton"));

        var span = component.Find("span");
        var style = span.GetAttribute("style");

        Assert.Contains("ds-skeleton", span.ClassList);
        Assert.Equal("rectangle", span.GetAttribute("data-variant"));
        Assert.Contains("width:200px", style);
        Assert.Contains("height:150px", style);
        Assert.Equal("true", span.GetAttribute("aria-hidden"));
        Assert.Equal("image-skeleton", span.GetAttribute("data-testid"));
    }

    [Fact]
    public void Skeleton_CircleVariant_CompleteConfiguration_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Skeleton.Skeleton>(parameters => parameters
            .AddUnmatched("variant", Variant.Circle)
            .AddUnmatched("width", 50)
            .AddUnmatched("height", 50)
            .AddUnmatched("data-testid", "avatar-skeleton"));

        var span = component.Find("span");
        var style = span.GetAttribute("style");

        Assert.Contains("ds-skeleton", span.ClassList);
        Assert.Equal("circle", span.GetAttribute("data-variant"));
        Assert.Contains("width:50px", style);
        Assert.Contains("height:50px", style);
        Assert.Equal("true", span.GetAttribute("aria-hidden"));
        Assert.Equal("avatar-skeleton", span.GetAttribute("data-testid"));
    }

    #endregion
}