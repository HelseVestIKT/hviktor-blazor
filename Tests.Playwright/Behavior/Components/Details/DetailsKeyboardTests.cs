using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Details;

/// <summary>
/// Keyboard behavior tests for the Details component.
/// Tests actual keyboard navigation behavior as per accessibility requirements:
/// - Space or Enter opens and closes Details
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class DetailsKeyboardTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "details";

    #region Open/Close with Space Key

    [Fact]
    [Trait(Traits.Component, "Details")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Details_SpaceKey_TogglesOpen()
    {
        await GoToPageAsync("keyboard");

        // Find the summary element and focus it
        var summary = GetByTestId("keyboard-navigation").Locator("summary").First;
        await summary.FocusAsync();

        // Press Space to open
        await PressSpaceAsync();

        // Verify details is open (content should be visible)
        var details = GetByTestId("keyboard-navigation");
        await Expect(details).ToHaveAttributeAsync("open", "");

        // Press Space again to close
        await PressSpaceAsync();

        // Verify details is closed
        await Expect(details).Not.ToHaveAttributeAsync("open", "");
    }

    #endregion

    #region Open/Close with Enter Key

    [Fact]
    [Trait(Traits.Component, "Details")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Details_EnterKey_TogglesOpen()
    {
        await GoToPageAsync("keyboard");

        // Find the summary element and focus it
        var summary = GetByTestId("keyboard-navigation").Locator("summary").First;
        await summary.FocusAsync();

        // Press Enter to open
        await PressEnterAsync();

        // Verify details is open
        var details = GetByTestId("keyboard-navigation");
        await Expect(details).ToHaveAttributeAsync("open", "");

        // Press Enter again to close
        await PressEnterAsync();

        // Verify details is closed
        await Expect(details).Not.ToHaveAttributeAsync("open", "");
    }

    #endregion
}