using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Tag;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Tag")]
public class TagTests : HviktorBunitContext
{

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void Tag_RendersSpanElement()
    {
        var component = Render<Hviktor.Components.Tag.Tag>();

        var span = component.Find("span");
        Assert.Equal("SPAN", span.TagName);
    }

    [Fact]
    public void Tag_HasDsTagClass()
    {
        var component = Render<Hviktor.Components.Tag.Tag>();

        var span = component.Find("span");
        Assert.Contains("ds-tag", span.ClassList);
    }

    [Fact]
    public void Tag_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddChildContent("Tag Label"));

        var span = component.Find("span");
        Assert.Contains("Tag Label", span.TextContent);
    }

    [Fact]
    public void Tag_RendersComplexChildContent()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddChildContent("<strong>Bold</strong> text"));

        var span = component.Find("span");
        Assert.Contains("Bold", span.InnerHtml);
        Assert.Contains("<strong>", span.InnerHtml);
    }

    [Fact]
    public void Tag_WithoutChildContent_RendersEmptySpan()
    {
        var component = Render<Hviktor.Components.Tag.Tag>();

        var span = component.Find("span");
        Assert.Empty(span.TextContent.Trim());
    }

    #endregion

    #region Size Tests.Playwright

    [Fact]
    public void Tag_NoSizeAttributeByDefault()
    {
        var component = Render<Hviktor.Components.Tag.Tag>();

        var span = component.Find("span");
        Assert.Null(span.GetAttribute("data-size"));
    }

    [Fact]
    public void Tag_AppliesSmallSize()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("size", Size.Small));

        var span = component.Find("span");
        Assert.Equal("sm", span.GetAttribute("data-size"));
    }

    [Fact]
    public void Tag_AppliesMediumSize()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("size", Size.Medium));

        var span = component.Find("span");
        Assert.Equal("md", span.GetAttribute("data-size"));
    }

    [Fact]
    public void Tag_AppliesLargeSize()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("size", Size.Large));

        var span = component.Find("span");
        Assert.Equal("lg", span.GetAttribute("data-size"));
    }

    #endregion

    #region Color Tests.Playwright

    [Fact]
    public void Tag_NoColorAttributeByDefault()
    {
        var component = Render<Hviktor.Components.Tag.Tag>();

        var span = component.Find("span");
        Assert.Null(span.GetAttribute("data-color"));
    }

    [Fact]
    public void Tag_AppliesAccentColor()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("color", Color.Accent));

        var span = component.Find("span");
        Assert.Equal("accent", span.GetAttribute("data-color"));
    }

    [Fact]
    public void Tag_AppliesNeutralColor()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("color", Color.Neutral));

        var span = component.Find("span");
        Assert.Equal("neutral", span.GetAttribute("data-color"));
    }

    [Fact]
    public void Tag_AppliesInfoColor()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("color", Color.Info));

        var span = component.Find("span");
        Assert.Equal("info", span.GetAttribute("data-color"));
    }

    [Fact]
    public void Tag_AppliesSuccessColor()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("color", Color.Success));

        var span = component.Find("span");
        Assert.Equal("success", span.GetAttribute("data-color"));
    }

    [Fact]
    public void Tag_AppliesWarningColor()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("color", Color.Warning));

        var span = component.Find("span");
        Assert.Equal("warning", span.GetAttribute("data-color"));
    }

    [Fact]
    public void Tag_AppliesDangerColor()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("color", Color.Danger));

        var span = component.Find("span");
        Assert.Equal("danger", span.GetAttribute("data-color"));
    }

    #endregion

    #region Combined Parameters Tests.Playwright

    [Fact]
    public void Tag_WithSizeAndColor_AppliesBoth()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("size", Size.Small)
            .AddUnmatched("color", Color.Success));

        var span = component.Find("span");
        Assert.Equal("sm", span.GetAttribute("data-size"));
        Assert.Equal("success", span.GetAttribute("data-color"));
    }

    [Fact]
    public void Tag_WithAllParameters_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("size", Size.Large)
            .AddUnmatched("color", Color.Warning)
            .AddChildContent("Important Tag"));

        var span = component.Find("span");
        Assert.Contains("ds-tag", span.ClassList);
        Assert.Equal("lg", span.GetAttribute("data-size"));
        Assert.Equal("warning", span.GetAttribute("data-color"));
        Assert.Contains("Important Tag", span.TextContent);
    }

    #endregion

    #region Additional Attributes Tests.Playwright

    [Fact]
    public void Tag_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("data-testid", "tag-test")
            .AddUnmatched("id", "my-tag"));

        var span = component.Find("span");
        Assert.Equal("tag-test", span.GetAttribute("data-testid"));
        Assert.Equal("my-tag", span.Id);
    }

    [Fact]
    public void Tag_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("class", "custom-tag highlight"));

        var span = component.Find("span");
        Assert.Contains("custom-tag", span.ClassList);
        Assert.Contains("highlight", span.ClassList);
        Assert.Contains("ds-tag", span.ClassList);
    }

    [Fact]
    public void Tag_AppliesAriaLabel()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("aria-label", "Status tag")
            .AddChildContent("Active"));

        var span = component.Find("span");
        Assert.Equal("Status tag", span.GetAttribute("aria-label"));
    }

    [Fact]
    public void Tag_AppliesRole()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("role", "status")
            .AddChildContent("Online"));

        var span = component.Find("span");
        Assert.Equal("status", span.GetAttribute("role"));
    }

    [Fact]
    public void Tag_AppliesTitle()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("title", "Tooltip text")
            .AddChildContent("Hover me"));

        var span = component.Find("span");
        Assert.Equal("Tooltip text", span.GetAttribute("title"));
    }

    #endregion

    #region Real-World Usage Tests.Playwright

    [Fact]
    public void Tag_StatusBadge_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("color", Color.Success)
            .AddUnmatched("size", Size.Small)
            .AddUnmatched("role", "status")
            .AddChildContent("Active"));

        var span = component.Find("span");
        Assert.Contains("ds-tag", span.ClassList);
        Assert.Equal("success", span.GetAttribute("data-color"));
        Assert.Equal("sm", span.GetAttribute("data-size"));
        Assert.Equal("status", span.GetAttribute("role"));
        Assert.Contains("Active", span.TextContent);
    }

    [Fact]
    public void Tag_CategoryLabel_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("color", Color.Accent)
            .AddChildContent("Technology"));

        var span = component.Find("span");
        Assert.Equal("accent", span.GetAttribute("data-color"));
        Assert.Contains("Technology", span.TextContent);
    }

    [Fact]
    public void Tag_WarningIndicator_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Tag.Tag>(parameters => parameters
            .AddUnmatched("color", Color.Danger)
            .AddUnmatched("size", Size.Medium)
            .AddUnmatched("aria-live", "polite")
            .AddChildContent("Error"));

        var span = component.Find("span");
        Assert.Equal("danger", span.GetAttribute("data-color"));
        Assert.Equal("md", span.GetAttribute("data-size"));
        Assert.Equal("polite", span.GetAttribute("aria-live"));
        Assert.Contains("Error", span.TextContent);
    }

    #endregion
}