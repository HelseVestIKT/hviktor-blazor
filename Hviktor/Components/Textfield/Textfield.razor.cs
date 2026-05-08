using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using FieldCounter = Field.Counter;

namespace Hviktor.Components.Textfield;

/// <summary>
/// <c>Textfield</c> allows users to enter free text or numbers.
/// </summary>
/// <use>
/// Use <c>Textfield</c> when:
/// <list type="bullet">
/// <item>you need a complete form field with label, help text, and validation message</item>
/// <item>the field must be validated either continuously or on submission</item>
/// <item>the value is unique and cannot be selected from a list</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid using Textfield when
/// <list type="bullet">
/// <item>you need a simple input field without form logic, use Input instead</item>
/// <item>the user should choose from a limited set of options, use Radio, Checkbox, Select, or Suggestion instead</item>
/// </list>
/// </avoid>
/// <guidelines>
/// <c>Input</c> is suitable for short, simple text.
/// <c>Textarea</c> is better for more detailed or longer responses.
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
///       <b>label</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The label for the textfield.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>description</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The description for the textfield, providing additional information about the field.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>error</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The error message to display when the textfield is invalid.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>counter</b>: <see cref="int"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Uses <see cref="FieldCounter">Field.Counter</see> to display a character counter. Pass a number to set a limit, or an object to configure the counter.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>multiline</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Use to render a <see cref="Textarea"/> instead of <see cref="Input"/> for multiline support.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>type</b>: <see cref="InputType"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="InputType.Text"/><br/>
///       <b>Allowed</b>: <see cref="InputType.Number"/> | <see cref="InputType.Hidden"/> | <see cref="InputType.Color"/> | <see cref="InputType.Date"/> | <see cref="InputType.DateTimeLocal"/> | <see cref="InputType.Email"/> | <see cref="InputType.File"/> | <see cref="InputType.Month"/> | <see cref="InputType.Password"/> | <see cref="InputType.Search"/> | <see cref="InputType.Tel"/> | <see cref="InputType.Text"/> | <see cref="InputType.Time"/> | <see cref="InputType.Url"/> | <see cref="InputType.Week"/><br/>
///       <b>Description</b>: The type of the textfield, which determines the expected input and may trigger different virtual keyboards on mobile devices.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>prefix</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The prefix text to display before the textfield.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>suffix</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The suffix text to display after the textfield.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="int"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Defines the width of <see cref="Input"/> in count of characters.
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
public partial class Textfield : ComponentBase
{
    [Inject] private IInputTypeService InputTypeService { get; set; } = null!;

    private Dictionary<string, object?>? preComputedAttributes;

    private string? internalId;

    private bool isMultiline;

    private string? internalLabel;
    private string? internalDescription;

    private string? prefix;
    private string? suffix;

    private string? internalValue;
    private string? error;

    private int? counter;

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private const string AriaDescribedByKey = "aria-describedby";

    private Dictionary<string, object?> ComputeAttributes() => preComputedAttributes!;

    private Task UpdateInternalValue(ChangeEventArgs e)
    {
        internalValue = e.Value?.ToString() ?? string.Empty;
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddIdentity(Cryptography.GenerateId())
            .AddClasses("ds-input");
        internalId = builder.GetValue("id") ?? Cryptography.GenerateId();

        internalValue = builder.GetValue("value") ?? string.Empty;

        // Ensures that the component internal oninput event is always present,
        // regardless of whether the user has provided an oninput event.
        builder.CombineEventCallback<ChangeEventArgs>("oninput", this, UpdateInternalValue);

        internalLabel = builder.ConsumeAttribute("label");
        prefix = builder.ConsumeAttribute("prefix");
        suffix = builder.ConsumeAttribute("suffix");

        var counterStr = builder.ConsumeAttribute("counter");
        counter = null;
        if (!string.IsNullOrWhiteSpace(counterStr) && int.TryParse(counterStr, out var counterValue))
        {
            counter = counterValue;
        }

        isMultiline = string.Equals(builder.ConsumeAttribute("multiline"), "true", StringComparison.OrdinalIgnoreCase);

        EnumValue<InputType> type = builder.ConsumeAttribute("type") ?? builder.ConsumeAttribute("data-type");
        if (!type.IsEmpty)
        {
            builder.AddAttribute("type", InputTypeService.GetDataAttribute(type));
        }

        internalDescription = builder.ConsumeAttribute("description");
        if (!string.IsNullOrWhiteSpace(internalDescription))
        {
            var describedby = $"{internalId}:description:1 {internalId}:description:2";
            if (!builder.TryAdd(AriaDescribedByKey, describedby))
            {
                builder[AriaDescribedByKey] += $" {describedby}";
            }
        }

        error = builder.ConsumeAttribute("error");
        if (!string.IsNullOrWhiteSpace(error))
        {
            if (!builder.TryAdd("aria-invalid", "true"))
            {
                builder["aria-invalid"] = "true";
            }

            var describedby = $"{internalId}:validation:1 {internalId}:validation:2";
            if (!builder.TryAdd(AriaDescribedByKey, describedby))
            {
                builder[AriaDescribedByKey] += $" {describedby}";
            }
        }

        preComputedAttributes = builder;
    }
}