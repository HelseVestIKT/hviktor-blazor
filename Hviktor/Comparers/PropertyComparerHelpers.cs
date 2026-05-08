namespace Hviktor.Comparers;

/// <summary>
/// Shared type comparison lookup for <see cref="PropertyComparer{TItem}"/>.
/// </summary>
internal static class PropertyComparerHelpers
{
    internal static readonly Dictionary<Type, Func<object, object, int>> TypeComparers = new()
    {
        [typeof(int)] = CompareValues<int>,
        [typeof(int?)] = CompareValues<int>,
        [typeof(double)] = CompareValues<double>,
        [typeof(double?)] = CompareValues<double>,
        [typeof(float)] = CompareValues<float>,
        [typeof(float?)] = CompareValues<float>,
        [typeof(decimal)] = CompareValues<decimal>,
        [typeof(decimal?)] = CompareValues<decimal>,
        [typeof(DateTime)] = CompareValues<DateTime>,
        [typeof(DateTime?)] = CompareValues<DateTime>,
        [typeof(DateTimeOffset)] = CompareValues<DateTimeOffset>,
        [typeof(DateTimeOffset?)] = CompareValues<DateTimeOffset>,
    };

    private static int CompareValues<TValue>(object valueX, object valueY) where TValue : IComparable<TValue> => ((TValue)valueX).CompareTo((TValue)valueY);

    /// <summary>
    /// Returns a comparison result when one or both values are null/default.
    /// Returns <c>null</c> when neither is null and comparison should continue.
    /// </summary>
    internal static int? CompareNullPair(bool xIsNull, bool yIsNull)
    {
        if (xIsNull && yIsNull)
        {
            return 0;
        }

        if (xIsNull)
        {
            return -1;
        }

        if (yIsNull)
        {
            return 1;
        }

        return null;
    }
}