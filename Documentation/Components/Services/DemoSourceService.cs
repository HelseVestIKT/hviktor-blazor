using System.Reflection;
using System.Text;

namespace Documentation.Components.Services;

/// <summary>
/// Reads demo source code from embedded resources and caches them for fast lookup.
/// </summary>
public sealed class DemoSourceService : IDemoSourceService
{
    private Dictionary<string, string>? sourceCache;
    private readonly Assembly assembly = typeof(DemoSourceService).Assembly;
    private const string ResourcePrefix = "Documentation.Components.Demos.";

    /// <summary>Ensures all demo sources are loaded from embedded resources.</summary>
    private Dictionary<string, string> EnsureLoaded()
    {
        if (sourceCache is not null)
        {
            return sourceCache;
        }

        sourceCache = new Dictionary<string, string>();
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith(ResourcePrefix) && name.EndsWith(".razor"));

        foreach (var resourceName in resourceNames)
        {
            // Extract component name - handle both flat and nested structures
            // e.g., "...Sections.ButtonDemo.razor" -> "ButtonDemo"
            // e.g., "...Sections.Chip.ChipRadioDemo.razor" -> "Chip.ChipRadioDemo"
            var relativePath = resourceName
                .Replace(ResourcePrefix, "")
                .Replace(".razor", "");

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var reader = new StreamReader(stream, Encoding.UTF8);
                var content = reader.ReadToEnd();

                // Extract only the demo markup (skip @using statements and other directives)
                sourceCache[relativePath] = ExtractDemoMarkup(content);
            }
        }

        return sourceCache;
    }

    private static string ExtractDemoMarkup(string content)
    {
        var lines = content.Split('\n');
        var relevantLines = new List<string>();
        var skipDirectives = true;

        foreach (var line in lines)
        {
            var trimmedLine = line.TrimStart();

            // Skip @using, @inject, @inherits and empty lines at the beginning
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

    /// <inheritdoc />
    public string GetDemoSource(string componentName)
    {
        var cache = EnsureLoaded();
        return cache.TryGetValue(componentName, out var source)
            ? source
            : throw new InvalidOperationException($"No source found for component '{componentName}'.");
    }
}