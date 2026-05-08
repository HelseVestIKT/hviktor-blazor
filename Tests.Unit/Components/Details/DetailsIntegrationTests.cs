using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Microsoft.AspNetCore.Components;

namespace Tests.Unit.Components.Details;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Details")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class DetailsIntegrationTests : HviktorBunitContext
{

    [Fact]
    public void Details_RendersSummaryAndContent()
    {
        const string summaryText = "Summary title";
        const string contentText = "Content body";

        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Details.Summary>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, summaryText)));
                builder.CloseComponent();

                builder.OpenComponent<global::Details.Content>(2);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, contentText)));
                builder.CloseComponent();
            }));

        var summary = component.Find("summary");
        var content = component.Find("details div");

        Assert.Contains(summaryText, summary.InnerHtml);
        Assert.Contains(contentText, content.InnerHtml);
    }

    [Fact]
    public void Details_SummaryInheritsParentContext()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("color", Color.Accent)
            .AddChildContent<global::Details.Summary>(summaryParams => summaryParams
                .AddChildContent("Summary")));

        var summary = component.Find("summary");
        Assert.NotNull(summary);
    }

    [Fact]
    public void Details_ContentInheritsParentContext()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("size", Size.Large)
            .AddChildContent<global::Details.Content>(contentParams => contentParams
                .AddChildContent("Content")));

        var content = component.Find("details div");
        Assert.NotNull(content);
    }

    [Fact]
    public void Details_FullStructureRendersCorrectly()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddUnmatched("size", Size.Medium)
            .AddUnmatched("color", Color.Accent)
            .AddUnmatched("variant", Variant.Tinted)
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Details.Summary>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Expand me")));
                builder.CloseComponent();

                builder.OpenComponent<global::Details.Content>(2);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Hidden content")));
                builder.CloseComponent();
            }));

        var details = component.Find("details");
        Assert.Equal("md", details.GetAttribute("data-size"));
        Assert.Equal("accent", details.GetAttribute("data-color"));
        Assert.Equal("tinted", details.GetAttribute("data-variant"));

        var summary = component.Find("summary");
        Assert.Contains("Expand me", summary.InnerHtml);
        Assert.Equal("button", summary.GetAttribute("role"));

        var content = component.Find("details div");
        Assert.Contains("Hidden content", content.InnerHtml);
    }
}