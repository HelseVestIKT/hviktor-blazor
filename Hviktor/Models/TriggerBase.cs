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
/// Represents the base class for a trigger component that can be used to handle user interactions
/// and control visual presentation. Inheriting classes can define specific behavior and appearance
/// for their respective trigger implementations.
/// </summary>
/// <remarks>
/// TriggerBase provides foundational properties and methods for components that are designed to
/// interact with user input and manage their visual state. It includes support for placement, size, variant, color,
/// and loading state, as well as integration with JavaScript for enhanced functionality.
/// </remarks>
public abstract class TriggerBase : AsyncOptionalNestedComponentBase<TriggerContextBase>
{
    [Inject] private ILogger<TriggerBase> Logger { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private IVariantService VariantService { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IPlacementService PlacementService { get; set; } = null!;

    /// <summary>
    /// Toggle input state.
    /// </summary>
    [Parameter]
    public bool Input { get; set; }

    /// <summary>
    /// Toggle loading state.
    /// </summary>
    [Parameter]
    public bool Loading { get; set; }

    /// <summary>
    /// The content to be displayed inside the trigger.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    internal ElementReference? ElementRef { get; set; }

    private bool jsInteropAllowed;

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddAttribute("type", "button")
            .AddAttribute("popovertarget", !string.IsNullOrWhiteSpace(Parent?.Id) ? Parent.Id : null)
            .Transform(attr =>
            {
                if (attr.ContainsKey("inline") && attr.TryAdd("data-popover", "inline"))
                {
                    attr.Remove("inline");
                }
                else
                {
                    attr.AddClasses("ds-button");
                }

                return attr;
            });

        EnumValue<Placement> placement = builder.ConsumeAttribute("placement") ?? builder.ConsumeAttribute("data-placement");
        builder.AddDataAttribute("placement", PlacementService.GetDataAttribute(placement, Placement.Bottom));

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        if (!variant.IsEmpty)
        {
            builder.AddDataAttribute("variant", VariantService.GetDataAttribute(variant));
        }

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        if (!color.IsEmpty)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(color));
        }

        return builder;
    }

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            jsInteropAllowed = true;
        }

        if (firstRender && !IsDisposed())
        {
            await InvokeVoidAsync("initialize");
        }
    }

    private async Task InvokeVoidAsync(string method)
        => await JsRuntime.InvokeVoidAsync("globalThis.Hviktor.Trigger." + method, ElementRef);

    private bool disposed;

    private bool IsDisposed([CallerMemberName] string? caller = null)
    {
        if (disposed)
        {
            Logger.LogError("{MethodCall}: Trigger component is disposed.", caller);
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeAsyncCore()
    {
        try
        {
            if (!disposed)
            {
                disposed = true;

                // Only attempt JS cleanup if we've had an interactive render
                if (jsInteropAllowed)
                {
                    try
                    {
                        Logger.LogTrace("Disposing component.");
                        await InvokeVoidAsync("dispose");
                    }
                    catch (TaskCanceledException)
                    {
                        // Handle the task cancellation if necessary
                        Logger.LogTrace("Task was canceled.");
                    }
                    catch (JSDisconnectedException)
                    {
                        // Handle the JS disconnected exception if necessary
                        Logger.LogTrace("JS disconnected.");
                    }
                    catch (InvalidOperationException)
                    {
                        Logger.LogTrace("JS runtime is no longer available.");
                    }
                }
            }
        }
        finally
        {
            await base.DisposeAsyncCore();
        }
    }
}