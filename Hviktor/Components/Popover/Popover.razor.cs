using Hviktor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Hviktor.Components.Popover;

/// <summary>
/// <c>Popover</c> appears above other elements in the interface and is linked to a specific element.
/// It is used to display additional information, interactive elements, or brief explanations without navigating away from the page.
/// </summary>
public partial class Popover : PopoverBase
{
    [Inject] private ILogger<Popover> Logger { get; set; } = null!;

    /// <inheritdoc/>
    protected override string ComponentName => nameof(Popover);

    /// <inheritdoc/>
    protected override ILogger ComponentLogger => Logger;

    /// <inheritdoc/>
    protected override IDisposable CreateDotNetObjectReference()
        => DotNetObjectReference.Create(this);
}