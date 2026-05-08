using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Link;

/// <summary>
/// Keyboard and focus behavior tests for the Link component.
/// Tests accessibility requirements:
/// - Link functions as standard &lt;a&gt; element.
/// - Focus adds a background around the link for visibility.
/// - Tab navigation between links works correctly.
/// - Enter key activates the link.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class LinkKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "link";

    #region Tab Navigation

    [Fact]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Link_TabNavigation_NavigatesBetweenLinks()
    {
        await GoToPageAsync("accessibility");

        // Focus first link
        var firstLink = GetByTestId("nav-link-1");
        await firstLink.FocusAsync();
        await Expect(firstLink).ToBeFocusedAsync();

        // Tab to second link
        await PressTabAsync();
        var secondLink = GetByTestId("nav-link-2");
        await Expect(secondLink).ToBeFocusedAsync();

        // Tab to third link
        await PressTabAsync();
        var thirdLink = GetByTestId("nav-link-3");
        await Expect(thirdLink).ToBeFocusedAsync();
    }

    #endregion

    #region Focus Styling

    [Fact]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Link_Focus_HasVisibleFocusIndicator()
    {
        await GoToPageAsync("accessibility");

        // Focus the link
        var link = GetByTestId("focusable-link");
        await link.FocusAsync();
        await Expect(link).ToBeFocusedAsync();

        // Verify the link has the ds-link class (which provides focus styling)
        await Expect(link).ToHaveClassAsync("ds-link");
    }

    #endregion

    #region Link Attributes

    [Fact]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Link_Basic_HasCorrectHref()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var link = section.Locator(".ds-link");

        await Expect(link).ToHaveAttributeAsync("href", "/home");
    }

    [Fact]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Link_ExternalNewTab_HasSecurityAttributes()
    {
        await GoToPageAsync("target");

        var section = GetByTestId("external-secure");
        var link = section.Locator(".ds-link");

        // External links opened in new tab should have rel attribute for security
        await Expect(link).ToHaveAttributeAsync("target", "_blank");

        // Check for rel attribute containing security values
        var relAttr = await link.GetAttributeAsync("rel");
        Assert.NotNull(relAttr);
        Assert.Equal("external noreferrer noopener", relAttr);
    }

    [Fact]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Link_ExternalNewTab_CanOverrideSecurityAttributes()
    {
        await GoToPageAsync("target");

        var section = GetByTestId("external-unsecure");
        var link = section.Locator(".ds-link");

        // External links opened in new tab should have rel attribute for security
        await Expect(link).ToHaveAttributeAsync("target", "_blank");

        // Check for rel attribute containing security values
        var relAttr = await link.GetAttributeAsync("rel");
        Assert.NotNull(relAttr);
        Assert.Equal("noopener", relAttr);
    }

    [Fact]
    [Trait(Traits.Component, "Link")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Link_InternalNewTab_HasNoSecurityAttributes()
    {
        await GoToPageAsync("target");

        var section = GetByTestId("internal");
        var link = section.Locator(".ds-link");

        // Internal links opened in new tab should have target but no rel security attribute
        await Expect(link).ToHaveAttributeAsync("target", "_blank");

        // Check that rel attribute is null or doesn't contain noopener (internal links don't need it)
        var relAttr = await link.GetAttributeAsync("rel");
        Assert.Null(relAttr);
    }

    #endregion
}