using Bunit;
using Table;

namespace Tests.Unit.Components.Table;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Table.HeaderCell")]
public class TableHeaderCellTests : HviktorBunitContext
{

    [Fact]
    public void TableHeaderCell_RendersThElement()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>())));

        var th = component.Find("th");
        Assert.Equal("TH", th.TagName);
    }

    [Fact]
    public void TableHeaderCell_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>(cellParams => cellParams
                        .AddChildContent("Column Name")))));

        var th = component.Find("th");
        Assert.Contains("Column Name", th.TextContent);
    }

    [Fact]
    public void TableHeaderCell_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>(cellParams => cellParams
                        .AddUnmatched("data-testid", "header-test")))));

        var th = component.Find("th");
        Assert.Equal("header-test", th.GetAttribute("data-testid"));
    }

    [Fact]
    public void TableHeaderCell_AppliesScope()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>(cellParams => cellParams
                        .AddUnmatched("scope", "col")))));

        var th = component.Find("th");
        Assert.Equal("col", th.GetAttribute("scope"));
    }

    [Fact]
    public void TableHeaderCell_WithSortAttribute_RendersButton()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>(cellParams => cellParams
                        .AddUnmatched("sort", true)
                        .AddChildContent("Sortable Column")))));

        var button = component.Find("th button");
        Assert.NotNull(button);
        Assert.Contains("Sortable Column", button.TextContent);
    }

    [Fact]
    public void TableHeaderCell_WithSortAttribute_HasAriaSortNone()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>(cellParams => cellParams
                        .AddUnmatched("sort", "none")
                        .AddChildContent("Sortable")))));

        var th = component.Find("th");
        Assert.Equal("none", th.GetAttribute("aria-sort"));
    }

    [Fact]
    public void TableHeaderCell_WithoutSortAttribute_NoButton()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>(cellParams => cellParams
                        .AddChildContent("Non-sortable Column")))));

        var buttons = component.FindAll("th button");
        Assert.Empty(buttons);
    }

    [Fact]
    public void TableHeaderCell_SortButtonClick_ChangesToAscending()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>(cellParams => cellParams
                        .AddUnmatched("sort", true)
                        .AddChildContent("Sortable")))));

        var button = component.Find("th button");
        button.Click();

        var th = component.Find("th");
        Assert.Equal("ascending", th.GetAttribute("aria-sort"));
    }

    [Fact]
    public void TableHeaderCell_SortButtonClickTwice_ChangesToDescending()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>(cellParams => cellParams
                        .AddUnmatched("sort", true)
                        .AddChildContent("Sortable")))));

        var button = component.Find("th button");
        button.Click(); // none -> ascending
        button.Click(); // ascending -> descending

        var th = component.Find("th");
        Assert.Equal("descending", th.GetAttribute("aria-sort"));
    }

    [Fact]
    public void TableHeaderCell_SortButtonClickThrice_ResetsToNone()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(headParams => headParams
                .AddChildContent<Row>(rowParams => rowParams
                    .AddChildContent<HeaderCell>(cellParams => cellParams
                        .AddUnmatched("sort", "none")
                        .AddChildContent("Sortable")))));

        var button = component.Find("th button");
        button.Click(); // none -> ascending
        button.Click(); // ascending -> descending
        button.Click(); // descending -> none

        var th = component.Find("th");
        Assert.Equal("none", th.GetAttribute("aria-sort"));
    }
}