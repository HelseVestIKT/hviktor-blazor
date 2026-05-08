using System.ComponentModel.DataAnnotations;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Hviktor.Components.Dialog;

/// <summary>
/// There are two types of dialogs: <c>modal</c> and <c>non-modal</c>.<br/>
/// A modal dialog requires users to take action before they can continue using the page.<br/>
/// A non-modal dialog allows users to continue using the page, even while the dialog is open.
/// </summary>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Attribute</term>
///     <description>Description</description>
///   </listheader>
///   <item>
///     <term>
///       <b>open</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="false"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Indicates whether the dialog is open or closed. If set to <see langword="true"/>, the dialog will be open; if set to <see langword="false"/>, it will be closed.<br/>
///                     <b>Note</b>: Unlike standard HTML, where the open attribute always opens a non-modal dialog, Dialog's open prop uses the <c>modal</c> prop to determine whether the Dialog is modal or non-modal.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>modal</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="true"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: Indicates whether the dialog is modal or non-modal.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>placement</b>: <see cref="Placement"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Allowed</b>: <see cref="Placement.Top"/> | <see cref="Placement.Bottom"/> | <see cref="Placement.Left"/> | <see cref="Placement.Right"/> | <see cref="Placement.Center"/><br/>
///       <b>Description</b>: The placement of the dialog on the screen.<br/>
///                     <b>Note</b>: When not center, displays dialog as a "drawer" from the specified side.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>closeButton</b>: <see cref="bool"/> | <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <c>"Lukk dialogvindu"</c><br/>
///       <b>Allowed</b>: Any string value, or <see langword="false"/> to hide the close button.<br/>
///       <b>Description</b>: The screen reader label for the close button. If set to <see langword="false"/>, the close button will be hidden and not accessible to screen readers.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>closedby</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="closerequest"/><br/>
///       <b>Allowed</b>: <see langword="none"/> | <see langword="closerequest"/> | <see langword="any"/><br/>
///       <b>Description</b>: Light dismiss behavior, allowing to close on backdrop click by setting <c>closedby="any"</c>.<br/>
///                     <see href="https://developer.mozilla.org/en-US/docs/Web/API/HTMLDialogElement/closedBy">MDN closedBy</see>
///     </description>
///   </item>
/// </list>
/// </parameters>
public sealed partial class Dialog : ComponentBase, IAsyncDisposable
{
    [Inject] private ILogger<Dialog> Logger { get; set; } = null!;

    [Inject] private IStringLocalizerService<Resources.Resources> Localizer { get; set; } = null!;
    [Inject] private IJsRuntimeService JsRuntimeService { get; set; } = null!;
    [Inject] private IJsObjectReferenceService JsObjectReferenceService { get; set; } = null!;
    [Inject] private IPlacementService PlacementService { get; set; } = null!;

    #region Fields

    /// <summary>
    /// The unique identifier for the dialog component instance.<br/>
    /// If not provided, a unique ID will be generated using the <see cref="Cryptography.GenerateId(int)"/> method.<br/>
    /// This ID is used to reference the dialog in JavaScript interop calls and for accessibility purposes.
    /// </summary>
    [Parameter, Required]
    public required string Id { get; set; } = Cryptography.GenerateId();

    private const string CloseButtonFallback = "Close dialog";

    private string CloseButtonDefault
    {
        get
        {
            var value = Localizer.GetValue("Hviktor.Components.Dialog.Button.Close");
            return string.IsNullOrWhiteSpace(value) ? CloseButtonFallback : value;
        }
    }

    /// <summary>
    /// Screen reader label of close button. Set false to hide the close button.
    /// </summary>
    [Parameter]
    public string? CloseButton { get; set; }

    private bool IsClosable => !string.Equals(CloseButton, "false", StringComparison.OrdinalIgnoreCase);

    private string GetCloseButtonAriaLabel()
    {
        if (string.IsNullOrWhiteSpace(CloseButton))
        {
            return CloseButtonDefault;
        }

        return CloseButton.Equals("false", StringComparison.OrdinalIgnoreCase) ? string.Empty : CloseButton;
    }

    /// <summary>
    /// The ChildContent to render inside the <see cref="Dialog"/>.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private ElementReference? ElementRef { get; set; }

    /// <summary>
    /// Additional attributes that will be applied to the dialog component.<br/>
    /// This allows for custom attributes to be added to the dialog's root element, such as data attributes or ARIA attributes.<br/>
    /// These attributes will be merged with the default attributes defined by the component.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
            .AddIdentity(Id)
            .AddClasses("ds-dialog");

        EnumValue<Placement> placement = builder.ConsumeAttribute("placement") ?? builder.ConsumeAttribute("data-placement");
        if (!placement.IsEmpty)
        {
            builder.AddDataAttribute("placement", PlacementService.GetDataAttribute(placement));
        }

        var modal = builder.ConsumeAttribute("modal");
        builder.AddDataAttribute("modal", modal is not null ? "true" : "false");

