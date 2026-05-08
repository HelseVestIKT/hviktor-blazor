namespace Hviktor.Abstractions.Enums.Table;

/// <summary>
/// Represents the sort order type for table columns.
/// </summary>
public enum SortOrderType
{
    /// <summary>
    /// No sort order is applied.
    /// </summary>
    None,

    /// <summary>
    /// Represents an ascending sort order for table columns, where items are sorted from lowest to highest.
    /// </summary>
    Ascending,

    /// <summary>
    /// Represents a descending sort order for table columns, where items are sorted from highest to lowest.
    /// </summary>
    Descending
}