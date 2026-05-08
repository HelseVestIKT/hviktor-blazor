using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Layout;

/// <summary>
/// Main application layout. Shows a persistent sidebar on component pages.
/// </summary>
public partial class MainLayout : LayoutComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>Extracts the component slug from the current URI, or <see langword="null"/>.</summary>
    private string? CurrentSlug
    {
        get
        {
            var uri = new Uri(NavigationManager.Uri);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // components/{slug}/{tab}
            return segments.Length >= 2 && segments[0].Equals("components", StringComparison.OrdinalIgnoreCase)
                ? segments[1]
                : null;
        }
    }

    /// <summary>Extracts the active tab from the current URI, defaulting to <c>"overview"</c>.</summary>
    private string CurrentTab
    {
        get
        {
            var uri = new Uri(NavigationManager.Uri);
            var segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // components/{slug}/{tab}
            return segments.Length >= 3 && segments[0].Equals("components", StringComparison.OrdinalIgnoreCase)
                ? segments[2]
                : "overview";
        }
    }
}