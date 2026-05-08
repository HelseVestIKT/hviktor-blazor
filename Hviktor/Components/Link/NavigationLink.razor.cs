using Hviktor.Abstractions.Enums;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;

namespace Hviktor.Components.Link;

/// <summary>
/// A navigation link component with active state matching.
/// Renders an <c>&lt;a&gt;</c> element that automatically applies <c>active</c> and
/// <c>aria-current="page"</c> attributes when the current URL matches the link's href.
/// </summary>
/// <remarks>
/// Use <see cref="NavigationLink"/> for navigation menus and sidebars where active state indication is needed.
/// For standard navigation links without active state, use <see cref="HyperLink"/>.
/// For links that trigger actions without navigation, use <see cref="ActionLink"/>.
/// </remarks>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Attribute</term>
///     <description>Description</description>
///   </listheader>
///   <item>
///     <term>
///       <b>href</b>: <see cref="string"/><br/>
///       <i>(required)</i>
///     </term>
///     <description>
///       <b>Description</b>: The URL of the link.<br/>
///                     The component will handle relative paths and convert them to absolute URLs based on the current navigation context.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>target</b>: <see cref="LinkTarget"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="LinkTarget.Self"/> | <see cref="LinkTarget.NewTab"/> | <see cref="LinkTarget.Blank"/><br/>
///       <b>Description</b>: Specifies where to open the linked document. If set to <see cref="LinkTarget.NewTab"/> or <see cref="LinkTarget.Blank"/>,
///                     the link will open in a new tab and include appropriate security attributes for external links.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///       <b>Description</b>: Size of the link text.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Color of the link text.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class NavigationLink : ComponentBase, IDisposable
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ILogger<NavigationLink> Logger { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the matching behavior for determining the active state of the link.
    /// When set to <see cref="NavLinkMatch.All"/>, the link is active only on an exact URL match.
    /// When set to <see cref="NavLinkMatch.Prefix"/>, the link is active when the URL starts with the href.
    /// </summary>
    [Parameter]
    public NavLinkMatch Match { get; set; } = NavLinkMatch.Prefix;

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

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        InvokeAsync(StateHasChanged);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
        GC.SuppressFinalize(this);
    }

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddIdentity(Cryptography.GenerateId())
            .AddClasses("ds-link");

        var resolvedTarget = ResolveTarget(builder);
        if (resolvedTarget is not null)
        {
            builder.AddAttribute("target", resolvedTarget.Value.GetDataAttribute());
        }

        // External links opened in new tabs must include security attributes
        builder.TryGetValue("href", out var hrefObj);
        var href = HandleRelativeHrefPath(hrefObj?.ToString());
        var isExternal = IsExternalLink(href);

        if (href is not null && isExternal && resolvedTarget is LinkTarget.NewTab or LinkTarget.Blank)
        {
            builder.AddAttribute("rel", "external noreferrer noopener");
        }

        if (href is not null && isExternal)
        {
            Logger.LogWarning("NavigationLink received an external href '{Href}'. Active state matching is not supported for external URLs. Use HyperLink for external links.", href);
        }

        // Active state handling for navigation (only for internal links)
        if (href is not null && !isExternal)
        {
            ApplyActiveState(builder, hrefObj?.ToString() ?? href);
        }

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        if (!color.IsEmpty)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(color));
        }

        return builder;
    }

    private static LinkTarget? ResolveTarget(Dictionary<string, object?> builder)
    {
        EnumValue<LinkTarget> target = builder.ConsumeAttribute("target");
        var resolvedTarget = target.EnumValueOrNull;
        if (resolvedTarget is null && target.RawValue is not null)
        {
            resolvedTarget = Enum.TryParse<LinkTarget>(target.RawValue, ignoreCase: true, out var parsed) ? parsed : null;
        }

        return resolvedTarget;
    }

    private void ApplyActiveState(Dictionary<string, object?> builder, string href)
    {
        var currentRelative = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        var hrefAbsolute = NavigationManager.ToAbsoluteUri(href).ToString();
        var hrefRelative = NavigationManager.ToBaseRelativePath(hrefAbsolute);

        var isActive = Match switch
        {
            NavLinkMatch.Prefix => hrefRelative.Length == 0 || currentRelative.StartsWith(hrefRelative, StringComparison.OrdinalIgnoreCase),
            NavLinkMatch.All => string.Equals(currentRelative, hrefRelative, StringComparison.OrdinalIgnoreCase),
            _ => false
        };

        if (isActive)
        {
            builder.AddAttribute("active", true);
            builder.AddAttribute("aria-current", "page");
        }
    }

    /// <summary>
    /// Determines whether the specified URL points to an external host.
    /// </summary>
    private bool IsExternalLink(string? href)
    {
        if (href is null)
        {
            return false;
        }

        if (Uri.TryCreate(href, UriKind.Absolute, out var uri) && uri.Scheme is "http" or "https")
        {
            var currentUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            return uri.Host != currentUri.Host;
        }

        return href.StartsWith("http://") || href.StartsWith("https://");
    }

    private string? HandleRelativeHrefPath(string? href)
    {
        if (href is null)
        {
            return null;
        }

        if (Uri.TryCreate(href, UriKind.Absolute, out var absoluteUri) && absoluteUri.Scheme is "http" or "https")
        {
            return href;
        }

        if (href.StartsWith('#'))
        {
            var currentUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var baseUri = currentUri.GetLeftPart(UriPartial.Query);
            return baseUri + href;
        }

        try
        {
            return NavigationManager.ToAbsoluteUri(href).ToString();
        }
        catch (UriFormatException ex)
        {
            Logger.LogError(ex, "Failed to resolve href '{Href}' to an absolute URI. Ensure that the href is a valid relative or absolute URL.", href);
            return href;
        }
    }
}