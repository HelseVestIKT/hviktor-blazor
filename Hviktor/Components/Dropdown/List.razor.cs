using Hviktor.Models;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Dropdown;

/// <summary>
/// <c>Dropdown.List</c> represents the list of items within the <see cref="Hviktor.Components.Dropdown.Dropdown">Dropdown</see> component.
/// </summary>
public partial class List : AsyncNestedComponentBase<Hviktor.Components.Dropdown.Dropdown>
{
    /// <summary>
    /// Content rendered inside the dropdown item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}