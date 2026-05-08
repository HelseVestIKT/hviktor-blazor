using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Table;

/// <summary>
/// The Row component represents a row in a table.
/// It is typically used within a <see cref="Table"/> component and contains <see cref="Cell"/> components as its children.
/// </summary>
public partial class Row : ComponentBase
{
    /// <summary>
    /// The content of the row, typically a collection of <see cref="Cell"/> components.
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