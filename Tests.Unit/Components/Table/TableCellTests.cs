using Bunit;
using Table;

namespace Tests.Unit.Components.Table;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Table.Cell")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class TableCellTests : HviktorBunitContext
{

    [Fact]
    public void TableCell_RendersTdElement()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(bodyParams => bodyParams
                .AddUnmatched("id", "body-cell")
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<Cell>())));

        var td = component.Find("td");
        Assert.Equal("TD", td.TagName);
    }

    [Fact]
    public void TableCell_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(bodyParams => bodyParams
                .AddUnmatched("id", "body-cell-content")
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<Cell>(cellParams => cellParams
                        .AddChildContent("Cell Data")))));

        var td = component.Find("td");
        Assert.Contains("Cell Data", td.TextContent);
    }

    [Fact]
    public void TableCell_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(bodyParams => bodyParams
                .AddUnmatched("id", "body-cell-attrs")
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<Cell>(cellParams => cellParams
                        .AddUnmatched("data-testid", "cell-test")))));

        var td = component.Find("td");
        Assert.Equal("cell-test", td.GetAttribute("data-testid"));
    }

    [Fact]
    public void TableCell_AppliesColspan()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(bodyParams => bodyParams
                .AddUnmatched("id", "body-cell-colspan")
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<Cell>(cellParams => cellParams
                        .AddUnmatched("colspan", "3")))));

        var td = component.Find("td");
        Assert.Equal("3", td.GetAttribute("colspan"));
    }

    [Fact]
    public void TableCell_AppliesRowspan()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Body>(bodyParams => bodyParams
                .AddUnmatched("id", "body-cell-rowspan")
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<Cell>(cellParams => cellParams
                        .AddUnmatched("rowspan", "2")))));

        var td = component.Find("td");
        Assert.Equal("2", td.GetAttribute("rowspan"));
    }
}