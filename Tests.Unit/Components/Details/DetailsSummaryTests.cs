using Bunit;

namespace Tests.Unit.Components.Details;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Details.Summary")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class DetailsSummaryTests : HviktorBunitContext
{

    [Fact]
    public void Summary_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Summary>());

        var summary = component.Find("summary");
        Assert.NotNull(summary);
    }

    [Fact]
    public void Summary_RendersAsUSummaryElement()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Summary>());

        var summary = component.Find("summary");
        Assert.Equal("SUMMARY", summary.TagName);
    }

    [Fact]
    public void Summary_HasRoleButton()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Summary>());

        var summary = component.Find("summary");
        Assert.Equal("button", summary.GetAttribute("role"));
    }

    [Fact]
    public void Summary_HasSlotSummary()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Summary>());

        var summary = component.Find("summary");
        Assert.Equal("summary", summary.GetAttribute("slot"));
    }

    [Fact]
    public void Summary_HasAriaExpanded()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Summary>());

        var summary = component.Find("summary");
        Assert.Equal("false", summary.GetAttribute("aria-expanded"));
    }

    [Fact]
    public void Summary_HasTabIndex()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Summary>());

        var summary = component.Find("summary");
        Assert.Equal("0", summary.GetAttribute("tabindex"));
    }

    [Fact]
    public void Summary_RendersChildContent()
    {
        const string summaryText = "Click to expand";
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Summary>(summaryParams => summaryParams
                .AddChildContent(summaryText)));

        var summary = component.Find("summary");
        Assert.Contains(summaryText, summary.InnerHtml);
    }

    [Fact]
    public void Summary_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Summary>(summaryParams => summaryParams
                .AddUnmatched("data-testid", "summary-test")));

        var summary = component.Find("summary");
        Assert.Equal("summary-test", summary.GetAttribute("data-testid"));
    }
}