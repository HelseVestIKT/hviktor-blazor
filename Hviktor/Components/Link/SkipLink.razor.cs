using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Link;

/// <summary>
/// <c>SkipLink</c> helps people using keyboard navigation to navigate, so they can easily go to the main content on a page.
/// </summary>
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
///                 The component will handle relative paths and convert them to absolute URLs based on the current navigation context.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <use>
/// Use <c>SkipLink</c> when:
/// <list type="bullet">
///   <item>There are several tab stops before reaching the main content</item>
///   <item>Users need a way to skip repeated elements such as menus, links, or headers</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>SkipLink</c> when:
/// <list type="bullet">
///   <item>The page has only a few tab stops before the main content</item>
///   <item>The website consists of a single page without repeated content</item>
/// </list>
/// </avoid>
/// <guidelines>
/// <c>SkipLink</c> should be one of the first elements the keyboard focus reaches when navigating with the keyboard.
/// <br/><br/>
/// When navigating with a keyboard, users must be able to skip content that appears on multiple pages, such as menus, links, and the page header. <c>SkipLink</c> helps users move quickly to the main content.
/// <br/><br/>
/// If there are only a few tab stops before the main content — for example, if the page only contains a single “Home” link at the top — adding a <c>SkipLink</c> may be unnecessary. In such cases, it could introduce more focusable elements than needed.
/// <br/><br/>
/// Place <c>SkipLink</c> as the first focusable element on the page. If the page includes a cookie banner or similar message, place the <c>SkipLink</c> immediately after that element in the code.
/// <br/><br/>
/// <c>SkipLink</c> should visually stand out from the rest of the content when focused, so users who can see but navigate with a keyboard can easily recognise it before moving on.
/// </guidelines>
public partial class SkipLink : ComponentBase
{
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    /// <summary>
    /// The content to be rendered within the component.
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
            .AddClasses("ds-skip-link");

        // External links opened in new tabs must include security attributes
        var href = builder.ConsumeAttribute("href");
        builder.AddAttribute("href", HandleRelativeHrefPath(href));

        return builder;
    }

    private string? HandleRelativeHrefPath(string? href)
    {
        if (href is null)
        {
            return null;
        }

        if (Uri.TryCreate(href, UriKind.Absolute, out _))
        {
            return href;
        }

        if (href.StartsWith('#'))
        {
            var currentUri = Navigation.ToAbsoluteUri(Navigation.Uri);
            var baseUri = currentUri.GetLeftPart(UriPartial.Query);
            return baseUri + href;
        }

        return Navigation.ToAbsoluteUri(href).ToString();
    }
}