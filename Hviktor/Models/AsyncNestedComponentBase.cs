using Hviktor.Abstractions.Interfaces.Components;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Models;

/// <summary>
/// Base class for components that must be nested inside a parent cascading component with async disposal support.
/// Throws an exception if the parent is not found.
/// </summary>
/// <typeparam name="T">The type of the required parent component.</typeparam>
public abstract class AsyncNestedComponentBase<T> : HtmlAttributesComponentBase, IAsyncCascadingComponent where T : IAsyncCascadingComponent
{
    /// <summary>
    /// A cascading parameter representing the parent component of type <typeparamref name="T"/>.
    /// This property is automatically set by the Blazor runtime when the child component
    /// is rendered inside a matching parent cascading component.
    /// Throws an exception during initialization if the parent is not found.
    /// </summary>
    /// <remarks>
    /// The parent component must implement the <see cref="Hviktor.Abstractions.Interfaces.Components.IAsyncCascadingComponent"/> interface
    /// and match the generic type constraint specified for <typeparamref name="T"/>.
    /// </remarks>
    [CascadingParameter]
    public T? Parent { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException("Parent must be of type " + typeof(T).FullName + " or a derived type.");
        }

        StateHasChanged();
    }

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