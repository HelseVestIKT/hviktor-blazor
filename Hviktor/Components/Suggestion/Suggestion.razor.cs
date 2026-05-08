using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SuggestionList = Suggestion.List;
using Select = Hviktor.Components.Select.Select;

namespace Hviktor.Components.Suggestion;

/// <summary>
/// <c>Suggestion</c> is a searchable <see cref="Select"/> with support for multiple selections.
/// </summary>
/// <use>
/// Use <c>Suggestion</c> when:
/// <list type="bullet">
/// <item>users need to choose one or several options from a large list</item>
/// <item>it is helpful that the list is filtered based on what the user types</item>
/// <item>searching is faster than scrolling</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Suggestion</c> when:
/// <list type="bullet">
/// <item>users need to choose between only a few options: use Radio or Checkbox instead</item>
/// <item>users expect an operating-system-native experience: in that case, use a native <c>select</c> element</item>
/// </list>
/// </avoid>
/// <guidelines>
/// <c>Suggestion</c> works best when users need to find the right option within a large list.
/// The strength of the component lies in its ability to filter as the user types,
/// making it easier to navigate many choices.
/// </guidelines>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Attribute</term>
///     <description>Description</description>
///   </listheader>
///   <item>
///     <term>
///       <b>filter</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="true"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Whether to enable filtering of options based on user input.<br/>
///                           If you want custom filtering logic, you can disable the built-in filter and implement your own in the <c>onBeforeMatch</c> callback.<br/>
///                           <b>Note</b>: if filtering is disabled, the component will not hide options that do not match the input,
///                                        but it will still highlight matching options based on the input value.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>creatable</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Whether to allow users to create new items that are not in the list of options.<br/>
///                           If enabled, users can type a value that does not match any existing option and press Enter to create and select it.
///                           The new value will be included in the list of selected values passed
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>name</b>: <see cref="string"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The name of the associated form control.<br/>
///                    This is important for form submissions and accessibility, as it allows the selected value(s) to be associated with a specific field name when submitted in a form.<br/>
///                    If not provided, the component will not be associated with any form field, and selected values will not be included in form submissions. However, you can still access the selected values through the <c>onSelectedChange</c> callback for custom handling.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>multiple</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Whether to allow selection of multiple items. If true, users can select more than one option from the list, and the component will manage an array of selected values. If false, only a single item can be selected at a time, and the component will manage a single string value for the selected item.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>selected</b>: <see cref="string"/>? | <see cref="string"/>[]?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The currently selected value(s).
///                    This can be a single string or an array of strings depending on whether the <c>multiple</c> attribute is set.
///                    If provided, the component will be controlled, and the selected value(s) will only change in response to parameter updates.
///                    If not provided, the component will manage its own internal state for the selected value(s) and will update them in response to user interactions.<br/>
///                    When using this attribute, you should also handle the <c>onSelectedChange</c> callback to update the selected value(s) in response to user interactions,
///                    since the component will not update its internal state automatically in controlled mode.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>defaultSelected</b>: <see cref="string"/>? | <see cref="string"/>[]?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: The default selected value(s) when the component is uncontrolled (i.e., when the <c>selected</c> attribute is not provided).<br/>
///                    This can be a single string or an array of strings depending on whether the <c>multiple</c> attribute is set. If provided, the component will initialize its internal state with the given value(s) and will manage the selected value(s) internally in response to user interactions.<br/>
///                    If not provided, the component will initialize with no selected items by default.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>onSelectedChange</b>: <see cref="EventCallback">EventCallback&lt;string&gt;</see>? | <see cref="EventCallback">EventCallback&lt;string[]&gt;</see>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Callback invoked when the selected value(s) change due to user interaction. The callback receives the full set of selected values as an array of strings if in multiple mode, or a single string if not in multiple mode. This allows the parent component to respond to selection changes and update its state accordingly, especially when using the component in controlled mode with the <c>selected</c> attribute.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>onBeforeSelect</b>: <see cref="EventCallback">EventCallback&lt;string&gt;</see>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Callback invoked before a selection is applied. Receives the value as <c>string</c>.
///                     This allows the parent component to validate or modify the selected value before it is applied.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>onAfterSelect</b>: <see cref="EventCallback">EventCallback&lt;string&gt;</see>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Callback invoked after a selection is applied. Receives the value as <c>string</c>.
///                     This allows the parent component to perform actions or updates after a selection has been made.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>onBeforeMatch</b>: <see cref="EventCallback">EventCallback&lt;string&gt;</see>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Callback invoked before a match is processed. Receives the value as <c>string</c>.
///                     This allows the parent component to perform actions or updates before a match is processed.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <remarks>
/// If thousands of options are available, it is recommended to handle filtering manually.<br/>
/// This can improve performance and reduce unnecessary computations.
/// </remarks>
public sealed partial class Suggestion : AsyncCascadingComponentBase
{
    [Inject] private ILogger<Suggestion> Logger { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    private DotNetObjectReference<Suggestion>? dotNetRef;
    private bool disposed;
    private bool initialized;
    private bool isMultiple;
    private bool selectedSyncNeeded;

    private EventCallback<string[]> onSelectedChangeCallback;
    private EventCallback<string> onBeforeSelectCallback;
    private EventCallback<string> onAfterSelectCallback;
    private EventCallback<string> onBeforeMatchCallback;

    private Dictionary<string, object?>? preComputedAttributes;

    /// <summary>
    /// The content to be displayed inside the <see cref="Suggestion"/>.<br/>
    /// Usually, this will be a sub-component of <see cref="Suggestion"/>, such as the <see cref="SuggestionList">Suggestion.List</see> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string? name;

    internal string? InternalId;
    internal bool InternalFilter = true;

    internal IReadOnlyList<string> InternalSelected = [];
    private IReadOnlyList<string> defaultSelected = [];
    private IReadOnlyList<string> previousSelected = [];

    internal bool ReadOnly;
    internal bool Disabled;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        preComputedAttributes = null;
        ComputeAttributes();
    }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddClasses("ds-suggestion");

