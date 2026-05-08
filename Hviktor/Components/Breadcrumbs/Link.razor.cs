using Hviktor.Models;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Breadcrumbs;

/// <summary>
/// <c>Breadcrumbs.Link</c> represents a link within the <c>Breadcrumbs</c> component. It is used to navigate to a specific page or section.
/// </summary>
public partial class Link : NestedComponentBase<Hviktor.Components.Breadcrumbs.Breadcrumbs>
{
    /// <summary>
    /// The content to be displayed inside the link.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}