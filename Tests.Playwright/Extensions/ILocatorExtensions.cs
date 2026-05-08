// ReSharper disable once CheckNamespace

namespace Microsoft.Playwright;

public static class ILocatorExtensions
{
    extension(ILocator locator)
    {
        public async Task WaitForSelectorStateAsync(WaitForSelectorState state, int timeout = 5000)
        {
            await locator.WaitForAsync(new LocatorWaitForOptions
            {
                State = state,
                Timeout = timeout
            });
        }
    }
}