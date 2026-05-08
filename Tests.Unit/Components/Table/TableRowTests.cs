using Bunit;
using Table;

namespace Tests.Unit.Components.Table;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Table.Row")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class TableRowTests : HviktorBunitContext
{

    [Fact]
    public void TableRow_RendersTrElement()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(bodyParams => bodyParams
                .AddUnmatched("id", "body-row")
                .AddChildContent<Row>()));

        var tr = component.Find("tr");
        Assert.Equal("TR", tr.TagName);
    }

    [Fact]
    public void TableRow_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(bodyParams => bodyParams
                .AddUnmatched("id", "body-row-content")
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent("<td>Row Content</td>"))));

        var tr = component.Find("tr");
        Assert.Contains("Row Content", tr.InnerHtml);
    }

    [Fact]
    public void TableRow_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(bodyParams => bodyParams
                .AddUnmatched("id", "body-row-attrs")
                .AddChildContent<Row>(rowParams => rowParams
                    .AddUnmatched("data-testid", "row-test"))));

        var tr = component.Find("tr");
        Assert.Equal("row-test", tr.GetAttribute("data-testid"));
    }

    [Fact]
    public void TableRow_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(bodyParams => bodyParams
                .AddUnmatched("id", "body-row-class")
                .AddChildContent<Row>(rowParams => rowParams
                    .AddUnmatched("class", "highlighted-row"))));

        var tr = component.Find("tr");
        Assert.Contains("highlighted-row", tr.ClassList);
    }
}