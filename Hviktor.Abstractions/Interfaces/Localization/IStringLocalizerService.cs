namespace Hviktor.Abstractions.Interfaces.Localization;

/// <summary>
/// Extension methods for IStringLocalizer to support resource overrides.
/// </summary>
public interface IStringLocalizerService<out TResource>
{
    /// <summary>
    /// Gets a localized string with support for resource overrides.<br/><br/>
    /// Prioritizes overrides from ResourceOverrideManager before falling back to the original localizer.
    /// </summary>
    /// <param name="key">The key of the resource to retrieve.</param>
    /// <returns>Override value if found, else the fallback value from the original resource.</returns>
    string GetValue(string key);
}