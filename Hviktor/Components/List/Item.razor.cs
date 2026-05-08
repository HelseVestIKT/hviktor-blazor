using Hviktor.Models;
using Hviktor.Models.List;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace List;

/// <summary>
/// The Item component represents an individual item within a list.
/// </summary>
public partial class Item : NestedComponentBase<ListBase>
{
    /// <summary>
    /// The content to be displayed inside the list item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}