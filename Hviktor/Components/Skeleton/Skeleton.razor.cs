using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Skeleton;

/// <summary>
/// <b>Skeleton</b> is used to indicate that content on a page is loading. It provides the user with a visual hint of what the content will eventually look like.<br/>
/// It provides a visual indication that content is being loaded, enhancing the user experience by preventing layout shifts.
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
///       <b>width</b>: <see cref="string"/>? | <see cref="int"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The width of the skeleton.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>height</b>: <see cref="string"/>? | <see cref="int"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The height of the skeleton.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>variant</b>: <see cref="Variant"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Variant.Rectangle"/><br/>
///       <b>Allowed</b>: <see cref="Variant.Rectangle"/> | <see cref="Variant.Circle"/> | <see cref="Variant.Text"/><br/>
///       <b>Description</b>: The visual variant of the skeleton.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <use>
/// Use <c>Skeleton</c> when:
/// <list type="bullet">
///   <item>You need to indicate that content on a page is in the process of loading</item>
///   <item>You want to show users what the layout of the content will look like once it has loaded</item>
///   <item>The content is expected to take more than one second to load</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Skeleton</c> when:
/// <list type="bullet">
///   <item>You need to indicate that the system is working on a task the user must wait for, use a Spinner instead</item>
/// </list>
/// </avoid>
/// <guidelines>
/// <c>Skeleton</c> is an alternative to <c>Spinner</c> when you want to indicate that content is loading.
/// It helps users understand what is coming and can make the waiting time feel shorter.
/// <br/><br/>
/// Choose the Skeleton variant based on the type of content being loaded, such as headings, text blocks or images.
/// Try to mimic the structure of the actual content to make the transition to the fully loaded content feel smooth.
/// </guidelines>
public partial class Skeleton : ComponentBase
{
    [Inject] private IVariantService VariantService { get; set; } = null!;

    /// <summary>
    /// The content to display inside the skeleton. When used with the <c>text</c> variant,
    /// the child content provides the width for the skeleton instead of the <c>width</c> attribute.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// <c>width</c>, <c>height</c>, and <c>variant</c> are consumed and handled rather than
    /// forwarded as bare HTML attributes.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private bool HasChildContent => ChildContent is not null;

    private static string? GetDataTextValue(CssLength width, EnumValue<Variant> variant, bool hasChildContent)
    {
        if (variant != Variant.Text)
        {
            return null;
        }

        if (hasChildContent)
        {
            return "-";
        }

        // Use the pixel count when available, otherwise fall back to a single dash placeholder.
        var charCount = width.PixelValue ?? 1;
        return charCount switch
        {
            1 => "-",
            2 => "--",
            3 => "---",
            _ => new string('-', charCount)
        };
    }

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-skeleton");

        // Accept both "variant" and "data-variant".
        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        if (variant.IsEmpty)
        {
            variant = Variant.Rectangle;
        }

        // Consume width, height, and variant before forwarding so they are not rendered as bare HTML attributes.
        CssLength width = builder.ConsumeAttribute("width") ?? builder.ConsumeAttribute("data-width");
        if (variant != Variant.Text)
        {
            CssLength height = builder.ConsumeAttribute("height") ?? builder.ConsumeAttribute("data-height");

            var widthCss = width.ToCssString();
            var heightCss = height.ToCssString();

            if (widthCss is not null && heightCss is not null)
            {
                builder.AddStyles([$"width:{widthCss}", $"height:{heightCss}"]);
            }
            else if (widthCss is not null)
            {
                builder.AddStyles($"width:{widthCss}");
            }
            else if (heightCss is not null)
            {
                builder.AddStyles($"height:{heightCss}");
            }
        }

        return builder
            .AddDataAttribute("text", GetDataTextValue(width, variant, HasChildContent))
            .AddDataAttribute("variant", VariantService.GetDataAttribute(variant))
            .HideFromAccessibility();
    }
}