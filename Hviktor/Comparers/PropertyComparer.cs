using System.Collections;
using System.Reflection;

namespace Hviktor.Comparers;

/// <summary>
/// Compares two items based on a specified property using reflection.
/// </summary>
/// <param name="property">The property to compare.</param>
/// <param name="stringComparison">The string comparison type for fallback comparisons.</param>
/// <typeparam name="TItem">The type of item being compared.</typeparam>
public class PropertyComparer<TItem>(PropertyInfo property, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase) : IComparer<TItem?>
{
    /// <summary>
    /// Compares two items based on the specified property.
    /// </summary>
    /// <param name="x">The first item to compare.</param>
    /// <param name="y">The second item to compare.</param>
    /// <returns>A signed integer indicating the relative order of the items.</returns>
    public int Compare(TItem? x, TItem? y)
    {
        var nullResult = CompareNullItems(x, y);
        if (nullResult.HasValue)
        {
            return nullResult.Value;
        }

        var valueX = property.GetValue(x);
        var valueY = property.GetValue(y);

        var nullValueResult = CompareNullValues(valueX, valueY);
        return nullValueResult ?? ComparePropertyValues(property.PropertyType, valueX!, valueY!);
    }

    private static int? CompareNullItems(TItem? x, TItem? y)
        => PropertyComparerHelpers.CompareNullPair(
            xIsNull: x is null,
            yIsNull: y is null
        );

    private static int? CompareNullValues(object? valueX, object? valueY)
        => PropertyComparerHelpers.CompareNullPair(valueX == null, valueY == null);

    private int ComparePropertyValues(Type type, object valueX, object valueY)
    {
        if (PropertyComparerHelpers.TypeComparers.TryGetValue(type, out var comparer))
        {
            return comparer(valueX, valueY);
        }

        if (typeof(IEnumerable).IsAssignableFrom(type) && type != typeof(string))
        {
            return CompareEnumerables(valueX, valueY);
        }

        return CompareStrings(valueX, valueY);
    }

    private int CompareStrings(object valueX, object valueY)
        => string.Compare(valueX.ToString(), valueY.ToString(), stringComparison);

    private int CompareEnumerables(object valueX, object valueY)
    {
        var stringX = string.Join(",", ((IEnumerable)valueX).Cast<object>().Select(v => v.ToString()));
        var stringY = string.Join(",", ((IEnumerable)valueY).Cast<object>().Select(v => v.ToString()));
        return CompareStrings(stringX, stringY);
    }
}