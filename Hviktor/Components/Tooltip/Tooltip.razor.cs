using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Tooltip;

/// <summary>
/// <c>Tooltip</c> displays brief information when the user hovers over or focuses on an element.
/// It renders no wrapper DOM. Instead, a hidden <c>&lt;template&gt;</c> marker carries tooltip
/// configuration that JavaScript applies to the next sibling element via <c>data-tooltip</c>,
/// <c>data-placement</c>, and <c>data-tooltip-type</c> attributes.
/// The tooltip popover is a singleton appended to the document body and shared across all instances.
/// </summary>
public sealed partial class Tooltip : ComponentBase
{
    [Inject] private IPlacementService PlacementService { get; set; } = null!;

    private Dictionary<string, object?>? preComputedMarkerAttributes;

    /// <summary>
    /// The text content to display in the tooltip.
    /// </summary>
    [Parameter]
    public string? Content { get; set; }

    /// <summary>
    /// The preferred placement direction for the tooltip relative to its anchor.
    /// </summary>
    [Parameter]
    public Placement? Placement { get; set; }

    /// <summary>
    /// The ARIA association type. Use <see cref="InputType.LabelledBy"/> to set
    /// <c>aria-describedby</c> instead of the default <c>aria-labelledby</c>.
    /// </summary>
    [Parameter]
    public InputType? Type { get; set; }

    /// <summary>
    /// Optional size parameter for future visual sizing support.
    /// </summary>
    [Parameter]
    public Size? Size { get; set; }

    /// <summary>
    /// Whether to automatically reposition the tooltip when there is insufficient space at the preferred placement.
    /// </summary>
    [Parameter]
    public bool AutoPlacement { get; set; } = true;

    /// <summary>
    /// The content that will trigger the tooltip (the element following the marker).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Additional HTML attributes. Tooltip-specific attributes are consumed;
    /// remaining attributes are ignored since there is no wrapper element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Computes attributes for the hidden <c>&lt;template&gt;</c> marker element.
    /// JavaScript reads these and applies them to the next sibling element.
    /// </summary>
    private Dictionary<string, object?> ComputeMarkerAttributes() => preComputedMarkerAttributes!;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        var builder = HtmlAttributeBuilder.ToDictionary();

        // Mark this as a tooltip configuration template
        builder.AddAttribute("data-tooltip-marker", "");

        // Tooltip content
        builder.AddAttribute("data-tooltip-content", Content ?? string.Empty);

        // Resolve placement
        var placementBuilder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes);
        EnumValue<Placement> placementAttr =
            placementBuilder.ConsumeAttribute("placement")
            ?? placementBuilder.ConsumeAttribute("data-placement");

        var resolvedPlacement = PlacementService.GetDataAttribute(
            Placement.HasValue ? Placement.Value : placementAttr,
            Abstractions.Enums.Attributes.Placement.Top);

        builder.AddAttribute("data-tooltip-placement", resolvedPlacement);

        // Auto-placement
        if (AutoPlacement)
        {
            builder.AddAttribute("data-tooltip-autoplacement", "true");
        }

        // ARIA type
        EnumValue<InputType> typeAttr = placementBuilder.ConsumeAttribute("type")
                                        ?? placementBuilder.ConsumeAttribute("data-type");
        if (Type == InputType.LabelledBy || (!Type.HasValue && !typeAttr.IsEmpty && typeAttr == InputType.LabelledBy))
        {
            builder.AddAttribute("data-tooltip-type", "labelledby");
        }

        preComputedMarkerAttributes = builder;
    }
}