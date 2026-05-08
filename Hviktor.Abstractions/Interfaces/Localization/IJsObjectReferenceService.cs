using Microsoft.JSInterop;

namespace Hviktor.Abstractions.Interfaces.Localization;

/// <summary>
/// Provides methods for <see cref="IJSObjectReference"/> to invoke JavaScript methods with type safety.
/// This is a stateless service that can be safely shared (Singleton lifetime).
/// </summary>
public interface IJsObjectReferenceService
{
    /// <summary>
    /// Invokes a JavaScript method with the type of <typeparamref name="T"/>.<br/>
    /// The method name is constructed by combining the type name of <typeparamref name="T"/> with the provided method name.<br/>
    /// This allows for type-safe invocation of JavaScript methods that are defined in a specific class or module.<br/>
    /// If the <paramref name="jsObjectReference"/> is null, the method returns without invoking anything.
    /// </summary>
    /// <param name="jsObjectReference">The JavaScript module reference to invoke the method on.</param>
    /// <param name="method">The method name to invoke.</param>
    /// <param name="args">Arguments to pass to the JavaScript method.</param>
    /// <typeparam name="T">The type to use for method name prefixing.</typeparam>
    Task InvokeVoidAsync<T>(IJSObjectReference? jsObjectReference, string method, params object?[]? args);

    /// <summary>
    /// Invokes a JavaScript method with the type of <typeparamref name="T"/>.<br/>
    /// The method name is constructed by combining the type name of <typeparamref name="T"/> with the provided method name.<br/>
    /// This allows for type-safe invocation of JavaScript methods that return a value of type <typeparamref name="TValue"/>.<br/>
    /// If the <paramref name="jsObjectReference"/> is null, it returns the default value of <typeparamref name="TValue"/>.
    /// </summary>
    /// <param name="jsObjectReference">The JavaScript module reference to invoke the method on.</param>
    /// <param name="method">The method name to invoke.</param>
    /// <param name="args">Arguments to pass to the JavaScript method.</param>
    /// <typeparam name="T">The type to use for method name prefixing.</typeparam>
    /// <typeparam name="TValue">The return type of the JavaScript method.</typeparam>
    /// <returns>The result from the JavaScript method, or default if the reference is null.</returns>
    Task<TValue?> InvokeAsync<T, TValue>(IJSObjectReference? jsObjectReference, string method, params object?[]? args);
}