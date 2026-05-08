using System.ComponentModel.DataAnnotations;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Components.Typography;
using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using FieldDescription = Field.Description;
using FieldCounter = Field.Counter;

namespace Hviktor.Components.Field;

/// <summary>
/// <c>Field</c> er et hjelpemiddel for å automatisk koble et felt sammen med <see cref="Label"/>, <see cref="FieldDescription">Field.Description</see>, <see cref="ValidationMessage"/> og <see cref="FieldCounter">Field.Counter</see>.
/// </summary>
/// <use>
/// Use <c>Field</c> when:
/// <list type="bullet">
///   <item>You need to ensure that <see cref="Label"/>, <see cref="FieldDescription">Field.Description</see>, <see cref="ValidationMessage"/> and <see cref="FieldCounter">Field.Counter</see> are correctly associated with a field.</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Field</c> when:
/// <list type="bullet">
///   <item>You need to semantically group multiple fields; use <see cref="Fieldset"/> instead</item>
/// </list>
/// </avoid>
/// <guidelines>
/// <c>Field</c> helps you structure form fields in an accessible and consistent way. It automatically links <see cref="Label"/>, <see cref="FieldDescription">Field.Description</see>, <see cref="ValidationMessage"/> and <see cref="FieldCounter">Field.Counter</see> to the relevant field using the correct ARIA attributes.
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
///       <b>position</b>: <see cref="Position"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Position.Start"/><br/>
///       <b>Allowed</b>: <see cref="Position.Start"/> | <see cref="Position.End"/><br/>
///       <b>Description</b>: Position of toggle inputs (<see cref="Radio"/>, <see cref="Checkbox"/>, <see cref="Switch"/>) in field.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Field : CascadingComponentBase
{
    [Inject] private IPositionService PositionService { get; set; } = null!;

    /// <summary>
    /// The ChildContent to render inside the Field.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// The unique identifier for the Field component.<br/>
    /// The ID is used to associate the Field with its Label, Description, ValidationMessage and Counter.
    /// </summary>
    [Parameter, Required]
    public string Id { get; set; } = Cryptography.GenerateId();

    internal ElementReference ElementRef { get; private set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddIdentity(Id)
            .AddClasses("ds-field");

        EnumValue<Position> position = builder.ConsumeAttribute("position") ?? builder.ConsumeAttribute("data-position");
        builder.AddDataAttribute("position", PositionService.GetDataAttribute(position, Position.Start));

        return builder;
    }
}