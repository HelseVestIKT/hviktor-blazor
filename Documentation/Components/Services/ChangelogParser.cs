using System.Globalization;
using System.Text.RegularExpressions;

namespace Documentation.Components.Services;

/// <summary>
/// Represents a single parsed release entry from the changelog.
/// </summary>
/// <param name="Version">The release version string, e.g. <c>1.0.0</c>.</param>
/// <param name="Date">The release date, or <see langword="null"/> for unreleased entries.</param>
/// <param name="Body">The Markdown body of the release notes, excluding the heading line.</param>
public sealed record ChangelogRelease(string Version, DateOnly? Date, string Body);

/// <summary>
/// Parses a git-cliff generated CHANGELOG.md into structured release entries
/// and extracts per-scope last-updated dates.
/// </summary>
public static partial class ChangelogParser
{
    private static readonly Regex ScopePattern = ScopeRegex();

    /// <summary>
    /// Splits raw changelog Markdown into a list of <see cref="ChangelogRelease"/> entries.
    /// </summary>
    /// <param name="markdown">The full changelog Markdown content.</param>
    /// <returns>A list of releases in the order they appear in the file.</returns>
    public static List<ChangelogRelease> ParseReleases(string markdown)
    {
        var matches = ReleaseHeaderRegex().Matches(markdown);
        if (matches.Count == 0)
        {
            return [];
        }

        var releases = new List<ChangelogRelease>(matches.Count);

        for (var i = 0; i < matches.Count; i++)
        {
            var match = matches[i];
            var bodyStart = match.Index + match.Length;
            var bodyEnd = i + 1 < matches.Count ? matches[i + 1].Index : markdown.Length;
            var body = markdown[bodyStart..bodyEnd].Trim();

            var dateGroup = match.Groups["date"];
            DateOnly? date = dateGroup.Success && DateOnly.TryParse(dateGroup.Value, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : null;

            releases.Add(new ChangelogRelease(
                Version: match.Groups["version"].Value,
                Date: date,
                Body: body));
        }

        return releases;
    }

    /// <summary>
    /// Builds a dictionary mapping each changelog scope (normalized to lowercase)
    /// to the most recent release date that mentions it.
    /// </summary>
    /// <param name="releases">Parsed changelog releases.</param>
    /// <returns>A dictionary of scope to last-updated <see cref="DateTime"/>.</returns>
    public static Dictionary<string, DateTime> BuildScopeDateMap(IEnumerable<ChangelogRelease> releases)
    {
        var map = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);

        foreach (var release in releases)
        {
            if (release.Date is not { } date)
            {
                continue;
            }

            var dateTime = date.ToDateTime(TimeOnly.MinValue);

            foreach (Match match in ScopePattern.Matches(release.Body))
            {
                var scope = match.Groups["scope"].Value;
                // Only keep the latest (first encountered) date per scope
                map.TryAdd(scope, dateTime);
            }
        }

        return map;
    }

    /// <summary>
    /// Matches a release header line. Accepts both <c>## [version]</c> and
    /// <c>### [version]</c> because the git-cliff template used by the
    /// changelog action emits H3 entries, while older changelog dumps used H2.
    /// </summary>
    [GeneratedRegex(@"^#{2,3} \[(?<version>[^\]]+)\](?:\([^)]*\))?(?:\s*-\s*(?<date>\d{4}-\d{2}-\d{2}))?\s*$", RegexOptions.Multiline)]
    private static partial Regex ReleaseHeaderRegex();

    /// <summary>Matches <c>*(scope)*</c> in changelog entries.</summary>
    [GeneratedRegex(@"\*\((?<scope>[^)]+)\)\*", RegexOptions.Compiled)]
    private static partial Regex ScopeRegex();
}