using System.Globalization;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Extensions.Localization;
using Microsoft.Extensions.Localization;

namespace Hviktor.Extensions.Services;

/// <summary>
/// Extension methods for IStringLocalizer to support resource overrides.
/// </summary>
public class StringLocalizerService<TResource>(IStringLocalizer<TResource> localizer) : IStringLocalizerService<TResource>
{
    /// <inheritdoc/>
    public string GetValue(string key)
    {
        if (ResourceExtensions.HasOverrides)
        {
            var overrideValue = ResourceExtensions.GetString(key, CultureInfo.CurrentUICulture);
            if (!string.IsNullOrWhiteSpace(overrideValue))
            {
                return overrideValue;
            }
        }

        var localizedValue = localizer[key];
        return localizedValue.ResourceNotFound
            ? key
            : localizedValue.Value;
    }
}