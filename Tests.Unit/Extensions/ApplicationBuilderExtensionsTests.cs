using System.Net;
using Hviktor.Accessors.Layout;
using Hviktor.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tests.Unit.Extensions;

/// <summary>
/// Unit tests for <see cref="Hviktor.Extensions.ApplicationBuilderExtensions.UseHviktor"/>.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
public class ApplicationBuilderExtensionsTests : IAsyncLifetime
{
    private IHost? host;
    private HttpClient? httpClient;

    #region Infrastructure

    /// <summary>
    /// Builds a <see cref="TestServer"/> with the given downstream handler and optional component types.
    /// </summary>
    /// <param name="downstreamHandler">Middleware that writes the final response.</param>
    /// <param name="componentTypes">Component types forwarded to <c>UseHviktor</c>.</param>
    private async Task<HttpClient> BuildClientAsync(RequestDelegate downstreamHandler, params Type[] componentTypes)
    {
        host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder.UseTestServer();
                webBuilder.ConfigureServices(services =>
                {
                    services.AddRouting();
                    services.AddHviktor();
                    services.AddLogging();
                });
                webBuilder.Configure(app =>
                {
                    app.UseHviktor(componentTypes);
                    app.Run(downstreamHandler);
                });
            })
            .StartAsync();

        httpClient = host.GetTestClient();
        return httpClient;
    }

    /// <summary>
    /// Returns a <see cref="RequestDelegate"/> that writes a 200 HTML response.
    /// </summary>
    private static RequestDelegate HtmlResponse(string body = "<html><head></head><body></body></html>") =>
        ctx =>
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "text/html; charset=utf-8";
            return ctx.Response.WriteAsync(body);
        };

    #endregion

    private static int CountOccurrences(string source, string value)
    {
        var count = 0;
        var index = 0;
        while ((index = source.IndexOf(value, index, StringComparison.Ordinal)) != -1)
        {
            count++;
            index += value.Length;
        }

        return count;
    }

    #region Entry script injection

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_InjectsEntryScript_BeforeClosingBodyTag()
    {
        var client = await BuildClientAsync(HtmlResponse());

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Contains(
            "<script type=\"module\" src=\"/_content/Hviktor/dist/entry.js\"></script></body>",
            html);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_EntryScriptAppearsExactlyOnce()
    {
        var client = await BuildClientAsync(HtmlResponse());

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(1, CountOccurrences(html, "_content/Hviktor/dist/entry.js"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_EntryScriptAppears_BeforeLastClosingBodyTag()
    {
        var client = await BuildClientAsync(
            HtmlResponse("<html><head></head><body><p>content</p></body></html>"));

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var scriptIndex = html.IndexOf("_content/Hviktor/dist/entry.js", StringComparison.Ordinal);
        var bodyCloseIndex = html.LastIndexOf("</body>", StringComparison.Ordinal);

        Assert.True(scriptIndex >= 0, "Entry script should be present.");
        Assert.True(scriptIndex < bodyCloseIndex,
            "Entry script should be injected before the final </body> tag.");
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_EntryScriptAppearsAfter_ExistingBodyScripts()
    {
        var client = await BuildClientAsync(
            HtmlResponse("<html><head></head><body><script src=\"/app.js\"></script></body></html>"));

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var existingScriptIndex = html.IndexOf("/app.js", StringComparison.Ordinal);
        var entryScriptIndex = html.IndexOf("_content/Hviktor/dist/entry.js", StringComparison.Ordinal);

        Assert.True(entryScriptIndex > existingScriptIndex,
            "The Hviktor entry script should appear after any pre-existing body scripts.");
    }

    #endregion

    #region Hviktor.Icons conditional injection

    /// <summary>
    /// Whether the Icons package assembly is loaded in this test process.
    /// <c>Tests.Unit</c> transitively loads <c>Hviktor.Icons</c> via the Documentation project.
    /// </summary>
    private static readonly bool IconsPackageLoaded = AppDomain.CurrentDomain
        .GetAssemblies()
        .Any(a => a.GetName().Name == "Hviktor.Icons");

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_InjectsIconsScript_WhenIconsAssemblyIsLoaded()
    {
        if (!IconsPackageLoaded)
        {
            return; // Hviktor.Icons not in process — skip
        }

        var client = await BuildClientAsync(HtmlResponse());

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Contains(
            "<script type=\"module\" src=\"/_content/Hviktor.Icons/dist/entry.js\"></script></body>",
            html);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_IconsScriptAppearsAfterEntryScript()
    {
        if (!IconsPackageLoaded)
        {
            return; // Hviktor.Icons not in process — skip
        }

        var client = await BuildClientAsync(HtmlResponse());

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        // Implementation injects entry.js first, then replaces </body> again with icons.js,
        // so the output order is: ...<script entry.js/><script icons.js/></body>
        var entryIndex = html.IndexOf("_content/Hviktor/dist/entry.js", StringComparison.Ordinal);
        var iconsIndex = html.IndexOf("_content/Hviktor.Icons/dist/entry.js", StringComparison.Ordinal);

        Assert.True(entryIndex >= 0, "Entry script should be present.");
        Assert.True(iconsIndex > entryIndex,
            "The Icons entry script should appear after the main Hviktor entry script.");
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_IconsScriptAppearsExactlyOnce_WhenIconsAssemblyIsLoaded()
    {
        if (!IconsPackageLoaded)
        {
            return; // Hviktor.Icons not in process — skip
        }

        var client = await BuildClientAsync(HtmlResponse());

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(1, CountOccurrences(html, "_content/Hviktor.Icons/dist/entry.js"));
    }

    #endregion

    #region Skip logic — path contains a dot (static assets, API files, etc.)

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData("/styles.css")]
    [InlineData("/bundle.js")]
    [InlineData("/favicon.ico")]
    [InlineData("/images/logo.png")]
    [InlineData("/fonts/open-sans.woff2")]
    public async Task UseHviktor_SkipsInjection_WhenPathContainsDot(string path)
    {
        var client = await BuildClientAsync(HtmlResponse());

        var response = await client.GetAsync(path, TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.DoesNotContain("_content/Hviktor/dist/entry.js", html);
    }

    #endregion

    #region Skip logic — internal Blazor/framework paths (start with /_)

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData("/_blazor")]
    [InlineData("/_blazor/negotiate")]
    [InlineData("/_framework")]
    public async Task UseHviktor_SkipsInjection_WhenPathStartsWithUnderscoreSlash(string path)
    {
        var client = await BuildClientAsync(HtmlResponse());

        var response = await client.GetAsync(path, TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.DoesNotContain("_content/Hviktor/dist/entry.js", html);
    }

    #endregion

    #region Skip logic — non-HTML content type

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData("application/json", "{\"ok\":true}")]
    [InlineData("text/plain", "plain text")]
    [InlineData("application/xml", "<root/>")]
    public async Task UseHviktor_SkipsInjection_WhenContentTypeIsNotHtml(
        string contentType, string responseBody)
    {
        RequestDelegate handler = ctx =>
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = contentType;
            return ctx.Response.WriteAsync(responseBody);
        };

        var client = await BuildClientAsync(handler);

        var response = await client.GetAsync("/api/data", TestContext.Current.CancellationToken);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.DoesNotContain("_content/Hviktor", body);
    }

    #endregion

    #region Skip logic — non-2xx status codes

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]
    [InlineData(503)]
    public async Task UseHviktor_SkipsInjection_ForNonSuccessStatusCode(int statusCode)
    {
        RequestDelegate errorHandler = ctx =>
        {
            ctx.Response.StatusCode = statusCode;
            ctx.Response.ContentType = "text/html; charset=utf-8";
            return ctx.Response.WriteAsync("<html><head></head><body>error</body></html>");
        };

        var client = await BuildClientAsync(errorHandler);

        var response = await client.GetAsync("/page", TestContext.Current.CancellationToken);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.DoesNotContain("_content/Hviktor/dist/entry.js", body);
    }

    #endregion

    #region Skip logic — empty response body

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_SkipsInjection_WhenResponseBodyIsEmpty()
    {
        RequestDelegate emptyHandler = ctx =>
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "text/html; charset=utf-8";
            return Task.CompletedTask;
        };

        var client = await BuildClientAsync(emptyHandler);

        var response = await client.GetAsync("/empty", TestContext.Current.CancellationToken);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.DoesNotContain("_content/Hviktor/dist/entry.js", body);
    }

    #endregion

    #region Original response content is preserved

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_PreservesOriginalBodyContent()
    {
        const string originalContent = "Hello, Hviktor!";
        var client = await BuildClientAsync(
            HtmlResponse($"<html><head></head><body>{originalContent}</body></html>"));

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Contains(originalContent, html);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_PreservesOriginalHeadContent()
    {
        const string metaTag = "<meta charset=\"utf-8\">";
        var client = await BuildClientAsync(
            HtmlResponse($"<html><head>{metaTag}</head><body></body></html>"));

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Contains(metaTag, html);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_ReturnsOkStatusCode_ForHtmlResponse()
    {
        var client = await BuildClientAsync(HtmlResponse());

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_PassesThroughOriginalBody_WhenSkippingNonHtmlResponse()
    {
        const string originalBody = "{\"ok\":true}";
        RequestDelegate jsonHandler = ctx =>
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.ContentType = "application/json";
            return ctx.Response.WriteAsync(originalBody);
        };

        var client = await BuildClientAsync(jsonHandler);

        var response = await client.GetAsync("/api/data", TestContext.Current.CancellationToken);
        var body = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(originalBody, body);
    }

    #endregion

    #region Consistent injection across multiple requests

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_InjectsEntryScript_OnEveryHtmlRequest()
    {
        var client = await BuildClientAsync(HtmlResponse());

        for (var i = 0; i < 3; i++)
        {
            var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
            var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            Assert.Contains("_content/Hviktor/dist/entry.js", html);
        }
    }

    #endregion

    #region Component stylesheet injection into <head>

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_InjectsComponentStylesheet_IntoHead_WhenComponentTypeProvided()
    {
        // "ReconnectModal" → split ["Reconnect", "Modal"] → "reconnect-modal"
        var client = await BuildClientAsync(HtmlResponse(), typeof(ReconnectModal));

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Contains(
            "<link rel=\"stylesheet\" href=\"/_content/Hviktor/dist/assets/reconnect-modal.css\">",
            html);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_StylesheetAppears_InsideHeadTag_WhenComponentTypeProvided()
    {
        var client = await BuildClientAsync(HtmlResponse(), typeof(ReconnectModal));

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        var linkIndex = html.IndexOf("reconnect-modal.css", StringComparison.Ordinal);
        var headCloseIndex = html.IndexOf("</head>", StringComparison.Ordinal);

        Assert.True(linkIndex >= 0, "Stylesheet link should be present.");
        Assert.True(linkIndex < headCloseIndex,
            "Stylesheet link should appear inside the <head> element.");
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public async Task UseHviktor_InjectsNoStylesheets_WhenNoComponentTypesProvided()
    {
        var client = await BuildClientAsync(HtmlResponse());

        var response = await client.GetAsync("/", TestContext.Current.CancellationToken);
        var html = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.DoesNotContain(
            "<link rel=\"stylesheet\" href=\"/_content/Hviktor/dist/assets/",
            html);
    }

    #endregion

    #region IAsyncLifetime

    /// <inheritdoc/>
    public ValueTask InitializeAsync() => new();

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        httpClient?.Dispose();
        if (host is not null)
        {
            await host.StopAsync(TestContext.Current.CancellationToken);
            host.Dispose();
        }

        GC.SuppressFinalize(this);
    }

    #endregion
}