using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;
using Tests.Playwright.Fixtures;

namespace Tests.Playwright.Composition;

public abstract class CompositionTestBase<TFixture>(TFixture fixture) : IClassFixture<TFixture>, IAsyncLifetime where TFixture : PlaywrightFixture
{
    protected IPage Page = null!;

    #region Lifecycle

    public async ValueTask InitializeAsync()
    {
        Page = await fixture.CreatePageAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await Page.CloseAsync();
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Helper Methods

    internal async Task GoToPageAsync(string url)
    {
        var response = await Page.GotoAsync($"{fixture.BaseUrl}/component/composition/{url}");
        Assert.NotNull(response);
        Assert.True(response.Ok, $"Failed to load page. Status: {response.Status}");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    private async Task<ILocator> GetElementByTestIdAsync(string testId)
    {
        var element = Page.GetByTestId(testId);
        var count = await element.CountAsync();
        Assert.True(count > 0, $"Element with id '{testId}' not found on page.");
        return element;
    }

    internal async Task RunAccessibilityTestAsync<TEnum>(string pagePath, TEnum enumValue, Func<TEnum, string> getTestId,
        AxeRunOptions? axeOptions = null) where TEnum : struct, Enum
    {
        var testId = getTestId(enumValue);

        await GoToPageAsync(pagePath);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var element = await GetElementByTestIdAsync(testId);
        await element.WaitForSelectorStateAsync(WaitForSelectorState.Visible);

        var result = await element.RunAxe(axeOptions);
        Assert.Empty(result.Violations);
    }

    internal async Task RunAccessibilityTestAsync(string pagePath, string testId, AxeRunOptions? axeOptions = null)
    {
        await GoToPageAsync(pagePath);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var element = await GetElementByTestIdAsync(testId);
        await element.WaitForSelectorStateAsync(WaitForSelectorState.Visible);

        var result = await element.RunAxe(axeOptions);
        Assert.Empty(result.Violations);
    }

    #endregion
}