using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Badge;

/// <summary>
/// The position of the badge.
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
///       <b>placement</b>: <see cref="Placement"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Placement.TopEnd"/><br/>
///       <b>Allowed</b>: <see cref="Placement.TopEnd"/> | <see cref="Placement.TopStart"/> | <see cref="Placement.BottomStart"/> | <see cref="Placement.BottomEnd"/><br/>
///       <b>Description</b>: The position of the badge in relation to the child element.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>overlap</b>: <see cref="Variant"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Variant.Rectangle"/><br/>
///       <b>Allowed</b>: <see cref="Variant.Rectangle"/> | <see cref="Variant.Circle"/><br/>
///       <b>Description</b>: The approximate shape of the child element controlling the badge position.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Position : ComponentBase
{
    [Inject] private IPlacementService PlacementService { get; set; } = null!;
    [Inject] private IVariantService VariantService { get; set; } = null!;

    /// <summary>
    /// The content to be displayed inside the badge.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-badge--position");

        EnumValue<Placement> placement = builder.ConsumeAttribute("placement") ?? builder.ConsumeAttribute("data-placement");
        builder.AddDataAttribute("placement", PlacementService.GetDataAttribute(placement, Placement.TopEnd)
            .Replace("-end", "-right")
            .Replace("-start", "-left"));

        EnumValue<Variant> overlap = builder.ConsumeAttribute("overlap") ?? builder.ConsumeAttribute("data-overlap");
        builder.AddDataAttribute("overlap", VariantService.GetDataAttribute(overlap, Variant.Rectangle));

        return builder;
    }
}