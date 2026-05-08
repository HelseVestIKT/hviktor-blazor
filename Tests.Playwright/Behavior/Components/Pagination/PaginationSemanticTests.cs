using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Pagination;

/// <summary>
/// Behavior tests for the Pagination component.
/// Tests semantic HTML structure and accessibility features:
/// - Pagination renders as &lt;nav&gt; element with aria-label
/// - PaginationList renders as &lt;ul&gt; element
/// - PaginationItem renders as &lt;li&gt; element
/// - PaginationButton renders as &lt;button&gt; element with proper attributes
/// - Keyboard navigation between pagination buttons
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class PaginationSemanticTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "pagination";

    #region Semantic HTML Structure

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Pagination_RendersAsNavElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var nav = section.Locator("nav.ds-pagination");
        
        await Expect(nav).ToBeVisibleAsync();
        await Expect(nav).ToHaveClassAsync("ds-pagination");
    }

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Pagination_HasAriaLabel()
    {
        await GoToPageAsync("accessibility");

        var section = GetByTestId("aria-label");
        var nav = section.Locator("nav.ds-pagination");
        
        await Expect(nav).ToHaveAttributeAsync("aria-label", "Sidenavigering for søkeresultater");
    }

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task PaginationList_RendersAsUlElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var list = section.Locator("nav.ds-pagination ul");
        
        await Expect(list).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task PaginationItem_RendersAsLiElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var items = section.Locator("nav.ds-pagination ul > li");
        
        await Expect(items).ToHaveCountAsync(3);
    }

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task PaginationButton_RendersAsButtonElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var buttons = section.Locator("nav.ds-pagination button.ds-button");
        
        // 2 navigation buttons (prev/next) + 3 page buttons
        await Expect(buttons).ToHaveCountAsync(5);
    }

    #endregion

    #region Aria Attributes

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task ActivePage_HasAriaCurrent()
    {
        await GoToPageAsync("accessibility");

        var section = GetByTestId("aria-current");
        var activeButton = section.Locator("button[aria-current='page']");
        
        await Expect(activeButton).ToBeVisibleAsync();
        await Expect(activeButton).ToHaveTextAsync("2");
    }

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task NavigationButtons_HaveAriaLabels()
    {
        await GoToPageAsync("accessibility");

        var prevButton = GetByTestId("prev-btn");
        var nextButton = GetByTestId("next-btn");
        
        await Expect(prevButton).ToHaveAttributeAsync("aria-label", "Forrige side");
        await Expect(nextButton).ToHaveAttributeAsync("aria-label", "Neste side");
    }

    #endregion

    #region Keyboard Navigation

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Pagination_TabNavigation_NavigatesBetweenButtons()
    {
        await GoToPageAsync("accessibility");

        // Focus first button (prev)
        var prevBtn = GetByTestId("prev-btn");
        await prevBtn.FocusAsync();
        await Expect(prevBtn).ToBeFocusedAsync();

        // Tab to first page button
        await PressTabAsync();
        var page1 = GetByTestId("page-1");
        await Expect(page1).ToBeFocusedAsync();

        // Tab to second page button (active)
        await PressTabAsync();
        var page2 = GetByTestId("page-2");
        await Expect(page2).ToBeFocusedAsync();

        // Tab to third page button
        await PressTabAsync();
        var page3 = GetByTestId("page-3");
        await Expect(page3).ToBeFocusedAsync();

        // Tab to next button
        await PressTabAsync();
        var nextBtn = GetByTestId("next-btn");
        await Expect(nextBtn).ToBeFocusedAsync();
    }

    #endregion

    #region Button Variants

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task PaginationButton_PrimaryVariant_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variants");

        var section = GetByTestId("variant-primary");
        var buttons = section.Locator("button.ds-button[data-variant='primary']");
        
        await Expect(buttons.First).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task PaginationButton_SecondaryVariant_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variants");

        var section = GetByTestId("variant-secondary");
        var buttons = section.Locator("button.ds-button[data-variant='secondary']");
        
        await Expect(buttons.First).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Pagination")]
    [Trait(Traits.Category, Categories.Color)]
    public async Task PaginationButton_TertiaryVariant_HasCorrectDataAttribute()
    {
        await GoToPageAsync("variants");

        var section = GetByTestId("variant-tertiary");
        var buttons = section.Locator("button.ds-button[data-variant='tertiary']");
        
        await Expect(buttons.First).ToBeVisibleAsync();
    }

    #endregion
}
