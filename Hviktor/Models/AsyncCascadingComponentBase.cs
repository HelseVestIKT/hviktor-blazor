using Hviktor.Abstractions.Interfaces.Components;

namespace Hviktor.Models;

/// <summary>
/// Base class for components that provide cascading context to child components with async disposal support.
/// </summary>
public abstract class AsyncCascadingComponentBase : HtmlAttributesComponentBase, IAsyncCascadingComponent
{
    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Override this method to perform any cleanup logic.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous dispose operation.</returns>
    protected virtual ValueTask DisposeAsyncCore()
    {
        return ValueTask.CompletedTask;
    }
}