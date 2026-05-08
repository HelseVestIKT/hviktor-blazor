using Hviktor.Abstractions.Enums;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Link;

/// <summary>
/// The abstract base class for link components, designed to provide a foundation
/// for extending and customizing link behaviors in Blazor applications.
/// </summary>
/// <remarks>
/// This class offers functionality for managing attributes, handling link states,
/// and processing additional configurations like sizes and colors for links.
/// </remarks>
public abstract class LinkBase : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier for the link element.
    /// When not provided, a cryptographically random identifier is generated.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the URL that the link points to.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the target behavior of the link, determining how it opens.
    /// </summary>
    [Parameter]
    public LinkTarget? Target { get; set; }

    /// <summary>
    /// Gets or sets the size of the link.
    /// </summary>
    [Parameter]
    public Size? Size { get; set; }

    /// <summary>
    /// Gets or sets the color theme of the link.
    /// </summary>
    [Parameter]
    public Color? Color { get; set; }

    /// <summary>
    /// Gets or sets the content to be rendered within the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Computes a dictionary of attributes for the component by transforming and processing
    /// parameters, adding identity attributes, data attributes based on
    /// enum values (e.g., size, color), and handling specific state transformations such
    /// as the active state of href tags.
    /// </summary>
    /// <returns>A dictionary containing the computed attributes for the component.</returns>
    protected Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddIdentity(Id ?? Cryptography.GenerateId())
            .AddClasses("ds-link");

        // Apply href and security attributes
        if (Href is not null)
        {
            builder.AddAttribute("href", Href);
        }

        builder = HandleHrefAttributes(builder);

        // Apply target
        if (Target.HasValue)
        {
            builder.AddAttribute("target", Target.Value.GetDataAttribute());
        }

        // Apply size
        if (Size.HasValue)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(Size.Value));
        }

        // Apply color
        if (Color.HasValue)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(Color.Value));
        }

        return builder;
    }

    /// <summary>
    /// Handles href-related attribute transformations, including adding
    /// security attributes for external links opened in new tabs.
    /// </summary>
    /// <param name="attr">The current attribute dictionary to transform.</param>
    /// <returns>The transformed attribute dictionary with any additional security configurations.</returns>
    protected virtual Dictionary<string, object?> HandleHrefAttributes(Dictionary<string, object?> attr)
    {
        if (Href is null)
        {
            return attr;
        }

        if (IsExternalLink(Href) && Target is LinkTarget.NewTab or LinkTarget.Blank)
        {
            attr.AddAttribute("rel", "external noreferrer noopener");
        }

        return attr;
    }

    /// <summary>
    /// Determines whether the specified URL points to an external host.
    /// </summary>
    /// <param name="href">The URL to check.</param>
    /// <returns><see langword="true"/> if the URL is external; otherwise, <see langword="false"/>.</returns>
    protected bool IsExternalLink(string href)
    {
        if (Uri.TryCreate(href, UriKind.Absolute, out var uri))
        {
            var currentUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            return uri.Host != currentUri.Host;
        }

        return href.StartsWith("http://") || href.StartsWith("https://");
    }
}