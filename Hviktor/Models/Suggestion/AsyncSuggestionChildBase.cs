namespace Hviktor.Models.Suggestion;

/// <summary>
/// Base class for child components nested inside a <see cref="Components.Suggestion.Suggestion"/> parent.
/// Inherits cascading parent resolution and propagates readonly/disabled state.
/// </summary>
public abstract class AsyncSuggestionChildBase : AsyncNestedComponentBase<Components.Suggestion.Suggestion>
{
    /// <summary>
    /// Whether the parent suggestion is in a disabled state.
    /// </summary>
    protected bool Disabled { get; private set; }

    /// <summary>
    /// Whether the parent suggestion is in a read-only state.
    /// </summary>
    protected bool ReadOnly { get; private set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (Parent is not null)
        {
            ReadOnly = Parent.ReadOnly;
            Disabled = Parent.Disabled;
        }
    }
}