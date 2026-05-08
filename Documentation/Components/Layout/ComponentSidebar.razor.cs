using Documentation.Components.Services;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Layout;

/// <summary>
/// Sidebar navigation listing all documented Hviktor components, filtered by the shared search service.
/// </summary>
public partial class ComponentSidebar : ComponentBase
{
    [Inject] private ComponentRegistry Registry { get; set; } = null!;

    /// <summary>The currently active component slug, used to highlight the active item.</summary>
    [Parameter]
    public string? CurrentSlug { get; set; }

    /// <summary>The currently active tab, used to preserve tab selection when navigating between components.</summary>
    [Parameter]
    public string CurrentTab { get; set; } = "overview";

    /// <summary>Locally tracked active slug, updated immediately on click.</summary>
    private string? ActiveSlug { get; set; }

    /// <summary>Tracks whether a re-render is needed.</summary>
    private bool shouldRender = true;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        if (!string.Equals(ActiveSlug, CurrentSlug, StringComparison.OrdinalIgnoreCase))
        {
            ActiveSlug = CurrentSlug;
            shouldRender = true;
        }
    }

    /// <inheritdoc />
    protected override bool ShouldRender()
    {
        if (!shouldRender) return false;
        shouldRender = false;
        return true;
    }

    /// <summary>Sets the active slug on click.</summary>
    private void SetActive(string slug)
    {
        ActiveSlug = slug;
        shouldRender = true;
    }
}