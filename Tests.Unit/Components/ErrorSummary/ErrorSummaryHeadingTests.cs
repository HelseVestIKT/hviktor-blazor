using Bunit;

namespace Tests.Unit.Components.ErrorSummary;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "ErrorSummary.Heading")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class ErrorSummaryHeadingTests : HviktorBunitContext
{

    [Fact]
    public void Heading_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Heading>());

        var heading = component.Find("h2");
        Assert.NotNull(heading);
    }

    [Fact]
    public void Heading_HasDsHeadingClass()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Heading>());

        var heading = component.Find("h2");
        Assert.Contains("ds-heading", heading.ClassList);
    }

    [Fact]
    public void Heading_HasGeneratedId()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Heading>());

        var heading = component.Find("h2");
        Assert.NotNull(heading.Id);
        Assert.NotEmpty(heading.Id);
    }

    [Fact]
    public void Heading_AcceptsCustomId()
    {
        const string customId = "my-heading-id";
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Heading>(headingParams => headingParams
                .Add(p => p.Id, customId)));

        var heading = component.Find("h2");
        Assert.Equal(customId, heading.Id);
    }

    [Fact]
    public void Heading_SetsParentAriaLabelledby()
    {
        const string headingId = "custom-heading-id";
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Heading>(headingParams => headingParams
                .Add(p => p.Id, headingId)));

        var div = component.Find("div");
        Assert.Equal(headingId, div.GetAttribute("aria-labelledby"));
    }

    [Theory]
    [InlineData(1, "H1")]
    [InlineData(2, "H2")]
    [InlineData(3, "H3")]
    [InlineData(4, "H4")]
    [InlineData(5, "H5")]
    [InlineData(6, "H6")]
    public void Heading_RendersCorrectLevel(int level, string expectedTagName)
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Heading>(headingParams => headingParams
                .Add(p => p.Level, level)));

        var heading = component.Find($"h{level}");
        Assert.Equal(expectedTagName, heading.TagName);
    }

    [Fact]
    public void Heading_RendersChildContent()
    {
        const string headingText = "There are errors in the form";
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Heading>(headingParams => headingParams
                .AddChildContent(headingText)));

        var heading = component.Find("h2");
        Assert.Contains(headingText, heading.InnerHtml);
    }

    [Fact]
    public void Heading_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Heading>(headingParams => headingParams
                .AddUnmatched("data-testid", "heading-test")));

        var heading = component.Find("h2");
        Assert.Equal("heading-test", heading.GetAttribute("data-testid"));
    }
}