using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Badge;

/// <summary>
/// <c>Badge</c> is a non-interactive component that displays the status with ot without numbers.
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
///       <b>variant</b>: <see cref="Variant"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Variant.Base"/><br/>
///       <b>Allowed</b>: <see cref="Variant.Base"/> | <see cref="Variant.Tinted"/><br/>
///       <b>Description</b>: Visual variant of the <c>Badge</c> component.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Color.Accent"/> | <see cref="Color.Neutral"/> | <see cref="Color.Info"/> | <see cref="Color.Success"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///       <b>Description</b>: Color theme of the <c>Badge</c> component.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <use>
/// Use <c>Badge</c> when:
/// <list type="bullet">
/// <item>Displaying numbers that show the count of new messages, notifications or tasks.</item>
/// <item>Indicating status using a circular marker, such as showing whether a person is busy, away or active.</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Badge</c> when:
/// <list type="bullet">
/// <item>Text is needed, use <see cref="Tag"/> instead.</item>
/// <item>Interactive actions are required, use <see cref="Chip"/> or <see cref="Button"/> instead.</item>
/// </list>
/// </avoid>
/// <guidelines>Use <c>Badge</c> to draw attention to statuses, notifications or numbers.</guidelines>
public partial class Badge : ComponentBase
{
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private IVariantService VariantService { get; set; } = null!;

    /// <summary>
    /// The Id of the badge, used to identify the badge in the DOM.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Cryptography.GenerateId();

    /// <summary>
    /// The number to display in the badge.<br/>If not provided, the badge displays without a count.
    /// </summary>
    [Parameter]
    public int? Count { get; set; }

    /// <summary>
    /// The maximum number to display in the badge. If the Count exceeds this number, it will display as "MaxCount+".<br/>
    /// If not provided, the badge displays without a count limit.
    /// </summary>
    [Parameter]
    public int? MaxCount { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddIdentity(Id).AddClasses("ds-badge");

        if (Count.HasValue)
        {
            builder.AddDataAttribute("count", GetCountDataAttribute(Count.Value));
        }

        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        builder.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, Variant.Base));

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        if (!color.IsEmpty)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(color));
        }

        return builder;
    }

    private string GetCountDataAttribute(int count)
        => MaxCount is > 0 && count > MaxCount ? $"{MaxCount}+" : count.ToString();
}