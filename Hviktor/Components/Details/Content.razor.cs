using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Details;

/// <summary>
/// <c>Content</c> component is used as a nested element within the <c>Details</c> component.<br/>
/// It serves as the main content of the details section, which is displayed when the <c>Details</c> component is expanded.
/// </summary>
public partial class Content : NestedComponentBase<Hviktor.Components.Details.Details>
{
    /// <summary>
    /// The content to be rendered within the <c>Content</c> component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
        => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes);
}