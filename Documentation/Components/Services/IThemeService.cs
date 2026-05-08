namespace Documentation.Components.Services;

/// <summary>
/// Manages the application color scheme (light / dark) and persists the user's preference.
/// </summary>
public interface IThemeService
{
    /// <summary>The currently active color scheme (<c>"light"</c> or <c>"dark"</c>).</summary>
    string CurrentScheme { get; }

    /// <summary>Raised when the global color scheme changes.</summary>
    event Action? OnSchemeChanged;

    /// <summary>
    /// Reads the persisted preference (or OS default) and applies it to the DOM.
    /// Call once on first render.
    /// </summary>
    Task InitializeAsync();

    /// <summary>Toggles between <c>"light"</c> and <c>"dark"</c>, persisting the choice.</summary>
    Task ToggleAsync();

    /// <summary>Sets a specific scheme, persisting the choice.</summary>
    /// <param name="scheme"><c>"light"</c> or <c>"dark"</c>.</param>
    Task SetAsync(string scheme);
}