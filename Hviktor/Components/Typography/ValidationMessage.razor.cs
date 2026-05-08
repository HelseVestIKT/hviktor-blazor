using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Typography;

/// <summary>
/// <c>ValidationMessage</c> is used to display feedback related to the validation of user input, such as error messages or warnings in forms.
/// </summary>
/// <guidelines>
/// Use <c>ValidationMessage</c> to provide immediate, context-specific feedback.<br/>
/// Messages should be placed close to the field they relate to, so that both visual users and users of assistive technologies can easily understand the connection.<br/>
/// See the article on <see href="https://designsystemet.no/en/patterns/errors">user-triggered error messages</see> for more guidance on good practice.
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
///       <b>color</b>: <see cref="Color"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Default</b>: <see cref="Color.Danger"/><br/>
///         <b>Allowed</b>: <see cref="Color.Danger"/> | <see cref="Color.Success"/> | <see cref="Color.Info"/> | <see cref="Color.Warning"/><br/>
///         <b>Description</b>: The color of the validation message is used to indicate the severity of the message.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///         <b>Description</b>: The size of the validation message text.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class ValidationMessage : ComponentBase
{
    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;

    /// <summary>
    /// The ChildContent to render inside the <see cref="ValidationMessage"/>.
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
            .AddClasses("ds-validation-message")
            .AddDataAttribute("field", "validation");

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