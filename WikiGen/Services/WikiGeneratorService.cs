using Documentation.Components.Services;
using Microsoft.Extensions.Logging;

namespace WikiGen.Services;

/// <summary>
/// Orchestrates wiki markdown generation from the component registry.
/// </summary>
public sealed partial class WikiGeneratorService
{
    private readonly ComponentRegistry registry;
    private readonly WikiMarkdownBuilder builder;
    private readonly ILogger<WikiGeneratorService> logger;

    /// <summary>Initializes the wiki generator service.</summary>
    public WikiGeneratorService(
        ComponentRegistry registry,
        WikiMarkdownBuilder builder,
        ILogger<WikiGeneratorService> logger)
    {
        this.registry = registry;
        this.builder = builder;
        this.logger = logger;
    }

    /// <summary>
    /// Generates the full GitHub Wiki markdown structure into the specified output directory.
    /// Creates <c>Home.md</c>, <c>_Sidebar.md</c>, and one page per component.
    /// </summary>
    /// <param name="path">Absolute path to the output directory (e.g. <c>wiki/</c>).</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    public async Task GenerateAsync(string path, CancellationToken cancellationToken = default)
    {
        var outputPath = Path.GetFullPath(path);
        Directory.CreateDirectory(outputPath);

        LogStartingGeneration(outputPath);

        var groups = registry.Groups;
        var totalComponents = groups.Sum(g => g.Items.Count);
        LogDiscoveredComponents(groups.Count, totalComponents);

        var homeMd = WikiMarkdownBuilder.BuildHomePage();
        await File.WriteAllTextAsync(Path.Combine(outputPath, "Home.md"), homeMd, cancellationToken);
        LogGeneratedFile("Home.md");

        var sidebarMd = WikiMarkdownBuilder.BuildSidebar(groups);
        await File.WriteAllTextAsync(Path.Combine(outputPath, "_Sidebar.md"), sidebarMd, cancellationToken);
        LogGeneratedFile("_Sidebar.md");

        var footerMd = WikiMarkdownBuilder.BuildFooter();
        await File.WriteAllTextAsync(Path.Combine(outputPath, "_Footer.md"), footerMd, cancellationToken);
        LogGeneratedFile("_Footer.md");

        // Copy static markdown pages from wwwroot
        var wwwrootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "wwwroot"));
        if (Directory.Exists(wwwrootPath))
        {
            var staticFiles = Directory.GetFiles(wwwrootPath, "*.md");
            LogFoundStaticPages(staticFiles.Length, wwwrootPath);

            foreach (var file in staticFiles)
            {
                var fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(outputPath, fileName), overwrite: true);
                LogCopiedStaticPage(fileName);
            }
        }
        else
        {
            LogStaticPagesDirectoryNotFound(wwwrootPath);
        }

        // Generate per-component pages
        var generated = 0;
        foreach (var group in groups)
        {
            LogGeneratingGroup(group.Title, group.Items.Count);
            foreach (var component in group.Items)
            {
                var fileName = $"{component.Title.Replace(' ', '_')}.md";
                var content = builder.BuildComponentPage(component, group.Title);
                await File.WriteAllTextAsync(Path.Combine(outputPath, fileName), content, cancellationToken);

                generated++;
                LogGeneratedComponentPage(component.Title, group.Title);
            }
        }

        LogGenerationComplete(generated, totalComponents, outputPath);
    }

    [LoggerMessage(Level = LogLevel.Information, Message = "Starting wiki generation to {OutputPath}")]
    private partial void LogStartingGeneration(string outputPath);

    [LoggerMessage(Level = LogLevel.Information, Message = "Discovered {GroupCount} groups with {ComponentCount} total components")]
    private partial void LogDiscoveredComponents(int groupCount, int componentCount);

    [LoggerMessage(Level = LogLevel.Information, Message = "Generated {FileName}")]
    private partial void LogGeneratedFile(string fileName);

    [LoggerMessage(Level = LogLevel.Information, Message = "Found {Count} static markdown pages in {Path}")]
    private partial void LogFoundStaticPages(int count, string path);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Copied static page {FileName}")]
    private partial void LogCopiedStaticPage(string fileName);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Static pages directory not found: {Path}")]
    private partial void LogStaticPagesDirectoryNotFound(string path);

    [LoggerMessage(Level = LogLevel.Information, Message = "Generating group '{GroupTitle}' with {Count} components")]
    private partial void LogGeneratingGroup(string groupTitle, int count);

    [LoggerMessage(Level = LogLevel.Debug, Message = "Generated component page '{ComponentTitle}' in group '{GroupTitle}'")]
    private partial void LogGeneratedComponentPage(string componentTitle, string groupTitle);

    [LoggerMessage(Level = LogLevel.Information, Message = "Wiki generation complete: {Generated}/{Total} component pages written to {OutputPath}")]
    private partial void LogGenerationComplete(int generated, int total, string outputPath);
}