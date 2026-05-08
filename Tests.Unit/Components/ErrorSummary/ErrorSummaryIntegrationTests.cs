using Bunit;
using Microsoft.AspNetCore.Components;

namespace Tests.Unit.Components.ErrorSummary;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "ErrorSummary")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class ErrorSummaryIntegrationTests : HviktorBunitContext
{

    [Fact]
    public void ErrorSummary_RendersFullStructure()
    {
        const string headingText = "There are 2 errors";
        const string error1 = "Name is required";
        const string error2 = "Email is invalid";

        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::ErrorSummary.Heading>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, headingText)));
                builder.CloseComponent();

                builder.OpenComponent<global::ErrorSummary.List>(2);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b =>
                {
                    b.OpenComponent<global::ErrorSummary.Item>(0);
                    b.AddAttribute(1, "ChildContent", (RenderFragment)(ib => ib.AddContent(0, error1)));
                    b.CloseComponent();

                    b.OpenComponent<global::ErrorSummary.Item>(2);
                    b.AddAttribute(3, "ChildContent", (RenderFragment)(ib => ib.AddContent(0, error2)));
                    b.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var heading = component.Find("h2");
        Assert.Contains(headingText, heading.InnerHtml);

        var list = component.Find("ul");
        Assert.NotNull(list);

        var items = component.FindAll("li");
        Assert.Equal(2, items.Count);
        Assert.Contains(error1, items[0].InnerHtml);
        Assert.Contains(error2, items[1].InnerHtml);
    }

    [Fact]
    public void ErrorSummary_HeadingIdLinksToAriaLabelledby()
    {
        const string headingId = "error-heading";

        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent<global::ErrorSummary.Heading>(headingParams => headingParams
                .Add(p => p.Id, headingId)
                .AddChildContent("Errors found")));

        var div = component.Find("div");
        var heading = component.Find("h2");

        Assert.Equal(headingId, heading.Id);
        Assert.Equal(headingId, div.GetAttribute("aria-labelledby"));
    }

    [Fact]
    public void ErrorSummary_RendersWithLinksInItems()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::ErrorSummary.Heading>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Fix the following")));
                builder.CloseComponent();

                builder.OpenComponent<global::ErrorSummary.List>(2);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b =>
                {
                    b.OpenComponent<global::ErrorSummary.Item>(0);
                    b.AddAttribute(1, "ChildContent", (RenderFragment)(ib =>
                    {
                        ib.OpenComponent<global::ErrorSummary.Link>(0);
                        ib.AddAttribute(1, "Href", "#name-field");
                        ib.AddAttribute(2, "ChildContent", (RenderFragment)(lb => lb.AddContent(0, "Name is required")));
                        ib.CloseComponent();
                    }));
                    b.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var link = component.Find("a");
        Assert.Equal("#name-field", link.GetAttribute("href"));
        Assert.Contains("Name is required", link.InnerHtml);
    }

    [Fact]
    public void ErrorSummary_CombinesAllParameters()
    {
        var component = Render<Hviktor.Components.ErrorSummary.ErrorSummary>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::ErrorSummary.Heading>(0);
                builder.AddAttribute(1, "Level", 3);
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Errors")));
                builder.CloseComponent();

                builder.OpenComponent<global::ErrorSummary.List>(3);
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(b =>
                {
                    b.OpenComponent<global::ErrorSummary.Item>(0);
                    b.AddAttribute(1, "ChildContent", (RenderFragment)(ib => ib.AddContent(0, "Error 1")));
                    b.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var heading = component.Find("h3");
        Assert.Contains("Errors", heading.InnerHtml);

        var items = component.FindAll("li");
        Assert.Single(items);
    }
}