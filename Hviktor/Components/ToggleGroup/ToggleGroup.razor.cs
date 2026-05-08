using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Abstractions.Interfaces.Services.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Hviktor.Components.ToggleGroup;

/// <summary>
/// <c>ToggleGroup</c> groups related options. The component consists of a group of buttons that are connected, where only one button can be selected at a time.
/// </summary>
/// <use>
/// Use <c>ToggleGroup</c> when:
/// <list type="bullet">
/// <item>You need to switch between views, for example between a list and a graph</item>
/// <item>Content in a list or table needs to be filtered</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>ToggleGroup</c> when:
/// <list type="bullet">
/// <item>There are so many options that they do not fit within the available width</item>
/// <item>The options contain long text, making the group heavy and difficult to scan</item>
/// <item>The options represent answers in a form - use <c>Radio</c> instead</item>
/// <item>The choice is an on/off setting - use <c>Switch</c> instead</item>
/// </list>
/// </avoid>
/// <guidelines>
/// It is not always straightforward to decide when to use a <c>ToggleGroup</c> instead of components such as <c>Tabs</c> or <c>Radio</c>.<br/>
/// The choice depends on the context and how the rest of the interface is structured.As a general rule,
/// <c>ToggleGroup</c> should be used when the selection has a direct and visible effect in the interface — for example when the content on the page or a specific element is updated immediately based on the user’s choice.
/// <br/><br/>
/// A <c>ToggleGroup</c> should have at least two options, but not so many that they no longer fit horizontally.<br/>
/// Ensure there is enough space for both the content of each option and the number of options without causing line breaks.<br/>
/// Make sure the entire component fits on screen, including on mobile, and that all options remain clearly visible to users.
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
///       <b>size</b>: <see cref="Size"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Allowed</b>: <see cref="Size.Small"/> | <see cref="Size.Medium"/> | <see cref="Size.Large"/><br/>
///         <b>Description</b>: ToggleGroup size.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>color</b>: <see cref="Color"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Allowed</b>: <see cref="Color.Accent"/> | <see cref="Color.Neutral"/> | <see cref="Color.Info"/> | <see cref="Color.Success"/> | <see cref="Color.Warning"/> | <see cref="Color.Danger"/><br/>
///         <b>Description</b>: Color theme.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>variant</b>: <see cref="Variant"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Default</b>: <see cref="Variant.Primary"/><br/>
///         <b>Allowed</b>: <see cref="Variant.Primary"/> | <see cref="Variant.Secondary"/><br/>
///         <b>Description</b>: Specify which variant to use.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>data-toggle-group</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Description</b>: Toggle group label for accessibility.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>@onchange</b>: <see cref="EventCallback"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///         <b>Description</b>: Callback with selected <c>ToggleGroupItem</c> <c>value</c>.
///     </description>
///   </item>
/// </list>
/// </parameters>
public sealed partial class ToggleGroup : AsyncCascadingComponentBase
{
    [Inject] private ILogger<ToggleGroup> Logger { get; set; } = null!;

    [Inject] private ISizeService SizeService { get; set; } = null!;
    [Inject] private IColorService ColorService { get; set; } = null!;
    [Inject] private IVariantService VariantService { get; set; } = null!;

    [Inject] private IJsRuntimeService JsRuntimeService { get; set; } = null!;
    [Inject] private IJsObjectReferenceService JsObjectReferenceService { get; set; } = null!;

    /// <summary>
    /// Specifies the name attribute that is applied to all child elements within the <see cref="ToggleGroup"/>.
    /// This property enables grouping of related items for accessibility and form functionality.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Specifies the initial value to be used when the component operates in uncontrolled mode,
    /// where no external binding is provided for the <see cref="Value"/> property.
    /// This value is used to set the initial internal state of the component.
    /// </summary>
    [Parameter]
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Represents the currently selected value within the <see cref="ToggleGroup"/>.
    /// This property allows external control of the selection state by binding a value.
    /// Changes to this property trigger the <see cref="ValueChanged"/> callback.
    /// </summary>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// A callback that is triggered whenever the <see cref="Value"/> property changes.
    /// This event allows consumers of the <see cref="ToggleGroup"/> to respond to state changes
    /// by providing custom logic or updating external state.
    /// </summary>
    [Parameter]
    public EventCallback<string?> ValueChanged { get; set; }

