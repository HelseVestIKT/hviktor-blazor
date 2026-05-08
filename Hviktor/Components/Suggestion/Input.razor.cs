using Hviktor.Models.Suggestion;
using Hviktor.Rendering;

// ReSharper disable once CheckNamespace
namespace Suggestion;

/// <summary>
/// Represents an input component that is a child of an asynchronous suggestion component.
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
///       <b>id</b>: <see cref="string"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: This attribute is controlled by the parent <see cref="Suggestion"/> components <c>id</c> attribute.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>aria-controls</b>: <see cref="string"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: This attribute is controlled by the parent <see cref="Suggestion"/> components <c>id</c> attribute.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>readonly</b>: <see cref="bool"/><br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Default</b>: <see langword="true"/><br/>
///       <b>Allowed</b>: <see langword="true"/> | <see langword="false"/><br/>
///       <b>Description</b>: This attribute is controlled by the parent <see cref="Suggestion"/> components <c>readonly</c> property.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <remarks>
/// The <see cref="Input"/> class extends functionality from <see cref="AsyncSuggestionChildBase"/>
/// and provides specific attributes and behavior required for handling input within a suggestion component.
/// </remarks>
public partial class Input : AsyncSuggestionChildBase
{
    private IReadOnlyList<string> Values { get; set; } = [];

    /// <inheritdoc/>
    protected override void OnInitialized()
    {
        base.OnInitialized();
        if (Parent is not null)
        {
            Values = Parent.InternalSelected;
        }
    }

    /// <inheritdoc/>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddClasses("ds-input")
            .AddAttribute("type", "text")
            .AddAttribute("placeholder", "")
            .AddAttribute("role", "combobox")
            .AddAttribute("aria-autocomplete", "list")
            .AddAttribute("aria-expanded", "false")
            .AddAttribute("aria-haspopup", "listbox")
            .AddAttribute("autocomplete", "off");

        if (!string.IsNullOrWhiteSpace(Parent?.InternalId))
        {
            var id = Parent.InternalId!;
            builder.AddIdentity($"{id}-input");
            builder.AddAttribute("aria-controls", $"{id}-list");
        }

        builder.AddAttribute("readonly", () => ReadOnly, true);
        return builder;
    }
}