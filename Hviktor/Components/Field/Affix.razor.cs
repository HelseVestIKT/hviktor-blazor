using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Field;

/// <summary>
/// Prefixes and suffixes are useful for displaying units, currency or other types of information relevant to the field.<br/>
/// You should <b>not</b> use these on their own, as screen readers do not read them out.<br/>
/// It is important that the same information displayed in the prefix or suffix is also included in the prompt.
/// </summary>
/// <example>
/// It should be used together with the <c>Field.Affixes</c> component.
/// <code>
/// &lt;Field.Affixes&gt;
///     &lt;Field.Affix&gt;NOK&lt;/Field.Affix&gt; <!-- This is the prefix -->
///     &lt;Input/&gt;
///     &lt;Field.Affix&gt;per month&lt;/Field.Affix&gt; <!-- This is the suffix -->
/// &lt;/Field.Affixes&gt;
/// </code>
/// </example>
public partial class Affix : ComponentBase
{
    /// <summary>
    /// The ChildContent to render inside the Affix.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
        .AddClasses("ds-field-affix").HideFromAccessibility();
}