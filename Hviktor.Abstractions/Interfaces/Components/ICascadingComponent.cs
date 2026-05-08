namespace Hviktor.Abstractions.Interfaces.Components;

/// <summary>
/// Represents a component that provides cascading context to its child components.<br/>
/// It is used to define a contract for components that act as context providers in a component hierarchy.
/// </summary>
public interface ICascadingComponent : IDisposable;