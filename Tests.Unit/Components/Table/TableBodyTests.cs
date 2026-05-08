using Bunit;
using Table;

namespace Tests.Unit.Components.Table;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Table.Body")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class TableBodyTests : HviktorBunitContext
{

    [Fact]
    public void TableBody_RendersTbodyElement()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(p => p
                .AddUnmatched("id", "body-1")));

        var tbody = component.Find("tbody");
        Assert.Equal("TBODY", tbody.TagName);
    }

    [Fact]
    public void TableBody_RequiresId()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(p => p
                .AddUnmatched("id", "required-id")));

        var tbody = component.Find("tbody");
        Assert.NotNull(tbody);
    }

    [Fact]
    public void TableBody_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(p => p
                .AddUnmatched("id", "body-content")
                .AddChildContent("<tr><td>Cell Content</td></tr>")));

        var tbody = component.Find("tbody");
        Assert.Contains("Cell Content", tbody.InnerHtml);
    }

    [Fact]
    public void TableBody_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(p => p
                .AddUnmatched("id", "body-attrs")
                .AddUnmatched("data-testid", "tbody-test")));

        var tbody = component.Find("tbody");
        Assert.Equal("tbody-test", tbody.GetAttribute("data-testid"));
    }
}