using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Select;

/// <summary>
/// <c>Select</c> allows users to choose an option from a list.
/// </summary>
/// <use>
/// Use <c>Select</c> when:
/// <list type="bullet">
/// <item>You need to display many options and the user can choose only one</item>
/// <item>You need a compact way of presenting a long list of options</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Select</c> when:
/// <list type="bullet">
/// <item>You have only a few options and enough space — use <c>Radio</c> instead</item>
/// <item>You are navigating between pages or sections in an application</item>
/// <item>
/// The user must select <c>multiple</c> options.
/// Although HTML select supports this through the multiple attribute, it is not user-friendly, especially on mobile.
/// We recommend <c>Checkbox</c> or <c>Suggestion</c> for a clearer and more accessible solution.
/// </item>
/// </list>
/// </avoid>
/// <guidelines>
/// Use <c>Select</c> when there are 7 or more options.
/// For fewer options, radio buttons may be easier for users.
/// Select is especially user-friendly on mobile and provides good accessibility, as it follows the operating system’s native behaviour.
/// It works well when the user must choose one option from a list.
/// If users need to select multiple options or filter the list, consider using Suggestion.
/// <br/><br/>
/// <b>Note:</b> <see href="https://design-system.service.gov.uk/components/select">Gov.uk recommends avoiding <c>select</c></see> in public digital services.
/// Their user research shows that people often find the component difficult to use correctly.
/// It may be better to ask additional questions to reduce the number of options.
/// If you ask the right questions first, radio buttons may be a better choice.
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
///       <b>width</b>: <see cref="Width"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Width.Full"/>.<br/>
///       <b>Allowed</b>: <see cref="Width.Auto"/> | <see cref="Width.Full"/>.<br/>
///       <b>Description</b>: Defines the width of <c>Select</c>, where "auto" matches the content width.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/>.<br/>
///       <b>Description</b>: Defines the size of <c>Select</c>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>aria-readonly</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/>.<br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/>.<br/>
///       <b>Description</b>: Toggle <c>Select</c> read-only state.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Select : ComponentBase
{
    [Inject] private IWidthService WidthService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;

    /// <summary>
    /// The HTML Content to render inside the <see cref="Select"/> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Additional HTML attributes that will be applied to the <c>select</c> element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddClasses("ds-input")
            .AddAttribute("aria-invalid", "false");

        EnumValue<Width> width = builder.ConsumeAttribute("width") ?? builder.ConsumeAttribute("data-width");
        builder.AddDataAttribute("width", WidthService.GetDataAttribute(width, Width.Full));

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        // When readonly, ensure aria-readonly is set for screen readers.
        // Interaction is prevented by the Option component, which disables
        // all child options when the parent cascades readonly or disabled.
        var isReadonly = builder.GetValue("aria-readonly") is not null;
        if (isReadonly)
        {
            builder.AddAttribute("readonly", "true");
        }

        return builder;
    }
}