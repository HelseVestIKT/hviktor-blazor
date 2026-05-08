using Microsoft.Playwright;
using Tests.Playwright.Fixtures;

namespace Tests.Playwright.Sanity;

[Collection(TestCollections.Prerequisite)]
[Trait(TestCollections.Traits.Collection, TestCollections.Prerequisite)]
public sealed class SanityTests(TestsFixture fixture) : IClassFixture<TestsFixture>, IAsyncLifetime
{
    private IPage page = null!;

    public async ValueTask InitializeAsync()
    {
        page = await fixture.CreatePageAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await page.CloseAsync();
    }

    [Fact]
    public void Fixture_IsInitialized()
    {
        Assert.NotNull(fixture.Playwright);
        Assert.NotNull(fixture.Browser);
        Assert.False(string.IsNullOrEmpty(fixture.BaseUrl));
    }

    [Fact]
    public void Page_IsCreated()
    {
        Assert.NotNull(page);
        Assert.False(page.IsClosed);
    }

    [Fact]
    public async Task Browser_CanNavigate()
    {
        await page.GotoAsync(fixture.BaseUrl);
        var url = page.Url;
        Assert.StartsWith(fixture.BaseUrl, url);
    }

    [Fact]
    public async Task Page_HasDocument()
    {
        await page.GotoAsync(fixture.BaseUrl);
        var documentElement = await page.QuerySelectorAsync("html");
        Assert.NotNull(documentElement);
    }

    [Fact]
    public async Task Page_HasBody()
    {
        await page.GotoAsync(fixture.BaseUrl);
        var body = await page.QuerySelectorAsync("body");
        Assert.NotNull(body);
    }

    [Fact]
    public async Task Page_CanExecuteJavaScript()
    {
        await page.GotoAsync(fixture.BaseUrl);
        var result = await page.EvaluateAsync<int>("() => 1 + 1");
        Assert.Equal(2, result);
    }

    [Fact]
    public async Task Page_TitleIsNotEmpty()
    {
        await page.GotoAsync(fixture.BaseUrl);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var title = await page.TitleAsync();
        Assert.False(string.IsNullOrEmpty(title), "Expected page title to be set, but it was empty or null");
    }
}