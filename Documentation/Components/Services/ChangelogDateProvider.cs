namespace Documentation.Components.Services;

/// <summary>
/// Provides per-component last-updated dates extracted from the changelog.
/// </summary>
public interface IChangelogDateProvider
{
    /// <summary>
    /// Gets the last-updated date for a component scope, or <see langword="null"/> if not found.
    /// </summary>
    /// <param name="scope">The changelog scope (e.g. "toggle-group", "select").</param>
    /// <returns>The most recent release date mentioning this scope, or <see langword="null"/>.</returns>
    DateTime? GetLastUpdated(string scope);

    /// <summary>
    /// Gets the last-updated date for a component scope, falling back to the provided default.
    /// </summary>
    /// <param name="scope">The changelog scope (e.g. "toggle-group", "select").</param>
    /// <param name="fallback">The fallback date if the scope is not found in the changelog.</param>
    /// <returns>The most recent release date mentioning this scope, or <paramref name="fallback"/>.</returns>
    DateTime GetLastUpdatedOrDefault(string scope, DateTime fallback);

    /// <summary>
    /// Initializes the provider by fetching and parsing the changelog.
    /// </summary>
    Task InitializeAsync();
}

/// <summary>
/// Fetches <c>changelog.md</c> and builds a scope-to-date lookup from the parsed entries.
/// </summary>
public sealed class ChangelogDateProvider(HttpClient http) : IChangelogDateProvider
{
    private Dictionary<string, DateTime> scopeDates = new(StringComparer.OrdinalIgnoreCase);

    /// <inheritdoc />
    public DateTime? GetLastUpdated(string scope) => scopeDates.GetValueOrDefault(scope);

    /// <inheritdoc />
    public DateTime GetLastUpdatedOrDefault(string scope, DateTime fallback) => scopeDates.GetValueOrDefault(scope, fallback);

    /// <inheritdoc />
    public async Task InitializeAsync()
    {
        try
        {
            var markdown = await http.GetStringAsync("changelog.md");
            if (string.IsNullOrWhiteSpace(markdown))
            {
                return;
            }

            markdown = markdown.TrimStart('\uFEFF');

            var releases = ChangelogParser.ParseReleases(markdown);
            scopeDates = ChangelogParser.BuildScopeDateMap(releases);
        }
        catch (HttpRequestException)
        {
            // Silently fall back to empty map; components will use their hardcoded defaults.
        }
    }
}