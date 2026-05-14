using Microsoft.Playwright;
using Tests.Playwright.Fixtures;

namespace Tests.Playwright.Behavior;

/// <summary>
/// Base class for keyboard and interaction behavior tests.
/// Provides common setup, teardown, navigation, and assertion helpers.
/// </summary>
public abstract class BehaviorTestBase<TFixture>(TFixture fixture) : IClassFixture<TFixture>, IAsyncLifetime
    where TFixture : PlaywrightFixture
{
    protected IPage Page { get; private set; } = null!;

    protected readonly List<string> Errors = [];

    /// <summary>
    /// The component path segment used in URLs (e.g., "dropdown", "dialog", "details").
    /// </summary>
    protected abstract string ComponentPath { get; }

    /// <summary>
    /// The path to the directory containing the component's test files (e.g., "components/compliance").
    /// </summary>
    protected virtual string DirectoryPath { get; } = "component/compliance";

    #region Lifecycle

    public async ValueTask InitializeAsync()
    {
        Page = await fixture.CreatePageAsync();
        Page.PageError += (_, exception) => Errors.Add(exception);
        Page.Console += (_, msg) =>
        {
            if (msg.Type == "error")
            {
                Errors.Add(msg.Text);
            }
        };
    }

    public async ValueTask DisposeAsync()
    {
        Errors.Clear();
        await Page.CloseAsync();
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Navigation

    /// <summary>
    /// Navigates to a component compliance test page.
    /// </summary>
    /// <param name="pageName">The page name (e.g., "keyboard", "state")</param>
    protected async Task GoToPageAsync(string pageName)
    {
        var url = $"{fixture.BaseUrl}/{DirectoryPath}/{ComponentPath}/{pageName}";
        var response = await Page.GotoAsync(url);
        Assert.NotNull(response);
        Assert.True(response.Ok, $"Failed to load page. Status: {response.Status}");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    #endregion

    #region Keyboard Helpers

    /// <summary>
    /// Presses a key on the keyboard.
    /// </summary>
    private Task PressKeyAsync(string key) => Page.Keyboard.PressAsync(key);

    /// <summary>
    /// Presses the Space key.
    /// </summary>
    protected Task PressSpaceAsync() => PressKeyAsync("Space");

    /// <summary>
    /// Presses the Enter key.
    /// </summary>
    protected Task PressEnterAsync() => PressKeyAsync("Enter");

    /// <summary>
    /// Presses the Escape key.
    /// </summary>
    protected Task PressEscapeAsync() => PressKeyAsync("Escape");

    /// <summary>
    /// Presses the Tab key.
    /// </summary>
    protected Task PressTabAsync() => PressKeyAsync("Tab");

    /// <summary>
    /// Presses Shift+Tab to navigate backwards.
    /// </summary>
    protected Task PressShiftTabAsync() => Page.Keyboard.PressAsync("Shift+Tab");

    /// <summary>
    /// Presses the Arrow Down key.
    /// </summary>
    protected Task PressArrowDownAsync() => PressKeyAsync("ArrowDown");

    /// <summary>
    /// Presses the Arrow Up key.
    /// </summary>
    protected Task PressArrowUpAsync() => PressKeyAsync("ArrowUp");

    /// <summary>
    /// Presses the Arrow Right key.
    /// </summary>
    protected Task PressArrowRightAsync() => PressKeyAsync("ArrowRight");

    /// <summary>
    /// Presses the Arrow Left key.
    /// </summary>
    protected Task PressArrowLeftAsync() => PressKeyAsync("ArrowLeft");

    /// <summary>
    /// Presses the Home key.
    /// </summary>
    protected Task PressHomeAsync() => PressKeyAsync("Home");

    /// <summary>
    /// Presses the End key.
    /// </summary>
    protected Task PressEndAsync() => PressKeyAsync("End");

    /// <summary>
    /// Types a character on the keyboard.
    /// </summary>
    protected Task TypeCharacterAsync(string character) => Page.Keyboard.TypeAsync(character);

    #endregion

    #region Locator Helpers

    /// <summary>
    /// Gets a locator by test ID.
    /// </summary>
    protected ILocator GetByTestId(string testId) => Page.GetByTestId(testId);

    /// <summary>
    /// Gets a locator by CSS selector.
    /// </summary>
    protected ILocator Locator(string selector) => Page.Locator(selector);

    /// <summary>
    /// Creates Playwright assertions for a locator.
    /// </summary>
    protected static ILocatorAssertions Expect(ILocator locator) => Assertions.Expect(locator);

    #endregion
}