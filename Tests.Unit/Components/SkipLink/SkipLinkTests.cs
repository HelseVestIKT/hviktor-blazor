using Bunit;

namespace Tests.Unit.Components.SkipLink;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "SkipLink")]
public class SkipLinkTests : HviktorBunitContext
{
    private const string DefaultHref = "#main";


    [Fact]
    public void SkipLink_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Link.SkipLink>(p => p.AddUnmatched("href", DefaultHref));
        Assert.NotNull(component.Instance);
    }

    [Fact]
    public void SkipLink_HasDsSkipLinkClass()
    {
        var component = Render<Hviktor.Components.Link.SkipLink>(p => p.AddUnmatched("href", DefaultHref));

        var link = component.Find("a");
        Assert.Contains("ds-skip-link", link.ClassList);
    }

    [Fact]
    public void SkipLink_RendersAsAnchorElement()
    {
        var component = Render<Hviktor.Components.Link.SkipLink>(p => p.AddUnmatched("href", DefaultHref));

        var link = component.Find("a");
        Assert.Equal("A", link.TagName);
    }

    [Fact]
    public void SkipLink_HasDefaultHref()
    {
        var component = Render<Hviktor.Components.Link.SkipLink>(p => p.AddUnmatched("href", DefaultHref));

        var link = component.Find("a");
        Assert.EndsWith(DefaultHref, link.GetAttribute("href"));
    }

    [Fact]
    public void SkipLink_AcceptsCustomHref()
    {
        const string customHref = "#main-content";
        var component = Render<Hviktor.Components.Link.SkipLink>(parameters => parameters
            .AddUnmatched("href", customHref));

        var link = component.Find("a");
        Assert.EndsWith(customHref, link.GetAttribute("href"));
    }

    [Fact]
    public void SkipLink_RendersDefaultText()
    {
        var component = Render<Hviktor.Components.Link.SkipLink>(p => p.AddUnmatched("href", DefaultHref));

        var link = component.Find("a");
        Assert.Contains("Skip to main content", link.InnerHtml);
    }

    [Fact]
    public void SkipLink_AcceptsCustomText()
    {
        var component = Render<Hviktor.Components.Link.SkipLink>(parameters => parameters
            .AddUnmatched("href", DefaultHref));

        var link = component.Find("a");
        Assert.Contains("Skip to main content", link.InnerHtml);
    }

    [Fact]
    public void SkipLink_RendersChildContentInsteadOfText()
    {
        const string childContent = "Custom skip link content";
        var component = Render<Hviktor.Components.Link.SkipLink>(parameters => parameters
            .AddUnmatched("href", DefaultHref)
            .AddChildContent(childContent));

        var link = component.Find("a");
        Assert.Contains(childContent, link.InnerHtml);
        Assert.DoesNotContain("Skip to main content", link.InnerHtml);
    }

    [Fact]
    public void SkipLink_HasNoColorAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Link.SkipLink>(p => p.AddUnmatched("href", DefaultHref));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("data-color"));
    }

    [Fact]
    public void SkipLink_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Link.SkipLink>(parameters => parameters
            .AddUnmatched("href", DefaultHref)
            .AddUnmatched("data-testid", "skip-link-test")
            .AddUnmatched("aria-label", "Skip navigation"));

        var link = component.Find("a");
        Assert.Equal("skip-link-test", link.GetAttribute("data-testid"));
        Assert.Equal("Skip navigation", link.GetAttribute("aria-label"));
    }

    [Fact]
    public void SkipLink_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Link.SkipLink>(parameters => parameters
            .AddUnmatched("href", DefaultHref)
            .AddUnmatched("class", "my-custom-skip-link"));

        var link = component.Find("a");
        Assert.Contains("my-custom-skip-link", link.ClassList);
        Assert.Contains("ds-skip-link", link.ClassList);
    }

    [Fact]
    public void SkipLink_CombinesAllParameters()
    {
        var component = Render<Hviktor.Components.Link.SkipLink>(parameters => parameters
            .AddUnmatched("href", "#navigation"));

        var link = component.Find("a");
        Assert.EndsWith("#navigation", link.GetAttribute("href"));
        Assert.Contains("Skip to main content", link.InnerHtml);
    }

    [Fact]
    public void SkipLink_ChildContentTakesPrecedenceOverText()
    {
        const string text = "Default text";
        const string childContent = "Custom child content";

        var component = Render<Hviktor.Components.Link.SkipLink>(parameters => parameters
            .AddUnmatched("href", DefaultHref)
            .AddChildContent(childContent));

        var link = component.Find("a");
        Assert.Contains(childContent, link.InnerHtml);
        Assert.DoesNotContain(text, link.InnerHtml);
    }
}