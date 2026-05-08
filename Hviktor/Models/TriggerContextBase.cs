using System.ComponentModel.DataAnnotations;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Models;

/// <summary>
/// Base class for trigger context components that provide an Id to child triggerable components.
/// </summary>
public class TriggerContextBase : AsyncCascadingComponentBase
{
    /// <summary>
    /// The unique identifier for the trigger context.
    /// </summary>
    [Parameter, Required]
    public string Id { get; set; } = Cryptography.GenerateId();

    /// <summary>
    /// The content to be displayed inside the trigger context.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}