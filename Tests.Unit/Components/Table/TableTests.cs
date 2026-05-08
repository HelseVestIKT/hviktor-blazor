using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using TableComponent = Hviktor.Components.Table.Table;

namespace Tests.Unit.Components.Table;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Table")]
public class TableTests : HviktorBunitContext
{

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void Table_RendersTableElement()
    {
        var component = Render<TableComponent>();

        var table = component.Find("table");
        Assert.Equal("TABLE", table.TagName);
    }

    [Fact]
    public void Table_HasDsTableClass()
    {
        var component = Render<TableComponent>();

        var table = component.Find("table");
        Assert.Contains("ds-table", table.ClassList);
    }

    [Fact]
    public void Table_HasDefaultId()
    {
        var component = Render<TableComponent>();

        Assert.NotNull(component.Instance.Id);
        Assert.NotEmpty(component.Instance.Id);
    }

    [Fact]
    public void Table_AcceptsCustomId()
    {
        var component = Render<TableComponent>(parameters => parameters
            .Add(p => p.Id, "my-table"));

        var table = component.Find("table");
        Assert.Equal("my-table", table.Id);
    }

    [Fact]
    public void Table_RendersChildContent()
    {
        var component = Render<TableComponent>(parameters => parameters
            .AddChildContent("<tbody><tr><td>Test</td></tr></tbody>"));

        var table = component.Find("table");
        Assert.Contains("Test", table.InnerHtml);
    }

    #endregion

    #region Color Tests.Playwright

    [Fact]
    public void Table_AppliesAccentColor()
    {
        var component = Render<TableComponent>(parameters => parameters
            .AddUnmatched("color", Color.Accent));

        var table = component.Find("table");
        Assert.Equal("accent", table.GetAttribute("data-color"));
    }

    [Fact]
    public void Table_AppliesNeutralColor()
    {
        var component = Render<TableComponent>(parameters => parameters
            .AddUnmatched("color", Color.Neutral));

        var table = component.Find("table");
        Assert.Equal("neutral", table.GetAttribute("data-color"));
    }

    [Fact]
    public void Table_NoColorAttributeWhenNull()
    {
        var component = Render<TableComponent>();

        var table = component.Find("table");
        Assert.Null(table.GetAttribute("data-color"));
    }

    #endregion

    #region StickyHeader Tests.Playwright

    [Fact]
    public void Table_StickyHeaderFalseByDefault()
    {
        var component = Render<TableComponent>();

        var table = component.Find("table");
        Assert.Null(table.GetAttribute("data-sticky-header"));
    }

    [Fact]
    public void Table_AppliesStickyHeader()
    {
        var component = Render<TableComponent>(parameters => parameters
            .AddUnmatched("stickyHeader", true));

        var table = component.Find("table");
        Assert.Equal("true", table.GetAttribute("data-sticky-header"));
    }

    #endregion

    #region Border Tests.Playwright

    [Fact]
    public void Table_BorderFalseByDefault()
    {
        var component = Render<TableComponent>();

        var table = component.Find("table");
        Assert.Null(table.GetAttribute("data-border"));
    }

    [Fact]
    public void Table_AppliesBorder()
    {
        var component = Render<TableComponent>(parameters => parameters
            .AddUnmatched("border", true));

        var table = component.Find("table");
        Assert.Equal("true", table.GetAttribute("data-border"));
    }

    #endregion

    #region Hover Tests.Playwright

    [Fact]
    public void Table_HoverFalseByDefault()
    {
        var component = Render<TableComponent>();

        var table = component.Find("table");
        Assert.Null(table.GetAttribute("data-hover"));
    }

    [Fact]
    public void Table_AppliesHover()
    {
        var component = Render<TableComponent>(parameters => parameters
            .AddUnmatched("hover", true));

        var table = component.Find("table");
        Assert.Equal("true", table.GetAttribute("data-hover"));
    }

    #endregion

    #region Zebra Tests.Playwright

    [Fact]
    public void Table_ZebraFalseByDefault()
    {
        var component = Render<TableComponent>();

        var table = component.Find("table");
        Assert.Null(table.GetAttribute("data-zebra"));
    }

    [Fact]
    public void Table_AppliesZebra()
    {
        var component = Render<TableComponent>(parameters => parameters
            .AddUnmatched("zebra", true));

        var table = component.Find("table");
        Assert.Equal("true", table.GetAttribute("data-zebra"));
    }

    #endregion

    #region Additional Attributes Tests.Playwright

    [Fact]
    public void Table_AppliesAdditionalAttributes()
    {
        var component = Render<TableComponent>(parameters => parameters
            .AddUnmatched("data-testid", "table-test")
            .AddUnmatched("aria-label", "Data table"));

        var table = component.Find("table");
        Assert.Equal("table-test", table.GetAttribute("data-testid"));
        Assert.Equal("Data table", table.GetAttribute("aria-label"));
    }

    [Fact]
    public void Table_AppliesCustomCssClass()
    {
        var component = Render<TableComponent>(parameters => parameters
            .AddUnmatched("class", "custom-table"));

        var table = component.Find("table");
        Assert.Contains("custom-table", table.ClassList);
        Assert.Contains("ds-table", table.ClassList);
    }

    #endregion

    #region Combined Features Tests.Playwright

    [Fact]
    public void Table_WithAllFeatures_RendersCorrectly()
    {
        var component = Render<TableComponent>(parameters => parameters
            .Add(p => p.Id, "full-table")
            .AddUnmatched("color", Color.Accent)
            .AddUnmatched("stickyHeader", true)
            .AddUnmatched("border", true)
            .AddUnmatched("hover", true)
            .AddUnmatched("zebra", true));

        var table = component.Find("table");

        Assert.Equal("full-table", table.Id);
        Assert.Contains("ds-table", table.ClassList);
        Assert.Equal("accent", table.GetAttribute("data-color"));
        Assert.Equal("true", table.GetAttribute("data-sticky-header"));
        Assert.Equal("true", table.GetAttribute("data-border"));
        Assert.Equal("true", table.GetAttribute("data-hover"));
        Assert.Equal("true", table.GetAttribute("data-zebra"));
    }

    #endregion
}