using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Pagination;

/// <summary>
/// <c>Pagination</c> is a list of buttons used to navigate between different pages of content, such as search results or tables.
/// </summary>
/// <use>
/// Use <c>Pagination</c> when:
/// <list type="bullet">
/// <item>Search results or large amounts of data need to be divided across multiple pages</item>
/// <item>Users need to understand where they are within a larger set of content</item>
/// <item>Loading the entire dataset at once would be too heavy or result in poor performance</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Pagination</c> when:
/// <list type="bullet">
/// <item>The total amount of content is small enough to fit on a single page</item>
/// <item>You want to divide a form into several pages</item>
/// <item>Progress needs to be displayed</item>
/// </list>
/// </avoid>
/// <guidelines>
/// Use <c>Pagination</c> when content is too large for a single page, such as search results or long lists, and the user needs to navigate back and forth.<br/>
/// Content should only be split across multiple pages if it improves loading time or usability.
/// </guidelines>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Attribute</term>
///     <description>Description</description>
///   </listheader>
///   <item>
///     <term>
///       <b>data-current</b>: <see cref="string"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Current page number. This is used to determine which page is currently active and should be highlighted in the pagination component.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>data-total</b>: <see cref="string"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Total number of pages. This is used to determine how many page buttons to display in the pagination component.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Pagination : CascadingComponentBase
{
    [Inject] private IStringLocalizerService<Resources.Resources> Localizer { get; set; } = null!;

    /// <summary>
    /// Gets or sets the child content to be rendered inside the <c>Pagination</c> component.
    /// This property is a <c>RenderFragment</c> that allows you to define custom UI elements
    /// or nested components within the pagination component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddClasses("ds-pagination")
        .AddAttribute("aria-label", Localizer.GetValue("Hviktor.Components.Pagination.AriaLabel"));
}