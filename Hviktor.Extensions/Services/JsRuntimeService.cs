using System.Globalization;
using System.Reflection;
using Hviktor.Abstractions.Interfaces.Localization;
using Microsoft.JSInterop;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Provides methods to import JavaScript modules dynamically.
/// </summary>
public class JsRuntimeService(IJSRuntime jsRuntime) : IJsRuntimeService
{
    private const string JsRoot = "./_content/";
    private static readonly string Ticks = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
    private static readonly string? EntryAssemblyName = Assembly.GetEntryAssembly()?.GetName().Name;

    /// <inheritdoc />
    public async Task<IJSObjectReference> ImportAsync<T>()
        => await ImportAsync(GetPath<T>());

    /// <inheritdoc />
    public async Task<IJSObjectReference> ImportAsync(string path)
        => await jsRuntime.InvokeAsync<IJSObjectReference>("import", path);

    private static string GetPath<T>()
    {
        var type = typeof(T);
        var assemblyName = type.Assembly.GetName().Name;
        var isAppAssembly = string.Equals(assemblyName, EntryAssemblyName, StringComparison.Ordinal);

        var path = type.FullName ?? type.Namespace + "." + type.Name;
        if (!string.IsNullOrEmpty(assemblyName) && path.StartsWith(assemblyName, StringComparison.Ordinal))
        {
            var restOfPath = path[assemblyName.Length..];
            if (restOfPath.StartsWith('.'))
            {
                restOfPath = restOfPath[1..];
            }

            path = isAppAssembly
                ? restOfPath.Replace(".", "/", StringComparison.OrdinalIgnoreCase)
                : $"{assemblyName}/{restOfPath.Replace(".", "/", StringComparison.OrdinalIgnoreCase)}";
        }
        else if (!string.IsNullOrEmpty(assemblyName) && !isAppAssembly)
        {
            // Razor components with folder-based namespaces
            var namespacePath = path.Replace(".", "/", StringComparison.OrdinalIgnoreCase);
            path = $"{assemblyName}/Components/{namespacePath}";
        }
        else
        {
            path = path.Replace(".", "/", StringComparison.OrdinalIgnoreCase);
        }

        if (!path.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
        {
            if (!path.EndsWith(".razor", StringComparison.OrdinalIgnoreCase))
            {
                path += ".razor";
            }

            path += ".js";
        }

        var root = isAppAssembly ? "./" : JsRoot;
        return $"{root}{path}?v={Ticks}";
    }
}