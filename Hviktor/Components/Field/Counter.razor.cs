using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

// ReSharper disable once CheckNamespace
namespace Field;

/// <summary>
/// The <c>Field.Counter</c> component is used to inform the users about the remaining characters allowed in a input field.
/// </summary>
public sealed partial class Counter : NestedComponentBase<Hviktor.Components.Field.Field>, IAsyncDisposable
{
    [Inject] private IJsRuntimeService JsRuntimeService { get; set; } = null!;
    [Inject] private IJsObjectReferenceService JsObjectReferenceService { get; set; } = null!;

    /// <summary>
    /// Label template for when `maxCount` is exceeded. Use `{0}` to insert the number of characters.
    /// </summary>
    [Parameter]
    public string Over { get; set; } = "{0} tegn for mye";

    /// <summary>
    /// Label template for count. Use `{0}` to insert the number of characters.
    /// </summary>
    [Parameter]
    public string Under { get; set; } = "{0} tegn igjen";

    /// <summary>
    /// The maximum allowed characters.
    /// </summary>
    [Parameter]
    public int Limit { get; set; }

    /// <summary>
    /// Optional manual count override. If not set, the component will automatically track the input's value length.
    /// </summary>
    [Parameter]
    public int? Count { get; set; }

    private int CurrentCount { get; set; }
    private int EffectiveCount => Count ?? CurrentCount;

    private bool HasExceededLimit => EffectiveCount > Limit;
    private int Remainder => Limit - EffectiveCount;

    private string CounterId { get; } = Cryptography.GenerateId();
    private string? InputId { get; set; }
    private string DescriptionId => InputId != null ? $"{InputId}:description:{CounterId}" : $"counter:{CounterId}";

    private IJSObjectReference? CounterModule { get; set; }
    private DotNetObjectReference<Counter>? DotNetRef { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .HideFromAccessibility();

    private Dictionary<string, object?> ComputeDescriptionAttributes()
    {
        return HtmlAttributeBuilder.ToDictionary()
            .AddIdentity(DescriptionId)
            .AddClasses("ds-sr-only")
            .HideFromAccessibility()
            .AddDataAttribute("field", "description");
    }

    private static string GetLabel(string pattern, int count)
        => pattern.Replace("{0}", Math.Abs(count).ToString());

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender && Count is null)
        {
            try
            {
                CounterModule = await JsRuntimeService.ImportAsync<Counter>();
                DotNetRef = DotNetObjectReference.Create(this);
                InputId = await JsObjectReferenceService.InvokeAsync<Counter, string?>(CounterModule, "initialize", Parent?.ElementRef, DotNetRef);
                StateHasChanged();
            }
            catch (JSException)
            {
                // JS interop not available (e.g., prerendering)
            }
        }
    }

    /// <summary>
    /// Called from JavaScript when the input value changes.
    /// </summary>
    [JSInvokable]
    public void OnInputValueChanged(int length)
    {
        CurrentCount = length;
        StateHasChanged();
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (CounterModule is not null)
            {
                await JsObjectReferenceService.InvokeVoidAsync<Counter>(CounterModule, "dispose", Parent?.ElementRef);
                await CounterModule.DisposeAsync();
            }
        }
        catch (JSDisconnectedException)
        {
            // Ignore
        }

        DotNetRef?.Dispose();
    }
}