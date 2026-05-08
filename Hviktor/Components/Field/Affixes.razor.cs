using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Field;

/// <summary>
/// The Affixes component is used to display additional information or context related to a form field.
/// </summary>
/// <example>
/// It should be used together with the <c>Field.Affix</c> component.
/// <code>
/// &lt;Field.Affixes&gt; <!-- This is the wrapper -->
///     &lt;Field.Affix&gt;NOK&lt;/Field.Affix&gt;
///     &lt;Input/&gt;
///     &lt;Field.Affix&gt;per month&lt;/Field.Affix&gt;
/// &lt;/Field.Affixes&gt;
/// </code>
/// </example>
public partial class Affixes : ComponentBase
{
    /// <summary>
    /// The ChildContent to render inside the Affixes.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes).AddClasses("ds-field-affixes");
}