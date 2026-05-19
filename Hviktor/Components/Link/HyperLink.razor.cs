using Hviktor.Abstractions.Enums;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Link;

/// <summary>
/// A standard hyperlink component for navigating to URLs.
/// Renders an <c>&lt;a&gt;</c> element with support for external link security attributes,
/// sizing, and color theming.
/// </summary>
/// <remarks>
/// Use <see cref="HyperLink"/> for standard navigation links to internal or external URLs.
/// For links that trigger actions without navigation, use <see cref="ActionLink"/>.
/// For navigation links with active state matching, use <see cref="NavigationLink"/>.
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
public partial class HyperLink : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;

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

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddIdentity(Cryptography.GenerateId())
            .AddClasses("ds-link");

        EnumValue<LinkTarget> target = builder.ConsumeAttribute("target");
        var resolvedTarget = target.EnumValueOrNull;
        if (resolvedTarget is null && target.RawValue is not null)
        {
            resolvedTarget = Enum.TryParse<LinkTarget>(target.RawValue, ignoreCase: true, out var parsed) ? parsed : null;
        }

        if (resolvedTarget is not null)
        {
            builder.AddAttribute("target", resolvedTarget.Value.GetDataAttribute());
        }

        // External links opened in new tabs must include security attributes
        builder.TryGetValue("href", out var hrefObj);
        var href = hrefObj?.ToString();

        if (href is not null && IsAbsoluteLink(href) && resolvedTarget is LinkTarget.NewTab or LinkTarget.Blank)
        {
            builder.AddAttribute("rel", "external noreferrer noopener");
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

    /// <summary>
    /// Determines whether the provided hyperlink is an absolute URL.
    /// </summary>
    /// <param name="href">The URL to verify.</param>
    /// <returns>
    /// <c>true</c> if the URL is absolute and points to a different host than the current application;
    /// otherwise, <c>false</c>.
    /// </returns>
    private bool IsAbsoluteLink(string href)
    {
        if (Uri.TryCreate(href, UriKind.Absolute, out var uri) && uri.Scheme is "http" or "https")
        {
            var currentUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            return uri.Host != currentUri.Host;
        }

        return href.StartsWith("http://") || href.StartsWith("https://");
    }
}