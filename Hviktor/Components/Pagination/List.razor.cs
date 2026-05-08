using Hviktor.Models;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Pagination;

/// <summary>
/// The List component represents the list of pagination items.
/// </summary>
public partial class List : NestedComponentBase<Hviktor.Components.Pagination.Pagination>
{
    /// <summary>
    /// Content rendered inside the pagination list.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}