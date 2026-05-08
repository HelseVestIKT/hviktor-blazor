using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Breadcrumbs;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Breadcrumbs")]
public class BreadcrumbsTests : HviktorBunitContext
{

    [Fact]
    public void Breadcrumbs_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>();

        Assert.NotNull(component.Instance);

        var attr = component.Instance.AdditionalAttributes;
        Assert.Null(attr?.GetValueOrDefault("data-color"));
        Assert.Null(attr?.GetValueOrDefault("data-size"));
    }

    [Fact]
    public void Breadcrumbs_HasDsBreadcrumbsClass()
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>();

        var breadcrumbs = component.Find("nav");
        Assert.Contains("ds-breadcrumbs", breadcrumbs.ClassList);
    }

    [Fact]
    public void Breadcrumbs_RendersAsNavElement()
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>();

        var breadcrumbs = component.Find("nav");
        Assert.Equal("NAV", breadcrumbs.TagName);
    }

    [Fact]
    public void Breadcrumbs_HasNoColorAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>();

        var breadcrumbs = component.Find("nav");
        Assert.Null(breadcrumbs.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    public void Breadcrumbs_AppliesColors(Color color, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>(parameters => parameters
            .AddUnmatched("Color", color));

        var breadcrumbs = component.Find("nav");
        Assert.Equal(expectedDataAttribute, breadcrumbs.GetAttribute("data-color"));
    }

    [Fact]
    public void Breadcrumbs_HasNoSizeAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>();

        var breadcrumbs = component.Find("nav");
        Assert.Null(breadcrumbs.GetAttribute("data-size"));
    }

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    public void Breadcrumbs_AppliesAllSizes(Size size, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>(parameters => parameters
            .AddUnmatched("size", size));

        var breadcrumbs = component.Find("nav");
        Assert.Equal(expectedDataAttribute, breadcrumbs.GetAttribute("data-size"));
    }

    [Fact]
    public void Breadcrumbs_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>(parameters => parameters
            .AddChildContent("<ol><li>Home</li><li>Products</li></ol>"));

        var breadcrumbs = component.Find("nav");
        Assert.Contains("Home", breadcrumbs.InnerHtml);
        Assert.Contains("Products", breadcrumbs.InnerHtml);
    }

    [Fact]
    public void Breadcrumbs_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var breadcrumbs = component.Find("nav");
        Assert.Contains("my-custom-class", breadcrumbs.ClassList);
        Assert.Contains("ds-breadcrumbs", breadcrumbs.ClassList);
    }

    [Fact]
    public void Breadcrumbs_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>(parameters => parameters
            .AddUnmatched("aria-label", "Breadcrumb navigation")
            .AddUnmatched("id", "main-breadcrumbs"));

        var breadcrumbs = component.Find("nav");
        Assert.Equal("Breadcrumb navigation", breadcrumbs.GetAttribute("aria-label"));
        Assert.Equal("main-breadcrumbs", breadcrumbs.GetAttribute("id"));
    }

    [Fact]
    public void Breadcrumbs_ColorAndSizeCanBeCombined()
    {
        var component = Render<Hviktor.Components.Breadcrumbs.Breadcrumbs>(parameters => parameters
            .AddUnmatched("color", Color.Accent)
            .AddUnmatched("size", Size.Small));

        var breadcrumbs = component.Find("nav");
        Assert.Equal("accent", breadcrumbs.GetAttribute("data-color"));
        Assert.Equal("sm", breadcrumbs.GetAttribute("data-size"));
    }
}