using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using PopoverContext = Popover.TriggerContext;

namespace Hviktor.Components.Dropdown;

/// <summary>
/// <c>Dropdown</c> is a generic dropdown list. It lays the foundation for building menus and lists.
/// </summary>
/// <use>
/// Use <c>Dropdown</c> when:
/// <list type="bullet">
/// <item>You want to offer multiple options without taking up much space in the interface.</item>
/// <item>The options are secondary but should still be easily accessible.</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Dropdown</c> when:
/// <list type="bullet">
/// <item>An action is primary or frequently used; use a visible Button instead .</item>
/// <item>There is only one option, as a dropdown adds unnecessary complexity .</item>
/// </list>
/// </avoid>
/// <guidelines>
/// <c>Dropdown</c> does not create a semantic menu, but provides the foundation for building one yourself. Read the "Menu and Menubar Pattern" on w3.org for more information on how to create accessible menus.
/// 
/// Keep the number of items manageable
/// Use headings to group related items
/// Avoid multiple levels of dropdowns (nested menus)
/// <list type="bullet">
/// <item>Use <c>Dropdown</c> for actions or navigation that do not need to be visible at all times.</item>
/// <item>Keep the number of items manageable</item>
/// <item>Use headings to group related items</item>
/// <item>Avoid multiple levels of dropdowns (nested menus)</item>
/// </list>
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
///       <b>id</b>: <see cref="string"/><br/>
///       <i>(required)</i>
///     </term>
///     <description>
///       <b>Description</b>: id to connect the trigger with the <see cref="Popover"/> - required when not using <see cref="PopoverContext">Popover.TriggerContext</see>.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>open</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Controls open-state. Using this removes automatic control of open-state.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>autoPlacement</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="true"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Whether to enable auto placement.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>placement</b>: <see cref="Placement"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Placement.BottomEnd"/><br/>
///       <b>Allowed</b>: <see cref="Placement.BottomEnd"/> | <see cref="Placement.BottomStart"/> | <see cref="Placement.Bottom"/> | <see cref="Placement.TopStart"/> | <see cref="Placement.TopEnd"/> | <see cref="Placement.Top"/> | <see cref="Placement.LeftStart"/> | <see cref="Placement.LeftEnd"/> | <see cref="Placement.Left"/> | <see cref="Placement.RightStart"/> | <see cref="Placement.RightEnd"/> | <see cref="Placement.Right"/><br/>"
///       <b>Description</b>: Preferred placement of the dropdown.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Dropdown : PopoverBase
{
    [Inject] private ILogger<Dropdown> Logger { get; set; } = null!;

    /// <inheritdoc/>
    protected override string ComponentName => nameof(Dropdown);

    /// <inheritdoc/>
    protected override ILogger ComponentLogger => Logger;

    /// <inheritdoc/>
    protected override IDisposable CreateDotNetObjectReference()
        => DotNetObjectReference.Create(this);

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddClasses("ds-dropdown");
}