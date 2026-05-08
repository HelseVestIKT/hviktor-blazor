using System.Text;
using Documentation.Components.Services;

namespace WikiGen.Services;

/// <summary>
/// Reads demo source code from the file system instead of embedded resources.
/// Used by the wiki generator which cannot embed the same files used by the Razor compiler.
/// </summary>
public sealed class FileBasedDemoSourceService : IDemoSourceService
{
    private readonly Dictionary<string, string> sourceCache = new(StringComparer.Ordinal);

    /// <summary>Initializes the service by scanning the demo directory on disk.</summary>
    public FileBasedDemoSourceService()
    {
        var demosDir = FindDemosDirectory();
        if (demosDir is not null)
        {
            LoadAllSources(demosDir);
        }
    }

    /// <inheritdoc />
    public string GetDemoSource(string componentName)
    {
        return sourceCache.TryGetValue(componentName, out var source)
            ? source
            : throw new InvalidOperationException($"No source found for component '{componentName}'.");
    }

    /// <summary>Walks up from the output directory to find the Documentation/Components/Demos folder.</summary>
    private static string? FindDemosDirectory()
    {
        var baseDir = AppContext.BaseDirectory;
        var current = new DirectoryInfo(baseDir);

        // Walk up to find the repo root (contains Documentation/Components/Demos)
        while (current is not null)
        {
            var candidatePath = Path.Combine(current.FullName, "Documentation", "Components", "Demos");
            if (Directory.Exists(candidatePath))
            {
                return candidatePath;
            }

            current = current.Parent;
        }

        return null;
    }

    /// <summary>Loads all .razor demo files from the specified directory.</summary>
    private void LoadAllSources(string demosDir)
    {
        var razorFiles = Directory.GetFiles(demosDir, "*.razor", SearchOption.AllDirectories);

        foreach (var filePath in razorFiles)
        {
            // Build the relative key matching the embedded resource convention
            var relativePath = Path.GetRelativePath(demosDir, filePath)
                .Replace(Path.DirectorySeparatorChar, '.')
                .Replace(Path.AltDirectorySeparatorChar, '.');

            // Strip the .razor extension to match ResourceKey format
            if (relativePath.EndsWith(".razor", StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath[..^".razor".Length];
            }

            var content = File.ReadAllText(filePath, Encoding.UTF8);
            var processed = ExtractDemoMarkup(content);
            sourceCache[relativePath] = processed;
        }
    }

    /// <summary>Extracts demo markup, skipping directive lines at the top of the file.</summary>
    private static string ExtractDemoMarkup(string content)
    {
        var lines = content.Split('\n');
        var relevantLines = new List<string>();
        var skipDirectives = true;

        foreach (var line in lines)
        {
            var trimmedLine = line.TrimStart();

            if (skipDirectives)
            {
                if (trimmedLine.StartsWith("@using") ||
                    trimmedLine.StartsWith("@inject") ||
                    trimmedLine.StartsWith("@inherits") ||
                    string.IsNullOrWhiteSpace(trimmedLine))
                {
                    continue;
                }

                skipDirectives = false;
            }

            relevantLines.Add(line);
        }

        return string.Join("\n", relevantLines).Trim();
    }
}

