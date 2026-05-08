using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Details;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Details")]
public class DetailsTests : HviktorBunitContext
{

    [Fact]
    public void Details_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Details.Details>();

        Assert.NotNull(component.Instance);

        var details = component.Find("details");
        Assert.Null(details.GetAttribute("data-size"));
        Assert.Null(details.GetAttribute("data-color"));
        Assert.Equal("default", details.GetAttribute("data-variant"));
    }

    [Fact]
    public void Details_HasDsDetailsClass()
    {
        var component = Render<Hviktor.Components.Details.Details>();

        var details = component.Find("details");
        Assert.Contains("ds-details", details.ClassList);
    }

    [Fact]
    public void Details_RendersAsUDetailsElement()
    {
        var component = Render<Hviktor.Components.Details.Details>();

        var details = component.Find("details");
        Assert.Equal("DETAILS", details.TagName);
    }

    [Fact]
    public void Details_HasRoleGroup()
    {
        var component = Render<Hviktor.Components.Details.Details>();

        var details = component.Find("details");
        Assert.Equal("group", details.GetAttribute("role"));
    }

    [Fact]
    public void Details_RendersChildContent()
    {
        const string content = "Details content";
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent(content));

        var details = component.Find("details");
        Assert.Contains(content, details.InnerHtml);
    }

    [Fact]
    public void Details_AppliesSize()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("size", Size.Small));

        var details = component.Find("details");
        Assert.Equal("sm", details.GetAttribute("data-size"));
    }

    [Fact]
    public void Details_AppliesColor()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("color", Color.Accent));

        var details = component.Find("details");
        Assert.Equal("accent", details.GetAttribute("data-color"));
    }

    [Fact]
    public void Details_AppliesVariant()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("variant", Variant.Tinted));

        var details = component.Find("details");
        Assert.Equal("tinted", details.GetAttribute("data-variant"));
    }

    [Fact]
    public void Details_DefaultVariantIsDefault()
    {
        var component = Render<Hviktor.Components.Details.Details>();

        var details = component.Find("details");
        Assert.Equal("default", details.GetAttribute("data-variant"));
    }

    [Fact]
    public void Details_HasNoSizeAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Details.Details>();

        var details = component.Find("details");
        Assert.Null(details.GetAttribute("data-size"));
    }

    [Fact]
    public void Details_HasNoColorAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Details.Details>();

        var details = component.Find("details");
        Assert.Null(details.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    public void Details_AppliesAllSizes(Size size, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("size", size));

        var details = component.Find("details");
        Assert.Equal(expectedDataAttribute, details.GetAttribute("data-size"));
    }

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    public void Details_AppliesAllColors(Color color, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("color", color));

        var details = component.Find("details");
        Assert.Equal(expectedDataAttribute, details.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData(Variant.Default, "default")]
    [InlineData(Variant.Tinted, "tinted")]
    public void Details_AppliesAllVariants(Variant variant, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("variant", variant));

        var details = component.Find("details");
        Assert.Equal(expectedDataAttribute, details.GetAttribute("data-variant"));
    }

    [Fact]
    public void Details_CombinesAllParameters()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("size", Size.Large)
            .AddUnmatched("color", Color.Neutral)
            .AddUnmatched("variant", Variant.Tinted)
            .AddChildContent("Full details"));

        var details = component.Find("details");
        Assert.Equal("lg", details.GetAttribute("data-size"));
        Assert.Equal("neutral", details.GetAttribute("data-color"));
        Assert.Equal("tinted", details.GetAttribute("data-variant"));
        Assert.Contains("Full details", details.InnerHtml);
    }

    [Fact]
    public void Details_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var details = component.Find("details");
        Assert.Contains("my-custom-class", details.ClassList);
        Assert.Contains("ds-details", details.ClassList);
    }

    [Fact]
    public void Details_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("data-testid", "details-test")
            .AddUnmatched("aria-label", "Expandable section"));

        var details = component.Find("details");
        Assert.Equal("details-test", details.GetAttribute("data-testid"));
        Assert.Equal("Expandable section", details.GetAttribute("aria-label"));
    }
}