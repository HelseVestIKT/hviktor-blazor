using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Chip;

/// <summary>
/// The <c>Checkbox</c> class represents a customizable checkbox component
/// that extends the functionality of the <see cref="Radio"/> class. It includes
/// specific attributes and behavior tailored for <c>Checkbox</c> input elements.
/// </summary>
/// <remarks>
/// This class builds upon the <see cref="Radio"/> class by modifying and enhancing
/// its attributes to properly support the semantics and functionality
/// of <c>Checkbox</c> inputs.
/// </remarks>
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
///       <b>Description</b>: Whether the <see cref="Chip.Checkbox"/> is checked or not.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>name</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Used to identify the <see cref="Hviktor.Components.Chip.Chip">Chip</see>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>value</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Represent the data associated with the <see cref="Hviktor.Components.Chip.Chip">Chip</see>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>@onchange</b>: <see cref="EventCallback"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Event callback that is triggered when the checked state of the <see cref="Hviktor.Components.Chip.Chip">Chip</see> changes.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Checkbox : Radio
{
    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddAttribute("type", "checkbox");
}