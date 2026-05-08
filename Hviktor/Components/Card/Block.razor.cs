using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Card;

/// <summary>
/// Use multiple <c>Card.Block</c> elements if you want to divide the card with separators or add images or video that extend to the edge.<br/>
/// <b>Note</b> that when you use <c>Card.Block</c>, all content must be placed inside a <c>Card.Block</c> and not directly in the <see cref="Card"/>.
/// </summary>
public partial class Block : ComponentBase
{
    /// <summary>
    /// The Id of the <see cref="Card.Block"/>, used to identify the block in the DOM.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = "";

    /// <summary>
    /// The HTML Content to render inside the <see cref="Card.Block"/> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
        .AddIdentity(Id)
        .AddClasses("ds-card__block");
}