namespace Documentation.Components.Services;

/// <summary>
/// Shared service that holds the current component search filter and notifies subscribers on changes.
/// </summary>
public sealed class ComponentSearchService
{
    private string filter = string.Empty;

    /// <summary>Raised when <see cref="Filter"/> changes.</summary>
    public event Action? OnFilterChanged;

    /// <summary>Gets or sets the current search filter text.</summary>
    public string Filter
    {
        get => filter;
        set
        {
            if (string.Equals(filter, value, StringComparison.Ordinal))
            {
                return;
            }

            filter = value;
            OnFilterChanged?.Invoke();
        }
    }

    /// <summary>Clears the current filter.</summary>
    public void Clear() => Filter = string.Empty;
}