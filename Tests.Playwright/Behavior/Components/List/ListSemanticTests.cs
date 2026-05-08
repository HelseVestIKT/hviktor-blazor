using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.List;

/// <summary>
/// Behavior tests for the List component.
/// Tests semantic HTML structure and accessibility features:
/// - List.Unordered renders as &lt;ul&gt; element
/// - List.Ordered renders as &lt;ol&gt; element
/// - List.Item renders as &lt;li&gt; element
/// - Navigation lists should be wrapped with nav and aria-label
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class ListSemanticTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "list";

    #region Semantic HTML Structure

    [Fact]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ListUnordered_RendersAsUlElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("unordered-basic");
        var list = section.Locator("ul.ds-list");

        await Expect(list).ToBeVisibleAsync();
        await Expect(list).ToHaveClassAsync("ds-list");
    }

    [Fact]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ListOrdered_RendersAsOlElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("ordered-basic");
        var list = section.Locator("ol.ds-list");

        await Expect(list).ToBeVisibleAsync();
        await Expect(list).ToHaveClassAsync("ds-list");
    }

    [Fact]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task ListItem_RendersAsLiElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("unordered-basic");
        var items = section.Locator("ul.ds-list > li");

        await Expect(items).ToHaveCountAsync(3);
    }

    [Fact]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task OrderedList_HasCorrectNumberOfItems()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("ordered-basic");
        var items = section.Locator("ol.ds-list > li");

        await Expect(items).ToHaveCountAsync(3);
    }

    #endregion

    #region Nested Lists

    [Fact]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Structure)]
    public async Task UnorderedList_HasCorrectStructure()
    {
        await GoToPageAsync("nested");

        var section = GetByTestId("nested-unordered");
        var list = section.Locator("ul.ds-list");
        var items = list.Locator("> li");

        await Expect(list).ToBeVisibleAsync();
        await Expect(items).ToHaveCountAsync(3);
    }

    [Fact]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Structure)]
    public async Task OrderedList_HasCorrectStructure()
    {
        await GoToPageAsync("nested");

        var section = GetByTestId("nested-ordered");
        var list = section.Locator("ol.ds-list");
        var items = list.Locator("> li");

        await Expect(list).ToBeVisibleAsync();
        await Expect(items).ToHaveCountAsync(3);
    }

    [Fact]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Structure)]
    public async Task MixedLists_BothTypesRendered()
    {
        await GoToPageAsync("nested");

        var section = GetByTestId("nested-mixed");
        var orderedList = section.Locator("ol.ds-list");
        var unorderedList = section.Locator("ul.ds-list");

        await Expect(orderedList).ToBeVisibleAsync();
        await Expect(unorderedList).ToBeVisibleAsync();
    }

    #endregion

    #region Accessibility Features

    [Fact]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task NavigationList_HasNavWrapperWithAriaLabel()
    {
        await GoToPageAsync("accessibility");

        var section = GetByTestId("navigation-list");
        var nav = section.Locator("nav[aria-label]");

        await Expect(nav).ToBeVisibleAsync();
        await Expect(nav).ToHaveAttributeAsync("aria-label", "Snarveier");

        // Verify the list is inside the nav
        var listInsideNav = nav.Locator("ul.ds-list");
        await Expect(listInsideNav).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "List")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task InteractiveList_ContainsButtons()
    {
        await GoToPageAsync("accessibility");

        var section = GetByTestId("interactive-list");
        var buttons = section.Locator("ul.ds-list li button");

        await Expect(buttons).ToHaveCountAsync(3);
    }

    #endregion
}