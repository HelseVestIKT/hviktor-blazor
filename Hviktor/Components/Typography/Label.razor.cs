using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Typography;

/// <summary>
/// <c>Label</c> functions as a clear and accessible text label that tells the user what an associated form element is about.
/// </summary>
/// <guidelines>
/// <c>Label</c> must be clearly connected to the associated field so that both visual users and users of assistive technologies understand the relationship.<br/>
/// If the field requires a specific format or limitations, this can be supplemented with a description (<c>description</c>) underneath rather than making the <c>Label</c> unnecessarily long.
/// </guidelines>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Attribute</term>
///     <description>Description</description>
///   </listheader>
///   <item>
///     <term>
///       <b>weight</b>: <see cref="Weight"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Default</b>: <see cref="Weight.Medium"/><br/>
///         <b>Allowed</b>: <see cref="Weight.Regular"/> | <see cref="Weight.Medium"/> | <see cref="Weight.SemiBold"/><br/>
///         <b>Description</b>: Adjusts font weight. Use this when you have a label hierarchy, such as <see cref="Checkbox">checkboxes</see>/<see cref="Radio">radios</see> in a <see cref="Fieldset">fieldset</see>.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Label : ComponentBase
{
    [Inject] private IWeightService WeightService { get; set; } = null!;

    /// <summary>
    /// The ChildContent parameter is used to define the content that will be rendered within the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Represents a collection of additional attributes that are not explicitly defined as component parameters.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-label");

        EnumValue<Weight> weight = builder.ConsumeAttribute("weight") ?? builder.ConsumeAttribute("data-weight");
        if (!weight.IsEmpty)
        {
            builder.AddDataAttribute("weight", WeightService.GetDataAttribute(weight));
        }

        return builder;
    }
}