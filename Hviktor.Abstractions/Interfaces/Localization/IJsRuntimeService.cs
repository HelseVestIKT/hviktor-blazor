using Microsoft.JSInterop;

namespace Hviktor.Abstractions.Interfaces.Localization;

/// <summary>
/// Provides methods to import JavaScript modules dynamically.
/// The <see cref="IJSRuntime"/> is injected automatically, so you don't need to pass it.
/// </summary>
public interface IJsRuntimeService
{
    /// <summary>
    /// Imports a JavaScript module asynchronously based on the type provided.
    /// </summary>
    /// <typeparam name="T">The type to use for determining the module path.</typeparam>
    /// <returns>The imported JavaScript module reference.</returns>
    Task<IJSObjectReference> ImportAsync<T>();

    /// <summary>
    /// Imports a JavaScript module asynchronously from a specified path.
    /// </summary>
    /// <param name="path">The path to the JavaScript module to import.</param>
    /// <returns>The imported JavaScript module reference.</returns>
    Task<IJSObjectReference> ImportAsync(string path);
}