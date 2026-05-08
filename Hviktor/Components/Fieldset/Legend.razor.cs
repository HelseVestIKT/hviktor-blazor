using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Fieldset;

/// <summary>
/// The legend of the fieldset.
/// </summary>
public partial class Legend : ComponentBase
{
    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <summary>
    /// The HTML Content to render inside the <see cref="Legend"/> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes).AddClasses("ds-label");
}