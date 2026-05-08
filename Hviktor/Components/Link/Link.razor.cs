using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;

namespace Hviktor.Components.Link;

/// <summary>
/// A general-purpose link component that supports navigation, active state matching, and external link security.
/// </summary>
/// <remarks>
/// This component is obsolete.<br/>
/// Use:
/// <list type="bullet">
///     <item><see cref="HyperLink"/> when you need a standard navigation links.</item>
///     <item><see cref="ActionLink"/> when the link triggers an action rather than navigation.</item>
///     <item><see cref="NavigationLink"/>  when you need navigation with active state management based on the current URL.</item>
/// </list>
/// </remarks>
[Obsolete("Use HyperLink, ActionLink, or NavigationLink instead. This component will be removed in a future version.")]
public partial class Link : LinkBase
{
    [Inject] private ILogger<Link> Logger { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the matching behavior for determining the active state of the link.
    /// </summary>
    [Parameter]
    public NavLinkMatch? Match { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> HandleHrefAttributes(Dictionary<string, object?> attr)
    {
        // Call base implementation for href and security attributes (rel for external links)
        attr = base.HandleHrefAttributes(attr);

        // Add active state handling for navigation
        if (Href is not null && Match.HasValue)
        {
            var relativeUri = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            var matchPrefix = Match == NavLinkMatch.Prefix && relativeUri.StartsWith(Href.TrimStart('/'), StringComparison.OrdinalIgnoreCase);
            var matchAll = Match == NavLinkMatch.All && string.Equals(relativeUri, Href.TrimStart('/'), StringComparison.OrdinalIgnoreCase);
            if (matchPrefix || matchAll)
            {
                attr.AddAttribute("active", true);
                attr.AddAttribute("aria-current", "page");
            }
        }

        return attr;
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        Logger.LogWarning("The Link component is obsolete. Use HyperLink, ActionLink, or NavigationLink instead. This component will be removed in a future version.");
    }
}