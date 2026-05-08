using System.Globalization;
using Bunit;
using Hviktor.Extensions.Localization;
using Microsoft.AspNetCore.Localization;
using Microsoft.JSInterop;

namespace Tests.Unit.Extensions.Localization;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class LocalizationExtensionsTests : BunitContext
{
    private readonly LocalizationExtensions sut;

    public LocalizationExtensionsTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        sut = new LocalizationExtensions(JSInterop.JSRuntime);
    }

    #region SetLanguageAsync

    [Fact]
    public async Task SetLanguageAsync_InvokesJsWithCorrectArguments()
    {
        const string culture = "nb-NO";

        await sut.SetLanguageAsync(culture);
        JSInterop.VerifyInvoke("globalThis.Hviktor.Storage.setCookie", 1);
    }

    [Fact]
    public async Task SetLanguageAsync_WithEnglishCulture_InvokesJs()
    {
        const string culture = "en-US";
        await sut.SetLanguageAsync(culture);

        JSInterop.VerifyInvoke("globalThis.Hviktor.Storage.setCookie", 1);
    }

    #endregion

    #region GetLanguage

    [Fact]
    public void GetLanguage_ReturnsCurrentCultureName()
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("nb-NO");
            var result = sut.GetLanguage();
            Assert.Equal("nb-NO", result);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public void GetLanguage_WithEnglishCulture_ReturnsEnglish()
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            var result = sut.GetLanguage();
            Assert.Equal("en-US", result);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    #endregion

    #region GetLanguageAsync

    [Fact]
    public async Task GetLanguageAsync_WithValidCookie_ReturnsCultureFromCookie()
    {
        var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("nn-NO"));

        JSInterop.Setup<string>("globalThis.Hviktor.Storage.getCookie",
                CookieRequestCultureProvider.DefaultCookieName)
            .SetResult(cookieValue);

        var result = await sut.GetLanguageAsync();
        Assert.Equal("nn-NO", result);
    }

    [Fact]
    public async Task GetLanguageAsync_WithEmptyCookie_ReturnsCurrentCulture()
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("nb-NO");

            JSInterop.Setup<string>("globalThis.Hviktor.Storage.getCookie",
                    CookieRequestCultureProvider.DefaultCookieName)
                .SetResult(string.Empty);

            var result = await sut.GetLanguageAsync();
            Assert.Equal("nb-NO", result);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public async Task GetLanguageAsync_WithNullCookie_ReturnsCurrentCulture()
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            JSInterop.Setup<string>("globalThis.Hviktor.Storage.getCookie",
                    CookieRequestCultureProvider.DefaultCookieName)
                .SetResult(null!);

            var result = await sut.GetLanguageAsync();
            Assert.Equal("en-US", result);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    [Fact]
    public async Task GetLanguageAsync_WhenJsThrows_ReturnsCurrentCulture()
    {
        var originalCulture = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = new CultureInfo("nb-NO");

            JSInterop.Setup<string>("globalThis.Hviktor.Storage.getCookie",
                    CookieRequestCultureProvider.DefaultCookieName)
                .SetException(new JSException("JS error"));

            var result = await sut.GetLanguageAsync();
            Assert.Equal("nb-NO", result);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
        }
    }

    #endregion
}