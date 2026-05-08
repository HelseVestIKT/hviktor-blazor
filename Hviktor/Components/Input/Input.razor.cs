using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Hviktor.Components.Input;

/// <summary>
/// <c>Input</c> is a form element used to collect user data.
/// It offers basic functionality and is ideal when you need full control over the component's layout and validation, making it suitable for building custom elements.
/// </summary>
/// <use>
/// Use <c>Input</c> when:
/// <list type="bullet">
///   <item>You need a basic input field without additional functionality</item>
///   <item>You are building custom fields or composite components</item>
///   <item>You want to control your own logic for error handling and description</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Input</c> when:
/// <list type="bullet">
///   <item>You need a complete form field with label, description and validation message, use `Textfield` instead</item>
///   <item>The field requires additional logic already provided by higher-level components such as `Textfield`</item>
///   <item>You need to semantically group multiple fields; use <c>Inputset</c> instead</item>
/// </list>
/// </avoid>
/// <guidelines>See the guidelines for <see cref="Textfield"/>.</guidelines>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Attribute</term>
///     <description>Description</description>
///   </listheader>
///   <item>
///     <term>
///       <b>type</b>: <see cref="InputType"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="InputType.Text"/><br/>
///       <b>Allowed</b>: <see cref="InputType.Number"/> | <see cref="InputType.Hidden"/> | <see cref="InputType.Color"/> | <see cref="InputType.Date"/> | <see cref="InputType.DateTimeLocal"/> | <see cref="InputType.Email"/> | <see cref="InputType.File"/> | <see cref="InputType.Month"/> | <see cref="InputType.Password"/> | <see cref="InputType.Search"/> | <see cref="InputType.Tel"/> | <see cref="InputType.Text"/> | <see cref="InputType.Time"/> | <see cref="InputType.Url"/> | <see cref="InputType.Week"/><br/>
///       <b>Description</b>: The type of the input field.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="int"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Defines the width of <c>Input</c> in count of characters.
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
///       <b>Description</b>: Disables the input field.<br/><b>Note</b>: Avoid using if possible for accessibility purposes.
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
///       <b>Description</b>: Makes the input field read-only.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>role</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The aria-role of the input field.<br/>
///                     Set role, i.e. `switch` when `checkbox` or `radio`
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
///       <b>Description</b>: Indeterminate state for <see cref="Checkbox"/> inputs Only works when used inside <see cref="Field"/> component.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Input : ComponentBase
{
    [Inject] private ILogger<Input> Logger { get; set; } = null!;
    [Inject] private IInputTypeService InputTypeService { get; set; } = null!;

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddIdentity(Cryptography.GenerateId())
            .AddClasses("ds-input");

        EnumValue<InputType> inputType = builder.ConsumeAttribute("type");
        builder.AddAttribute("type", InputTypeService.GetDataAttribute(inputType, InputType.Text));

        var defaultValue = builder.ConsumeAttribute("defaultValue");
        if (!string.IsNullOrEmpty(defaultValue))
        {
            Logger.LogWarning("'defaultValue' is deprecated. Use 'value' instead.");
            builder.AddAttribute("value", defaultValue);
        }

        return builder;
    }
}