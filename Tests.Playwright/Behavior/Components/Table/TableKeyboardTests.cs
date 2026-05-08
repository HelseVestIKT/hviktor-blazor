using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Table;

/// <summary>
/// Keyboard behavior tests for the Table component.
/// Tests keyboard navigation for sortable columns:
/// - Tab navigates to sortable header buttons
/// - Space/Enter activates sorting
/// - aria-sort attribute updates correctly
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TableKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "table";

    #region Tab Navigation to Sortable Headers

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_TabNavigatesToSortableHeader()
    {
        await GoToPageAsync("sorting");

        // Tab from focus-start to first sortable header
        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        // The button inside the sortable header should be focused
        var sortableTable = GetByTestId("sortable");
        var sortButton = sortableTable.Locator("thead th button").First;

        await Expect(sortButton).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_TabNavigatesBetweenSortableHeaders()
    {
        await GoToPageAsync("sorting");

        var sortableTable = GetByTestId("sortable");
        var sortButtons = sortableTable.Locator("thead th button");

        // Focus first sort button
        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        var firstButton = sortButtons.First;
        await Expect(firstButton).ToBeFocusedAsync();

        // Tab to second sort button
        await PressTabAsync();

        var secondButton = sortButtons.Nth(1);
        await Expect(secondButton).ToBeFocusedAsync();
    }

    #endregion

    #region Sort Activation with Space Key

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_SpaceKey_ActivatesSorting()
    {
        await GoToPageAsync("sorting");

        var sortableTable = GetByTestId("sortable");
        var nameHeader = sortableTable.Locator("th").First;

        // Tab to first sortable header
        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        // Verify initial state is "none"
        await Expect(nameHeader).ToHaveAttributeAsync("aria-sort", "none");

        // Press Space to activate sorting
        await PressSpaceAsync();

        // Verify sort changed to ascending
        await Expect(nameHeader).ToHaveAttributeAsync("aria-sort", "ascending");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_SpaceKey_CyclesSortOrder()
    {
        await GoToPageAsync("sorting");

        var sortableTable = GetByTestId("sortable");
        var nameHeader = sortableTable.Locator("th").First;

        // Tab to first sortable header
        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        // Press Space to cycle: none -> ascending
        await PressSpaceAsync();
        await Expect(nameHeader).ToHaveAttributeAsync("aria-sort", "ascending");

        // Press Space again: ascending -> descending
        await PressSpaceAsync();
        await Expect(nameHeader).ToHaveAttributeAsync("aria-sort", "descending");

        // Press Space again: descending -> none
        await PressSpaceAsync();
        await Expect(nameHeader).ToHaveAttributeAsync("aria-sort", "none");
    }

    #endregion

    #region Sort Activation with Enter Key

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_EnterKey_ActivatesSorting()
    {
        await GoToPageAsync("sorting");

        var sortableTable = GetByTestId("sortable");
        var nameHeader = sortableTable.Locator("th").First;

        // Tab to first sortable header
        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        // Verify initial state
        await Expect(nameHeader).ToHaveAttributeAsync("aria-sort", "none");

        // Press Enter to activate sorting
        await PressEnterAsync();

        // Verify sort changed
        await Expect(nameHeader).ToHaveAttributeAsync("aria-sort", "ascending");
    }

    #endregion

    #region Focus Retention After Sort

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_SortingRetainsFocus()
    {
        await GoToPageAsync("sorting");

        var sortableTable = GetByTestId("sortable");
        var sortButton = sortableTable.Locator("thead th button").First;

        // Tab to first sortable header
        var focusStart = GetByTestId("focus-start");
        await focusStart.FocusAsync();
        await PressTabAsync();

        await Expect(sortButton).ToBeFocusedAsync();

        // Press Space to sort
        await PressSpaceAsync();

        // Focus should remain on the sort button
        await Expect(sortButton).ToBeFocusedAsync();
    }

    #endregion

    #region Aria-Sort Attribute Tests

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Table_SortableHeader_HasAriaSortAttribute()
    {
        await GoToPageAsync("sorting");

        var sortableTable = GetByTestId("sortable");
        var sortableHeaders = sortableTable.Locator("th[aria-sort]");

        // Should have 2 sortable headers (Name and Age)
        await Expect(sortableHeaders).ToHaveCountAsync(2);
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Table_AscendingSortHeader_HasCorrectAriaSortValue()
    {
        await GoToPageAsync("sorting");
        var ascendingHeader = GetByTestId("sort-ascending");
        await Expect(ascendingHeader).ToHaveAttributeAsync("aria-sort", "ascending");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Table_DescendingSortHeader_HasCorrectAriaSortValue()
    {
        await GoToPageAsync("sorting");
        var descendingHeader = GetByTestId("sort-descending");
        await Expect(descendingHeader).ToHaveAttributeAsync("aria-sort", "descending");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Table_OtherSortHeader_HasCorrectAriaSortValue()
    {
        await GoToPageAsync("sorting");
        var otherHeader = GetByTestId("sort-other");
        await Expect(otherHeader).ToHaveAttributeAsync("aria-sort", "other");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Table_NoneSortHeader_HasCorrectAriaSortValue()
    {
        await GoToPageAsync("sorting");
        var noneHeader = GetByTestId("sort-name");
        await Expect(noneHeader).ToHaveAttributeAsync("aria-sort", "none");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Table_EmptySortHeader_HasCorrectAriaSortValue()
    {
        await GoToPageAsync("sorting");
        var emptyHeader = GetByTestId("sort-empty");
        // Empty sort attribute should result in empty aria-sort value
        await Expect(emptyHeader).ToHaveAttributeAsync("aria-sort", "");
    }

    #endregion

    #region Sort Cycling Tests for Different Initial Values

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_OtherSort_CyclesCorrectly()
    {
        await GoToPageAsync("sorting");

        var otherTable = GetByTestId("other");
        var otherHeader = GetByTestId("sort-other");
        var sortButton = otherTable.Locator("thead th button").First;

        // Focus the sort button
        await sortButton.FocusAsync();

        // Verify initial state is "other"
        await Expect(otherHeader).ToHaveAttributeAsync("aria-sort", "other");

        // Press Space: other → ascending
        await PressSpaceAsync();
        await Expect(otherHeader).ToHaveAttributeAsync("aria-sort", "ascending");

        // Press Space: ascending → descending
        await PressSpaceAsync();
        await Expect(otherHeader).ToHaveAttributeAsync("aria-sort", "descending");

        // Press Space: descending → other (back to initial)
        await PressSpaceAsync();
        await Expect(otherHeader).ToHaveAttributeAsync("aria-sort", "other");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_NoneSort_CyclesCorrectly()
    {
        await GoToPageAsync("sorting");

        var sortableTable = GetByTestId("sortable");
        var noneHeader = GetByTestId("sort-name");
        var sortButton = sortableTable.Locator("thead th button").First;

        // Focus the sort button
        await sortButton.FocusAsync();

        // Verify initial state is "none"
        await Expect(noneHeader).ToHaveAttributeAsync("aria-sort", "none");

        // Press Space: none → ascending
        await PressSpaceAsync();
        await Expect(noneHeader).ToHaveAttributeAsync("aria-sort", "ascending");

        // Press Space: ascending → descending
        await PressSpaceAsync();
        await Expect(noneHeader).ToHaveAttributeAsync("aria-sort", "descending");

        // Press Space: descending → none (back to initial)
        await PressSpaceAsync();
        await Expect(noneHeader).ToHaveAttributeAsync("aria-sort", "none");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_EmptySort_CyclesCorrectly()
    {
        await GoToPageAsync("sorting");

        var emptyTable = GetByTestId("empty-sort");
        var emptyHeader = GetByTestId("sort-empty");
        var sortButton = emptyTable.Locator("thead th button").First;

        // Focus the sort button
        await sortButton.FocusAsync();

        // Verify initial state is "" (empty)
        await Expect(emptyHeader).ToHaveAttributeAsync("aria-sort", "");

        // Press Space: "" → ascending
        await PressSpaceAsync();
        await Expect(emptyHeader).ToHaveAttributeAsync("aria-sort", "ascending");

        // Press Space: ascending → descending
        await PressSpaceAsync();
        await Expect(emptyHeader).ToHaveAttributeAsync("aria-sort", "descending");

        // Press Space: descending → "" (back to initial)
        await PressSpaceAsync();
        await Expect(emptyHeader).ToHaveAttributeAsync("aria-sort", "");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_AscendingSort_TogglesOnlyBetweenAscendingAndDescending()
    {
        await GoToPageAsync("sorting");

        var ascendingTable = GetByTestId("ascending");
        var ascendingHeader = GetByTestId("sort-ascending");
        var sortButton = ascendingTable.Locator("thead th button").First;

        // Focus the sort button
        await sortButton.FocusAsync();

        // Verify initial state is "ascending"
        await Expect(ascendingHeader).ToHaveAttributeAsync("aria-sort", "ascending");

        // Press Space: ascending → descending
        await PressSpaceAsync();
        await Expect(ascendingHeader).ToHaveAttributeAsync("aria-sort", "descending");

        // Press Space: descending → ascending (NOT none or other)
        await PressSpaceAsync();
        await Expect(ascendingHeader).ToHaveAttributeAsync("aria-sort", "ascending");

        // Press Space: ascending → descending (continues toggle)
        await PressSpaceAsync();
        await Expect(ascendingHeader).ToHaveAttributeAsync("aria-sort", "descending");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Table_DescendingSort_TogglesOnlyBetweenAscendingAndDescending()
    {
        await GoToPageAsync("sorting");

        var descendingTable = GetByTestId("descending");
        var descendingHeader = GetByTestId("sort-descending");
        var sortButton = descendingTable.Locator("thead th button").First;

        // Focus the sort button
        await sortButton.FocusAsync();

        // Verify initial state is "descending"
        await Expect(descendingHeader).ToHaveAttributeAsync("aria-sort", "descending");

        // Press Space: descending → ascending
        await PressSpaceAsync();
        await Expect(descendingHeader).ToHaveAttributeAsync("aria-sort", "ascending");

        // Press Space: ascending → descending (NOT none or other)
        await PressSpaceAsync();
        await Expect(descendingHeader).ToHaveAttributeAsync("aria-sort", "descending");

        // Press Space: descending → ascending (continues toggle)
        await PressSpaceAsync();
        await Expect(descendingHeader).ToHaveAttributeAsync("aria-sort", "ascending");
    }

    #endregion
}