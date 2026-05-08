using Hviktor.Models.Suggestion;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Suggestion;

/// <summary>
/// Represents a list component that is a child of an asynchronous suggestion component.
/// The list component is used to display the list of suggestions.
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
///       <b>nofilter</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: This attribute is controlled by the parent <see cref="Suggestion"/> components <c>filter</c> property.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class List : AsyncSuggestionChildBase
{
    /// <summary>
    /// The content to be rendered inside the <see cref="Suggestion.List"/> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var dict = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddAttribute("role", "listbox")
            .AddAttribute("popover", "manual");

        if (Parent != null)
        {
            dict.AddIdentity($"{Parent.InternalId}-list");
        }

        if (Parent is { InternalFilter: false })
        {
            dict.AddDataAttribute("nofilter", true);
        }

        return dict;
    }
}