        var multiple = builder.ConsumeAttribute("multiple");
        isMultiple = multiple is not null && !multiple.Equals("false", StringComparison.OrdinalIgnoreCase);
        if (isMultiple)
        {
            builder.AddDataAttribute("multiple", "true");
        }

        var creatable = builder.ConsumeAttribute("creatable");
        if (creatable is not null && !creatable.Equals("false", StringComparison.OrdinalIgnoreCase))
        {
            builder.AddDataAttribute("creatable", "true");
        }

        var filter = builder.ConsumeAttribute("filter");
        if (filter is not null)
        {
            InternalFilter = filter.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        if (!InternalFilter)
        {
            builder.AddDataAttribute("nofilter", "true");
        }

        // Consume callback delegates and wrap them in EventCallback
        // so Blazor re-renders the owning component when they fire.
        onSelectedChangeCallback = ConsumeEventCallback<string[]>(builder, "onSelectedChange");
        onBeforeSelectCallback = ConsumeEventCallback<string>(builder, "onBeforeSelect");
        onAfterSelectCallback = ConsumeEventCallback<string>(builder, "onAfterSelect");
        onBeforeMatchCallback = ConsumeEventCallback<string>(builder, "onBeforeMatch");

        defaultSelected = ConsumeStringList(builder, "defaultSelected");
        InternalSelected = ConsumeStringList(builder, "selected") is { Count: > 0 } sel ? sel : defaultSelected;

        // Only sync to JS when the selected values actually changed from a parameter update,
        // not when re-rendering after a JS-initiated selection.
        if (!InternalSelected.SequenceEqual(previousSelected))
        {
            selectedSyncNeeded = true;
            previousSelected = InternalSelected;
        }

        InternalId = builder.GetValue("id");
        if (InternalId is null)
        {
            InternalId = Cryptography.GenerateId();
            builder.AddIdentity(InternalId);
        }

        name = builder.GetValue("name");

        ReadOnly = builder.GetValue("readonly") is not null;
        Disabled = builder.GetValue("disabled") is not null;

        preComputedAttributes = builder;
        return preComputedAttributes;
    }

    /// <summary>
    /// Consumes a callback attribute and wraps it in an <see cref="EventCallback"/>.
    /// Using <c>EventCallback.Factory</c> ensures that Blazor re-renders the owning
    /// component (the consumer) when the callback fires.
    /// </summary>
    private EventCallback<T> ConsumeEventCallback<T>(Dictionary<string, object?> builder, string key)
    {
        if (!builder.Remove(key, out var raw) || raw is null)
        {
            return default;
        }

        return raw switch
        {
            EventCallback<T> ec => ec,
            Action<T> action => EventCallback.Factory.Create(this, action),
            Func<T, Task> func => EventCallback.Factory.Create(this, func),
            _ => default
        };
    }

