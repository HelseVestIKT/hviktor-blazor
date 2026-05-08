using System.Net.Sockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;

namespace Tests.Playwright.Fixtures;

public abstract class PlaywrightFixture : IAsyncLifetime
{
    internal IPlaywright Playwright { get; private set; } = null!;
    internal IBrowser Browser { get; private set; } = null!;
    public string BaseUrl { get; private set; } = null!;

    private IHost host = null!;

    /// <summary>
    /// The relative path from the test output directory to the project folder.
    /// Example: "Hviktor.Samples" or "Documentation"
    /// </summary>
    protected abstract string ProjectFolderName { get; }

    /// <summary>
    /// The subfolder containing wwwroot, relative to the project folder.
    /// Example: "Hviktor.Samples" for nested structure, or empty string if wwwroot is directly in project folder.
    /// </summary>
    protected virtual string WwwRootSubFolder => string.Empty;

    /// <summary>
    /// Configure services for the test host.
    /// </summary>
    protected abstract void ConfigureServices(WebApplicationBuilder builder);

    /// <summary>
    /// Configure the application pipeline for the test host.
    /// </summary>
    protected abstract void ConfigureApplication(WebApplication app);

    /// <summary>
    /// Get the application name for the test host.
    /// </summary>
    protected abstract string GetApplicationName();

    public async Task<IPage> CreatePageAsync()
    {
        return await Browser.NewPageAsync();
    }

    public async ValueTask InitializeAsync()
    {
        var port = GetAvailablePort();
        BaseUrl = $"http://localhost:{port}";

        var projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../..", ProjectFolderName));
        var wwwrootPath = string.IsNullOrEmpty(WwwRootSubFolder)
            ? Path.Combine(projectPath, "wwwroot")
            : Path.Combine(projectPath, WwwRootSubFolder, "wwwroot");

        if (!Directory.Exists(projectPath))
        {
            throw new DirectoryNotFoundException($"Project path not found: {projectPath}");
        }

        if (!Directory.Exists(wwwrootPath))
        {
            throw new DirectoryNotFoundException($"wwwroot not found in: {wwwrootPath}");
        }

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ContentRootPath = projectPath,
            WebRootPath = wwwrootPath,
            ApplicationName = GetApplicationName()
        });

        StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

        builder.WebHost.UseUrls(BaseUrl);
        builder.WebHost.UseWebRoot(wwwrootPath);

        ConfigureServices(builder);

        var app = builder.Build();

        ConfigureApplication(app);

        host = app;
        await host.StartAsync();

        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        var settings = TestConfig.Playwright.Settings;
        var browserType = settings.Browser switch
        {
            "firefox" => Playwright.Firefox,
            "webkit" => Playwright.Webkit,
            _ => Playwright.Chromium
        };

        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = settings.Headless,
            SlowMo = settings.SlowMo,
            Timeout = settings.Timeout
        };

        // Only set channel for Chromium
        if (browserType == Playwright.Chromium && !string.IsNullOrEmpty(settings.Channel))
        {
            launchOptions.Channel = settings.Channel;
        }

        Browser = await browserType.LaunchAsync(launchOptions);
    }

    public async ValueTask DisposeAsync()
    {
        await Browser.DisposeAsync();
        Playwright.Dispose();
        await host.StopAsync();
        host.Dispose();

        GC.SuppressFinalize(this);
    }

    private static int GetAvailablePort()
    {
        using var listener = new TcpListener(System.Net.IPAddress.Loopback, 0);
        listener.Start();
        var port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
        listener.Stop();
        return port;
    }
}