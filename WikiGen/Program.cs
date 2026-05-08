using Documentation.Components.Services;
using WikiGen.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.SetMinimumLevel(LogLevel.Information);
});

// Register a no-op changelog provider (wiki generation does not need live changelog dates)
builder.Services.AddSingleton<IChangelogDateProvider, NoOpChangelogDateProvider>();
builder.Services.AddSingleton<IComponentMetadataService, ComponentMetadataService>();
builder.Services.AddSingleton<IDemoSourceService, FileBasedDemoSourceService>();
builder.Services.AddSingleton<ComponentRegistry>();
builder.Services.AddSingleton<WikiMarkdownBuilder>();
builder.Services.AddSingleton<WikiGeneratorService>();

var host = builder.Build();

var generator = host.Services.GetRequiredService<WikiGeneratorService>();

// Resolve output path: first argument or default to <repo-root>/wiki/
var outputPath = args.Length > 0
    ? args[0]
    : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "Hviktor Wiki"));

await generator.GenerateAsync(outputPath);