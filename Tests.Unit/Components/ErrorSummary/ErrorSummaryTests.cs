using Bunit;

namespace Tests.Unit.Components.ErrorSummary;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "ErrorSummary")]
public class ErrorSummaryTests : HviktorBunitContext
{

    [Fact]
    public void ErrorSummary_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>();
        Assert.NotNull(component.Instance);
    }

    [Fact]
    public void ErrorSummary_HasDsErrorSummaryClass()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>();

        var div = component.Find("div");
        Assert.Contains("ds-error-summary", div.ClassList);
    }

    [Fact]
    public void ErrorSummary_RendersAsDivElement()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>();

        var div = component.Find("div");
        Assert.Equal("DIV", div.TagName);
    }

    [Fact]
    public void ErrorSummary_HasAriaLabelledby()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>();

        var div = component.Find("div");
        Assert.NotNull(div.GetAttribute("aria-labelledby"));
        Assert.NotEmpty(div.GetAttribute("aria-labelledby")!);
    }

    [Fact]
    public void ErrorSummary_HasNegativeTabIndex()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>();

        var div = component.Find("div");
        Assert.Equal("-1", div.GetAttribute("tabindex"));
    }

    [Fact]
    public void ErrorSummary_HasNoSizeAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>();

        var div = component.Find("div");
        Assert.Null(div.GetAttribute("data-size"));
    }

    [Fact]
    public void ErrorSummary_RendersChildContent()
    {
        const string content = "Error summary content";
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent(content));

        var div = component.Find("div");
        Assert.Contains(content, div.InnerHtml);
    }

    [Fact]
    public void ErrorSummary_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddUnmatched("data-testid", "error-summary-test")
            .AddUnmatched("role", "alert"));

        var div = component.Find("div");
        Assert.Equal("error-summary-test", div.GetAttribute("data-testid"));
        Assert.Equal("alert", div.GetAttribute("role"));
    }

    [Fact]
    public void ErrorSummary_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddUnmatched("class", "my-custom-error-summary"));

        var div = component.Find("div");
        Assert.Contains("my-custom-error-summary", div.ClassList);
        Assert.Contains("ds-error-summary", div.ClassList);
    }
}