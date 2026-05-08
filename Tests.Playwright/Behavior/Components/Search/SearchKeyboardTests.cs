using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Search;

/// <summary>
/// Keyboard and behavior tests for the Search component.
/// Tests accessibility requirements:
/// - Escape key clears the search field for consistent cross-browser behavior
/// - Tab navigation between input, clear button, and search button
/// - Enter key submits the search when focus is in the input field
/// - Clear button has aria-label for screen readers
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class SearchKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "search";

    #region Escape Key Behavior

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Search_EscapeKey_ClearsInput()
    {
        await GoToPageAsync("accessibility");

        var input = GetByTestId("keyboard-input");

        await input.FillAsync("test search query");
        await Expect(input).ToHaveValueAsync("test search query");

        await input.FocusAsync();
        await PressEscapeAsync();

        await Expect(input).ToHaveValueAsync("");
    }

    #endregion

    #region Tab Navigation

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Search_TabNavigation_NavigatesBetweenElements()
    {
        await GoToPageAsync("accessibility");

        var input = GetByTestId("keyboard-input");
        var searchBtn = GetByTestId("keyboard-button");

        await input.FocusAsync();
        await Expect(input).ToBeFocusedAsync();

        await PressTabAsync();

        // Continue tabbing until we reach the search button
        const int maxTabs = 3;
        for (var i = 0; i < maxTabs; i++)
        {
            if (await searchBtn.EvaluateAsync<bool>("el => el === document.activeElement"))
            {
                break;
            }

            await PressTabAsync();
        }

        await Expect(searchBtn).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Search_TabNavigation_NavigatesBetweenSearchFields()
    {
        await GoToPageAsync("accessibility");

        var search1Input = GetByTestId("search-1-input");
        var search1Button = GetByTestId("search-1-button");
        var search2Input = GetByTestId("search-2-input");

        await search1Input.FocusAsync();
        await Expect(search1Input).ToBeFocusedAsync();

        // Tab through first search field until we reach the button
        var maxTabs = 3;
        for (var i = 0; i < maxTabs; i++)
        {
            await PressTabAsync();
            if (await search1Button.EvaluateAsync<bool>("el => el === document.activeElement"))
            {
                break;
            }
        }

        await Expect(search1Button).ToBeFocusedAsync();
        await PressTabAsync();
        await Expect(search2Input).ToBeFocusedAsync();
    }

    #endregion

    #region Semantic Structure

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Search_RendersAsSearchContainer()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var search = section.Locator(".ds-search");

        await Expect(search).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Search_InputHasCorrectClass()
    {
        await GoToPageAsync("basic");
        var input = GetByTestId("basic-input");
        await Expect(input).ToHaveClassAsync("ds-input");
    }

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Search_ClearButtonHasAriaLabel()
    {
        await GoToPageAsync("basic");
        var clearBtn = GetByTestId("basic-clear");
        await Expect(clearBtn).ToHaveAttributeAsync("aria-label", "Tøm");
    }

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Search_ClearButtonHasResetType()
    {
        await GoToPageAsync("basic");
        var clearBtn = GetByTestId("basic-clear");
        await Expect(clearBtn).ToHaveAttributeAsync("type", "reset");
    }

    #endregion

    #region Clear Button Functionality

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Search_ClearButton_ClearsInput()
    {
        await GoToPageAsync("accessibility");

        var input = GetByTestId("keyboard-input");
        var clearBtn = GetByTestId("keyboard-clear");

        await input.FillAsync("test value");
        await Expect(input).ToHaveValueAsync("test value");

        await clearBtn.DispatchEventAsync("click");
        await Expect(input).ToHaveValueAsync("");
    }

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Search_ClearButton_EnterKeyClearsInput()
    {
        await GoToPageAsync("accessibility");

        var input = GetByTestId("keyboard-input");
        var clearBtn = GetByTestId("keyboard-clear");

        await input.FillAsync("test value");
        await Expect(input).ToHaveValueAsync("test value");

        await clearBtn.FocusAsync();
        await PressEnterAsync();
        await Expect(input).ToHaveValueAsync("");
    }

    [Fact]
    [Trait(Traits.Component, "Search")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Search_ClearButton_SpaceKeyClearsInput()
    {
        await GoToPageAsync("accessibility");

        var input = GetByTestId("keyboard-input");
        var clearBtn = GetByTestId("keyboard-clear");

        await input.FillAsync("test value");
        await Expect(input).ToHaveValueAsync("test value");

        await clearBtn.FocusAsync();
        await PressSpaceAsync();
        await Expect(input).ToHaveValueAsync("");
    }

    #endregion
}