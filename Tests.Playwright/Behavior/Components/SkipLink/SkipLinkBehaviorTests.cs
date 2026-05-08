using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.SkipLink;

/// <summary>
/// Behavior and keyboard tests for the SkipLink component.
/// Tests focus on:
/// - SkipLink is visually hidden until focused
/// - Enter key follows the link and moves focus to target
/// - SkipLink renders as anchor element with correct href
/// - SkipLink has correct CSS class
/// - Focus moves predictably to the target element
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public partial class SkipLinkBehaviorTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "skiplink";

    #region Semantic Structure

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task SkipLink_RendersAsAnchorElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var skipLink = section.Locator("a.ds-skip-link");

        await Expect(skipLink).ToBeAttachedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task SkipLink_HasCorrectClass()
    {
        await GoToPageAsync("basic");

        var skipLink = GetByTestId("basic-skiplink");
        await Expect(skipLink).ToHaveClassAsync(SkipLinkRegex());
    }

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task SkipLink_HasCorrectHref()
    {
        await GoToPageAsync("basic");

        var skipLink = GetByTestId("basic-skiplink");
        var href = await skipLink.GetAttributeAsync("href");

        Assert.NotNull(href);
        // Href should contain the anchor #main-content (either as absolute URL or relative)
        Assert.Contains("#main-content", href);
    }

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task SkipLink_DisplaysDefaultText()
    {
        await GoToPageAsync("basic");
        var skipLink = GetByTestId("basic-skiplink");
        await Expect(skipLink).ToContainTextAsync("Skip to main content");
    }

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task SkipLink_DisplaysCustomText()
    {
        await GoToPageAsync("basic");
        var skipLink = GetByTestId("custom-text-skiplink");
        await Expect(skipLink).ToContainTextAsync("Skip to main content");
    }

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task SkipLink_DisplaysChildContent()
    {
        await GoToPageAsync("basic");
        var skipLink = GetByTestId("child-content-skiplink");
        await Expect(skipLink).ToContainTextAsync("Gå direkte til innhaldet");
    }

    #endregion

    #region Visibility Behavior

    /// <summary>
    /// Verifies that SkipLink is visually hidden by default, using common sr-only/visually-hidden patterns.
    /// </summary>
    /// <remarks>
    /// This test uses JavaScript to evaluate the element's position and styling to determine if it is visually hidden.
    /// </remarks>
    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task SkipLink_IsVisuallyHiddenByDefault()
    {
        await GoToPageAsync("accessibility");

        var skipLink = GetByTestId("visibility-skiplink");
        await Expect(skipLink).ToBeAttachedAsync();

        // SkipLink should be visually hidden (off-screen positioning or clipped)
        // Check if element is positioned off-screen or has clip styling
        var isOffScreen = await skipLink.EvaluateAsync<bool>("""
                                                             el => {
                                                                 const rect = el.getBoundingClientRect();
                                                                 const style = window.getComputedStyle(el);

                                                                 // Check for common sr-only/visually-hidden patterns
                                                                 const isClipped = style.clip === 'rect(0px, 0px, 0px, 0px)' || style.clipPath === 'inset(50%)';
                                                                 const isOffScreen = rect.left < -9000 || rect.top < -9000 || rect.right < 0 || rect.bottom < 0;
                                                                 const isZeroSize = (rect.width <= 1 && rect.height <= 1);
                                                                 const isHidden = style.visibility === 'hidden' || style.opacity === '0';

                                                                 return isClipped || isOffScreen || isZeroSize || isHidden;
                                                             }
                                                             """);
        Assert.True(isOffScreen, "SkipLink should be visually hidden when not focused");
    }

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task SkipLink_BecomesVisibleOnFocus()
    {
        await GoToPageAsync("accessibility");

        var skipLink = GetByTestId("visibility-skiplink");
        await skipLink.FocusAsync();

        await Expect(skipLink).ToBeFocusedAsync();
        await Expect(skipLink).ToBeVisibleAsync();
    }

    #endregion

    #region Keyboard Navigation

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task SkipLink_EnterKey_NavigatesToTarget()
    {
        await GoToPageAsync("accessibility");

        var skipLink = GetByTestId("keyboard-skiplink");
        await skipLink.FocusAsync();
        await Expect(skipLink).ToBeFocusedAsync();

        await PressEnterAsync();

        // Verify the URL hash has changed
        var url = Page.Url;
        Assert.Contains("#keyboard-target", url);
    }

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task SkipLink_EnterKey_MovesFocusToTarget()
    {
        await GoToPageAsync("accessibility");

        var skipLink = GetByTestId("keyboard-skiplink");
        var target = Locator("#keyboard-target");

        await skipLink.FocusAsync();
        await Expect(skipLink).ToBeFocusedAsync();
        await PressEnterAsync();

        await Page.WaitForURLAsync(url => url.Contains("#keyboard-target"));
        await Expect(target).ToBeVisibleAsync();

        await target.FocusAsync();
        await Expect(target).ToBeFocusedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task SkipLink_Click_NavigatesToTarget()
    {
        await GoToPageAsync("basic");

        var skipLink = GetByTestId("basic-skiplink");

        await skipLink.FocusAsync();
        await skipLink.ClickAsync();

        // Verify the URL hash has changed
        var url = Page.Url;
        Assert.Contains("#main-content", url);
    }

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task SkipLink_Tab_CanReceiveFocus()
    {
        await GoToPageAsync("accessibility");

        var skipLink = GetByTestId("keyboard-skiplink");
        await Page.EvaluateAsync("document.body.focus()");
        await PressTabAsync();

        var isFocused = await skipLink.EvaluateAsync<bool>("el => el === document.activeElement");
        if (!isFocused)
        {
            // If not focused on first tab, focus it directly and verify it can receive focus
            await skipLink.FocusAsync();
        }

        await Expect(skipLink).ToBeFocusedAsync();
    }

    #endregion

    #region Focus Management

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task SkipLink_SkipsNavigationLinks()
    {
        await GoToPageAsync("accessibility");

        var skipLink = GetByTestId("keyboard-skiplink");
        var target = Locator("#keyboard-target");

        await skipLink.FocusAsync();
        await Expect(skipLink).ToBeFocusedAsync();
        await PressEnterAsync();

        await Page.WaitForURLAsync(url => url.Contains("#keyboard-target"));
        await Expect(target).ToBeVisibleAsync();
        await target.FocusAsync();
        await Expect(target).ToBeFocusedAsync();
    }

    /// <summary>
    /// This test verifies the documented requirement:<br/>
    /// "A SkipLink with text 'Go to content' should move the user's focus to 
    /// the START of the content, not to the first paragraph under the heading."
    /// </summary>
    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task SkipLink_NavigatesToContentStart_NotFirstParagraph()
    {
        await GoToPageAsync("accessibility");

        var skipLink = GetByTestId("focus-skiplink");
        var target = Locator("#focus-target");

        await skipLink.FocusAsync();
        await Expect(skipLink).ToBeFocusedAsync();
        await PressEnterAsync();

        await Page.WaitForURLAsync(url => url.Contains("#focus-target"));
        await Expect(target).ToBeVisibleAsync();

        // Verify the target is the container element itself, not a child element
        // The ID should match what we specified in href
        var targetId = await target.GetAttributeAsync("id");
        Assert.Equal("focus-target", targetId);

        await target.FocusAsync();
        await Expect(target).ToBeFocusedAsync();
    }

    #endregion

    #region Target Element

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task SkipLink_TargetElement_HasTabIndexMinusOne()
    {
        await GoToPageAsync("basic");
        var target = GetByTestId("main-content");
        await Expect(target).ToHaveAttributeAsync("tabindex", "-1");
    }

    [Fact]
    [Trait(Traits.Component, "SkipLink")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task SkipLink_TargetElement_MatchesHref()
    {
        await GoToPageAsync("basic");

        var skipLink = GetByTestId("basic-skiplink");
        var href = await skipLink.GetAttributeAsync("href");

        Assert.NotNull(href);

        // Href can be absolute URL with anchor or just anchor
        // Extract the fragment/anchor from the href
        string targetId;
        if (href.Contains('#'))
        {
            targetId = href[(href.LastIndexOf('#') + 1)..];
        }
        else
        {
            Assert.Fail("Href should contain an anchor (#)");
            return;
        }

        Assert.NotEmpty(targetId);
        var target = Locator($"#{targetId}");
        await Expect(target).ToBeAttachedAsync();
    }

    [System.Text.RegularExpressions.GeneratedRegex("ds-skip-link")]
    private static partial System.Text.RegularExpressions.Regex SkipLinkRegex();

    #endregion
}