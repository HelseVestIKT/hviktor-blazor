using Hviktor.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tests.Components;

namespace Tests.Playwright.Fixtures;

public sealed class TestsFixture : PlaywrightFixture
{
    protected override string ProjectFolderName => "Tests";

    protected override string GetApplicationName()
    {
        return typeof(App).Assembly.GetName().Name!;
    }

    protected override void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.AddRazorComponents().AddInteractiveServerComponents();
        builder.Services.AddHviktor();
    }

    protected override void ConfigureApplication(WebApplication app)
    {
        app.UseAntiforgery();
        app.UseHviktor();
        app.MapStaticAssets();
        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
    }
}