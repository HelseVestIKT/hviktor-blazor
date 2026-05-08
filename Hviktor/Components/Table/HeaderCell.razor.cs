using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Table;

/// <summary>
/// Represents a cell in a table head, typically containing the name, sorting and filtering actions for a specific column.
/// </summary>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Parameter</term>
///     <description></description>
///   </listheader>
///   <item>
///     <term>
///       <b>sort</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see langword="none"/> | <see langword="ascending"/> | <see langword="descending"/> | <see langword="other"/><br/>
///       <b>Description</b>: Adds a button to the header cell and change aria-sort and icon.<br/>
///                     The value of the "sort" attribute determines the sorting direction for the column.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class HeaderCell : ComponentBase
{
    /// <summary>
    /// The content of the cell, typically the name of the column.<br/>
    /// This can be a simple string or a more complex RenderFragment.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Represents the current sorting direction for the column.<br/>
    /// This property determines whether the sorting is ascending, descending, or not set.
    /// </summary>
    private string SortDirection { get; set; } = "";

    /// <summary>
    /// Specifies the initial sorting direction for the column when sorting is first applied.<br/>
    /// Valid values are typically "ascending" or "descending", and this property influences the default sort behavior.
    /// </summary>
    private string? InitialSortDirection { get; set; }

    private const string Ascending = "ascending";
    private const string Descending = "descending";

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
        => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .Transform(attr =>
            {
                if (attr.Remove("sort", out var sortValue) && InitialSortDirection == null)
                {
                    var strValue = sortValue?.ToString()?.ToLowerInvariant() ?? "";
                    InitialSortDirection = strValue;
                    SortDirection = strValue;
                }

                if (InitialSortDirection != null)
                {
                    attr["aria-sort"] = SortDirection;
                }

                return attr;
            });

    private void HandleSort()
    {
        if (InitialSortDirection is Ascending or Descending)
        {
            SortDirection = SortDirection == Ascending ? Descending : Ascending;
            return;
        }

        SortDirection = SortDirection switch
        {
            Ascending => Descending,
            Descending => InitialSortDirection ?? "",
            _ => Ascending
        };
    }
}