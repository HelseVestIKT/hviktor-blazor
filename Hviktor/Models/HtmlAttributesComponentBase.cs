using Microsoft.AspNetCore.Components;

namespace Hviktor.Models;

/// <summary>
/// Serves as a base class for components supporting HTML attributes propagation.
/// Provides functionality to merge and compute additional attributes that can be passed to the component.
/// </summary>
public abstract class HtmlAttributesComponentBase : ComponentBase
{
    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Returns a new dictionary containing the additional attributes.
    /// Always creates a new instance to prevent concurrent modification issues.
    /// </summary>
    protected virtual Dictionary<string, object?> ComputeAttributes()
    {
        return AdditionalAttributes is null or { Count: 0 }
            ? new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
            // Always create a new dictionary - AdditionalAttributes may be mutated by Blazor
            : new Dictionary<string, object?>(AdditionalAttributes, StringComparer.OrdinalIgnoreCase);
    }
}