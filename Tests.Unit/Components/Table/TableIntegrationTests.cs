using Bunit;
using Table;

namespace Tests.Unit.Components.Table;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Table")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class TableIntegrationTests : HviktorBunitContext
{

    [Fact]
    public void Table_CompleteStructure_RendersAllElements()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .Add(p => p.Id, "complete-table")
            .AddChildContent(builder =>
            {
                // Head
                builder.OpenComponent<Head>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(headBuilder =>
                {
                    headBuilder.OpenComponent<Row>(0);
                    headBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<HeaderCell>(0);
                        rowBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Name")));
                        rowBuilder.CloseComponent();

                        rowBuilder.OpenComponent<HeaderCell>(2);
                        rowBuilder.AddAttribute(3, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Age")));
                        rowBuilder.CloseComponent();
                    }));
                    headBuilder.CloseComponent();
                }));
                builder.CloseComponent();

                // Body
                builder.OpenComponent<Body>(2);
                builder.AddAttribute(3, "Id", "table-body");
                builder.AddAttribute(4, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(bodyBuilder =>
                {
                    bodyBuilder.OpenComponent<Row>(0);
                    bodyBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<Cell>(0);
                        rowBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "John")));
                        rowBuilder.CloseComponent();

                        rowBuilder.OpenComponent<Cell>(2);
                        rowBuilder.AddAttribute(3, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "30")));
                        rowBuilder.CloseComponent();
                    }));
                    bodyBuilder.CloseComponent();
                }));
                builder.CloseComponent();

                // Foot
                builder.OpenComponent<Foot>(5);
                builder.AddAttribute(6, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(footBuilder =>
                {
                    footBuilder.OpenComponent<Row>(0);
                    footBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<Cell>(0);
                        rowBuilder.AddAttribute(1, "colspan", "2");
                        rowBuilder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Total: 1")));
                        rowBuilder.CloseComponent();
                    }));
                    footBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        // Verify structure
        var table = component.Find("table");
        var thead = component.Find("thead");
        var tbody = component.Find("tbody");
        var tfoot = component.Find("tfoot");
        var headerCells = component.FindAll("th");
        var dataCells = component.FindAll("td");
        var rows = component.FindAll("tr");

        Assert.NotNull(table);
        Assert.NotNull(thead);
        Assert.NotNull(tbody);
        Assert.NotNull(tfoot);
        Assert.Equal(2, headerCells.Count);
        Assert.Equal(3, dataCells.Count); // 2 data cells + 1 footer cell
        Assert.Equal(3, rows.Count); // 1 header row + 1 body row + 1 footer row
    }

    [Fact]
    public void Table_WithZebraAndHover_AppliesBothAttributes()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddUnmatched("zebra", true)
            .AddUnmatched("hover", true)
            .AddChildContent<Body>(bodyParams => bodyParams
                .AddUnmatched("id", "zebra-hover-body")
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<Cell>(cellParams => cellParams
                        .AddChildContent("Data")))));

        var table = component.Find("table");
        Assert.Equal("true", table.GetAttribute("data-zebra"));
        Assert.Equal("true", table.GetAttribute("data-hover"));
    }

    [Fact]
    public void Table_WithStickyHeaderAndBorder_AppliesBothAttributes()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddUnmatched("stickyHeader", true)
            .AddUnmatched("border", true)
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>(cellParams => cellParams
                        .AddChildContent("Header")))));

        var table = component.Find("table");
        Assert.Equal("true", table.GetAttribute("data-sticky-header"));
        Assert.Equal("true", table.GetAttribute("data-border"));
    }

    [Fact]
    public void Table_MultipleRows_RendersAllRows()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Body>(0);
                builder.AddAttribute(1, "Id", "multi-row-body");
                builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(bodyBuilder =>
                {
                    bodyBuilder.OpenComponent<Row>(0);
                    bodyBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<Cell>(0);
                        rowBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Row 1")));
                        rowBuilder.CloseComponent();
                    }));
                    bodyBuilder.CloseComponent();

                    bodyBuilder.OpenComponent<Row>(2);
                    bodyBuilder.AddAttribute(3, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<Cell>(0);
                        rowBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Row 2")));
                        rowBuilder.CloseComponent();
                    }));
                    bodyBuilder.CloseComponent();

                    bodyBuilder.OpenComponent<Row>(4);
                    bodyBuilder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<Cell>(0);
                        rowBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Row 3")));
                        rowBuilder.CloseComponent();
                    }));
                    bodyBuilder.CloseComponent();

                    bodyBuilder.OpenComponent<Row>(6);
                    bodyBuilder.AddAttribute(7, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<Cell>(0);
                        rowBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Row 4")));
                        rowBuilder.CloseComponent();
                    }));
                    bodyBuilder.CloseComponent();

                    bodyBuilder.OpenComponent<Row>(8);
                    bodyBuilder.AddAttribute(9, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<Cell>(0);
                        rowBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Row 5")));
                        rowBuilder.CloseComponent();
                    }));
                    bodyBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var rows = component.FindAll("tr");
        Assert.Equal(5, rows.Count);
    }

    [Fact]
    public void Table_MultipleSortableColumns_EachHasOwnSortState()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Head>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(headBuilder =>
                {
                    headBuilder.OpenComponent<Row>(0);
                    headBuilder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(rowBuilder =>
                    {
                        rowBuilder.OpenComponent<HeaderCell>(0);
                        rowBuilder.AddAttribute(1, "sort", "none");
                        rowBuilder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Column A")));
                        rowBuilder.CloseComponent();

                        rowBuilder.OpenComponent<HeaderCell>(3);
                        rowBuilder.AddAttribute(4, "sort", "none");
                        rowBuilder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Column B")));
                        rowBuilder.CloseComponent();
                    }));
                    headBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var buttons = component.FindAll("th button");
        Assert.Equal(2, buttons.Count);

        // Click first column
        buttons[0].Click();

        var headerCells = component.FindAll("th");
        Assert.Equal("ascending", headerCells[0].GetAttribute("aria-sort"));
        Assert.Equal("none", headerCells[1].GetAttribute("aria-sort"));
    }
}