using Deque.AxeCore.Commons;
using Deque.AxeCore.Playwright;
using Microsoft.Playwright;
using Tests.Playwright.Fixtures;

namespace Tests.Playwright.Compliance;

public abstract class WcagTestBase<TFixture>(TFixture fixture) : IClassFixture<TFixture>, IAsyncLifetime where TFixture : PlaywrightFixture
{
    internal IPage Page { get; private set; } = null!;

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
        var response = await Page.GotoAsync($"{fixture.BaseUrl}/component/compliance{url}");
        Assert.NotNull(response);
        Assert.True(response.Ok, $"Failed to load page. Status: {response.Status}");
    }

    internal async Task<ILocator> GetElementByTestIdAsync(string testId)
    {
        var element = Page.GetByTestId(testId);
        var count = await element.CountAsync();
        Assert.True(count > 0, $"Element with id '{testId}' not found on page.");
        return element;
    }

    internal async Task<AxeResult> RunAccessibilityTestAsync<TEnum>(string pagePath, TEnum enumValue, Func<TEnum, string> getTestId,
        AxeRunOptions? axeOptions = null) where TEnum : struct, Enum
    {
        var testId = getTestId(enumValue);

        await GoToPageAsync(pagePath);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var element = await GetElementByTestIdAsync(testId);
        await element.WaitForSelectorStateAsync(WaitForSelectorState.Visible);

        return await element.RunAxe(axeOptions);
    }

    internal async Task<AxeResult> RunAccessibilityTestAsync(string pagePath, string testId, AxeRunOptions? axeOptions = null)
    {
        await GoToPageAsync(pagePath);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var element = await GetElementByTestIdAsync(testId);
        await element.WaitForSelectorStateAsync(WaitForSelectorState.Visible);

        return await element.RunAxe(axeOptions);
    }

    /// <summary>
    /// Runs axe-core at all standard WCAG levels (A, AA, AAA, BestPractices) and asserts no violations.
    /// Use this to collapse 4 identical test methods into a single call.
    /// </summary>
    internal async Task AssertAllWcagLevelsAsync<TEnum>(string pagePath, TEnum enumValue, Func<TEnum, string> getTestId, params string[]? skip)
        where TEnum : struct, Enum
    {
        var testId = getTestId(enumValue);
        await AssertAllWcagLevelsAsync(pagePath, testId, skip);
    }

    /// <summary>
    /// Runs axe-core at all standard WCAG levels (A, AA, AAA, BestPractices) and asserts no violations.
    /// </summary>
    internal async Task AssertAllWcagLevelsAsync(string pagePath, string testId, params string[]? skip)
    {
        await GoToPageAsync(pagePath);
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var element = await GetElementByTestIdAsync(testId);
        await element.WaitForSelectorStateAsync(WaitForSelectorState.Visible);

        var axeOptions = new[]
            {
                "wcag2a",
                "wcag2aa",
                "wcag2aaa",
                TestCollections.Tags.BestPractice
            }.Where(tag => skip == null || !skip.Contains(tag))
            .ToList();
        var levels = new[]
        {
            new AxeRunOptions
            {
                RunOnly = new RunOnlyOptions
                {
                    Type = "tag",
                    Values = axeOptions
                }
            }
        };

        foreach (var level in levels)
        {
            var result = await element.RunAxe(level);
            Assert.Empty(result.Violations);
        }
    }

    #endregion
}