using Bunit;
using Hviktor.Abstractions.Enums;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Components.Link;
using Microsoft.AspNetCore.Components.Routing;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Tests.Unit.Components.Link;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Link")]
public class LinkTests : HviktorBunitContext
{

    [Fact]
    public void Link_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Link.Link>();

        Assert.NotNull(component.Instance);
        Assert.Null(component.Instance.Size);
        Assert.Null(component.Instance.Color);
        Assert.Null(component.Instance.Href);
        Assert.Null(component.Instance.Match);
        Assert.Null(component.Instance.Target);
    }

    [Fact]
    public void Link_HasDsLinkClass()
    {
        var component = Render<Hviktor.Components.Link.Link>();

        var link = component.Find("a");
        Assert.Contains("ds-link", link.ClassList);
    }

    [Fact]
    public void Link_RendersAsAnchorElement()
    {
        var component = Render<Hviktor.Components.Link.Link>();

        var link = component.Find("a");
        Assert.Equal("A", link.TagName);
    }

    [Fact]
    public void Link_AcceptsCustomId()
    {
        const string customId = "my-custom-link";
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Id, customId));

        Assert.Equal(customId, component.Instance.Id);
        var link = component.Find("a");
        Assert.Equal(customId, link.Id);
    }

    [Fact]
    public void Link_AppliesHref()
    {
        const string href = "/about";
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Href, href));

        var link = component.Find("a");
        Assert.Equal(href, link.GetAttribute("href"));
    }

    [Fact]
    public void Link_DefaultsHrefToNull()
    {
        var component = Render<Hviktor.Components.Link.Link>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("href"));
    }

    [Fact]
    public void Link_RendersChildContent()
    {
        const string content = "Click me";
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .AddChildContent(content));

        var link = component.Find("a");
        Assert.Contains(content, link.InnerHtml);
    }

    [Fact]
    public void Link_AppliesSize()
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Size, Size.Small));

        Assert.Equal(Size.Small, component.Instance.Size);
        var link = component.Find("a");
        Assert.Equal("sm", link.GetAttribute("data-size"));
    }

    [Fact]
    public void Link_HasNoSizeAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Link.Link>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("data-size"));
    }

    [Fact]
    public void Link_AppliesColor()
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Color, Color.Accent));

        Assert.Equal(Color.Accent, component.Instance.Color);
        var link = component.Find("a");
        Assert.Equal("accent", link.GetAttribute("data-color"));
    }

    [Fact]
    public void Link_HasNoColorAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Link.Link>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("data-color"));
    }

    [Fact]
    public void Link_AppliesTargetBlank()
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Target, LinkTarget.NewTab));

        var link = component.Find("a");
        Assert.Equal("_blank", link.GetAttribute("target"));
    }

    [Fact]
    public void Link_AppliesTargetSelf()
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Target, LinkTarget.SameTab));

        var link = component.Find("a");
        Assert.Equal("_self", link.GetAttribute("target"));
    }

    [Fact]
    public void Link_HasNoTargetWhenNull()
    {
        var component = Render<Hviktor.Components.Link.Link>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("target"));
    }

    [Fact]
    public void Link_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .AddUnmatched("data-testid", "link-test")
            .AddUnmatched("title", "Link title"));

        var link = component.Find("a");
        Assert.Equal("link-test", link.GetAttribute("data-testid"));
        Assert.Equal("Link title", link.GetAttribute("title"));
    }

    [Fact]
    public void Link_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .AddUnmatched("class", "my-custom-link"));

        var link = component.Find("a");
        Assert.Contains("my-custom-link", link.ClassList);
        Assert.Contains("ds-link", link.ClassList);
    }

    [Fact]
    public void Link_AppliesAriaLabel()
    {
        const string ariaLabel = "Navigate to about page";
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .AddUnmatched("aria-label", ariaLabel));

        var link = component.Find("a");
        Assert.Equal(ariaLabel, link.GetAttribute("aria-label"));
    }

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    public void Link_AppliesAllSizes(Size size, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Size, size));

        var link = component.Find("a");
        Assert.Equal(expectedDataAttribute, link.GetAttribute("data-size"));
    }

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    public void Link_AppliesAllColors(Color color, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Color, color));

        var link = component.Find("a");
        Assert.Equal(expectedDataAttribute, link.GetAttribute("data-color"));
    }

    [Fact]
    public void Link_CombinesAllParameters()
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Id, "full-link")
            .Add(p => p.Href, "/contact")
            .Add(p => p.Size, Size.Large)
            .Add(p => p.Color, Color.Neutral)
            .Add(p => p.Target, LinkTarget.NewTab)
            .AddChildContent("Contact us"));

        var link = component.Find("a");
        Assert.Equal("full-link", link.Id);
        Assert.Equal("/contact", link.GetAttribute("href"));
        Assert.Equal("lg", link.GetAttribute("data-size"));
        Assert.Equal("neutral", link.GetAttribute("data-color"));
        Assert.Equal("_blank", link.GetAttribute("target"));
        Assert.Contains("Contact us", link.InnerHtml);
    }

    [Fact]
    public void Link_RendersComplexChildContent()
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .AddChildContent("<span>Learn more</span><svg></svg>"));

        var link = component.Find("a");
        Assert.Contains("<span>Learn more</span>", link.InnerHtml);
    }

    [Fact]
    public void Link_AcceptsMatchParameter()
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Href, "/about")
            .Add(p => p.Match, NavLinkMatch.Prefix));

        Assert.Equal(NavLinkMatch.Prefix, component.Instance.Match);
    }

    [Fact]
    public void Link_AcceptsMatchAllParameter()
    {
        var component = Render<Hviktor.Components.Link.Link>(parameters => parameters
            .Add(p => p.Href, "/about")
            .Add(p => p.Match, NavLinkMatch.All));

        Assert.Equal(NavLinkMatch.All, component.Instance.Match);
    }

    [Fact]
    public void Link_InheritsFromLinkBase()
    {
        var component = Render<Hviktor.Components.Link.Link>();
        Assert.IsType<LinkBase>(component.Instance, exactMatch: false);
    }
}