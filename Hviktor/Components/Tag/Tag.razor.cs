using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Tag;

// Use Tag when
//
// content needs a category, for example “Beta”, “New”, or “Popular”
// a status should be shown, for example “Ongoing”, “Completed”, or “Cancelled”
// Avoid using Tag when
//
// linking to another page, use Link instead
// the element triggers an action, use Button instead
// the user needs to filter content, use Chip instead
/// <summary>
/// <c>Tag</c> is a label that can be used to categorize items or communicate progress, status, or process.
/// Tags can provide users with a quicker overview of content.
/// </summary>
/// <use>
/// Use <c>Tag</c> when:
/// <list type="bullet">
/// <item>Content needs a category, for example “Beta”, “New”, or “Popular”</item>
/// <item>A status should be shown, for example “Ongoing”, “Completed”, or “Cancelled”</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Tag</c> when:
/// <list type="bullet">
/// <item>Linking to another page, use <c>Link</c> instead</item>
/// <item>The element triggers an action, use <c>Button</c> instead</item>
/// <item>The user needs to filter content, use <c>Chip</c> instead</item>
/// </list>
/// </avoid>
/// <guidelines>
/// Use <c>Tag</c> to help users quickly identify content based on category, status, or characteristics.<br/>
/// <c>Tag</c> works well for presenting metadata, such as document type, status, or topic.<br/>
/// They should be used consistently, kept short and easy to understand, and avoided when the information is already clear from context.<br/>
/// A tag is not clickable, it is purely a label.
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
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///       <b>Description</b>: <c>Tag</c> size.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Color.Accent"/> | <see cref="Color.Neutral"/> | <see cref="Color.Brand1"/> | <see cref="Color.Brand2"/> | <see cref="Color.Brand3"/> | <see cref="Color.Info"/> | <see cref="Color.Success"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///       <b>Description</b>: Color theme of the <c>Tag</c> component.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>variant</b>: <see cref="Variant"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Variant.Default"/><br/>
///       <b>Allowed</b>: <see cref="Variant.Default"/> | <see cref="Variant.Outline"/><br/>
///       <b>Description</b>: Visual variant of the <c>Tag</c> component.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Tag : ComponentBase
{
    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private IVariantService VariantService { get; set; } = null!;

    /// <summary>
    /// Specifies the content to be rendered inside the <c>Tag</c> component.
    /// This content typically represents the label, category, or status
    /// relevant to the purpose of the tag, such as “Beta”, “New”, “Ongoing”, etc.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element of the <c>Tag</c> component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
        => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-tag")
            .Transform(attr =>
            {
                EnumValue<Size> size = attr.ConsumeAttribute("size") ?? attr.ConsumeAttribute("data-size");
                if (!size.IsEmpty)
                {
                    attr.AddDataAttribute("size", SizeService.GetDataAttribute(size));
                }

                EnumValue<Color> color = attr.ConsumeAttribute("color") ?? attr.ConsumeAttribute("data-color");
                if (!color.IsEmpty)
                {
                    attr.AddDataAttribute("color", ColorService.GetDataAttribute(color));
                }

                EnumValue<Variant> variant = attr.ConsumeAttribute("variant") ?? attr.ConsumeAttribute("data-variant");
                attr.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, Variant.Default));

                return attr;
            });
}