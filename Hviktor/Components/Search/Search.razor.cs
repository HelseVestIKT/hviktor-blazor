using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Hviktor.Components.Search;

/// <summary>
/// <c>Search</c> allows users to quickly find relevant content on a website or in an application.
/// The component consists of a search field, with or without a search button.
/// </summary>
/// <use>
/// Use <c>Search</c> when:
/// <list type="bullet">
/// <item>Users need help finding relevant information quickly on a website or in an application</item>
/// <item>Users are expected to enter keywords or phrases to locate the most relevant content</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Search</c> when:
/// <list type="bullet">
/// <item>The content is easy to navigate without search</item>
/// <item>It replaces good navigation — search should complement navigation, not be the only method</item>
/// </list>
/// </avoid>
/// <guidelines>
/// The width of the search field should reflect the length of the search terms users typically enter.
/// The size of the field signals what users can type.<br/>
/// For example, a search field for national identity numbers should match the length of such a number.
/// A main search field for a website should be wider so users can see several words at once.
/// To avoid forcing users to scroll within the field, ensure it is not so short that parts of the text become hidden.
/// </guidelines>
public sealed partial class Search : AsyncCascadingComponentBase
{
    [Inject] private ILogger<Search> Logger { get; set; } = null!;
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    /// <summary>
    /// Gets or sets the content to be rendered inside the component.
    /// This allows consumers of the component to define custom content
    /// that will be displayed within the component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the component.
    /// This identifier is used internally for initializing and managing
    /// the lifecycle of the component and can also help to differentiate
    /// it from other components in the same context.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = Cryptography.GenerateId();

    private DotNetObjectReference<Search>? dotNetRef;

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddIdentity(Id)
        .AddClasses("ds-search");

    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            try
            {
                dotNetRef = DotNetObjectReference.Create(this);
                await JsRuntime.InvokeVoidAsync("globalThis.Hviktor.Search.initializeSearch", Id, dotNetRef);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Failed to initialize Search component JavaScript interop");
            }
        }
    }

    /// <inheritdoc/>
    protected override async ValueTask DisposeAsyncCore()
    {
        try
        {
            if (dotNetRef is not null)
            {
                await JsRuntime.InvokeVoidAsync("globalThis.Hviktor.Search.disposeSearch", Id);
            }

            dotNetRef?.Dispose();
        }
        catch (JSDisconnectedException)
        {
            // Ignore - circuit is already disconnected
        }
        catch (InvalidOperationException)
        {
            // Ignore - component is being statically rendered (prerendering),
            // JS interop is not available in this context
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error disposing Search component");
        }
        finally
        {
            await base.DisposeAsyncCore();
        }
    }
}