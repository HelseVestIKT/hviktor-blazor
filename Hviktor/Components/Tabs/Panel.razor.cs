using Hviktor.Models;
using Hviktor.Rendering;
using Hviktor.Security;
using Microsoft.AspNetCore.Components;

// ReSharper disable once CheckNamespace
namespace Tabs;

/// <summary>
/// The <c>Panel</c> is a child component of the <c>Tabs</c> component and is used to define its own content that is displayed when its tab is selected.
/// </summary>
/// <parameters>
/// <para>Additional attributes</para>
/// <list type="table">
///   <listheader>
///     <term>Parameter</term>
///     <description></description>
///   </listheader>
///   <item>
///     <term>
///       <b>value</b>: <see cref="string"/><br/>
///       <i>(required)</i>
///     </term>
///     <description>
///       When this value is selected as the current state, render this <c>Tabs.Panel</c> component.<br/>
///       <b>Note</b>: <c>value</c> must match the value of a <c>Tabs.Tab</c> component.
///     </description>
///   </item>
/// </list>
/// </parameters>
public partial class Panel : NestedComponentBase<Hviktor.Components.Tabs.Tabs>
{
    internal bool IsSelected { get; private set; }

    internal string? PanelId { get; set; } = $"tabpanel-{Cryptography.GenerateId()}";
    internal string? TabId { get; set; } = $"tab-{Cryptography.GenerateId()}";

    private Dictionary<string, object?>? preComputedAttributes;

    internal string InternalValue = string.Empty;

    /// <summary>
    /// The content to be displayed inside the tab panel.<br/>
    /// This content is typically rendered when the corresponding tab is selected, allowing users to view the associated information or interface related to that tab.<br/>
    /// It can include text, images, or any other markup as needed to effectively present the content relevant to the selected tab.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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
            .AddIdentity(PanelId)
            .AddAttribute("role", "tabpanel")
            .AddAttribute("aria-labelledby", TabId)
            .AddAttribute("hidden", IsSelected is not true)
            .AddToNaturalTabOrder();

        var value = builder.GetValue("value");
        if (value is not null)
        {
            InternalValue = value;
        }

        preComputedAttributes = builder;
        return preComputedAttributes;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        preComputedAttributes = null;
        ComputeAttributes();
        Parent?.AddPanel(this);
    }

    /// <summary>
    /// Sets the selected state of the tab panel.
    /// </summary>
    /// <param name="selected"></param>
    public void SetSelectedState(bool selected)
    {
        IsSelected = selected;
        preComputedAttributes = null;
        StateHasChanged();
    }
}