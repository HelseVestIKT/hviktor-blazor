using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Microsoft.AspNetCore.Components;
using Hviktor.Rendering;

namespace Hviktor.Components.Alert;

/// <summary>
/// <c>Alert</c> provides users with information that is especially important for them to see and understand.<br/>
/// The component is designed to capture users' attention.<br/><br/>
/// The text in the alert should be short and clear.
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
///       <b>color</b>: <see cref="Color"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Color.Info"/><br/>
///       <b>Allowed</b>: <see cref="Color.Info"/> | <see cref="Color.Success"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///       <b>Description</b>: In code, you change the type of alert by changing data-color.<br/>The icon is controlled via CSS.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Size.Medium"/><br/>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///       <b>Description</b>: The alert box size.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Alert : ComponentBase
{
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;

    /// <summary>
    /// The HTML Content to render inside the <see cref="Alert"/> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    // The current design prioritizes clarity and type safety over minor performance gains. Keep IReadOnlyDictionary<string, object> as the return type.
    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes);
        builder.AddClasses("ds-alert");

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        builder.AddDataAttribute("color", ColorService.GetDataAttribute(color, Color.Info));

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        return builder;
    }
}