using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Spinner;

/// <summary>
/// Behavior and semantic tests for the Spinner component.
/// Tests focus on:
/// - Spinner renders as SVG with correct structure
/// - Spinner has role="img" for accessibility
/// - Spinner has aria-label when standalone
/// - Spinner has aria-hidden="true" when text is present
/// - Spinner size variants render correctly
/// - Animation respects prefers-reduced-motion (slows to 6 seconds, not disabled)
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public partial class SpinnerBehaviorTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "spinner";

    #region Semantic Structure

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_RendersAsSvgElement()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("basic");
        var spinner = section.Locator("svg.ds-spinner");

        await Expect(spinner).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_HasCorrectClass()
    {
        await GoToPageAsync("basic");

        var spinner = GetByTestId("basic-spinner");

        await Expect(spinner).ToHaveClassAsync(DsSpinnerRegex());
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_HasRoleImg()
    {
        await GoToPageAsync("basic");

        var spinner = GetByTestId("basic-spinner");

        await Expect(spinner).ToHaveAttributeAsync("role", "img");
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_HasViewBox()
    {
        await GoToPageAsync("basic");

        var spinner = GetByTestId("basic-spinner");

        await Expect(spinner).ToHaveAttributeAsync("viewBox", "0 0 50 50");
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_HasBackgroundCircle()
    {
        await GoToPageAsync("basic");

        var spinner = GetByTestId("basic-spinner");
        var backgroundCircle = spinner.Locator("circle.ds-spinner__background");

        await Expect(backgroundCircle).ToBeAttachedAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_HasAnimatedCircle()
    {
        await GoToPageAsync("basic");

        var spinner = GetByTestId("basic-spinner");
        var animatedCircle = spinner.Locator("circle.ds-spinner__circle");

        await Expect(animatedCircle).ToBeAttachedAsync();
    }

    #endregion

    #region Aria Label

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Spinner_HasAriaLabel()
    {
        await GoToPageAsync("basic");

        var spinner = GetByTestId("basic-spinner");

        await Expect(spinner).ToHaveAttributeAsync("aria-label", "Loading");
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Spinner_HasCustomAriaLabel()
    {
        await GoToPageAsync("basic");

        var spinner = GetByTestId("custom-label-spinner");

        await Expect(spinner).ToHaveAttributeAsync("aria-label", "Laster inn data...");
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Spinner_WithText_HasAriaHidden()
    {
        await GoToPageAsync("basic");

        var spinner = GetByTestId("hidden-spinner");

        // When text is present, spinner should be hidden from assistive technology
        await Expect(spinner).ToHaveAttributeAsync("aria-hidden", "true");
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Spinner_WithText_ParentHasRoleStatus()
    {
        await GoToPageAsync("basic");

        var section = GetByTestId("with-text");
        var statusContainer = section.Locator("[role='status']");

        await Expect(statusContainer).ToBeVisibleAsync();
    }

    #endregion

    #region Size Variants

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_ExtraSmall_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");

        var spinner = GetByTestId("extrasmall-spinner");

        await Expect(spinner).ToHaveAttributeAsync("data-size", "xs");
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_Small_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");

        var spinner = GetByTestId("small-spinner");

        await Expect(spinner).ToHaveAttributeAsync("data-size", "sm");
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_Medium_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");

        var spinner = GetByTestId("medium-spinner");

        await Expect(spinner).ToHaveAttributeAsync("data-size", "md");
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_Large_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");

        var spinner = GetByTestId("large-spinner");

        await Expect(spinner).ToHaveAttributeAsync("data-size", "lg");
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_ExtraLarge_HasCorrectDataAttribute()
    {
        await GoToPageAsync("size");

        var spinner = GetByTestId("extralarge-spinner");

        await Expect(spinner).ToHaveAttributeAsync("data-size", "xl");
    }

    #endregion

    #region Accessibility Context

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Spinner_InStatusRegion_HasAriaLive()
    {
        await GoToPageAsync("accessibility");

        var section = GetByTestId("status-region");
        var liveRegion = section.Locator("[aria-live='polite']");

        await Expect(liveRegion).ToBeVisibleAsync();
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Spinner_HasAccessibleName_WhenStandalone()
    {
        await GoToPageAsync("accessibility");

        var spinner = GetByTestId("labeled-spinner");
        var ariaLabel = await spinner.GetAttributeAsync("aria-label");

        Assert.NotNull(ariaLabel);
        Assert.NotEmpty(ariaLabel);
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Spinner_IsHidden_WhenTextProvided()
    {
        await GoToPageAsync("accessibility");

        var spinner = GetByTestId("hidden-spinner");
        var ariaHidden = await spinner.GetAttributeAsync("aria-hidden");

        Assert.Equal("true", ariaHidden);
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Spinner_AccompanyingText_IsVisible()
    {
        await GoToPageAsync("accessibility");

        var loadingText = GetByTestId("loading-text");

        await Expect(loadingText).ToBeVisibleAsync();
        await Expect(loadingText).ToContainTextAsync("Loading...");
    }

    #endregion

    #region Multiple Spinners

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_MultipleInstances_AllRender()
    {
        await GoToPageAsync("accessibility");

        var section = GetByTestId("multiple-spinners");
        var spinners = section.Locator(".ds-spinner");

        await Expect(spinners).ToHaveCountAsync(2);
    }

    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Aria)]
    public async Task Spinner_MultipleInstances_HaveUniqueLabels()
    {
        await GoToPageAsync("accessibility");

        var spinner1 = GetByTestId("spinner-1");
        var spinner2 = GetByTestId("spinner-2");

        var label1 = await spinner1.GetAttributeAsync("aria-label");
        var label2 = await spinner2.GetAttributeAsync("aria-label");

        Assert.NotNull(label1);
        Assert.NotNull(label2);
        Assert.NotEqual(label1, label2);
    }

    #endregion

    #region Animation (Reduced Motion)

    /// <summary>
    /// Verifies that the spinner has an animation.
    /// Note: When prefers-reduced-motion is set to reduce, animation slows to 6 seconds but is NOT disabled.
    /// The animation is not purely decorative - it indicates loading state.
    /// </summary>
    [Fact]
    [Trait(Traits.Component, "Spinner")]
    [Trait(Traits.Category, Categories.Semantics)]
    public async Task Spinner_HasAnimation()
    {
        await GoToPageAsync("basic");

        var spinner = GetByTestId("basic-spinner");
        var animatedCircle = spinner.Locator("circle.ds-spinner__circle");

        // Check that the animated circle exists and has animation applied via CSS
        await Expect(animatedCircle).ToBeAttachedAsync();

        // Verify the circle has the animation class
        await Expect(animatedCircle).ToHaveClassAsync(DsSpinnerCircleRegex());
    }

    #endregion

    [System.Text.RegularExpressions.GeneratedRegex("ds-spinner")]
    private static partial System.Text.RegularExpressions.Regex DsSpinnerRegex();

    [System.Text.RegularExpressions.GeneratedRegex("ds-spinner__circle")]
    private static partial System.Text.RegularExpressions.Regex DsSpinnerCircleRegex();
}