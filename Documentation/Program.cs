using Documentation.Components;
using Documentation.Components.Services;
using Hviktor.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress);

var app = builder.Build();
await app.RunAsync();

return;

static void ConfigureServices(IServiceCollection services, string baseAddress)
{
    var httpClient = new HttpClient { BaseAddress = new Uri(baseAddress) };
    var changelogProvider = new ChangelogDateProvider(httpClient);

    services.AddScoped(_ => httpClient);
    services.AddSingleton<IDemoSourceService, DemoSourceService>();
    services.AddSingleton<IComponentMetadataService, ComponentMetadataService>();

    services.AddSingleton<IChangelogDateProvider>(changelogProvider);
    services.AddSingleton(changelogProvider);

    services.AddSingleton<ComponentRegistry>();
    services.AddScoped<ComponentSearchService>();
    services.AddScoped<IThemeService, ThemeService>();

    services.AddHviktor();
}