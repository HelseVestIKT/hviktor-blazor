using Hviktor.Models;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Dropdown;

/// <summary>
/// <c>Dropdown.Heading</c> is used to display a heading or title for the dropdown menu.
/// </summary>
/// <inheritdoc cref="Hviktor.Components.Typography.Heading" />
public partial class Heading : AsyncNestedComponentBase<Hviktor.Components.Dropdown.Dropdown>
{
    /// <summary>
    /// Content rendered inside the dropdown heading.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}