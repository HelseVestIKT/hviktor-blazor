using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Table;

/// <summary>
/// Represents the footer section of a table, typically containing summary information or actions.
/// </summary>
public partial class Foot : ComponentBase
{
    /// <summary>
    /// The content of the footer, typically a collection of <see cref="Row"/> components.
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