        return builder;
    }

    private IJSObjectReference? DialogModule { get; set; }

    private readonly TaskCompletionSource moduleReady = new(TaskCreationOptions.RunContinuationsAsynchronously);

    #endregion

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                DialogModule = await JsRuntimeService.ImportAsync<Dialog>();
                moduleReady.TrySetResult();
            }
            catch (Exception ex)
            {
                moduleReady.TrySetException(ex);
            }
        }
    }

    /// <summary>
    /// Waits for the JS module to be imported and available.
    /// </summary>
    private Task WaitForModuleAsync() => moduleReady.Task;

    #region State Management

    /// <summary>
    /// Opens the dialog as a modal by invoking the JavaScript <c>showModal()</c> method.<br/>
    /// Awaits JS module initialization if it has not completed yet.
    /// </summary>
    /// <remarks>
    /// Call this directly from component code-behind methods.
    /// From Razor markup (e.g. <c>@onclick</c>), prefer <see cref="ShowModal"/> which
    /// marshals onto the Blazor synchronization context.
    /// </remarks>
    /// <returns>A task that represents the asynchronous open operation.</returns>
    public async Task ShowModalAsync()
    {
        await WaitForModuleAsync();

        try
        {
            await JsObjectReferenceService.InvokeVoidAsync<Dialog>(DialogModule, "open", ElementRef);
            LogOpenRequestSent(Logger, Id);
        }
        catch (Exception ex)
        {
            LogErrorOpeningDialog(Logger, ex, Id);
        }
    }

    /// <summary>
    /// Opens the dialog as a modal, dispatched via <see cref="ComponentBase.InvokeAsync(Func{Task})"/>
    /// to ensure the call runs on the Blazor synchronization context.
    /// </summary>
    /// <remarks>
    /// Use this from Razor markup (e.g. <c>@onclick="@(() => DialogRef.ShowModal())"</c>).
    /// </remarks>
    /// <returns>A task that represents the asynchronous open operation.</returns>
    public Task ShowModal() => InvokeAsync(ShowModalAsync);

    /// <summary>
    /// Closes the dialog by invoking the JavaScript <c>close()</c> method.<br/>
    /// Awaits JS module initialization if it has not completed yet.
    /// </summary>
    /// <remarks>
    /// Call this directly from component code-behind methods.
    /// From Razor markup (e.g. <c>@onclick</c>), prefer <see cref="Close"/> which
    /// marshals onto the Blazor synchronization context.
    /// </remarks>
    /// <returns>A task that represents the asynchronous close operation.</returns>
    public async Task CloseAsync()
    {
        await WaitForModuleAsync();

        try
        {
            await JsObjectReferenceService.InvokeVoidAsync<Dialog>(DialogModule, "close", ElementRef);
            LogCloseRequestSent(Logger, Id);
        }
        catch (Exception ex)
        {
            LogErrorClosingDialog(Logger, ex, Id);
        }
    }

    /// <summary>
    /// Closes the dialog, dispatched via <see cref="ComponentBase.InvokeAsync(Func{Task})"/>
    /// to ensure the call runs on the Blazor synchronization context.
    /// </summary>
    /// <remarks>
    /// Use this from Razor markup (e.g. <c>@onclick="@(() => DialogRef.Close())"</c>).
    /// </remarks>
    /// <returns>A task that represents the asynchronous close operation.</returns>
    public Task Close() => InvokeAsync(CloseAsync);

    #endregion

    #region IDisposable

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        // Cancel the TCS so WaitForModuleAsync does not hang indefinitely if
        // the component is disposed before OnAfterRenderAsync completes.
        moduleReady.TrySetCanceled();

        try
        {
            if (DialogModule is not null)
            {
                LogDisposingComponent(Logger);
                await DialogModule.DisposeAsync();
            }
        }
        catch (TaskCanceledException)
        {
            LogTaskCanceled(Logger);
        }
        catch (JSDisconnectedException)
        {
            LogJsDisconnected(Logger);
        }
    }

    #endregion

    // 100-199: Lifecycle events (disposal, cleanup)
    // 200-299: User actions (open, close)
    // 300-399: Warnings
    // 400-499: Errors

    [LoggerMessage(EventId = 100, Level = LogLevel.Trace, Message = "Disposing Dialog component")]
    private static partial void LogDisposingComponent(ILogger logger);

    [LoggerMessage(EventId = 101, Level = LogLevel.Trace, Message = "Dialog disposal task was canceled")]
    private static partial void LogTaskCanceled(ILogger logger);

    [LoggerMessage(EventId = 102, Level = LogLevel.Trace, Message = "Dialog JS module disconnected during disposal")]
    private static partial void LogJsDisconnected(ILogger logger);

    [LoggerMessage(EventId = 200, Level = LogLevel.Debug, Message = "Close request sent for dialog '{DialogId}'")]
    private static partial void LogCloseRequestSent(ILogger logger, string dialogId);

    [LoggerMessage(EventId = 201, Level = LogLevel.Debug, Message = "Open request sent for dialog '{DialogId}'")]
    private static partial void LogOpenRequestSent(ILogger logger, string dialogId);

    [LoggerMessage(EventId = 400, Level = LogLevel.Error, Message = "Error occurred while opening dialog '{DialogId}'")]
    private static partial void LogErrorOpeningDialog(ILogger logger, Exception exception, string dialogId);

    [LoggerMessage(EventId = 401, Level = LogLevel.Error, Message = "Error occurred while closing dialog '{DialogId}'")]
    private static partial void LogErrorClosingDialog(ILogger logger, Exception exception, string dialogId);
}