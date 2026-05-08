using Bunit;
using PaginationComponent = Hviktor.Components.Pagination.Pagination;

namespace Tests.Unit.Components.Pagination;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Pagination")]
public class PaginationTests : HviktorBunitContext
{

    [Fact]
    public void Pagination_RendersAsNavElement()
    {
        var component = Render<PaginationComponent>();

        var nav = component.Find("nav");
        Assert.Equal("NAV", nav.TagName);
    }

    [Fact]
    public void Pagination_HasDsPaginationClass()
    {
        var component = Render<PaginationComponent>();

        var nav = component.Find("nav");
        Assert.Contains("ds-pagination", nav.ClassList);
    }

    [Fact]
    public void Pagination_HasAriaLabel()
    {
        var component = Render<PaginationComponent>();

        var nav = component.Find("nav");
        Assert.NotNull(nav.GetAttribute("aria-label"));
    }

    [Fact]
    public void Pagination_RendersChildContent()
    {
        const string customContent = "<span>Custom pagination content</span>";
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent(customContent));

        var nav = component.Find("nav");
        Assert.Contains("Custom pagination content", nav.InnerHtml);
    }

    [Fact]
    public void Pagination_AppliesAdditionalAttributes()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddUnmatched("data-testid", "pagination-test")
            .AddUnmatched("id", "my-pagination"));

        var nav = component.Find("nav");
        Assert.Equal("pagination-test", nav.GetAttribute("data-testid"));
        Assert.Equal("my-pagination", nav.Id);
    }

    [Fact]
    public void Pagination_AppliesCustomCssClass()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddUnmatched("class", "my-custom-pagination"));

        var nav = component.Find("nav");
        Assert.Contains("my-custom-pagination", nav.ClassList);
        Assert.Contains("ds-pagination", nav.ClassList);
    }

    [Fact]
    public void Pagination_WithoutChildContent_RendersEmptyNav()
    {
        var component = Render<PaginationComponent>();

        var nav = component.Find("nav");
        Assert.Empty(nav.InnerHtml.Trim());
    }
}