using System.Linq.Expressions;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Hviktor.Components.Checkbox;

/// <summary>
/// <c>Checkbox</c> allows users to select one or more options.<br/>
/// It can also be used in situations where the user needs to confirm something.
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
///       <b>checked</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Whether the <see cref="Checkbox"/> is checked or not.
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
///       <b>Description</b>: Whether the <see cref="Checkbox"/> is disabled or not.<br/>
///                     <b>Note</b>: Avoid using if possible for accessibility purposes
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
///       <b>Description</b>: Whether the <see cref="Checkbox"/> is readonly or not. If true, the <see cref="Checkbox"/> cannot be changed by the user.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>allowIndeterminate</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Whether the <see cref="Checkbox"/> allows indeterminate state or not.
///                     If true, the <see cref="Checkbox"/> can be in an indeterminate state, which is visually represented as a dash or minus sign.
///                     This state is typically used to indicate a mixed selection or an intermediate state between checked and unchecked.
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
///       <b>Description</b>: Whether the <see cref="Checkbox"/> is in an indeterminate state or not.<br/><br/>
///                     Only works when <b>allowIndeterminate</b> is set to true.<br/>
///                     This state is typically used to indicate a mixed selection or an intermediate state between checked and unchecked.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Checkbox : ComponentBase
{
    [Inject] private IJsRuntimeService JsRuntimeService { get; set; } = null!;
    [Inject] private IJsObjectReferenceService JsObjectReferenceService { get; set; } = null!;
    [Inject] private ILogger<Checkbox> Logger { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier for the <see cref="Checkbox"/> component.<br/>
    /// The unique identifier is used to associate the <see cref="Checkbox"/> with its label and description elements.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Cryptography.GenerateId();

    private string LabelId => $"{Id}:label";
    private string DescriptionId => $"{Id}:description";

    /// <summary>
    /// Gets or sets the label associated with the <see cref="Checkbox"/> component.<br/>
    /// The label provides a description or identifier for the <see cref="Checkbox"/>, aiding user comprehension.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Gets or sets a description associated with the <see cref="Checkbox"/> component.<br/>
    /// The description provides additional context or information about the <see cref="Checkbox"/>'s purpose or usage.
    /// </summary>
    [Parameter]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets an expression that identifies the bound value.
    /// </summary>
    [Parameter]
    public Expression<Func<bool>>? ValueExpression { get; set; }

    private bool IsIntermediateAllowed { get; set; }

    private ElementReference? ElementRef { get; set; }
    private IJSObjectReference? CheckboxModule { get; set; }

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private bool? isChecked;

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddIdentity(Id)
            .AddAttribute("type", "checkbox")
            .AddClasses("ds-input")
            .AddAttribute("aria-describedby", DescriptionId)
            // Check if allowIndeterminate attribute is present, and set IsIntermediateAllowed accordingly
            .ContainsKey("allowIndeterminate", result => IsIntermediateAllowed = result);

        var isCheckedOrIndeterminate = builder.ConsumeAttribute("checked");
        isChecked = isCheckedOrIndeterminate switch
        {
            not null when bool.TryParse(isCheckedOrIndeterminate, out var parsed) => parsed,
            _ => null
        };
        builder.AddAttribute("checked", isChecked is true ? true : null);

        builder.ContainsKey("readonly", result =>
        {
            if (result)
            {
                builder.AddAttribute("onclick", "return false");
            }
        });

        return builder;
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            CheckboxModule = await JsRuntimeService.ImportAsync<Checkbox>();
        }

        // Always set indeterminate state after render if allowed
        if (IsIntermediateAllowed && CheckboxModule is not null && ElementRef.HasValue)
        {
            await JsObjectReferenceService.InvokeVoidAsync<Checkbox>(CheckboxModule, "setIndeterminateState", ElementRef, isChecked == null);
        }
    }
}