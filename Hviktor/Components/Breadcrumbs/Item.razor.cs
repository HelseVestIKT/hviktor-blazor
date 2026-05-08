using Hviktor.Models;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Breadcrumbs;

/// <summary>
/// <c>Breadcrumbs.Item</c> represents an individual breadcrumb within the <c>Breadcrumbs</c> component. It is used to define each step in the breadcrumb navigation trail.
/// </summary>
public partial class Item : NestedComponentBase<Hviktor.Components.Breadcrumbs.Breadcrumbs>
{
    /// <summary>
    /// The HTML Content to render inside the <see cref="Item"/> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}