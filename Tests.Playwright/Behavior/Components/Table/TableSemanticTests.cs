using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Table;

/// <summary>
/// Behavior tests for the Table component.
/// Tests semantic HTML structure and accessibility features:
/// - Table renders as &lt;table&gt; element with proper structure
/// - Head renders as &lt;thead&gt; element
/// - Body renders as &lt;tbody&gt; element
/// - Row renders as &lt;tr&gt; element
/// - HeaderCell renders as &lt;th&gt; element
/// - Cell renders as &lt;td&gt; element
/// - Caption provides accessible table description
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TableSemanticTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "table";

    #region Semantic HTML Structure

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_RendersAsTableElement()
    {
        await GoToPageAsync("basic");

        var table = GetByTestId("basic");

        await Expect(table).ToBeVisibleAsync();
        await Expect(table).ToHaveClassAsync("ds-table");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_HasTheadElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var thead = section.Locator("thead");

        await Expect(thead).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_HasTbodyElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var tbody = section.Locator("tbody");

        await Expect(tbody).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_HasHeaderCellsWithThElement()
    {
        await GoToPageAsync("basic");

        var table = GetByTestId("basic");
        var headerCells = table.Locator("thead th");

        await Expect(headerCells).ToHaveCountAsync(3);
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_HasDataCellsWithTdElement()
    {
        await GoToPageAsync("basic");

        var table = GetByTestId("basic");
        var dataCells = table.Locator("tbody td");

        // 3 rows × 3 columns = 9 cells
        await Expect(dataCells).ToHaveCountAsync(9);
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_HasRowsWithTrElement()
    {
        await GoToPageAsync("basic");

        var table = GetByTestId("basic");
        var headerRows = table.Locator("thead tr");
        var bodyRows = table.Locator("tbody tr");

        await Expect(headerRows).ToHaveCountAsync(1);
        await Expect(bodyRows).ToHaveCountAsync(3);
    }

    #endregion

    #region Caption Tests

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_HasCaptionElement()
    {
        await GoToPageAsync("caption");

        var table = GetByTestId("with-caption");
        var caption = table.Locator("caption");

        await Expect(caption).ToBeVisibleAsync();
        await Expect(caption).ToHaveTextAsync("Employee Information Table");
    }

    #endregion

    #region Scope Attribute Tests

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_HeaderCells_HaveColumnScope()
    {
        await GoToPageAsync("scope");

        var table = GetByTestId("col-scope");
        var headerCells = table.Locator("thead th[scope='col']");

        await Expect(headerCells).ToHaveCountAsync(3);
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_HeaderCells_HaveRowScope()
    {
        await GoToPageAsync("scope");

        var table = GetByTestId("row-scope");
        var rowHeaderCells = table.Locator("tbody th[scope='row']");

        await Expect(rowHeaderCells).ToHaveCountAsync(2);
    }

    #endregion

    #region Data Attributes Tests

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_WithZebra_HasDataAttribute()
    {
        await GoToPageAsync("zebra");

        var table = GetByTestId("zebra");

        await Expect(table).ToHaveAttributeAsync("data-zebra", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_WithBorder_HasDataAttribute()
    {
        await GoToPageAsync("border");

        var table = GetByTestId("border");

        await Expect(table).ToHaveAttributeAsync("data-border", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_WithHover_HasDataAttribute()
    {
        await GoToPageAsync("hover");

        var table = GetByTestId("hover");

        await Expect(table).ToHaveAttributeAsync("data-hover", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_WithStickyHeader_HasDataAttribute()
    {
        await GoToPageAsync("sticky-header");

        var table = GetByTestId("sticky-header");

        await Expect(table).ToHaveAttributeAsync("data-sticky-header", "true");
    }

    #endregion

    #region Empty Cells Tests

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Table_EmptyCells_AreProperTdElements()
    {
        await GoToPageAsync("empty-cells");

        var table = GetByTestId("empty-cells");
        var allCells = table.Locator("tbody td");

        // 3 rows × 3 columns = 9 cells (including empty ones)
        await Expect(allCells).ToHaveCountAsync(9);
    }

    #endregion
}