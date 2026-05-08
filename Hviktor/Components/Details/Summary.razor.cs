using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Details;

/// <summary>
/// <c>Summary</c> component is used as a nested element within the <c>Details</c> component.<br/>
/// It serves as the summary or title of the details section, which users can click to expand or collapse the content of the <c>Details</c> component.<br/>
/// The content of the <c>Summary</c> component is typically a brief description or heading that gives users an idea of what information is contained within the details section when it is expanded.
/// </summary>
public partial class Summary : NestedComponentBase<Hviktor.Components.Details.Details>
{
    /// <summary>
    /// The content to be rendered within the <c>Summary</c> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
        .AddAttribute("role", "button")
        .AddAttribute("slot", "summary")
        .AddAttribute("aria-expanded", "false")
        .AddToNaturalTabOrder();
}