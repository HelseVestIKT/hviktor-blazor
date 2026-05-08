using Microsoft.AspNetCore.Components;

namespace Documentation.Components.Layout;

/// <summary>
/// Right-side page outline navigation showing anchor links to sections on the current page.
/// Only renders when there are entries to display.
/// </summary>
public sealed partial class OnThisPage : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>The outline entries to display as anchor links.</summary>
    [Parameter]
    public IReadOnlyList<OnThisPageEntry> Entries { get; set; } = [];

    /// <summary>The currently active fragment (without <c>#</c>).</summary>
    private string? ActiveFragment { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        // Sync from the URL fragment, or default to the first entry (top of page).
        ActiveFragment = GetFragment(NavigationManager.Uri)
                         ?? (Entries.Count > 0 ? Entries[0].Id : null);
    }

    /// <summary>Sets the active fragment on click. The browser handles scrolling natively.</summary>
    private void SetActive(string id)
    {
        ActiveFragment = id;
    }

    /// <summary>Gets the current page URI without any fragment, for use as the anchor link base.</summary>
    private string CurrentPathWithoutFragment
    {
        get
        {
            var uri = NavigationManager.Uri;
            var fragmentIndex = uri.IndexOf('#', StringComparison.Ordinal);
            return fragmentIndex >= 0 ? uri[..fragmentIndex] : uri;
        }
    }

    /// <summary>Gets the component slug title from the URL path (e.g., "Alert" from "/components/alert/code").</summary>
    private string FeedbackTitle
    {
        get
        {
            var relativePath = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var segments = relativePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            // Expect "components/{slug}/..." pattern; use the second segment as the title.
            if (segments.Length >= 2)
            {
                var slug = segments[1];
                return string.Concat(slug.Split('-').Select(s => System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(s)));
            }

            return relativePath;
        }
    }

    /// <summary>Extracts the fragment from a URI string, or <see langword="null"/> if none.</summary>
    private static string? GetFragment(string uri)
    {
        var fragmentIndex = uri.IndexOf('#', StringComparison.Ordinal);
        return fragmentIndex >= 0 ? uri[(fragmentIndex + 1)..] : null;
    }

    /// <summary>Represents a single entry in the on-this-page outline navigation.</summary>
    /// <param name="Id">The HTML <c>id</c> attribute value of the target section (without <c>#</c>).</param>
    /// <param name="Label">The display label shown in the navigation link.</param>
    /// <param name="NestLevel">Nesting depth for indentation (0 = top-level, 1+ = indented).</param>
    /// <remarks>The <c>NestLevel</c> should never exceed 3.</remarks>
    public sealed record OnThisPageEntry(string Id, string Label, int NestLevel = 0);
}