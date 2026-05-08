namespace Documentation.Components.Services;

/// <summary>
/// Service for reading component source code (<c>.razor</c> and <c>.razor.cs</c>) from disk
/// to display in the documentation "Source" tab.
/// </summary>
public interface IComponentSourceService
{
    /// <summary>
    /// Gets the Razor markup source for the given component type, or <see langword="null"/> if not found.
    /// </summary>
    string? GetRazorSource(Type componentType);

    /// <summary>
    /// Gets the code-behind (<c>.razor.cs</c>) source for the given component type, or <see langword="null"/> if not found.
    /// </summary>
    string? GetCodeBehindSource(Type componentType);
}

