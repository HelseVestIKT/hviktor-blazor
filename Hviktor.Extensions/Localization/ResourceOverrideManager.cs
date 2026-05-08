using System.Reflection;

namespace Hviktor.Extensions.Localization;

/// <summary>
/// Manages resource overrides for localization.<br/><br/>
/// This allows you to register custom resource managers that can override the default resources used by the application.<br/>
/// It is useful for providing localized strings or resources that differ from the original package resources.<br/>
/// Can be used as static methods or as an instance for fluent, chainable registration.
/// </summary>
public class ResourceOverrideManager
{
    /// <summary>
    /// Registers a resource manager for overriding resources (instance method for fluent chaining).<br/><br/>
    /// This method allows you to register a resource manager that will be checked before the default resources.<br/>
    /// If multiple managers are registered, they will be checked in the order they were added, with the first one taking precedence.
    /// </summary>
    /// <param name="baseName">The base name of the resource (e.g., "Namespace.ResourceName").</param>
    /// <param name="assembly">The assembly containing the resource.</param>
    /// <returns>The current instance for method chaining.</returns>
    public ResourceOverrideManager RegisterOverride(string baseName, Assembly assembly)
    {
        ResourceExtensions.Register(baseName, assembly);
        return this;
    }

    /// <summary>
    /// Registers a resource manager for overriding resources using a resource type (instance method for fluent chaining).<br/><br/>
    /// This method extracts the base name from the type's full name and the assembly from the type's assembly.<br/>
    /// The base name is constructed by taking the namespace and class name of the type.<br/>
    /// If multiple managers are registered, they will be checked in the order they were added, with the first one taking precedence.
    /// </summary>
    /// <param name="resourceType">The type of the resource class (e.g., typeof(MyResources)).</param>
    /// <returns>The current instance for method chaining.</returns>
    public ResourceOverrideManager RegisterOverride(Type resourceType)
    {
        var baseName = resourceType.FullName ?? throw new ArgumentException("Resource type must have a full name.", nameof(resourceType));
        ResourceExtensions.Register(baseName, resourceType.Assembly);
        return this;
    }

    /// <summary>
    /// Registers a resource manager for overriding resources using a resource type (instance method for fluent chaining).<br/><br/>
    /// This is a generic overload that provides type safety.<br/>
    /// The base name is constructed from the type's full name and the assembly from the type's assembly.
    /// </summary>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <returns>The current instance for method chaining.</returns>
    public ResourceOverrideManager RegisterOverride<TResource>()
    {
        return RegisterOverride(typeof(TResource));
    }
}