using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

namespace Hviktor.Components.Divider;

/// <summary>
/// <c>Divider</c> is used to create a visual separation between content. It is a simple horizontal line that spans the available width.
/// </summary>
/// <use>
/// Use <c>Divider</c> when:
/// <list type="bullet">
/// <item>You want to visually separate content</item>
/// <item>You want to add a visual separation between sections of content</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid using <c>Divider</c> when:
/// <list type="bullet">
/// <item>Natural spacing or other visual elements provide enough separation </item>
/// <item>Too many Divider elements create a cluttered visual appearance</item>
/// </list>
/// </avoid>
/// <guidelines>
/// Divider is used to break content into smaller sections, making it easier to read and understand.<br/>
/// It can also be used to separate content that is related but still benefits from a visual distinction.
/// </guidelines>
public partial class Divider : ComponentBase
{
    /// <summary>
    /// Represents additional attributes that will be applied to the root element of the component.
    /// This allows for flexible customization and extension of the component's behavior and appearance by passing arbitrary attributes.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

    private Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(AdditionalAttributes)
        .AddClasses("ds-divider")
        .HideFromAccessibility();
}