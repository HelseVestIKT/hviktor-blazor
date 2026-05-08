using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

// ReSharper disable once CheckNamespace
namespace Tabs;

/// <summary>
/// The <c>List</c> wraps the <c>Tab</c> components and is responsible for rendering the tab list.
/// </summary>
public sealed partial class List : NestedComponentBase<Hviktor.Components.Tabs.Tabs>
{
    [Inject] private ILogger<List> Logger { get; set; } = null!;
    [Inject] private IJsRuntimeService JsRuntimeService { get; set; } = null!;
    [Inject] private IJsObjectReferenceService JsObjectReferenceService { get; set; } = null!;

    /// <summary>
    /// Content rendered inside the tab list.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private ElementReference ElementRef;
    private IJSObjectReference? ListModule { get; set; }

    internal string? GetPanelId(string? tabValue) => Parent?.GetPanelId(tabValue);
    internal void SetSelectedState(string? value) => Parent?.SetSelection(value);
    internal bool IsCurrentSelected(string? value) => Parent?.IsCurrentSelected(value) ?? false;

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    /// <returns>A dictionary of HTML attributes to be applied to the root element.</returns>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddAttribute("role", "tablist");

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ListModule = await JsRuntimeService.ImportAsync<List>();
            await InvokeVoidAsync("initialize");
        }
    }

    private async Task InvokeVoidAsync(string method)
        => await JsObjectReferenceService.InvokeVoidAsync<List>(ListModule, method, ElementRef);

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing && ListModule is not null)
        {
            try
            {
                LogDisposingComponent(Logger);
                InvokeVoidAsync("dispose").ContinueWith(async _ =>
                {
                    if (ListModule is not null)
                    {
                        await ListModule.DisposeAsync();
                    }
                }, TaskScheduler.Default);
            }
            catch (JSDisconnectedException)
            {
                LogJsDisconnected(Logger);
            }
            catch (InvalidOperationException)
            {
                // Component is being statically rendered (prerendering),
                // JS interop is not available in this context
            }
        }

        base.Dispose(disposing);
    }

    [LoggerMessage(LogLevel.Trace, "Disposing component.")]
    static partial void LogDisposingComponent(ILogger<List> logger);

    [LoggerMessage(LogLevel.Trace, "JS disconnected.")]
    static partial void LogJsDisconnected(ILogger<List> logger);
}