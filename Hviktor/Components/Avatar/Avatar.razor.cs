using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Hviktor.Components.Avatar;

/// <summary>
/// <c>Avatar</c> displays an image, initials, or icon for a person, entity, or profile.
/// </summary>
/// <use>
/// Use <see cref="Avatar"/> when:
/// <list type="bullet">
///  <item>Representing a person or an entity</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <see cref="Avatar"/> when:
/// <list type="bullet">
///   <item>Representing anything other than a person or an entity, such as a document. Use icons or other visual symbols instead.</item>
///   <item>It is used purely for decoration without function or meaning.</item>
/// </list>
/// </avoid>
/// <guidelines>
/// <see cref="Avatar"/> should help make the user interface more recognisable and easier to navigate without distracting or taking up unnecessary space.<br/>
/// <list type="bullet">
///   <item>Use the same avatar shape consistently throughout the solution.</item>
///   <item>The avatar should usually be combined with text, unless it is entirely clear who or what it represents.</item>
///   <item>Only use an avatar when it actually adds value to the user experience, for example to show who authored something or who is responsible.</item>
/// </list>
/// </guidelines>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Parameter</term>
///     <description></description>
///   </listheader>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Color.Accent"/> | <see cref="Color.Neutral"/> | <see cref="Color.Info"/> | <see cref="Color.Success"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///       <b>Description</b>: The color theme of the avatar.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>variant</b>: <see cref="Variant"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Variant.Circle"/><br/>
///       <b>Allowed</b>: <see cref="Variant.Circle"/> | <see cref="Variant.Square"/><br/>
///       <b>Description</b>: The visual variant of the avatar, determining its shape.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///       <b>Description</b>: The size of the avatar, affecting its dimensions and the size of its content.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>data-tooltip</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The content of the tooltip that appears when hovering over the avatar.<br/>
///                     <b>Note</b>: It is recommended to use the <see cref="Tooltip"/> component for this purpose.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Avatar : ComponentBase
{
    [Inject] private ILogger<Avatar> Logger { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private IVariantService VariantService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;

    /// <summary>
    /// Initials to display inside the avatar.
    /// </summary>
    [Parameter]
    public string? Initials { get; set; }

    /// <summary>
    /// Image, icon or initials to display inside the avatar.<br/>
    /// Gets `aria-hidden="true"`
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
            .AddClasses("ds-avatar")
            .AddDataAttribute("initials", Initials);

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        if (!color.IsEmpty)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(color));
        }

        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        builder.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, Variant.Circle));

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        builder.AddAttribute("role", "img");
        if (builder.ContainsKey("asChild"))
        {
            Logger.LogWarning("The 'asChild' attribute is not supported on the Avatar component and will be ignored.");
        }

        return builder;
    }
}