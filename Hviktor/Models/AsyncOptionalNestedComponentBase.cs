using Hviktor.Abstractions.Interfaces.Components;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Models;

/// <summary>
/// Base class for triggerable components that can optionally be nested inside a parent cascading component
/// with async disposal support.
/// The parent is optional - components can work standalone with their own Id or inside a TriggerContext.
/// Use this for components like Popover and Dropdown that support both usage patterns.
/// </summary>
/// <typeparam name="T">The type of the optional parent component.</typeparam>
public abstract class AsyncOptionalNestedComponentBase<T> : HtmlAttributesComponentBase, IAsyncCascadingComponent where T : IAsyncCascadingComponent
{
    /// <summary>
    /// A cascading parameter representing the parent component of type <typeparamref name="T"/>.
    /// This property is automatically set by the Blazor runtime when the child component is rendered within a parent component that provides a cascading value of type <typeparamref name="T"/>.
    /// It allows the child component to access properties and methods of the parent component if it exists, while still functioning independently if the parent is not present.
    /// </summary>
    /// <remarks>
    /// The parent component must implement the <see cref="Hviktor.Abstractions.Interfaces.Components.IAsyncCascadingComponent"/> interface to ensure that it supports asynchronous disposal, which is important for proper resource management in Blazor applications.
    /// </remarks>
    [CascadingParameter]
    public T? Parent { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        // Parent is optional for triggerable components
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