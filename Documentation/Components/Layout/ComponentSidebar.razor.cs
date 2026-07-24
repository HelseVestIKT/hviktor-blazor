using Documentation.Components.Services;
using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Layout;

/// <summary>
/// Sidebar navigation listing all documented Hviktor components, with an inline search filter.
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

    /// <summary>Current sidebar search filter text.</summary>
    private string filterText = string.Empty;

    /// <summary>Cached filtered groups, invalidated whenever <see cref="filterText"/> changes.</summary>
    private IReadOnlyList<ComponentGroup>? cachedFilteredGroups;

    /// <summary>Tracks whether a re-render is needed.</summary>
    private bool shouldRender = true;

    /// <summary>
    /// Returns the component groups filtered by <see cref="filterText"/>.
    /// The result is cached and only recomputed when the filter changes.
    /// </summary>
    private IReadOnlyList<ComponentGroup> FilteredGroups => cachedFilteredGroups ??= ComputeFilteredGroups();

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
        if (!shouldRender)
        {
            return false;
        }

        shouldRender = false;
        return true;
    }

    /// <summary>Sets the active slug on click.</summary>
    private void SetActive(string slug)
    {
        ActiveSlug = slug;
        shouldRender = true;
    }

    /// <summary>Handles search input changes and filters sidebar items.</summary>
    private void OnSearchInput(ChangeEventArgs e)
    {
        var value = e.Value?.ToString() ?? string.Empty;
        if (string.Equals(filterText, value, StringComparison.Ordinal))
        {
            return;
        }

        filterText = value;
        cachedFilteredGroups = null;
        shouldRender = true;
    }

    /// <summary>Computes the groups and items visible under the current filter.</summary>
    private IReadOnlyList<ComponentGroup> ComputeFilteredGroups()
    {
        var groups = Registry.Groups;

        if (string.IsNullOrWhiteSpace(filterText))
        {
            return groups;
        }

        var result = new List<ComponentGroup>(groups.Count);

        foreach (var group in groups)
        {
            var items = group.Items
                .Where(c => c.Title.Contains(filterText, StringComparison.OrdinalIgnoreCase)
                            || c.Slug.Contains(filterText, StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (items.Length > 0)
            {
                result.Add(new ComponentGroup(group.Title, items));
            }
        }

        return result;
    }

    /// <summary>Clears the current search filter.</summary>
    private void ClearSearch()
    {
        filterText = string.Empty;
        cachedFilteredGroups = null;
        shouldRender = true;
    }
}