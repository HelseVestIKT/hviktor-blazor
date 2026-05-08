using Hviktor.Abstractions.Interfaces.Components;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Models;

/// <summary>
/// Base class for triggerable components that can optionally be nested inside a parent cascading component.
/// The parent is optional - components can work standalone with their own Id or inside a TriggerContext.
/// Use this for components like Popover and Dropdown that support both usage patterns.
/// </summary>
/// <typeparam name="T">The type of the optional parent component.</typeparam>
public abstract class OptionalNestedComponentBase<T> : HtmlAttributesComponentBase, ICascadingComponent where T : ICascadingComponent
{
    /// <summary>
    /// A cascading parameter representing the parent component of type <typeparamref name="T"/>.
    /// This property is automatically set by the Blazor runtime when the child component is rendered within a parent component that provides a cascading value of type <typeparamref name="T"/>.
    /// It allows the child component to access properties and methods of the parent component if it exists, while still functioning independently if the parent is not present.
    /// </summary>
    /// <remarks>
    /// The parent component must implement the <see cref="Hviktor.Abstractions.Interfaces.Components.ICascadingComponent"/> interface.
    /// </remarks>
    [CascadingParameter] public T? Parent { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        // Parent is optional for triggerable components
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