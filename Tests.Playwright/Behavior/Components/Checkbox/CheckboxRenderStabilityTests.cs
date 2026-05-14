using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Behavior.Components.Checkbox;

/// <summary>
/// End-to-end tests verifying that the Checkbox component does not produce
/// render tree errors when toggling state. Validates the fix for the
/// ElementReferenceCapture sequence number stability issue.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class CheckboxRenderStabilityTests(TestsFixture fixture) : BehaviorTestBase<TestsFixture>(fixture)
{
    protected override string ComponentPath => "checkbox";
    protected override string DirectoryPath => "component/behavior";

    [Fact]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Checkbox_Toggle_DoesNotProduceConsoleErrors()
    {
        await GoToPageAsync("exception");

        var checkbox = GetByTestId("unchecked");
        await checkbox.FocusAsync();

        // Toggle multiple times to trigger re-renders with varying attribute counts
        for (var i = 0; i < 5; i++)
        {
            await PressSpaceAsync();
            await Page.WaitForTimeoutAsync(100);
        }

        Assert.Empty(Errors);
    }

    [Fact]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Checkbox_ToggleChecked_DoesNotThrowElementReferenceCaptureError()
    {
        await GoToPageAsync("exception");

        var checkedBox = GetByTestId("checked");
        await checkedBox.ClickAsync();
        await Page.WaitForTimeoutAsync(100);
        await checkedBox.ClickAsync();
        await Page.WaitForTimeoutAsync(100);

        Assert.Empty(Errors);
    }

    [Fact]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Checkbox_IndeterminateToggle_DoesNotProduceConsoleErrors()
    {
        await GoToPageAsync("exception");

        var indeterminate = GetByTestId("indeterminate");
        await indeterminate.FocusAsync();

        // Toggle indeterminate checkbox to cycle through states
        await PressSpaceAsync();
        await Page.WaitForTimeoutAsync(100);
        await PressSpaceAsync();
        await Page.WaitForTimeoutAsync(100);
        await PressSpaceAsync();
        await Page.WaitForTimeoutAsync(100);

        Assert.Empty(Errors);
    }

    [Fact]
    [Trait(Traits.Component, "Checkbox")]
    [Trait(Traits.Category, Categories.Keyboard)]
    public async Task Checkbox_RemainsInteractableAfterMultipleToggles()
    {
        await GoToPageAsync("exception");

        var checkbox = GetByTestId("unchecked");
        var renderCount = GetByTestId("render-count");

        var initialCount = await renderCount.TextContentAsync();

        await checkbox.ClickAsync();
        await Expect(checkbox).ToBeCheckedAsync();

        await checkbox.ClickAsync();
        await Expect(checkbox).Not.ToBeCheckedAsync();

        await checkbox.ClickAsync();
        await Expect(checkbox).ToBeCheckedAsync();

        await Expect(checkbox).ToBeEnabledAsync();

        var finalCount = await renderCount.TextContentAsync();
        Assert.NotEqual(initialCount, finalCount);

        var reconnectModal = Page.Locator("#components-reconnect-modal");
        await Expect(reconnectModal).ToBeHiddenAsync();
    }
}