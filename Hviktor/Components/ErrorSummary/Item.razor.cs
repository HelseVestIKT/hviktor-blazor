using Hviktor.Models;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace ErrorSummary;

/// <summary>
/// Represents a single item in the <see cref="List"/> component.
/// </summary>
public partial class Item : NestedComponentBase<Hviktor.Components.ErrorSummary.ErrorSummary>
{
    /// <summary>
    /// The content to be displayed inside the item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}