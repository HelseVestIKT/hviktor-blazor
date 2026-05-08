using Hviktor.Abstractions.Interfaces.Components;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Models;

/// <summary>
/// Base class for components that must be nested inside a parent cascading component.
/// Throws an exception if the parent is not found.
/// </summary>
/// <typeparam name="T">The type of the required parent component.</typeparam>
public abstract class NestedComponentBase<T> : HtmlAttributesComponentBase, ICascadingComponent where T : ICascadingComponent
{
    /// <summary>
    /// A cascading parameter representing the parent component of type <typeparamref name="T"/>.
    /// This property is automatically set by the Blazor runtime when the child component is rendered within a parent component that provides a cascading value of type <typeparamref name="T"/>.
    /// Throws an exception during initialization if the parent is not found.
    /// </summary>
    /// <remarks>
    /// The parent component must implement the <see cref="Hviktor.Abstractions.Interfaces.Components.ICascadingComponent"/> interface.
    /// </remarks>
    [CascadingParameter]
    public T? Parent { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        if (Parent is null)
        {
            throw new InvalidOperationException("Parent must be of type \"" + typeof(T).FullName + "\" or a derived type.");
        }

        StateHasChanged();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Override this method to perform any cleanup logic.
    /// </summary>
    /// <param name="disposing">Indicates whether the method is called from a Dispose method or from a finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        // Cleanup
    }
}