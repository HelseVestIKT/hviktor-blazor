using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Select;

/// <summary>
/// The Option component represents an option within a select dropdown.
/// </summary>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Attribute</term>
///     <description>Description</description>
///   </listheader>
///   <item>
///     <term>
///       <b>value</b>: <see cref="string"/><br/>
///       <i>(required)</i>
///     </term>
///     <description>
///       <b>Description</b>: The value of the option.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Option : ComponentBase
{
    /// <summary>
    /// Cascading parameter for the <see cref="Select"/> component.
    /// </summary>
    [CascadingParameter]
    public IReadOnlyDictionary<string, object?>? CascadingAttributes { get; set; }

    /// <summary>
    /// The content of the option.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
        // If either value from sourceKeys is true, add "disabled": true to the target dictionary
        .Combine(CascadingAttributes, "disabled",
            (current, next) => current is true || next is true, ["readonly", "disabled"]);
}