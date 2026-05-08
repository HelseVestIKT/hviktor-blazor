using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Tabs;

/// <summary>
/// <c>Tab</c> is a part of a set of tabs.<br/>
/// This component is responsible for encapsulating the logic and rendering of a single tab.
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
///       <b>value</b>: <see cref="string"/><br/>
///       <i>(required)</i>
///     </term>
///     <description>
///       <b>Description</b>: Unique value that will be set in the <see cref="Tabs"/> components state when the <c>tab</c> is activated.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Tab : NestedComponentBase<List>
{
    /// <summary>
    /// The HTML Content to render inside the <see cref="Tab"/> component.<br/>
    /// This content typically represents the label or title of the tab that users will see and interact with to switch between different panels.<br/>
    /// It can include text, icons, or any other markup as needed to effectively convey the purpose of the tab.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    internal string? InternalId;
    internal string? InternalValue;

    private Dictionary<string, object?>? preComputedAttributes;

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        preComputedAttributes = null;
        ComputeAttributes();
        Parent?.Parent?.AddTab(this);
    }

    /// <summary>
    /// Captures all unmatched attributes and applies them to the root element.
    /// </summary>
    /// <returns>A dictionary of HTML attributes to be applied to the root element.</returns>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddAttribute("type", "button")
            .AddAttribute("role", "tab");

        InternalId = builder.GetValue("id");
        if (InternalId is null)
        {
            InternalId = Cryptography.GenerateId();
            builder.AddIdentity(InternalId);
        }

        InternalValue = builder.ConsumeAttribute("value") ?? builder.ConsumeAttribute("data-value");
        if (InternalValue is not null)
        {
            builder.AddDataAttribute("value", InternalValue);
        }

        var isSelected = Parent?.IsCurrentSelected(InternalValue) == true;
        builder.AddAttribute("aria-controls", Parent?.GetPanelId(InternalValue));
        builder.AddAttribute("aria-selected", isSelected ? "true" : "false");

        // WAI-ARIA roving tabindex: selected tab is in the tab order, others are not
        if (isSelected)
        {
            builder.AddToNaturalTabOrder();
        }
        else
        {
            builder.RemoveFromTabOrder();
        }

        preComputedAttributes = builder;
        return preComputedAttributes;
    }

    private void OnClickEvent() => Parent?.SetSelectedState(InternalValue);
}