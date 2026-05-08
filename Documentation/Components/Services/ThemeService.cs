using Microsoft.JSInterop;

namespace Documentation.Components.Services;

/// <summary>
/// Manages the <c>data-color-scheme</c> attribute on <c>&lt;html&gt;</c> and persists
/// the user's preference to <c>localStorage</c> for instant restoration on next visit.
/// </summary>
public sealed class ThemeService : IThemeService
{
    private const string StorageKey = "color-scheme";
    private const string Light = "light";
    private const string Dark = "dark";

    private readonly IJSRuntime jsRuntime;
    private string currentScheme = Light;

    /// <summary>Initializes a new instance of the <see cref="ThemeService"/> class.</summary>
    public ThemeService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    /// <inheritdoc/>
    public string CurrentScheme => currentScheme;

    /// <inheritdoc/>
    public event Action? OnSchemeChanged;

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        try
        {
            var stored = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);

            if (stored is Light or Dark)
            {
                currentScheme = stored;
            }
            else
            {
                // Fall back to OS preference
                var prefersDark = await jsRuntime.InvokeAsync<bool>("eval", "window.matchMedia('(prefers-color-scheme: dark)').matches");
                currentScheme = prefersDark ? Dark : Light;
            }

            await ApplyAsync();
        }
        catch (InvalidOperationException)
        {
            // JS interop not available during prerendering, ignored.
        }
    }

    /// <inheritdoc/>
    public async Task ToggleAsync()
    {
        currentScheme = currentScheme == Dark ? Light : Dark;
        await ApplyAsync();
    }

    /// <inheritdoc/>
    public async Task SetAsync(string scheme)
    {
        if (scheme is not (Light or Dark))
        {
            return;
        }

        currentScheme = scheme;
        await ApplyAsync();
    }

    /// <summary>Applies the current scheme to the DOM and persists it.</summary>
    private async Task ApplyAsync()
    {
        try
        {
            await jsRuntime.InvokeVoidAsync(
                "eval",
                $"document.documentElement.setAttribute('data-color-scheme','{currentScheme}')");
            await jsRuntime.InvokeVoidAsync(
                "localStorage.setItem", StorageKey, currentScheme);
        }
        catch (InvalidOperationException)
        {
            // JS interop not available, ignored.
        }

        OnSchemeChanged?.Invoke();
    }
}