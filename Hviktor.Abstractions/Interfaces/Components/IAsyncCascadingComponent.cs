namespace Hviktor.Abstractions.Interfaces.Components;

/// <summary>
/// Represents an asynchronous component that provides cascading context to its child components
/// and supports asynchronous disposal.<br/>
/// It is used to define a contract for components that act as context providers in a component hierarchy.
/// </summary>
public interface IAsyncCascadingComponent : IAsyncDisposable;

