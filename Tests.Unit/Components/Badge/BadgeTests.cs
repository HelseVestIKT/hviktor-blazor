using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;

namespace Tests.Unit.Components.Badge;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Badge")]
public class BadgeTests : HviktorBunitContext
{

    [Fact]
    public void Badge_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Badge.Badge>();
        Assert.NotNull(component.Instance);

        var badge = component.Find("span.ds-badge");
        Assert.Equal("base", badge.GetAttribute("data-variant"));
        Assert.Null(badge.GetAttribute("data-count"));
        Assert.Null(badge.GetAttribute("data-max-count"));

        Assert.Null(component.Instance.Count);
        Assert.Null(component.Instance.MaxCount);
    }

    [Fact]
    public void Badge_HasDsBadgeClass()
    {
        var component = Render<Hviktor.Components.Badge.Badge>();

        var badge = component.Find("span");
        Assert.Contains("ds-badge", badge.ClassList);
    }

    [Fact]
    public void Badge_RendersAsDivElement()
    {
        var component = Render<Hviktor.Components.Badge.Badge>();

        var badge = component.Find("span");
        Assert.Equal("SPAN", badge.TagName);
    }

    [Fact]
    public void Badge_HasGeneratedId()
    {
        var component = Render<Hviktor.Components.Badge.Badge>();

        Assert.NotNull(component.Instance.Id);
        Assert.NotEmpty(component.Instance.Id);
    }

    [Fact]
    public void Badge_AcceptsCustomId()
    {
        const string customId = "my-custom-badge";
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .Add(p => p.Id, customId));

        Assert.Equal(customId, component.Instance.Id);
        var badge = component.Find("span");
        Assert.Equal(customId, badge.Id);
    }

    [Fact]
    public void Badge_DefaultVariantIsBase()
    {
        var component = Render<Hviktor.Components.Badge.Badge>();

        var badge = component.Find("span");
        Assert.Equal("base", badge.GetAttribute("data-variant"));
    }

    [Theory]
    [InlineData(Variant.Base, "base")]
    [InlineData(Variant.Tinted, "tinted")]
    public void Badge_AppliesAllowedVariants(Variant variant, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .AddUnmatched("variant", variant));

        var badge = component.Find("span");
        Assert.Equal(expectedDataAttribute, badge.GetAttribute("data-variant"));
    }

    [Fact]
    public void Badge_DisplaysCount()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .Add(p => p.Count, 5));

        var badge = component.Find("span");
        Assert.Equal("5", badge.GetAttribute("data-count"));
    }

    [Fact]
    public void Badge_DisplaysMaxCountWithPlus()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .Add(p => p.Count, 150)
            .Add(p => p.MaxCount, 99));

        var badge = component.Find("span");
        Assert.Equal("99+", badge.GetAttribute("data-count"));
    }

    [Fact]
    public void Badge_DisplaysCountWhenBelowMax()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .Add(p => p.Count, 50)
            .Add(p => p.MaxCount, 99));

        var badge = component.Find("span");
        Assert.Equal("50", badge.GetAttribute("data-count"));
    }

    [Fact]
    public void Badge_DisplaysCountWhenEqualToMax()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .Add(p => p.Count, 99)
            .Add(p => p.MaxCount, 99));

        var badge = component.Find("span");
        Assert.Equal("99", badge.GetAttribute("data-count"));
    }

    [Fact]
    public void Badge_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var badge = component.Find("span");
        Assert.Contains("my-custom-class", badge.ClassList);
        Assert.Contains("ds-badge", badge.ClassList);
    }

    [Fact]
    public void Badge_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .AddUnmatched("aria-label", "Notifications")
            .AddUnmatched("title", "5 new notifications"));

        var badge = component.Find("span");
        Assert.Equal("Notifications", badge.GetAttribute("aria-label"));
        Assert.Equal("5 new notifications", badge.GetAttribute("title"));
    }

    [Fact]
    public void Badge_GeneratesUniqueIds()
    {
        var component1 = Render<Hviktor.Components.Badge.Badge>();
        var component2 = Render<Hviktor.Components.Badge.Badge>();

        Assert.NotEqual(component1.Instance.Id, component2.Instance.Id);
    }

    #region Color Tests

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    [InlineData(Color.Info, "info")]
    [InlineData(Color.Success, "success")]
    [InlineData(Color.Warning, "warning")]
    [InlineData(Color.Danger, "danger")]
    public void Badge_AppliesAllowedColors(Color color, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .AddUnmatched("color", color));

        var badge = component.Find("span");
        Assert.Equal(expectedDataAttribute, badge.GetAttribute("data-color"));
    }

    [Fact]
    public void Badge_DefaultColorIsNotRendered()
    {
        var component = Render<Hviktor.Components.Badge.Badge>();

        var badge = component.Find("span");
        Assert.Null(badge.GetAttribute("data-color"));
    }

    [Fact]
    public void Badge_AppliesColorViaDataColorAttribute()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .AddUnmatched("data-color", Color.Info));

        var badge = component.Find("span");
        Assert.Equal("info", badge.GetAttribute("data-color"));
    }

    #endregion

    #region Data Attribute Variants

    [Fact]
    public void Badge_AppliesVariantViaDataVariantAttribute()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .AddUnmatched("data-variant", Variant.Tinted));

        var badge = component.Find("span");
        Assert.Equal("tinted", badge.GetAttribute("data-variant"));
    }

    #endregion

    #region Count Edge Cases

    [Fact]
    public void Badge_NullCountDoesNotRenderDataCount()
    {
        var component = Render<Hviktor.Components.Badge.Badge>();

        var badge = component.Find("span");
        Assert.Null(badge.GetAttribute("data-count"));
    }

    [Fact]
    public void Badge_ZeroCountRendersZero()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .Add(p => p.Count, 0));

        var badge = component.Find("span");
        Assert.Equal("0", badge.GetAttribute("data-count"));
    }

    [Fact]
    public void Badge_MaxCountZeroDoesNotTruncate()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .Add(p => p.Count, 50)
            .Add(p => p.MaxCount, 0));

        var badge = component.Find("span");
        Assert.Equal("50", badge.GetAttribute("data-count"));
    }

    [Fact]
    public void Badge_NegativeMaxCountDoesNotTruncate()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .Add(p => p.Count, 50)
            .Add(p => p.MaxCount, -1));

        var badge = component.Find("span");
        Assert.Equal("50", badge.GetAttribute("data-count"));
    }

    [Fact]
    public void Badge_MaxCountWithoutCountDoesNotRender()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .Add(p => p.MaxCount, 99));

        var badge = component.Find("span");
        Assert.Null(badge.GetAttribute("data-count"));
    }

    #endregion

    [Fact]
    public void Badge_AllPropertiesCanBeCombined()
    {
        var component = Render<Hviktor.Components.Badge.Badge>(parameters => parameters
            .AddUnmatched("variant", Variant.Tinted)
            .Add(p => p.Count, 42)
            .Add(p => p.MaxCount, 99));

        var badge = component.Find("span");
        EnumValue<Variant> variant = badge.GetAttribute("data-variant");
        Assert.Equal(Variant.Tinted, variant);

        Assert.Equal("42", badge.GetAttribute("data-count"));
    }

    #region Position Tests

    [Fact]
    public void Position_RendersWithDefaultValues()
    {
        var component = Render<global::Badge.Position>();
        Assert.NotNull(component.Instance);

        var badge = component.Find("span");
        EnumValue<Placement> placement = badge.GetAttribute("data-placement");
        Assert.Equal("top-right", placement);

        EnumValue<Variant> overlap = badge.GetAttribute("data-overlap");
        Assert.Equal(Variant.Rectangle, overlap);
    }

    [Fact]
    public void Position_HasDsBadgePositionClass()
    {
        var component = Render<global::Badge.Position>();

        var position = component.Find("span");
        Assert.Contains("ds-badge--position", position.ClassList);
    }

    [Fact]
    public void Position_RendersAsSpanElement()
    {
        var component = Render<global::Badge.Position>();

        var position = component.Find("span");
        Assert.Equal("SPAN", position.TagName);
    }

    [Fact]
    public void Position_DefaultPlacementIsTopEnd()
    {
        var component = Render<global::Badge.Position>();

        var position = component.Find("span");
        Assert.Equal("top-right", position.GetAttribute("data-placement"));
    }

    [Fact]
    public void Position_DefaultOverlapIsRectangle()
    {
        var component = Render<global::Badge.Position>();

        var position = component.Find("span");
        Assert.Equal("rectangle", position.GetAttribute("data-overlap"));
    }

    [Theory]
    [InlineData(Placement.TopEnd, "top-right")]
    [InlineData(Placement.TopStart, "top-left")]
    [InlineData(Placement.BottomEnd, "bottom-right")]
    [InlineData(Placement.BottomStart, "bottom-left")]
    public void Position_AppliesAllowedPlacements(Placement placement, string expectedDataAttribute)
    {
        var component = Render<global::Badge.Position>(parameters => parameters
            .AddUnmatched("placement", placement));

        var position = component.Find("span");
        Assert.Equal(expectedDataAttribute, position.GetAttribute("data-placement"));
    }

    [Theory]
    [InlineData(Variant.Rectangle, "rectangle")]
    [InlineData(Variant.Circle, "circle")]
    public void Position_AppliesAllowedOverlapVariants(Variant overlap, string expectedDataAttribute)
    {
        var component = Render<global::Badge.Position>(parameters => parameters
            .AddUnmatched("overlap", overlap));

        var position = component.Find("span");
        Assert.Equal(expectedDataAttribute, position.GetAttribute("data-overlap"));
    }

    [Fact]
    public void Position_RendersChildContent()
    {
        var component = Render<global::Badge.Position>(parameters => parameters
            .AddChildContent("<div class=\"test-child\">Child Content</div>"));

        var position = component.Find("span");
        Assert.Contains("Child Content", position.InnerHtml);
        Assert.NotNull(component.Find(".test-child"));
    }

    [Fact]
    public void Position_AppliesCustomCssClass()
    {
        var component = Render<global::Badge.Position>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var position = component.Find("span");
        Assert.Contains("my-custom-class", position.ClassList);
        Assert.Contains("ds-badge--position", position.ClassList);
    }

    [Fact]
    public void Position_AppliesAdditionalAttributes()
    {
        var component = Render<global::Badge.Position>(parameters => parameters
            .AddUnmatched("aria-label", "Badge position")
            .AddUnmatched("title", "Notification badge"));

        var position = component.Find("span");
        Assert.Equal("Badge position", position.GetAttribute("aria-label"));
        Assert.Equal("Notification badge", position.GetAttribute("title"));
    }

    [Fact]
    public void Position_AppliesPlacementViaDataPlacementAttribute()
    {
        var component = Render<global::Badge.Position>(parameters => parameters
            .AddUnmatched("data-placement", Placement.BottomEnd));

        var position = component.Find("span");
        Assert.Equal("bottom-right", position.GetAttribute("data-placement"));
    }

    [Fact]
    public void Position_AppliesOverlapViaDataOverlapAttribute()
    {
        var component = Render<global::Badge.Position>(parameters => parameters
            .AddUnmatched("data-overlap", Variant.Circle));

        var position = component.Find("span");
        Assert.Equal("circle", position.GetAttribute("data-overlap"));
    }

    [Fact]
    public void Position_CanContainBadgeComponent()
    {
        var component = Render<global::Badge.Position>(parameters => parameters
            .AddUnmatched("Placement", Placement.TopEnd)
            .AddUnmatched("Overlap", Variant.Rectangle)
            .AddChildContent<Hviktor.Components.Badge.Badge>(badgeParams => badgeParams
                .Add(b => b.Count, 5)));

        var position = component.Find("span.ds-badge--position");
        var badge = component.Find("span.ds-badge");

        Assert.NotNull(position);
        Assert.NotNull(badge);
        Assert.Equal("5", badge.GetAttribute("data-count"));
    }

    #endregion
}