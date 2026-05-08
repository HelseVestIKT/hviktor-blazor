using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Select;

/// <summary>
/// The <c>Select.Optgroup</c> component represents a group of related options within a select dropdown, allowing for better organization and accessibility.
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
///       <b>label</b>: <see cref="string"/><br/>
///       <i>(required)</i>
///     </term>
///     <description>
///       <b>Description</b>: The label of the option group.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Optgroup : ComponentBase
{
    [Inject] private ILogger<Optgroup> Logger { get; set; } = null!;

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

    private string internalLabel = string.Empty;

    private Dictionary<string, object?>? preComputedMarkerAttributes;

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
    {
        if (preComputedMarkerAttributes is not null)
        {
            return preComputedMarkerAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            // If either value from sourceKeys is true, add "disabled": true to the target dictionary
            .Combine(CascadingAttributes, "disabled",
                (current, next) => current is true || next is true, ["readonly", "disabled"]);

        var label = builder.ConsumeAttribute("label");
        if (string.IsNullOrWhiteSpace(label))
        {
            Logger.LogWarning("The 'label' attribute is required for Optgroup. Please provide a label to ensure proper accessibility.");
            preComputedMarkerAttributes = builder;
            return builder;
        }

        internalLabel = label;

        preComputedMarkerAttributes = builder;
        return builder;
    }
}