    private ElementReference ElementRef { get; set; }

    /// <summary>
    /// Internal state used when the component operates in uncontrolled mode
    /// (i.e. <see cref="ValueChanged"/> has no external binding).
    /// Initialised from <see cref="DefaultValue"/> on first render.
    /// </summary>
    internal string? InternalValue { get; private set; }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (ValueChanged.HasDelegate)
        {
            // Controlled mode: always sync from the bound Value parameter.
            InternalValue = Value;
        }
        else
        {
            // Uncontrolled mode: seed from DefaultValue only once.
            InternalValue ??= DefaultValue;
        }
    }

    internal async Task OnClickEventAsync(string value)
    {
        InternalValue = value;
        await ValueChanged.InvokeAsync(value);
        StateHasChanged();
    }

    /// <summary>
    /// The ChildContent to render inside the ToggleGroup.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private IJSObjectReference? ToggleGroupModule { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder
            .ToDictionary(base.ComputeAttributes())
            .AddClasses("ds-toggle-group");

        EnumValue<Size> size = builder.ConsumeAttribute("size") ?? builder.ConsumeAttribute("data-size");
        if (!size.IsEmpty)
        {
            builder.AddDataAttribute("size", SizeService.GetDataAttribute(size));
        }

        EnumValue<Color> color = builder.ConsumeAttribute("color") ?? builder.ConsumeAttribute("data-color");
        if (!color.IsEmpty)
        {
            builder.AddDataAttribute("color", ColorService.GetDataAttribute(color));
        }

        EnumValue<Variant> variant = builder.ConsumeAttribute("variant") ?? builder.ConsumeAttribute("data-variant");
        builder.AddDataAttribute("variant", VariantService.GetDataAttribute(variant, Variant.Primary));

        return builder;
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            ToggleGroupModule = await JsRuntimeService.ImportAsync<ToggleGroup>();
            await InvokeVoidAsync("initialize");
        }
    }

    private async Task InvokeVoidAsync(string method)
        => await JsObjectReferenceService.InvokeVoidAsync<ToggleGroup>(ToggleGroupModule, method, ElementRef);

    #region IDisposable

    /// <inheritdoc/>
    protected override async ValueTask DisposeAsyncCore()
    {
        try
        {
            if (ToggleGroupModule is not null)
            {
                LogMethodcallClassDisposingComponent(Logger);
                await InvokeVoidAsync("dispose");
                await ToggleGroupModule.DisposeAsync();
            }
        }
        catch (TaskCanceledException)
        {
            LogMethodcallClassTaskWasCanceled(Logger);
        }
        catch (JSDisconnectedException)
        {
            LogMethodcallClassJsDisconnected(Logger);
        }
        catch (InvalidOperationException)
        {
            // Ignore - component is being statically rendered (prerendering),
            // JS interop is not available in this context
        }
        finally
        {
            await base.DisposeAsyncCore();
        }
    }

    #endregion

    [LoggerMessage(LogLevel.Trace, "Disposing component.")]
    static partial void LogMethodcallClassDisposingComponent(ILogger<ToggleGroup> logger);

    [LoggerMessage(LogLevel.Trace, "Task was canceled.")]
    static partial void LogMethodcallClassTaskWasCanceled(ILogger<ToggleGroup> logger);

    [LoggerMessage(LogLevel.Trace, "JS disconnected.")]
    static partial void LogMethodcallClassJsDisconnected(ILogger<ToggleGroup> logger);
}