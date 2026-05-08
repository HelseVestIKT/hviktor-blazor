using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Table;

/// <summary>
/// Represents the head section of a table, typically containing header cells that define the columns of the table.
/// </summary>
public partial class Head : ComponentBase
{
    /// <summary>
    /// The content of the head, typically a collection of <see cref="HeaderCell"/> components.
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