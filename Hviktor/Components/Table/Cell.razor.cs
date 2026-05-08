using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Table;

/// <summary>
/// The Cell component represents a cell in a table row.
/// It is typically used within a <see cref="Row"/> component and contains content that represents the data for a specific cell.
/// </summary>
public partial class Cell : ComponentBase
{
    /// <summary>
    /// The content of the cell, typically a text or a collection of <see cref="RenderFragment"/> components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
        => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes);
}