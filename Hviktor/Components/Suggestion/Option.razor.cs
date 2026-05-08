using Hviktor.Models.Suggestion;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Suggestion;

/// <summary>
/// Renders a selectable option inside a <see cref="Hviktor.Components.Suggestion.Suggestion"/> list.
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
///       <b>disabled</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: This attribute is controlled by the parent <see cref="Suggestion"/> components <c>readonly</c> or <c>disabled</c> properties.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Option : AsyncSuggestionChildBase
{
    /// <summary>
    /// The content to be rendered inside the <see cref="Suggestion.Option"/> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddAttribute("role", "option")
        .AddAttribute("disabled", ReadOnly || Disabled ? true : null);
}