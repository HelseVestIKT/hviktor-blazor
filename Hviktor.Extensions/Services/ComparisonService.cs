using Hviktor.Abstractions.Interfaces.Services;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Provides comparison utilities for various types, including strings, numbers, dates, and enumerables.
/// </summary>
public class ComparisonService : IComparisonService
{
    /// <summary>
    /// Specifies the string comparison type used for case-insensitive comparisons.
    /// </summary>
    public StringComparison StringComparison { get; } = StringComparison.CurrentCultureIgnoreCase;

    /// <summary>
    /// Specifies the string comparer used for case-insensitive comparisons.
    /// </summary>
    public StringComparer StringComparer { get; } = StringComparer.CurrentCultureIgnoreCase;
}