using Hviktor.Abstractions.Interfaces.Localization;
using Hviktor.Models.Suggestion;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Suggestion;

/// <summary>
/// Renders a clear button inside a <see cref="Hviktor.Components.Suggestion.Suggestion"/> component,
/// allowing users to remove the current selection.
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
///       <b>aria-label</b>: <see cref="string"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: "Clear selection"<br/>
///       <b>Description</b>: The aria-label attribute provides an accessible name for the clear button.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>role</b>: <see cref="string"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: "button"<br/>
///       <b>Description</b>: <see href="https://developer.mozilla.org/en-US/docs/Web/Accessibility/ARIA/Roles/button_role">HTML button role (MDN)</see>.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Clear : AsyncSuggestionChildBase
{
    [Inject] private IStringLocalizerService<Hviktor.Resources.Resources> Localizer { get; set; } = null!;

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes() => HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
        .AddToNaturalTabOrder()
        .AddAttribute("aria-label", Localizer.GetValue("Hviktor.Components.Suggestion.Clear"))
        .AddAttribute("role", "button");
}