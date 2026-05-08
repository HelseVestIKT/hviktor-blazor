using Hviktor.Models;
using Hviktor.Rendering;
using Microsoft.AspNetCore.Components;
using Tabs;

namespace Hviktor.Components.Tabs;

/// <summary>
/// <c>Tabs</c> allow users to navigate between related sections of content, with only one section visible at a time.<br/>
/// This is an effective way to organize and present related content on the same page.
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
///       <b>value</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Controlled state for <c>Tabs</c> component.<br/>
///                     If not set, the component will manage the selected panel internally, starting with <c>defaultValue</c> if provided.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>defaultValue</b>: <see cref="string"/>?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Default selected tab value<br/>
///                     If <b>value</b> is not set, this will determine the initially selected tab.
///                     If <b>value</b> is set, this will only be used for the initial selection when the corresponding panel registers, and ignored afterwards.
///     </description>
///   </item>
///   <item>
///     <term>
///       <b>@onchange</b>: EventCallback&lt;ChangeEventArgs&gt;?<br/>
///       <i>(optional)</i>
///     </term>
///     <description>
///       <b>Description</b>: Callback with selected <see cref="Tab"/> value when selection changes.
///     </description>
///   </item>
/// </list>
/// </parameters>
/// <use>
/// Use <c>Tabs</c> when:
/// <list type="bullet">
///   <item>Content needs to be structured without loading a new page</item>
///   <item>Information benefits from being divided into clearly labelled sections</item>
///   <item>Navigation becomes easier when users do not need to view all content at once</item>
/// </list>
/// </use>
/// <avoid>
/// Avoid <c>Tabs</c> when:
/// <list type="bullet">
///   <item>It is important to give an immediate overview of the content</item>
///   <item>Information needs to be compared across sections</item>
///   <item>Users must go through a step-by-step process where the order matters</item>
///   <item>Large amounts of content may slow down the page</item>
///   <item>The content could just as well be shown on a single page or spread across several pages</item>
/// </list>
/// </avoid>
/// <guidelines>
/// <c>Tabs</c> are used to organise related content into separate sections.<br/>
/// Alternatives to Tabs<br/>
/// Before choosing Tabs, consider whether it might be better to:
/// <list type="bullet">
///   <item>Simplify and reduce the amount of content</item>
///   <item>Distribute the content across several pages</item>
///   <item>Keep everything on a single page with clear headings</item>
///   <item>Use a table of contents for easier navigation</item>
/// </list>
/// </guidelines>
public partial class Tabs : CascadingComponentBase
{
    /// <summary>
    /// Content rendered inside the tabs component.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string? internalValue;
    private string? previousInternalValue;
    internal string? internalDefaultValue;
    private bool IsControlled => internalValue is not null;
    private Dictionary<string, object?>? preComputedAttributes;
    private Dictionary<string, Panel> PanelList { get; } = [];

    /// <summary>
    /// Computes the attributes for the tabs component
    /// </summary>
    /// <returns>A dictionary of HTML attributes to be applied to the root element.</returns>
    protected override Dictionary<string, object?> ComputeAttributes()
    {
        if (preComputedAttributes is not null)
        {
            return preComputedAttributes;
        }

        var builder = HtmlAttributeBuilder.ToDictionary(base.ComputeAttributes())
            .AddClasses("ds-tabs");

        internalDefaultValue = builder.ConsumeAttribute("defaultValue");
        internalValue = builder.ConsumeAttribute("value");

        preComputedAttributes = builder;
        return preComputedAttributes;
    }

    /// <inheritdoc />
    protected override void OnParametersSet()
    {
        preComputedAttributes = null;
        ComputeAttributes();

        // In controlled mode, apply the new value when it changes
        if (IsControlled && previousInternalValue != internalValue)
        {
            previousInternalValue = internalValue;
            SetSelection(internalValue);
        }
    }

    /// <summary>
    /// Sets the selected panel to the panel with the given key.
    /// </summary>
    /// <param name="key">The key of the panel to select.</param>
    internal void SetSelection(string? key)
    {
        foreach (var panel in PanelList.Values)
        {
            panel.SetSelectedState(panel.InternalValue == key);
        }

        StateHasChanged();
    }

    /// <summary>
    /// Checks if the panel with the given key is currently selected.
    /// </summary>
    /// <param name="key">The key of the panel to check.</param>
    /// <returns>True if the panel is selected, false otherwise.</returns>
    internal bool IsCurrentSelected(string? key)
    {
        // In controlled mode, compare directly against the controlled value
        if (IsControlled)
        {
            return key == internalValue;
        }

        var panel = PanelList.Values.FirstOrDefault(p => p.InternalValue == key);

        // If panel not registered yet, check against defaultValue
        if (panel is null)
        {
            return key == internalDefaultValue;
        }

        return panel.IsSelected;
    }

    /// <summary>
    /// Registers a new panel with the tabs component.
    /// </summary>
    /// <param name="panel">The panel to register.</param>
    internal void AddPanel(Panel panel)
    {
        if (!PanelList.TryAdd(panel.InternalValue, panel))
        {
            // Update existing panel reference if re-registering
            PanelList[panel.InternalValue] = panel;
            return;
        }

        // In controlled mode, match against the controlled value
        if (IsControlled)
        {
            panel.SetSelectedState(panel.InternalValue == internalValue);
            return;
        }

        // Apply default selection only on initial registration
        if (!string.IsNullOrWhiteSpace(internalDefaultValue))
        {
            panel.SetSelectedState(panel.InternalValue == internalDefaultValue);
        }
    }

    internal void AddTab(Tab tab)
    {
        var id = tab.InternalValue ?? string.Empty;
        if (PanelList.TryGetValue(id, out var panel))
        {
            panel.TabId = tab.InternalId;
        }
    }

    internal string? GetPanelId(string? tabValue)
    {
        var match = PanelList.Values.FirstOrDefault(panel => panel.InternalValue == tabValue);
        return match?.PanelId;
    }
}