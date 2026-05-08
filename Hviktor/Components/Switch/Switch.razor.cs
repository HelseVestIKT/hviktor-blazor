using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Switch;

/// <summary>
/// <c>Switch</c> gives users a choice between two alternatives.
/// The switch can either be turned off or on and must always be set with a default choice.
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
///       <b>value</b>: <see cref="string"/>? | <see cref="string"/>[]?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see langword="string"/> | <see langword="int"/> | <see langword="readonly"/> | <see cref="string"/>[]<br/>
///       <b>Description</b>: Represent the data associated with the <see cref="Switch"/>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>position</b>: <see cref="Position"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Position.Start"/><br/>
///       <b>Allowed</b>: <see cref="Position.Start"/> | <see cref="Position.End"/><br/>
///       <b>Description</b>: Position of <see cref="Switch"/>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>label</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Label for <see cref="Switch"/>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>description</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Description for <see cref="Hviktor.Components.Field.Field">Field</see>.
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
///       <b>Description</b>: Whether the <see cref="Switch"/> is readonly or not.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <use>
/// Use <c>Switch</c> when:
/// <list type="bullet">
///   <item>The choice controls a function or setting that can be turned on or off</item>
///   <item>The change takes effect immediately when the user toggles the switch</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Switch</c> when:
/// <list type="bullet">
///   <item>Users are answering questions in forms, use <see cref="Radio"/> or <see cref="Checkbox"/> instead</item>
///   <item>Content should switch between two categories, use <see cref="ToggleGroup"/> instead</item>
///   <item>Content should be filtered, use <see cref="Chip"/> instead</item>
/// </list>
/// </avoid>
/// <guidelines>
/// Use the <c>Switch</c> to turn settings, functions, or notifications on or off.
/// It provides users with a clear visual indication of state.
/// </guidelines>
public partial class Switch : ComponentBase
{
    [Inject] private IPositionService PositionService { get; set; } = null!;

    /// <summary>
    /// Occurs when the checked state of the switch changes.
    /// This event is triggered whenever the user toggles the switch,
    /// providing the updated boolean value representing the new checked state.
    /// </summary>
    [Parameter]
    public EventCallback<bool> CheckedChanged { get; set; }

    private string? internalPosition;

    private string? label;
    private string? forId;
    private string? description;

    private Dictionary<string, object?>? preComputedAttributes;

    /// <summary>
    /// Gets or sets a collection of additional attributes that will be applied to the created element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        preComputedAttributes = null;
        ComputeAttributes();
    }

    private Dictionary<string, object?> ComputeAttributes()
    {
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddIdentity(Cryptography.GenerateId())
            .AddAttribute("type", "checkbox")
            .AddAttribute("role", "switch");

        EnumValue<Position> position = builder.ConsumeAttribute("position");
        internalPosition = PositionService.GetDataAttribute(position, Position.Start);

        // Disables the switch label interaction if readonly is set
        builder.ContainsKey("readonly", result =>
        {
            if (result)
            {
                builder.AddAttribute("onclick", "return false");
            }
        });

        label = builder.ConsumeAttribute("label");
        description = builder.ConsumeAttribute("description");
        if (label is not null)
        {
            var id = builder.TryGetValue("id", out var idObj) ? idObj?.ToString() ?? string.Empty : string.Empty;
            forId = id;
        }

        preComputedAttributes = builder;
        return preComputedAttributes;
    }
}