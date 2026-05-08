using System.Diagnostics.CodeAnalysis;

namespace Documentation.Components.Services;

/// <summary>
/// Reads component metadata (parameters, types, default values) via reflection
/// and XML documentation if available.
/// </summary>
public interface IComponentMetadataService
{
    /// <summary>
    /// Returns all <see cref="ParameterInfo"/> entries for the given component type.
    /// </summary>
    /// <param name="componentType">A Blazor <see cref="Microsoft.AspNetCore.Components.ComponentBase"/> derived type.</param>
    /// <returns>Ordered list of parameters.</returns>
    IReadOnlyList<ParameterInfo> GetParameters([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type componentType);

    /// <summary>
    /// Returns the XML doc <c>&lt;summary&gt;</c> for the given component class, or <see langword="null"/> if unavailable.
    /// </summary>
    /// <param name="componentType">The component type to look up.</param>
    /// <returns>The summary text, or <see langword="null"/>.</returns>
    string? GetClassSummary(Type componentType);

    /// <summary>
    /// Returns the full XML documentation for the given component class, including
    /// <c>&lt;summary&gt;</c>, <c>&lt;remarks&gt;</c>, <c>&lt;use&gt;</c>, <c>&lt;avoid&gt;</c>,
    /// and <c>&lt;guidelines&gt;</c> sections.
    /// </summary>
    /// <param name="componentType">The component type to look up.</param>
    /// <returns>A <see cref="ClassDocumentation"/> instance, or <see langword="null"/> if no XML docs are available.</returns>
    ClassDocumentation? GetClassDocumentation(Type componentType);

    /// <summary>
    /// Returns all public methods declared on the given component type that are intended
    /// for consumer use (e.g. via <c>@ref</c>). Excludes lifecycle, framework, and private methods.
    /// </summary>
    /// <param name="componentType">The component type to inspect.</param>
    /// <returns>Ordered list of public methods with XML doc summaries.</returns>
    IReadOnlyList<ComponentMethodInfo> GetPublicMethods([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicMethods)] Type componentType);

    /// <summary>
    /// Builds an HTML table for the given parameters using the same <c>ds-table</c> structure
    /// and rendering pipeline (popovers, HTML encoding, tags) as the structured XML doc tables.
    /// </summary>
    /// <param name="parameters">The reflected and/or implicit parameters to render.</param>
    /// <returns>An HTML string containing a <c>&lt;table&gt;</c> element, or <see langword="null"/> if the list is empty.</returns>
    string? BuildParametersHtml(IReadOnlyList<ParameterInfo> parameters);
    /// <summary>
    /// Builds an HTML table for the given methods using the same <c>ds-table</c> structure
    /// and rendering pipeline (popovers, HTML encoding) as the parameter tables.
    /// </summary>
    /// <param name="methods">The reflected methods to render.</param>
    /// <returns>An HTML string containing a <c>&lt;table&gt;</c> element, or <see langword="null"/> if the list is empty.</returns>
    string? BuildMethodsHtml(IReadOnlyList<ComponentMethodInfo> methods);
}