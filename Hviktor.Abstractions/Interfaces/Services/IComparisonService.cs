namespace Hviktor.Abstractions.Interfaces.Services;

/// <summary>
/// Provides comparison utilities for various types, including strings, numbers, dates, and enumerables.
/// </summary>
public interface IComparisonService
{
    /// <summary>
    /// Specifies the string comparison type used for case-insensitive comparisons.
    /// </summary>
    StringComparison StringComparison { get; }

    /// <summary>
    /// Specifies the string comparer used for case-insensitive comparisons.
    /// </summary>
    StringComparer StringComparer { get; }
}