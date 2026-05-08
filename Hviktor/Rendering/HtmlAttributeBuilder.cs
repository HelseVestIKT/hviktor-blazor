using Microsoft.AspNetCore.Components;

namespace Hviktor.Rendering;

/// <summary>
/// Provides a utility class for constructing and manipulating HTML attributes.
/// </summary>
internal static class HtmlAttributeBuilder
{
    internal static Dictionary<string, object?> ToDictionary() => new(StringComparer.OrdinalIgnoreCase);

    // Always create a defensive copy to ensure we don't accidentally mutate the original!
    internal static Dictionary<string, object?> ToDictionary(IReadOnlyDictionary<string, object?>? dict)
        => dict is null or { Count: 0 }
            ? ToDictionary()
            : new Dictionary<string, object?>(dict, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Returns an interned <c>data-{key}</c> string for known keys, falling back to concatenation.
    /// Avoids a string allocation on every render for the most common data-attribute names.
    /// </summary>
    private static string GetDataKey(string key) => key switch
    {
        "variant" => "data-variant",
        "color" => "data-color",
        "size" => "data-size",
        "placement" => "data-placement",
        "text" => "data-text",
        _ => string.Concat("data-", key)
    };

    extension(Dictionary<string, object?> dict)
    {
        internal Dictionary<string, object?> Transform(Func<Dictionary<string, object?>, Dictionary<string, object?>> transform)
            => transform(dict);

        /// <summary>
        /// Merges all key-value pairs from the source dictionary into the target dictionary.
        /// Existing keys in target are not overwritten.
        /// </summary>
        internal Dictionary<string, object?> Merge(IReadOnlyDictionary<string, object?>? from)
        {
            if (from is null)
            {
                return dict;
            }

            foreach (var kvp in from)
            {
                dict.TryAdd(kvp.Key, kvp.Value);
            }

            return dict;
        }

        /// <summary>
        /// Combines values from specific keys in the source dictionary into a single key in the target dictionary.
        /// </summary>
        internal Dictionary<string, object?> Combine(IReadOnlyDictionary<string, object?>? from, string combineIntoKey,
            Func<object?, object?, object?> combiner,
            string[] combineFromKeys)
        {
            if (from is null)
            {
                return dict;
            }

            object? combinedValue = null;
            foreach (var key in combineFromKeys)
            {
                if (from.TryGetValue(key, out var value))
                {
                    combinedValue = combiner(combinedValue, value);
                }
            }

            if (combinedValue is not null)
            {
                dict.TryAdd(combineIntoKey, combinedValue);
            }

            return dict;
        }

        /// <summary>
        /// Merges a single value under <paramref name="key"/>.
        /// If the key already exists its value is prepended; otherwise the value is added as-is.
        /// </summary>
        private Dictionary<string, object?> MergeValue(string key, string value)
        {
            if (dict.TryGetValue(key, out var existing) && existing is string existingStr)
            {
                dict[key] = string.Concat(value, " ", existingStr);
            }
            else
            {
                dict.TryAdd(key, value);
            }

            return dict;
        }

        /// <summary>
        /// Merges multiple values (joined by <paramref name="separator"/>) under <paramref name="key"/>.
        /// If the key already exists its value is prepended; otherwise the joined value is added as-is.
        /// </summary>
        private Dictionary<string, object?> MergeValues(string key, string separator, params string[] values)
        {
            var joined = string.Join(separator, values);
            if (dict.TryGetValue(key, out var existing) && existing is string existingStr)
            {
                dict[key] = string.Concat(joined, " ", existingStr);
            }
            else
            {
                dict.TryAdd(key, joined);
            }

            return dict;
        }

        internal Dictionary<string, object?> RemoveFromTabOrder()
        {
            dict.TryAdd("tabindex", -1);
            return dict;
        }

        internal Dictionary<string, object?> AddToNaturalTabOrder()
        {
            dict.TryAdd("tabindex", 0);
            return dict;
        }

        internal Dictionary<string, object?> HideFromAccessibility()
        {
            dict.TryAdd("aria-hidden", "true");
            return dict;
        }

        /// <summary>
        /// Adds a single CSS class. Prefer this over the <c>params</c> overload when adding one class
        /// to avoid the array allocation.
        /// </summary>
        internal Dictionary<string, object?> AddClasses(string className)
            => dict.MergeValue("class", className);

        internal Dictionary<string, object?> AddClasses(string[] classes)
            => dict.MergeValues("class", " ", classes);

        internal Dictionary<string, object?> AddClasses(Func<bool> condition, string style)
            => condition() ? dict.AddClasses(style) : dict;

        internal Dictionary<string, object?> AddClasses(Func<bool> condition, string[] styles)
            => condition() ? dict.AddClasses(styles) : dict;

        /// <summary>
        /// Adds a single inline style. Prefer this over the <c>params</c> overload when adding one style
        /// to avoid the array allocation.
        /// </summary>
        internal Dictionary<string, object?> AddStyles(string style)
            => dict.MergeValue("style", style);

        internal Dictionary<string, object?> AddStyles(string[] styles)
            => dict.MergeValues("style", ";", styles);

        internal Dictionary<string, object?> AddStyles(Func<bool> condition, string[] styles)
            => condition() ? dict.AddStyles(styles) : dict;

        internal Dictionary<string, object?> AddIdentity(Func<bool> condition, string? id)
        {
            return condition() ? dict.AddIdentity(id) : dict;
        }

        internal Dictionary<string, object?> AddIdentity(string? id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                dict.TryAdd("id", id);
            }

            return dict;
        }

        internal Dictionary<string, object?> AddAttribute(string key, Func<bool> condition, object? value, object? @default = null)
            => condition() ? dict.AddAttribute(key, value, @default) : dict;

        internal Dictionary<string, object?> AddAttribute(string key, object? value, object? @default = null)
        {
            var valueToAssign = value ?? @default;
            if (valueToAssign is null)
            {
                return dict;
            }

            dict.TryAdd(key, valueToAssign);
            return dict;
        }

        internal Dictionary<string, object?> AddAttribute(string key, string? value, string? @default = null)
        {
            var valueToAssign = value ?? @default;
            if (valueToAssign is null)
            {
                return dict;
            }

            dict.TryAdd(key, valueToAssign);
            return dict;
        }

        internal Dictionary<string, object?> AddDataAttribute(string key, object? value)
            => dict.AddAttribute(GetDataKey(key), value);

        internal Dictionary<string, object?> AddDataAttribute(string key, string? value, string? @default = null)
            => dict.AddAttribute(GetDataKey(key), value, @default);

        /// <summary>
        /// Removes the specified key from the dictionary and returns its value as a string,
        /// or <see langword="null"/> if the key was not present.
        /// Use this to consume an attribute so it is not forwarded as a bare HTML attribute.
        /// </summary>
        internal string? ConsumeAttribute(string key)
        {
            // AdditionalAttributes can store null values at runtime.
            if (dict.Remove(key, out var value))
            {
                // value can be null at runtime despite type annotation
                return value as string ?? value?.ToString();
            }

            return null;
        }

        internal string? GetValue(string key) => dict.TryGetValue(key, out var value) ? value as string ?? value?.ToString() : null;

        internal Dictionary<string, object?> ContainsKey(string key, Action<bool> callback)
        {
            callback(dict.ContainsKey(key));
            return dict;
        }

        /// <summary>
        /// Combines an internal <see cref="EventCallback"/> with any consumer-supplied callback already
        /// stored under <paramref name="key"/>. Both callbacks are invoked in order: the consumer's first,
        /// then <paramref name="internal"/>. If no consumer callback is present, only the internal one is registered.
        /// </summary>
        internal Dictionary<string, object?> CombineEventCallback<TValue>(string key, object receiver, Func<TValue, Task> @internal)
        {
            if (dict.TryGetValue(key, out var existing) && existing is EventCallback<TValue> consumerCallback)
            {
                dict[key] = EventCallback.Factory.Create(receiver, async (TValue e) =>
                {
                    await consumerCallback.InvokeAsync(e);
                    await @internal(e);
                });
            }
            else
            {
                dict[key] = EventCallback.Factory.Create(receiver, @internal);
            }

            return dict;
        }
    }
}