    /// <summary>
    /// Consumes an attribute that may be a <see cref="string"/>, <c>string[]</c>,
    /// or <see cref="IEnumerable{T}"/> of strings, and returns a normalized list.
    /// </summary>
    private static IReadOnlyList<string> ConsumeStringList(Dictionary<string, object?> builder, string key)
    {
        if (!builder.Remove(key, out var raw) || raw is null)
        {
            return [];
        }

        return raw switch
        {
            string s when !string.IsNullOrWhiteSpace(s) => [s],
            string[] arr => arr.Where(v => !string.IsNullOrWhiteSpace(v)).ToArray(),
            IEnumerable<string> enumerable => enumerable.Where(v => !string.IsNullOrWhiteSpace(v)).ToArray(),
            _ => raw.ToString() is { Length: > 0 } str ? [str] : []
        };
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && !disposed)
        {
            try
            {
                dotNetRef = DotNetObjectReference.Create(this);
                await JsRuntime.InvokeVoidAsync(
                    "globalThis.Hviktor.Suggestion.initializeCombobox", InternalId, dotNetRef);
                initialized = true;

                if (InternalSelected.Count > 0)
                {
                    selectedSyncNeeded = false;
                    await JsRuntime.InvokeVoidAsync(
                        "globalThis.Hviktor.Suggestion.setSelected", InternalId, InternalSelected);
                }
            }
            catch (JSDisconnectedException)
            {
                // Circuit already disconnected during first render
            }
            catch (InvalidOperationException)
            {
                // JS interop is not available during prerendering
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to initialize Suggestion component JavaScript interop");
            }
        }
        else if (initialized && !disposed && selectedSyncNeeded)
        {
            selectedSyncNeeded = false;
            try
            {
                await JsRuntime.InvokeVoidAsync(
                    "globalThis.Hviktor.Suggestion.setSelected", InternalId, InternalSelected);
            }
            catch (JSDisconnectedException)
            {
                // Circuit already disconnected
            }
            catch (InvalidOperationException)
            {
                // JS interop not available
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to sync selected values to Suggestion DOM");
            }
        }
    }

    /// <summary>
    /// JS-invokable callback invoked before a selection is applied.
    /// </summary>
    /// <param name="value">The value being selected.</param>
    [JSInvokable]
    public async Task OnBeforeSelectCallback(string value)
    {
        if (disposed)
        {
            return;
        }

        await InvokeStringCallbackAsync(onBeforeSelectCallback, value);
    }

    /// <summary>
    /// JS-invokable callback invoked after an item is selected or deselected.
    /// Receives the full list of currently selected values from JS.
    /// </summary>
    /// <param name="allSelected">All currently selected values.</param>
    [JSInvokable]
    public async Task OnAfterSelectCallback(string[] allSelected)
    {
        if (disposed)
        {
            return;
        }

        // Update previousSelected so the re-render from StateHasChanged()
        // does not falsely detect a parameter change and call setSelected().
        previousSelected = allSelected;

        // Invoke onAfterSelect with the last changed value for backwards compatibility
        if (allSelected.Length > 0)
        {
            await InvokeStringCallbackAsync(onAfterSelectCallback, allSelected[^1]);
        }

        // Invoke onSelectedChange with string or string[] depending on mode
        await InvokeSelectedChangeAsync(allSelected);
    }

    /// <summary>
    /// JS-invokable callback invoked when matching input value against options.
    /// </summary>
    /// <param name="value">The input value to match against options.</param>
    [JSInvokable]
    public async Task OnBeforeMatchCallback(string value)
    {
        if (disposed)
        {
            return;
        }

        await InvokeStringCallbackAsync(onBeforeMatchCallback, value);
    }

    /// <summary>
    /// Invokes the <c>onSelectedChange</c> consumer callback with the full set of selected values.
    /// </summary>
    private async Task InvokeSelectedChangeAsync(string[] values)
    {
        if (onSelectedChangeCallback.HasDelegate)
        {
            await onSelectedChangeCallback.InvokeAsync(values);
        }
    }

    /// <summary>
    /// Invokes a stored callback that expects a single string value.
    /// </summary>
    private static async Task InvokeStringCallbackAsync(EventCallback<string> callback, string value)
    {
        if (callback.HasDelegate)
        {
            await callback.InvokeAsync(value);
        }
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeAsyncCore()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        initialized = false;

        try
        {
            if (dotNetRef is not null)
            {
                await JsRuntime.InvokeVoidAsync("globalThis.Hviktor.Suggestion.disposeCombobox", InternalId);
            }
        }
        catch (JSDisconnectedException)
        {
            // Ignore: circuit is already disconnected
        }
        catch (InvalidOperationException)
        {
            // Ignore: JS interop is not available during prerendering
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error disposing Suggestion component");
        }
        finally
        {
            dotNetRef?.Dispose();
            dotNetRef = null;
            await base.DisposeAsyncCore();
        }
    }
}