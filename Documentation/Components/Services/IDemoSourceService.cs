namespace Documentation.Components.Services;

/// <summary>
/// Service for reading source code of demo components to display in documentation.
/// The source files are embedded as resources at build time.
/// </summary>
public interface IDemoSourceService
{
    /// <summary>
    /// Gets the source code of a demo component.
    /// </summary>
    /// <param name="componentName">The name of the demo component (e.g., "ButtonDemo").</param>
    /// <exception cref="InvalidOperationException">Thrown if the source file for the specified component is not found.</exception>
    /// <returns>The source code of the component, or an empty string if not found.</returns>
    string GetDemoSource(string componentName);
}