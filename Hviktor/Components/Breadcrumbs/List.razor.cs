using Hviktor.Models;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Breadcrumbs;

/// <summary>
/// <c>Breadcrumbs.List</c> represents the list of breadcrumbs within the `Breadcrumbs` component.
/// </summary>
public partial class List : NestedComponentBase<Hviktor.Components.Breadcrumbs.Breadcrumbs>
{
    /// <summary>
    /// Content rendered inside the breadcrumb list.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}