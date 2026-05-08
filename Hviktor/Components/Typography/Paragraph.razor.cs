using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Typography;

/// <summary>
/// <c>Paragraph</c> is used for continuous text and is typically applied in articles, components, help text, and similar content.
/// </summary>
/// <guidelines>
/// Keep paragraphs focused and well structured,
/// and break up long blocks of text into smaller sections to make the content easier to scan and understand.
/// Ensure that line length and spacing support good readability.
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
///       <b>variant</b>: <see cref="Variant"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Default</b>: <see cref="Variant.Default"/><br/>
///         <b>Allowed</b>: <see cref="Variant.Long"/> | <see cref="Variant.Default"/> | <see cref="Variant.Short"/><br/>
///         <b>Description</b>: Adjusts styling for paragraph length.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Paragraph : ComponentBase
{
    [Inject] private ISizeService SizeService { get; set; } = null!;

    /// <summary>
    /// The ChildContent to render inside the <see cref="Paragraph"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-paragraph");

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        return builder;
    }
}