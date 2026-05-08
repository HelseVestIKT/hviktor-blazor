using Documentation.Components.Services;

namespace WikiGen.Services;

/// <summary>
/// A no-op changelog date provider for wiki generation.
/// Wiki pages do not require live changelog dates.
/// </summary>
public sealed class NoOpChangelogDateProvider : IChangelogDateProvider
{
    /// <inheritdoc />
    public DateTime? GetLastUpdated(string scope) => null;

    /// <inheritdoc />
    public DateTime GetLastUpdatedOrDefault(string scope, DateTime fallback) => fallback;

    /// <inheritdoc />
    public Task InitializeAsync() => Task.CompletedTask;
}

