using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Search;

/// <summary>
/// The Clear component represents a reset button within a search form, allowing users to clear the search input and reset the form to its default state.
/// </summary>
public partial class Clear : AsyncNestedComponentBase<Hviktor.Components.Search.Search>
{
    /// <summary>
    /// Represents the content to be rendered inside the Clear component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddIdentity(() => Parent is not null, $"{Parent?.Id}-clear")
        .AddAttribute("type", "reset")
        .AddAttribute("aria-label", "Tøm")
        .AddDataAttribute("icon", "true");
}