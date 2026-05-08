using Hviktor.Models;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Pagination;

/// <summary>
/// The Item component represents a single item in the pagination list.
/// </summary>
public partial class Item : NestedComponentBase<Hviktor.Components.Pagination.Pagination>
{
    /// <summary>
    /// The ChildContent to render inside the Item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}