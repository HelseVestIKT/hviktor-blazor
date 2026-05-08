using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Breadcrumbs;

/// <summary>
/// <c>Breadcrumbs</c> help users understand where they are within a structure and make it possible to navigate back to higher levels.
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
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Color.Accent"/> | <see cref="Color.Neutral"/> | <see cref="Color.Info"/> | <see cref="Color.Success"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///       <b>Description</b>: The color of the <c>Breadcrumb</c> links
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///       <b>Description</b>: The size of the <c>Breadcrumb</c> links
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Breadcrumbs : CascadingComponentBase
{
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;

    /// <summary>
    /// The HTML Content to render inside the <see cref="Breadcrumbs"/> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    /// <returns></returns>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-breadcrumbs");

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        if (!color.IsEmpty)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(color));
        }

        return builder;
    }
}