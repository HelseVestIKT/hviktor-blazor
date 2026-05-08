using System.Globalization;
using Hviktor.Abstractions.Interfaces.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.JSInterop;

namespace Hviktor.Services;

/// <inheritdoc cref="ILocalizationService"/>
public sealed class LocalizationService(IJSRuntime jsRuntime) : ILocalizationService
{
    /// <inheritdoc cref="ILocalizationService.SetLanguageAsync"/>
    public async Task SetLanguageAsync(string culture)
    {
        await jsRuntime.InvokeVoidAsync(
            "globalThis.Hviktor.Storage.setCookie",
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture))
        );
    }

    /// <inheritdoc cref="ILocalizationService.GetLanguage"/>
    public string GetLanguage() => CultureInfo.CurrentCulture.Name;

    /// <inheritdoc cref="ILocalizationService.GetLanguageAsync"/>
    public async Task<string> GetLanguageAsync()
    {
        try
        {
            var cookieValue = await jsRuntime.InvokeAsync<string>("globalThis.Hviktor.Storage.getCookie", CookieRequestCultureProvider.DefaultCookieName);
            if (string.IsNullOrEmpty(cookieValue))
            {
                return CultureInfo.CurrentCulture.Name;
            }

            // Parse the cookie value which is in format c=en|uic=en
            var cultureParts = CookieRequestCultureProvider.ParseCookieValue(cookieValue);
            var result = cultureParts?.Cultures.FirstOrDefault().Value ?? CultureInfo.CurrentCulture.Name;
            return result;
        }
        catch (Exception ex) when (ex is JSException or ArgumentException)
        {
            // Fallback to current culture if JS error or argument error occurs
            return CultureInfo.CurrentCulture.Name;
        }
    }
}