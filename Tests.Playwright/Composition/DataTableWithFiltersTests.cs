using Microsoft.Playwright;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Composition;

/// <summary>
/// Composition tests for Data Table with Filters.
/// Tests component integration: Table, Search, Chip filters, Tags, Badges, Buttons.
/// </summary>
[Collection(TestCollections.Composition)]
[Trait(Traits.Collection, TestCollections.Composition)]
public class DataTableWithFiltersTests(TestsFixture fixture) : CompositionTestBase<TestsFixture>(fixture)
{
    #region Initial Render Tests

    [Fact]
    [Trait(Traits.Component, "Heading")]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Component, "Badge")]
    [Trait(Traits.Component, "Button")]
    public async Task Page_RendersAllComponents()
    {
        await GoToPageAsync("data-table-with-filters");

        await Expect(Page.GetByTestId("table-title")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("search-input")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("users-table")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("result-count")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("add-user-button")).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    public async Task Table_DisplaysAllUsers()
    {
        await GoToPageAsync("data-table-with-filters");

        await Expect(Page.GetByTestId("user-row-1")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-2")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-3")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-4")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-5")).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Badge")]
    public async Task Badge_ShowsCorrectCount()
    {
        await GoToPageAsync("data-table-with-filters");
        var badge = Page.GetByTestId("result-count");
        await Expect(badge).ToHaveAttributeAsync("data-count", "5");
    }

    #endregion

    #region Search Tests

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Component, "Table")]
    public async Task Search_FiltersResultsByName()
    {
        await GoToPageAsync("data-table-with-filters");

        await Page.GetByTestId("search-input").FillAsync("Ola");
        await Expect(Page.GetByTestId("user-row-1")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-2")).Not.ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Component, "Table")]
    public async Task Search_FiltersResultsByEmail()
    {
        await GoToPageAsync("data-table-with-filters");

        await Page.GetByTestId("search-input").FillAsync("kari@");
        await Expect(Page.GetByTestId("user-row-2")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-1")).Not.ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Component, "Table")]
    public async Task Search_ShowsNoResultsMessage_WhenNoMatch()
    {
        await GoToPageAsync("data-table-with-filters");
        await Page.GetByTestId("search-input").FillAsync("nonexistent");
        await Expect(Page.GetByTestId("no-results")).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Component, "Table")]
    public async Task Search_UpdatesBadgeCount()
    {
        await GoToPageAsync("data-table-with-filters");

        await Page.GetByTestId("search-input").FillAsync("Ola");
        var badge = Page.GetByTestId("result-count");
        await Expect(badge).ToHaveAttributeAsync("data-count", "1");
    }

    #endregion

    #region Filter Tests

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Component, "ToggleGroup")]
    public async Task Filter_ActiveOnly_ShowsActiveUsers()
    {
        await GoToPageAsync("data-table-with-filters");

        await Page.GetByTestId("filter-active").ClickAsync(new LocatorClickOptions { Force = true });
        await Expect(Page.GetByTestId("user-row-1")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-2")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-5")).ToBeVisibleAsync();

        await Expect(Page.GetByTestId("user-row-3")).Not.ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-4")).Not.ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Component, "ToggleGroup")]
    public async Task Filter_InactiveOnly_ShowsInactiveUsers()
    {
        await GoToPageAsync("data-table-with-filters");

        await Page.GetByTestId("filter-inactive").ClickAsync(new LocatorClickOptions { Force = true });
        await Expect(Page.GetByTestId("user-row-3")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-1")).Not.ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Component, "ToggleGroup")]
    public async Task Filter_All_ShowsAllUsers()
    {
        await GoToPageAsync("data-table-with-filters");

        await Page.GetByTestId("filter-active").ClickAsync(new() { Force = true });
        await Page.GetByTestId("filter-all").ClickAsync(new() { Force = true });

        await Expect(Page.GetByTestId("user-row-1")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-3")).ToBeVisibleAsync();
        await Expect(Page.GetByTestId("user-row-4")).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Component, "ToggleGroup")]
    [Trait(Traits.Component, "Search")]
    public async Task Filter_CombinedWithSearch_FiltersCorrectly()
    {
        await GoToPageAsync("data-table-with-filters");

        await Page.GetByTestId("filter-active").ClickAsync(new LocatorClickOptions { Force = true });
        await Page.GetByTestId("search-input").FillAsync("Kari");
        await Expect(Page.GetByTestId("user-row-2")).ToBeVisibleAsync();

        var badge = Page.GetByTestId("result-count");
        await Expect(badge).ToHaveAttributeAsync("data-count", "1");
    }

    #endregion

    #region Action Button Tests

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Component, "Button")]
    [Trait(Traits.Component, "Badge")]
    public async Task DeleteButton_RemovesUserFromTable()
    {
        await GoToPageAsync("data-table-with-filters");

        await Page.GetByTestId("delete-button-1").ClickAsync();
        await Expect(Page.GetByTestId("user-row-1")).Not.ToBeVisibleAsync();

        var badge = Page.GetByTestId("result-count");
        await Expect(badge).ToHaveAttributeAsync("data-count", "4");
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Component, "Button")]
    [Trait(Traits.Component, "Paragraph")]
    public async Task ShowingCount_UpdatesAfterDelete()
    {
        await GoToPageAsync("data-table-with-filters");
        await Page.GetByTestId("delete-button-1").ClickAsync();
        await Expect(Page.GetByTestId("showing-count")).ToContainTextAsync("Showing 4 of 4 users");
    }

    #endregion

    #region Status Tag Tests

    [Fact]
    [Trait(Traits.Component, "Tag")]
    public async Task StatusTag_ShowsCorrectColor_ForActiveUser()
    {
        await GoToPageAsync("data-table-with-filters");
        var statusTag = Page.GetByTestId("user-status-1");
        await Expect(statusTag).ToHaveAttributeAsync("data-color", "success");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    public async Task StatusTag_ShowsCorrectColor_ForInactiveUser()
    {
        await GoToPageAsync("data-table-with-filters");
        var statusTag = Page.GetByTestId("user-status-3");
        await Expect(statusTag).ToHaveAttributeAsync("data-color", "neutral");
    }

    [Fact]
    [Trait(Traits.Component, "Tag")]
    public async Task StatusTag_ShowsCorrectColor_ForPendingUser()
    {
        await GoToPageAsync("data-table-with-filters");
        var statusTag = Page.GetByTestId("user-status-4");
        await Expect(statusTag).ToHaveAttributeAsync("data-color", "warning");
    }

    #endregion

    private static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);
}