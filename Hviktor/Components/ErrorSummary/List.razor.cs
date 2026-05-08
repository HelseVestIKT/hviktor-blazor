using Hviktor.Models;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace ErrorSummary;

/// <summary>
/// Represents a list of items in the <see cref="ErrorSummary"/> component.
/// </summary>
public partial class List : NestedComponentBase<Hviktor.Components.ErrorSummary.ErrorSummary>
{
    /// <summary>
    /// The content to be displayed inside the list.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}