using System.ComponentModel.DataAnnotations;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Radio;

/// <summary>
/// A <c>Radio</c> is an option the user can select. Use multiple <c>Radio</c> to show a list of options.
/// Users can switch between the options, but can only select one.
/// </summary>
/// <use>
/// Use <c>Radio</c> when:
/// <list type="bullet">
///   <item>The user must choose one of several options in a form</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Radio</c> when:
/// <list type="bullet">
///   <item>The user should be able to choose more than one option, use <c>Checkbox</c> instead</item>
/// </list>
/// </avoid>
/// <guidelines>
/// Use <c>Radio</c> when users must select only one option. If they need to select more, use <c>Checkbox</c>.<br/><br/>
/// There should not be more than seven options in one group. If users need more choices, consider using <c>Suggestion</c> or <c>Select</c>.
/// If there is only one choice to make, a <c>Switch</c> or a <c>Checkbox</c> may be more suitable.
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
///       <b>value</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Value of the <c>input</c> element.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>label</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The label for the <c>Radio</c> input.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>description</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The description for the <c>Radio</c> input, providing additional information about the option.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>disabled</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Disables the <c>Radio</c> input.<br/><b>Note</b>: Avoid using if possible for accessibility purposes.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>readonly</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Toggle <c>Radio</c> input read-only state.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>data-indeterminate</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Indeterminate state for checkbox inputs. Only works when used inside <see cref="Field"/> component.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Radio : ComponentBase
{
    /// <summary>
    /// Gets or sets the unique identifier for the <c>Radio</c> component.
    /// This identifier is used to associate the radio input element with its related attributes
    /// and to provide a distinct identifier within a group of <c>Radio</c> components.
    /// </summary>
    /// <remarks>
    /// If not explicitly set, a unique identifier will be auto-generated using the <see cref="Cryptography.GenerateId()"/> method.
    /// </remarks>
    [Parameter, Required]
    public required string Id { get; set; } = Cryptography.GenerateId();

    private string? DescriptionId { get; set; }

    private string? Label { get; set; }
    private string? Description { get; set; }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
        .AddIdentity(Id)
        .AddClasses("ds-input")
        .AddAttribute("type", "radio")
        .Transform(attr =>
        {
            // Disables the radio label interaction if readonly is set
            attr.ContainsKey("readonly", result =>
            {
                if (result)
                {
                    attr.AddAttribute("onclick", "return false");
                }
            });

            Label = attr.ConsumeAttribute("label");
            Description = attr.ConsumeAttribute("description");
            DescriptionId = string.IsNullOrWhiteSpace(Description) ? null : $"{Id}:description";

            return attr;
        })
        .AddAttribute("aria-describedby", string.IsNullOrWhiteSpace(Description) ? null : DescriptionId);
}