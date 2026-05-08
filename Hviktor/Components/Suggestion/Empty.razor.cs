using Hviktor.Models.Suggestion;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Suggestion;

/// <summary>
/// Renders a placeholder option displayed when no suggestions match the user's input.
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
///       <b>data-empty</b>: <see cref="string"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="string.Empty"/><br/>
///       <b>Description</b>: The presence of the data-empty attribute is used to identify this element as the empty state placeholder in the <see cref="Suggestion"/> component.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>value</b>: <see cref="string"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="string.Empty"/><br/>
///       <b>Description</b>: The value attribute is used to indicate an empty state in the <see cref="Suggestion"/> component.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>hidden</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="true"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/hidden">HTML hidden attribute (MDN)</see>.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Empty : AsyncSuggestionChildBase
{
    /// <summary>
    /// The content to be rendered inside the <see cref="Suggestion.Empty"/> component,
    /// when no results are found in the <see cref="Hviktor.Components.Suggestion.Suggestion">Suggestion</see> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary()
        .AddAttribute("data-empty", "")
        .AddAttribute("value", "")
        .AddAttribute("hidden", true);
}