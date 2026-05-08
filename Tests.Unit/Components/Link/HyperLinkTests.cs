using Bunit;
using Hviktor.Abstractions.Enums;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Components.Link;

namespace Tests.Unit.Components.Link;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "HyperLink")]
public class HyperLinkTests : HviktorBunitContext
{

    #region Rendering

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void HyperLink_RendersWithDefaultValues()
    {
        var component = Render<HyperLink>();
        Assert.NotNull(component.Instance);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void HyperLink_RendersAsAnchorElement()
    {
        var component = Render<HyperLink>();

        var link = component.Find("a");
        Assert.Equal("A", link.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void HyperLink_HasDsLinkClass()
    {
        var component = Render<HyperLink>();

        var link = component.Find("a");
        Assert.Contains("ds-link", link.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void HyperLink_HasGeneratedId()
    {
        var component = Render<HyperLink>();

        var link = component.Find("a");
        Assert.NotNull(link.Id);
        Assert.NotEmpty(link.Id);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void HyperLink_RendersChildContent()
    {
        const string content = "Click me";
        var component = Render<HyperLink>(p => p
            .AddChildContent(content));

        var link = component.Find("a");
        Assert.Contains(content, link.InnerHtml);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void HyperLink_RendersComplexChildContent()
    {
        var component = Render<HyperLink>(p => p
            .AddChildContent("<span>Learn more</span>"));

        var link = component.Find("a");
        Assert.Contains("<span>Learn more</span>", link.InnerHtml);
    }

    #endregion

    #region Attributes

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_AppliesHref()
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("href", "/about"));

        var link = component.Find("a");
        Assert.Equal("/about", link.GetAttribute("href"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_HasNoHrefWhenNotProvided()
    {
        var component = Render<HyperLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("href"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_AppliesAdditionalAttributes()
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("data-testid", "hyper-link-test")
            .AddUnmatched("title", "A title"));

        var link = component.Find("a");
        Assert.Equal("hyper-link-test", link.GetAttribute("data-testid"));
        Assert.Equal("A title", link.GetAttribute("title"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_AppliesCustomCssClass()
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("class", "my-class"));

        var link = component.Find("a");
        Assert.Contains("my-class", link.ClassList);
        Assert.Contains("ds-link", link.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_AppliesAriaLabel()
    {
        const string ariaLabel = "Navigate to about page";
        var component = Render<HyperLink>(p => p
            .AddUnmatched("aria-label", ariaLabel));

        var link = component.Find("a");
        Assert.Equal(ariaLabel, link.GetAttribute("aria-label"));
    }

    #endregion

    #region Target

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_AppliesTargetBlank()
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("target", LinkTarget.NewTab));

        var link = component.Find("a");
        Assert.Equal("_blank", link.GetAttribute("target"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_AppliesTargetSelf()
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("target", LinkTarget.Self));

        var link = component.Find("a");
        Assert.Equal("_self", link.GetAttribute("target"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_HasNoTargetWhenNotProvided()
    {
        var component = Render<HyperLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("target"));
    }

    #endregion

    #region Security

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_AddsRelForExternalLinkWithTargetBlank()
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("href", "https://external.example.com")
            .AddUnmatched("target", LinkTarget.NewTab));

        var link = component.Find("a");
        Assert.Equal("external noreferrer noopener", link.GetAttribute("rel"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_DoesNotAddRelForInternalLink()
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("href", "/about")
            .AddUnmatched("target", LinkTarget.NewTab));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("rel"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_DoesNotAddRelForExternalLinkWithoutTarget()
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("href", "https://external.example.com"));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("rel"));
    }

    #endregion

    #region Size

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_AppliesAllSizes(Size size, string expected)
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("size", size));

        var link = component.Find("a");
        Assert.Equal(expected, link.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_HasNoSizeWhenNotProvided()
    {
        var component = Render<HyperLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("data-size"));
    }

    #endregion

    #region Color

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_AppliesAllColors(Color color, string expected)
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("color", color));

        var link = component.Find("a");
        Assert.Equal(expected, link.GetAttribute("data-color"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void HyperLink_HasNoColorWhenNotProvided()
    {
        var component = Render<HyperLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("data-color"));
    }

    #endregion

    #region Combined

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void HyperLink_CombinesAllParameters()
    {
        var component = Render<HyperLink>(p => p
            .AddUnmatched("href", "https://external.example.com")
            .AddUnmatched("target", LinkTarget.NewTab)
            .AddUnmatched("size", Size.Large)
            .AddUnmatched("color", Color.Neutral)
            .AddChildContent("External link"));

        var link = component.Find("a");
        Assert.Equal("https://external.example.com", link.GetAttribute("href"));
        Assert.Equal("_blank", link.GetAttribute("target"));
        Assert.Equal("lg", link.GetAttribute("data-size"));
        Assert.Equal("neutral", link.GetAttribute("data-color"));
        Assert.Equal("external noreferrer noopener", link.GetAttribute("rel"));
        Assert.Contains("External link", link.InnerHtml);
    }

    #endregion
}