using System.Runtime.CompilerServices;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Hviktor.Models;

/// <summary>
/// Base class for popover-like components (Popover, Dropdown, etc.) that share common
/// functionality for positioning, controlled state, and JS interop.
/// </summary>
public abstract partial class PopoverBase : AsyncOptionalNestedComponentBase<TriggerContextBase>
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private IVariantService VariantService { get; set; } = null!;
    [Inject] private IPlacementService PlacementService { get; set; } = null!;

    /// <summary>
    /// The id of the popover. Used as the target for the popovertarget attribute on triggers.
    /// When used inside a TriggerContext, this is automatically set from the parent.
    /// </summary>
    [Parameter]
    public required string Id { get; set; }

    /// <summary>
    /// Controls the open state of the popover. When set, the popover becomes controlled
    /// and will not auto-close on internal clicks. Use with OnClose to handle closing.
    /// </summary>
    [Parameter]
    public bool? Open { get; set; }

    /// <summary>
    /// Two-way binding for the Open parameter.
    /// </summary>
    [Parameter]
    public EventCallback<bool?> OpenChanged { get; set; }

    /// <summary>
    /// Callback invoked when the popover should close (e.g., clicking outside or pressing Escape).
    /// Only used when the popover is in controlled mode (Open parameter is set).
    /// </summary>
    [Parameter]
    public EventCallback OnClose { get; set; }

    /// <summary>
    /// The content to be displayed inside the popover.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Reference to the HTML element representing the popover.
    /// </summary>
    protected ElementReference ElementRef { get; set; }

    /// <summary>
    /// Gets the effective Id for this popover - either from the parent context or the direct Id parameter.
    /// </summary>
    private string EffectiveId => Parent?.Id ?? Id;

    /// <summary>
    /// Indicates whether this popover is used standalone (without TriggerContext).
    /// </summary>
    private bool IsStandalone => Parent is null;

    /// <summary>
    /// Indicates whether this popover is in controlled mode.
    /// </summary>
    private bool IsControlled => Open.HasValue;

    /// <summary>
    /// Gets the component name for logging purposes.
    /// </summary>
    protected abstract string ComponentName { get; }

    /// <summary>
    /// Gets the logger for this component. Must be provided by derived class.
    /// </summary>
    protected abstract ILogger ComponentLogger { get; }

    private bool? previousOpen;
    private IDisposable? dotNetRef;
    private bool disposed;
    private bool jsInteropAllowed;

    /// <summary>
    /// Creates a DotNetObjectReference for this component. 
    /// Override in derived classes to create the correctly typed reference.
    /// </summary>
    protected abstract IDisposable CreateDotNetObjectReference();

    /// <summary>
    /// Computes and builds a dictionary of HTML attributes, adding component-specific attributes
    /// such as identity, classes, and popover-related attributes. Customizes data attributes
    /// based on placement and variant values.
    /// </summary>
    /// <returns>
    /// A dictionary containing the computed HTML attributes for the component.
    /// </returns>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddIdentity(EffectiveId)
            .AddClasses("ds-popover")
            .AddAttribute("popover", "manual");

        EnumValue<Placement> placement = builder.ConsumeAttribute("placement") ?? builder.ConsumeAttribute("data-placement");
        builder.AddDataAttribute("placement", PlacementService.GetDataAttribute(placement, Placement.BottomStart));

        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        builder.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, Variant.Primary));

        return builder;
    }

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (string.IsNullOrWhiteSpace(EffectiveId))
        {
            LogNoIDWasProvidedForTheComponentnameComponentEitherUseATriggercontextOrProvideAnId(ComponentLogger, ComponentName);
        }
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            jsInteropAllowed = true;
        }

        if (firstRender && IsStandalone && !IsDisposed())
        {
            try
            {
                await InvokeVoidAsync("initialize");
            }
            catch (JSException)
            {
                LogFailedToInitializeComponentnamePositioning(ComponentLogger, ComponentName);
            }
        }

        // Register controlled popover on first render
        if (firstRender && IsControlled && !IsDisposed())
        {
            try
            {
                dotNetRef = CreateDotNetObjectReference();
                await JsRuntime.InvokeVoidAsync("globalThis.Hviktor.Popover.registerControlled", ElementRef, dotNetRef);
            }
            catch (JSException)
            {
                LogFailedToRegisterControlledComponentname(ComponentLogger, ComponentName);
            }
        }

        // Handle controlled state changes
        if (IsControlled && Open != previousOpen && !IsDisposed())
        {
            previousOpen = Open;
            try
            {
                if (Open == true)
                {
                    await JsRuntime.InvokeVoidAsync("globalThis.Hviktor.Popover.show", ElementRef);
                }
                else
                {
                    await JsRuntime.InvokeVoidAsync("globalThis.Hviktor.Popover.hide", ElementRef);
                }
            }
            catch (JSException)
            {
                LogFailedToUpdateComponentnameOpenState(ComponentLogger, ComponentName);
            }
        }
    }

    /// <summary>
    /// Called from JavaScript when the popover should close (e.g., clicking outside or pressing Escape).
    /// </summary>
    [JSInvokable]
    public async Task OnCloseFromJs()
    {
        if (OnClose.HasDelegate)
        {
            await OnClose.InvokeAsync();
        }

        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(false);
        }
    }

    private async Task InvokeVoidAsync(string method)
        => await JsRuntime.InvokeVoidAsync("globalThis.Hviktor.Popover." + method, ElementRef);

    private bool IsDisposed([CallerMemberName] string? caller = null)
    {
        if (disposed)
        {
            LogMethodcallComponentnameComponentIsDisposed(ComponentLogger, caller ?? "PopoverBase", ComponentName);
            return true;
        }

        return false;
    }

    private async Task DisposeJsResourcesAsync()
    {
        try
        {
            LogMethodcallClassDisposingComponent(ComponentLogger, ComponentName);
            if (IsStandalone)
            {
                await InvokeVoidAsync("dispose");
            }

            if (IsControlled)
            {
                await JsRuntime.InvokeVoidAsync("globalThis.Hviktor.Popover.unregisterControlled", ElementRef);
            }
        }
        catch (JSDisconnectedException)
        {
            LogMethodcallClassJsDisconnected(ComponentLogger, nameof(PopoverBase));
        }
        catch (TaskCanceledException)
        {
            LogMethodcallClassTaskWasCanceled(ComponentLogger, nameof(PopoverBase));
        }
        catch (InvalidOperationException)
        {
            LogMethodcallClassJsRuntimeIsNoLongerAvailable(ComponentLogger, nameof(PopoverBase));
        }
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeAsyncCore()
    {
        try
        {
            if (!disposed)
            {
                disposed = true;
                if (jsInteropAllowed)
                {
                    await DisposeJsResourcesAsync();
                }

                dotNetRef?.Dispose();
            }
        }
        finally
        {
            await base.DisposeAsyncCore();
        }
    }

    [LoggerMessage(LogLevel.Warning, "No id was provided for the {componentName} component. Either use a TriggerContext or provide an Id parameter.")]
    static partial void LogNoIDWasProvidedForTheComponentnameComponentEitherUseATriggercontextOrProvideAnId(ILogger logger, string componentName);

    [LoggerMessage(LogLevel.Error, "Failed to initialize {componentName} positioning")]
    static partial void LogFailedToInitializeComponentnamePositioning(ILogger logger, string componentName);

    [LoggerMessage(LogLevel.Error, "Failed to register controlled {componentName}")]
    static partial void LogFailedToRegisterControlledComponentname(ILogger logger, string componentName);

    [LoggerMessage(LogLevel.Error, "Failed to update {componentName} open state")]
    static partial void LogFailedToUpdateComponentnameOpenState(ILogger logger, string componentName);

    [LoggerMessage(LogLevel.Error, "{methodCall}: {componentName} component is disposed.")]
    static partial void LogMethodcallComponentnameComponentIsDisposed(ILogger logger, string methodCall, string componentName);

    [LoggerMessage(LogLevel.Trace, "DisposeAsyncCore<{class}>: Disposing component.")]
    static partial void LogMethodcallClassDisposingComponent(ILogger logger, string @class);

    [LoggerMessage(LogLevel.Trace, "DisposeAsyncCore<{class}>: JS disconnected.")]
    static partial void LogMethodcallClassJsDisconnected(ILogger logger, string @class);

    [LoggerMessage(LogLevel.Trace, "DisposeAsyncCore<{class}>: Task was canceled.")]
    static partial void LogMethodcallClassTaskWasCanceled(ILogger logger, string @class);

    [LoggerMessage(LogLevel.Trace, "DisposeAsyncCore<{class}>: JS runtime is no longer available.")]
    static partial void LogMethodcallClassJsRuntimeIsNoLongerAvailable(ILogger logger, string @class);
}