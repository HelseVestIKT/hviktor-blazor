using Hviktor.Abstractions.Enums.Attributes;

// ReSharper disable once CheckNamespace
namespace Dropdown;

/// <summary>
/// The trigger of the <see cref="Hviktor.Components.Dropdown.Dropdown">Dropdown</see>.
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
///       <b>inline</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Will render the trigger as inline text.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>command</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Native invoker commands.
///                     Specifies actions to perform on an element specified by commandfor.
///                     Polyfilled by designsystemet-web and includes a custom --show-non-modal command.
///                     "show-modal", "close", "request-close", "show-popover", "hide-popover", "toggle-popover", "--show-non-modal"
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>commandfor</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Specifies the target element for "command". value is ID of target
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>icon</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Toggle icon only styling, pass icon as children When combined with loading, the loading-icon will be shown instead of the icon.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>loading</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Toggle loading state. Pass an element if you want to display a custom loader.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>variant</b>: <see cref="Variant"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see cref="Variant.Primary"/><br/>
///       <b>Allowed</b>: <see cref="Variant.Primary"/> | <see cref="Variant.Secondary"/> | <see cref="Variant.Tertiary"/><br/>
///       <b>Description</b>: Specify which variant to use.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Trigger : Hviktor.Models.TriggerBase;