using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Textarea;

/// <summary>
/// <c>Textarea</c> is used when the user needs to enter text that spans multiple lines.
/// </summary>
/// <use>
/// Use <c>Textarea</c> when:
/// <list type="bullet">
/// <item>users need to write more than one line of text</item>
/// <item>free-form input is required</item>
/// <item>the answer does not follow a fixed structure</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Textarea</c> when:
/// <list type="bullet">
/// <item>short answers are expected, use Textfield instead</item>
/// <item>the input consists of structured data that requires validation</item>
/// </list>
/// </avoid>
/// <guidelines>
/// Use <c>Textarea</c> when users are expected to write more than one line of text, for example open questions, feedback, or other situations requiring longer input.
/// Be aware that some users find free-text fields challenging. In certain cases, it may be better to break the question into smaller, more structured parts and let users answer with components such as <c>Radio</c>.
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
///       <b>Description</b>: <c>Textarea</c> size.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Textarea : ComponentBase
{
    [Inject] private ISizeService SizeService { get; set; } = null!;

    #region Parameters

    /// <summary>
    /// Gets or sets the content to be rendered inside the <c>Textarea</c> component.
    /// This property allows for the inclusion of custom child elements or text.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes to be applied to the rendered HTML element.
    /// This property allows arbitrary HTML attributes to be added to the component,
    /// facilitating customization and integration with external libraries or frameworks.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    #endregion

    private Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
        .AddClasses("ds-input")
        .Transform(attr =>
        {
            EnumValue<Size> size = attr.ConsumeAttribute("size") ?? attr.ConsumeAttribute("data-size");
            if (!size.IsEmpty)
            {
                attr.AddDataAttribute("size", SizeService.GetDataAttribute(size));
            }

            return attr;
        });
}