using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Enums.Table;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Comparers;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using Table;

namespace Hviktor.Components.Table;

/// <summary>
/// <c>Table</c> is used to display structured information in a neat and organized manner.
/// Tables can make it easier for users to scan and compare information.
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
///       <b>color</b>: <see cref="Color"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Sets the color theme of the table. This can be used to apply different color schemes to the table for better visual distinction.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>size</b>: <see cref="Size"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Sets the size of the table. This can be used to adjust the appearance of the table based on the available space.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>sticky-header</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Enables sticky header for the table.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>border</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Enables border around the table.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>hover</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Enables hover effect on table rows.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>zebra</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Enables zebra striping for the table rows.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <use>
/// Use <c>Table</c> when:
/// <list type="bullet">
///   <item>The goal is to help users compare information easily</item>
///   <item>The data needs a clear structure with rows and columns</item>
///   <item>The content can benefit from sorting</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Table</c> when:
/// <list type="bullet">
///   <item>The purpose is to create layout or visually organise content on a page</item>
///   <item>The information is better suited to a list or card view</item>
///   <item>The content is very limited, for example only one column or very few data points without a need for comparison</item>
///   <item>The table becomes difficult to read on mobile because the data set is large</item>
/// </list>
/// </avoid>
/// <guidelines>
/// Use the <c>Table</c> to structure and present data clearly in rows and columns.
/// <list type="bullet">
///   <item>Content in tables should be left-aligned, except for numbers, which should be right-aligned to make comparison easier.</item>
///   <item>Use <see cref="HeaderCell"/> for header cells, not <see cref="Cell"/>. A cell counts as a header when it describes the content of the same row or column.</item>
///   <item>In table rows with several actions, you can use a menu to save space if the actions do not need to be visible at all times.</item>
/// </list>
/// </guidelines>
public partial class Table : ComponentBase
{
    [Inject] private IColorService ColorService { get; set; } = null!;

    /// <summary>
    /// The Id of the table, used to identify the table in the DOM and for accessibility purposes.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Cryptography.GenerateId();

    /// <summary>
    /// The child content of the table component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private Dictionary<string, SortOrderType> SortKeys { get; } = [];

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
        .AddIdentity(Id)
        .AddClasses("ds-table")
        .Transform(attr =>
        {
            EnumValue<Color> color = attr.ConsumeAttribute("color") ?? attr.ConsumeAttribute("data-color");
            if (!color.IsEmpty)
            {
                attr.AddDataAttribute("color", ColorService.GetDataAttribute(color));
            }

            CssBoolean stickyHeader = attr.ConsumeAttribute("stickyHeader")
                                      ?? attr.ConsumeAttribute("data-stickyHeader")
                                      ?? attr.ConsumeAttribute("sticky-header")
                                      ?? attr.ConsumeAttribute("data-sticky-header");
            if (stickyHeader.IsTruthy)
            {
                attr.AddDataAttribute("sticky-header", "true");
            }

            CssBoolean border = attr.ConsumeAttribute("border") ?? attr.ConsumeAttribute("data-border");
            if (border.IsTruthy)
            {
                attr.AddDataAttribute("border", "true");
            }

            CssBoolean hover = attr.ConsumeAttribute("hover") ?? attr.ConsumeAttribute("data-hover");
            if (hover.IsTruthy)
            {
                attr.AddDataAttribute("hover", "true");
            }

            CssBoolean zebra = attr.ConsumeAttribute("zebra") ?? attr.ConsumeAttribute("data-zebra");
            if (zebra.IsTruthy)
            {
                attr.AddDataAttribute("zebra", "true");
            }


            return attr;
        });

    #region Sorting

