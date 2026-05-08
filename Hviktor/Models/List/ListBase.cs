using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Models.List;

/// <summary>
/// <c>List</c> is used to present content in a clear and structured way, for example to summarize main points or show the user which steps must be followed in a specific order.
/// </summary>
/// <use>
/// Use <c>List</c> when:
/// <list type="bullet">
/// <item>Users need a quick overview of content in longer texts</item>
/// <item>Several steps in a process should be listed</item>
/// <item>Criteria need to be presented clearly</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>List</c> when:
/// <list type="bullet">
/// <item>Users are expected to read longer blocks of text</item>
/// <item>Several groups of information need to be compared, use a Table instead</item>
/// </list>
/// </avoid>
/// <guidelines>
/// Lists make content easier to scan and understand. Use ordered lists when the sequence matters, and unordered lists when it does not.<br/>
/// Be mindful of structure and avoid long lists. Consider splitting content into several shorter lists to improve readability.<br/><br/>
/// <b>List or table?</b><br/>
/// Lists and tables serve different purposes. Using the correct component makes the experience clearer for all users, especially for those with cognitive or learning difficulties, or those using screen readers.
/// <list type="bullet">
/// <item>A list presents related information in a clear, scannable way.</item>
/// <item>A table allows users to view and compare multiple groups of information.</item>
/// </list>
/// </guidelines>
public abstract class ListBase : CascadingComponentBase
{
    /// <summary>
    /// The content to be displayed inside the list.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddClasses("ds-list");

        return builder;
    }
}