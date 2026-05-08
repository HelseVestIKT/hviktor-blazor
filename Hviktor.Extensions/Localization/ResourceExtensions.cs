using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Hviktor.Extensions.Localization;

/// <summary>
/// Manages resource overrides for localization.<br/><br/>
/// This allows you to register custom resource managers that can override the default resources used by the application.<br/>
/// It is useful for providing localized strings or resources that differ from the original package resources.<br/>
/// Can be used as static methods or as an instance for fluent, chainable registration.
/// </summary>
public static class ResourceExtensions
{
    private static readonly Lock SyncLock = new();
    private static readonly List<ResourceManager> OverrideManagers = [];
    internal static bool HasOverrides
    {
        get
        {
            lock (SyncLock)
            {
                return OverrideManagers.Count > 0;
            }
        }
    }

    /// <summary>
    /// Registers a resource manager for overriding resources (static method).<br/><br/>
    /// This method allows you to register a resource manager that will be checked before the default resources.<br/>
    /// If multiple managers are registered, they will be checked in the order they were added, with the first one taking precedence.
    /// </summary>
    public static void Register(string baseName)
        => Register(baseName, Assembly.GetCallingAssembly());

    /// <summary>
    /// Registers a resource manager for overriding resources with a specific assembly (static method).<br/><br/>
    /// This method allows you to register a resource manager that will be checked before the default resources.<br/>
    /// If multiple managers are registered, they will be checked in the order they were added, with the first one taking precedence.
    /// </summary>
    public static void Register(string baseName, Assembly assembly)
    {
        lock (SyncLock)
        {
            OverrideManagers.Insert(0, new ResourceManager(baseName, assembly));
        }
    }

    /// <summary>
    /// Gets a localized string by name using the registered resource managers.<br/><br/>
    /// This method checks the registered resource managers in order and returns the first non-null value found.<br/>
    /// If no value is found, it returns null, allowing the original resource manager to handle it.
    /// </summary>
    public static string? GetString(string name)
        => GetString(name, CultureInfo.CurrentUICulture);

    /// <summary>
    /// Gets a localized string by name using the registered resource managers for a specific culture.<br/><br/>
    /// This method checks the registered resource managers in order and returns the first non-null value found.<br/>
    /// If no value is found, it returns null, allowing the original resource manager to handle it.
    /// </summary>
    public static string? GetString(string name, CultureInfo culture)
    {
        ResourceManager[] snapshot;
        lock (SyncLock)
        {
            snapshot = [.. OverrideManagers];
        }

        foreach (var manager in snapshot)
        {
            try
            {
                // This ensures we only get values explicitly defined for this culture
                var resourceSet = manager.GetResourceSet(culture, true, false);
                if (resourceSet?.GetObject(name) is string value)
                {
                    return value;
                }
            }
            catch (ArgumentNullException)
            {
                // Ignore errors when the resource manager is null
            }
            catch (MissingManifestResourceException)
            {
                // Ignore errors when the resource manager cannot find the resource
            }
            catch (ObjectDisposedException)
            {
                // Ignore errors when the resource manager is disposed
            }
        }

        return null;
    }
}