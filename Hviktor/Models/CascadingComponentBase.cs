using Hviktor.Abstractions.Interfaces.Components;

namespace Hviktor.Models;

/// <summary>
/// Base class for components that provide cascading context to child components.
/// </summary>
public abstract class CascadingComponentBase : HtmlAttributesComponentBase, ICascadingComponent
{
    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="CascadingComponentBase"/> class.
    /// Invokes the cleanup process and suppresses finalization to optimize garbage collection.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="CascadingComponentBase"/> class.
    /// Invokes the cleanup process and suppresses finalization to optimize garbage collection.
    /// </summary>
    /// <param name="disposing">Indicates whether the method is called from the <see cref="Dispose()"/> method or from the finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        // Cleanup
    }
}