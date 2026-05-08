using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.AvatarStack;

/// <summary>
/// <c>AvatarStack</c> stacks a collection of <see cref="Avatar"/> elements.
/// </summary>
/// <use>
/// Use <see cref="AvatarStack"/> when:
/// <list type="bullet">
///  <item>A group of people or entities should be displayed together, but space is limited.</item>
///  <item>You need to signal that multiple people are involved, without showing everyone at full size.</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <see cref="AvatarStack"/> when:
/// <list type="bullet">
///   <item>There is only ever a single avatar to display.</item>
///   <item>Each person or entity must be clearly identifiable by name or role at the same time.</item>
/// </list>
/// </avoid>
/// <guidelines>
/// <see cref="AvatarStack"/> should be used with caution, as it can be challenging for users to identify individual avatars in a tightly packed stack.
/// Ensure that the use of <see cref="AvatarStack"/> makes sense in the context of the user experience and does not compromise accessibility or usability.
/// <br/><br/>
/// AvatarStack does not support wrapping across multiple lines. If there are more avatars than can fit in the stack,
/// you can use <c>suffix</c> or an additional<see cref="Avatar"/> with <c>+xx</c> to indicate the number of hidden avatars.
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
///       <b>gap</b>: <see cref="string"/> | <see cref="CssLength"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="2px"/><br/>
///       <b>Description</b>: Adjusts gap-mask between avatars in the stack. Must be a valid css length value (px, em, rem, var(--ds-size-1) etc.)
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>avatar-size</b>: <see cref="string"/> | <see cref="CssLength"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="var(--ds-size-12)"/><br/>
///       <b>Description</b>: Control the size of the avatars. Must be a valid css length value (px, em, rem, var(--ds-size-12) etc.)
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>overlap</b>: <see cref="int"/> | <see cref="double"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="50"/><br/>
///       <b>Description</b>: A number which represents the percentage value of how much avatars should overlap. 
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>suffix</b>: <see cref="string"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Text to the right of the avatars to show a number representing additional avatars not shown such as '+5'".
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>expandable</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Expand on hover to show full avatars. 'fixed': AvatarStack physical width does not change when avatars are expanded.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Color.Accent"/> | <see cref="Color.Neutral"/> | <see cref="Color.Info"/> | <see cref="Color.Success"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///       <b>Description</b>: The color theme of the <see cref="AvatarStack"/>.<br/>
///                     <b>Note</b>: The color of the avatars should be controlled by the <see cref="Avatar"/> component, or the <c>avatar-size</c> attribute.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///       <b>Description</b>: The size of the <see cref="AvatarStack"/>, affecting its dimensions and the size of its content.
///                     <b>Note</b>: The size of the avatars should be controlled by the <see cref="Avatar"/> component.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class AvatarStack : ComponentBase
{
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;

    private Dictionary<string, object?>? preComputedAttributes;

    /// <summary>
    /// The ChildContent to render inside the <see cref="AvatarStack"/>.<br/>
    /// Expected to be a collection of <see cref="Avatar"/> components.
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
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-avatar-stack");

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        if (!color.IsEmpty)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(color));
        }

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        CssLength gap = builder.ConsumeAttribute("gap") ?? "2px";
        var gapCss = gap.ToCssString();
        if (gapCss is not null)
        {
            builder.AddStyles($"--dsc-avatar-stack-gap: {gapCss};");
        }

        CssLength avatarSize = builder.ConsumeAttribute("avatarSize") ?? builder.ConsumeAttribute("avatar-size") ?? "var(--ds-size-12)";
        var avatarSizeCss = avatarSize.ToCssString();
        if (avatarSizeCss is not null)
        {
            builder.AddStyles($"--dsc-avatar-stack-size: {avatarSizeCss};");
        }

        var overlapStr = builder.ConsumeAttribute("overlap");
        var isNumeric = int.TryParse(overlapStr, out var overlapInt);
        if (!isNumeric)
        {
            overlapInt = 50;
        }

        if (overlapInt < 0)
        {
            overlapInt = 0;
        }

        if (overlapInt > 100)
        {
            overlapInt = 100;
        }

        builder.AddStyles($"--dsc-avatar-stack-overlap: {overlapInt};");

        // Convert regular suffix to data attribute
        var suffix = builder.ConsumeAttribute("suffix");
        if (suffix is not null)
        {
            builder.AddDataAttribute("suffix", suffix);
        }

        CssBoolean expandable = builder.ConsumeAttribute("expandable");
        if (expandable.IsTruthy)
        {
            builder.AddDataAttribute("expandable", true);
        }

        preComputedAttributes = builder;
        return preComputedAttributes;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        preComputedAttributes = null;
        ComputeAttributes();
    }
}