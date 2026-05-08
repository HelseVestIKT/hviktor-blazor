using System.Collections.Frozen;
using System.Text;
using System.Web;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Logo;

/// <summary>
/// Company Logos.<br/>
/// The logo is rendered as an inline SVG element with the company name as the aria-label for accessibility.<br/>
/// The company is determined by the "company" HTML attribute, which maps to the <see cref="Company"/> enum values.<br/>
/// </summary>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Parameter</term>
///     <description></description>
///   </listheader>
///   <item>
///     <term>
///       <b>company</b>: <see cref="Company"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The company to display the logo for.<br/>
///                 The company is determined by the "company" HTML attribute, which maps to the <see cref="Company"/> enum values.<br/>
///                 The company name is used as the aria-label for accessibility purposes.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Size.Medium"/><br/>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/> or raw values "2", "4", "sm", "lg", "small", "large".<br/>
///       <b>Description</b>: The size of the logo.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// 
public partial class Logo : ComponentBase
{
    private EnumValue<Company> CompanyEnumValue { get; set; }
    private EnumValue<Size> SizeEnumValue { get; set; }

    private string? renderedMarkup;
    private string? previousMarkup;

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddAttribute("role", "img")
            .AddAttribute("xmlns", "http://www.w3.org/2000/svg");

        var companyRaw = builder.ConsumeAttribute("company");
        CompanyEnumValue = !string.IsNullOrWhiteSpace(companyRaw)
                           && Enum.TryParse<Company>(companyRaw, ignoreCase: true, out var parsedCompany)
            ? parsedCompany
            : default;

        var company = CompanyEnumValue.EnumValueOrNull;
        if (company is not null && AriaLabels.TryGetValue((Company)company, out var ariaLabel))
        {
            builder.AddAttribute("aria-label", ariaLabel);
        }

        SizeEnumValue = builder.ConsumeAttribute("size");
        builder.AddAttribute("height", MapSizeString(SizeEnumValue));

        renderedMarkup = RenderSvgElement(builder);
    }

    /// <inheritdoc />
    protected override bool ShouldRender()
    {
        if (renderedMarkup == previousMarkup)
        {
            return false;
        }

        previousMarkup = renderedMarkup;
        return true;
    }

    /// <summary>
    /// Builds the full inline SVG markup string for the resolved company logo.
    /// </summary>
    private string? RenderSvgElement(Dictionary<string, object?> attributes)
    {
        var company = CompanyEnumValue.EnumValueOrNull;
        if (company is null || !SvgData.TryGetValue((Company)company, out var svgInfo))
        {
            return null;
        }

        var sb = new StringBuilder(svgInfo.Paths.Length + 256);
        sb.Append("<svg");

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
            sb.Append(HttpUtility.HtmlAttributeEncode(val?.ToString() ?? string.Empty));
            sb.Append('"');
        }

        sb.Append(" viewBox=\"");
        sb.Append(svgInfo.ViewBox);
        sb.Append("\">");
        sb.Append(svgInfo.Paths);
        sb.Append("</svg>");

        return sb.ToString();
    }

    private static readonly FrozenDictionary<Company, string> AriaLabels = new Dictionary<Company, string>
    {
        [Company.DOTS] = "Helse Vest",
        [Company.HVE] = "Helse Vest",
        [Company.HVIKT] = "Helse Vest IKT",
        [Company.HVIKTOR] = "Hviktor",
        [Company.HBE] = "Helse Bergen",
        [Company.HBE_HUS] = "Helse Bergen, Haukeland universitetssjukehus",
        [Company.HFD] = "Helse Førde",
        [Company.HFO] = "Helse Fonna",
        [Company.HST] = "Helse Stavanger",
        [Company.HST_SUS] = "Helse Stavanger, Stavanger universitetssjukehus",
        [Company.SAV] = "Sjukehusapoteka Vest",
    }.ToFrozenDictionary();

    private const string SmallPx = "48px";
    private const string MediumPx = "56px";
    private const string LargePx = "64px";

    /// <summary>
    /// Maps an <see cref="EnumValue{Size}"/> to the pre-formatted pixel size string.
    /// </summary>
    private static string MapSizeString(EnumValue<Size> size)
    {
        if (size.IsRaw)
        {
            return size.RawValue!.ToLowerInvariant() switch
            {
                "2" or "sm" or "small" => SmallPx,
                "4" or "lg" or "large" => LargePx,
                _ => MediumPx
            };
        }

        return size.EnumValueOrNull switch
        {
            Size.ExtraExtraSmall => SmallPx,
            Size.ExtraSmall => SmallPx,
            Size.Small => SmallPx,
            Size.Large => LargePx,
            Size.ExtraLarge => LargePx,
            Size.ExtraExtraLarge => LargePx,
            _ => MediumPx
        };
    }
}