    /// <summary>
    /// Sets the sort key value for a given key and direction.<br/>
    /// If the key already exists, it updates the direction.<br/>
    /// If the key does not exist, it adds a new key-value pair to the <see cref="SortKeys"/> dictionary.
    /// <br/>
    /// This method is used to manage the sorting state of the table.
    /// </summary>
    /// <param name="key">
    /// The key to set the sort value for. This should match a property name in the data model.
    /// </param>
    /// <param name="direction">
    /// The direction of the sort order, which can be <see cref="SortOrderType.Ascending"/>, <see cref="SortOrderType.Descending"/>, or <see cref="SortOrderType.None"/>.
    /// </param>
    public void SetSortKeyValue(string key, SortOrderType direction)
    {
        if (!SortKeys.TryAdd(key, direction))
        {
            SortKeys[key] = direction;
        }
    }

    /// <summary>
    /// Gets the sort key value for a given key.<br/>
    /// If the key does not exist, it returns <see cref="SortOrderType.None"/>.
    /// <br/>
    /// This method is used to retrieve the current sort order for a specific key in the <see cref="SortKeys"/> dictionary.
    /// </summary>
    /// <param name="key">
    /// The key to get the sort value for. This should match a property name in the data model.
    /// </param>
    /// <returns>
    /// The <see cref="SortOrderType"/> for the specified key.
    /// </returns>
    public SortOrderType GetSortKeyValue(string key)
    {
        SortKeys.TryGetValue(key, out var sortOrder);
        return sortOrder;
    }

    /// <summary>
    /// Sorts the provided data by the specified key and direction.<br/>
    /// If the key does not exist in the data model, it returns the original data without sorting.<br/>
    /// If the data is empty, it returns an empty array.<br/>
    /// This method uses the <see cref="PropertyComparer{TItem}"/> to compare the property values of the data items.<br/>
    /// The sorting is done using LINQ's <c>OrderBy</c> and <c>OrderByDescending</c> methods based on the specified <paramref name="sortOrder"/>.<br/>
    /// If no <paramref name="propertyComparer"/> is provided, a default comparer is created based on the property type.<br/>
    /// The method returns an <see cref="IEnumerable{T}"/> of the sorted data.<br/>
    /// If the <paramref name="sortOrder"/> is <see cref="SortOrderType.None"/>, no sorting is applied and the original data is returned.
    /// </summary>
    /// <param name="data">
    /// The data to be sorted, which should be an <see cref="IEnumerable{T}"/> of items where <c>T</c> is the type of the data model.<br/>
    /// The data can be an array, list, or any other enumerable collection.
    /// </param>
    /// <param name="key">
    /// The key to sort the data by. This should match a property name in the data model.
    /// </param>
    /// <param name="sortOrder">
    /// The direction of the sort order, which can be <see cref="SortOrderType.Ascending"/>, <see cref="SortOrderType.Descending"/>, or <see cref="SortOrderType.None"/>.
    /// </param>
    /// <param name="propertyComparer">
    /// An optional <see cref="PropertyComparer{T}"/> to use for comparing the property values of the data items.
    /// </param>
    /// <typeparam name="T">
    /// The type of the data model items in the <paramref name="data"/> collection.
    /// </typeparam>
    /// <returns>
    /// An <see cref="IEnumerable{T}"/> of the sorted data.
    /// </returns>
    public IEnumerable<T> SortData<T>(IEnumerable<T> data, string key, SortOrderType sortOrder, PropertyComparer<T>? propertyComparer = null)
    {
        SetSortKeyValue(key, sortOrder);
        foreach (var sortKey in SortKeys.Where(sortKey => sortKey.Key != key))
        {
            SetSortKeyValue(sortKey.Key, SortOrderType.None);
        }

        var sortedTable = data as T[] ?? data.ToArray();
        if (sortedTable.Length == 0)
        {
            return [];
        }

        var property = typeof(T).GetProperty(key);
        if (property is null)
        {
            return sortedTable;
        }

        var comparer = propertyComparer ?? new PropertyComparer<T>(property);
        var ordered = sortOrder switch
        {
            SortOrderType.Ascending => sortedTable.OrderBy(item => item, comparer).ToArray(),
            SortOrderType.Descending => sortedTable.OrderByDescending(item => item, comparer).ToArray(),
            _ => sortedTable // No sorting applied
        };
        return ordered;
    }

    #endregion
}