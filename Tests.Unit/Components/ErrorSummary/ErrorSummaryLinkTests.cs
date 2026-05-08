using Bunit;

namespace Tests.Unit.Components.ErrorSummary;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "ErrorSummary.Link")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class ErrorSummaryLinkTests : HviktorBunitContext
{

    [Fact]
    public void Link_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Link>());

        var link = component.Find("a");
        Assert.NotNull(link);
    }

    [Fact]
    public void Link_RendersAsAnchorElement()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Link>());

        var link = component.Find("a");
        Assert.Equal("A", link.TagName);
    }

    [Fact]
    public void Link_AppliesHref()
    {
        const string href = "#field-name";
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Link>(linkParams => linkParams
                .AddUnmatched("href", href)));

        var link = component.Find("a");
        Assert.Equal(href, link.GetAttribute("href"));
    }

    [Fact]
    public void Link_RendersChildContent()
    {
        const string linkText = "Fix this error";
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Link>(linkParams => linkParams
                .AddChildContent(linkText)));

        var link = component.Find("a");
        Assert.Contains(linkText, link.InnerHtml);
    }

    [Fact]
    public void Link_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Link>(linkParams => linkParams
                .AddUnmatched("data-testid", "error-link-test")));

        var link = component.Find("a");
        Assert.Equal("error-link-test", link.GetAttribute("data-testid"));
    }

    [Fact]
    public void Link_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Link>(linkParams => linkParams
                .AddUnmatched("class", "my-error-link")));

        var link = component.Find("a");
        Assert.Contains("my-error-link", link.ClassList);
    }

    [Fact]
    public void Link_CombinesHrefAndContent()
    {
        const string href = "#email-field";
        const string linkText = "Email is required";

        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Link>(linkParams => linkParams
                .AddUnmatched("href", href)
                .AddChildContent(linkText)));

        var link = component.Find("a");
        Assert.Equal(href, link.GetAttribute("href"));
        Assert.Contains(linkText, link.InnerHtml);
    }
}