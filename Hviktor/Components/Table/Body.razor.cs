using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Table;

/// <summary>
/// Represents the body section of a table,
/// typically containing rows of data represented by <see cref="Row"/> components.
/// </summary>
public partial class Body : ComponentBase
{
    /// <summary>
    /// The content of the body, typically a collection of <see cref="Row"/> components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// A collection of additional arbitrary parameters that are passed to the rendered HTML element.
    /// These attributes are typically used to add custom HTML attributes or event handlers.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes() 
        => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes);
}