using System.Text;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Icons.Abstractions.Types;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Icon;

/// <summary>
/// Renders an icon from the <c>@helsevestikt/hviktor-icons</c> library as an inline SVG element.<br/>
/// The SVG path data is stored in <see cref="IconDefinition.PathData"/>.
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
///       <b>size</b>: <see cref="Abstractions.Enums.Attributes.Size"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Default</b>: <see cref="Size.Medium"/><br/>
///         <b>Allowed</b>: <see cref="Size.ExtraExtraSmall"/> | <see cref="Size.ExtraSmall"/> | <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/> | <see cref="Size.ExtraLarge"/> | <see cref="Size.ExtraExtraLarge"/><br/>
///         <b>Description</b>: The size of the icon, which maps to specific pixel dimensions.<br/>
///                         ExtraExtraSmall -> 8px<br/>
///                         ExtraSmall -> 12px<br/>
///                         Small -> 16px<br/>
///                         Medium -> 24px<br/>
///                         Large -> 32px<br/>
///                         ExtraLarge -> 48px<br/>
///                         ExtraExtraLarge -> 64px<br/>
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>width</b>: <see cref="string"/>? | <see cref="int"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Description</b>: The width of the icon. Must be a valid CSS length value (e.g., "16px", "1.5em", "100%").<br/>
///                         If not specified, the width defaults to the pixel size determined by the <see cref="Size"/> parameter. If both <see cref="Width"/> and the "width" key in <see cref="AdditionalAttributes"/> are provided, the typed parameter takes precedence.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>height</b>: <see cref="string"/>? | <see cref="int"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Description</b>: The height of the icon. Must be a valid CSS length value (e.g., "16px", "1.5em", "100%").<br/>
///                         If not specified, the height defaults to the pixel size determined by the <see cref="Size"/> parameter. If both <see cref="Height"/> and the "height" key in <see cref="AdditionalAttributes"/> are provided, the typed parameter takes precedence.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Allowed</b>: <see cref="Color.Accent"/> | <see cref="Color.Neutral"/> | <see cref="Color.Info"/> | <see cref="Color.Success"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///         <b>Description</b>: The color theme of the icon.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <seealso href="https://aksel.nav.no/ikoner">Aksel icons (NAV)</seealso>
public partial class Icon : ComponentBase
{
    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;

    /// <summary>
    /// The definition of the icon to be displayed.<br/>
    /// This parameter is required.<br/>
    /// Example usage: <c>&lt;Icon Definition="IconSet.Checkmark" /&gt;</c>
    /// </summary>
    [Parameter]
    public IconDefinition? Definition { get; set; }

    private CssLength Width { get; set; }
    private CssLength Height { get; set; }

    private EnumValue<Color> Color { get; set; }
    private string? Size { get; set; }

    /// <summary>
    /// The child content of the icon component.<br/>
    /// Rendered only in the fallback branch when <see cref="Definition"/> has no value.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched HTML attributes and forwards them to the icon element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Builds the inline SVG markup for the icon.<br/>
    /// Constructed as a raw HTML string and rendered via <see cref="MarkupString"/>
    /// because Blazor does not support dynamic SVG path attributes in Razor markup.
    /// </summary>
    private string RenderIconElement()
    {
        var attributes = ComputeAttributes();
        var sb = new StringBuilder(512);

        var widthValue = !Width.IsEmpty ? Width.ToCssString()! : $"{Size}";
        var heightValue = !Height.IsEmpty ? Height.ToCssString()! : $"{Size}";

        sb.Append("<svg");
        sb.Append($" width=\"{System.Web.HttpUtility.HtmlAttributeEncode(widthValue)}\"");
        sb.Append($" height=\"{System.Web.HttpUtility.HtmlAttributeEncode(heightValue)}\"");
        sb.Append(" viewBox=\"0 0 24 24\"");
        sb.Append(" fill=\"none\"");
        sb.Append(" xmlns=\"http://www.w3.org/2000/svg\"");

        foreach ((var key, var val) in attributes)
        {
            sb.Append(' ');
            sb.Append(key);

            if (val is bool boolVal)
            {
                if (boolVal)
                {
                    sb.Append("=\"true\"");
                }

                continue;
            }

            sb.Append("=\"");
            sb.Append(System.Web.HttpUtility.HtmlAttributeEncode(val?.ToString() ?? string.Empty));
            sb.Append('"');
        }

        if (Definition is { HasValue: true })
        {
            sb.Append("><path fill-rule=\"evenodd\" clip-rule=\"evenodd\" fill=\"currentColor\" d=\"");
            sb.Append(Definition.PathData);
            sb.Append("\" /></svg>");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Assembles all typed parameters and <see cref="AdditionalAttributes"/> into a single
    /// attribute dictionary forwarded to the SVG element.<br/>
    /// Typed parameters take precedence over any colliding keys in <see cref="AdditionalAttributes"/>.
    /// </summary>
    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes);
        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        var sizeAsStr = SizeService.GetDataAttribute(size);
        Size = $"{sizeAsStr switch
        {
            "2xs" => 8,
            "xs" => 12,
            "sm" => 16,
            "lg" => 32,
            "xl" => 48,
            "2xl" => 64,
            _ => 24
        }}px";

        Width = builder.ConsumeAttribute("width");
        Height = builder.ConsumeAttribute("height");

        Color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        if (!Color.IsEmpty)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(Color));
        }

        // Decorative icons are hidden from assistive technology by default
        builder.HideFromAccessibility();
        return builder;
    }
}