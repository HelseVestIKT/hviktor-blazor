using Bunit;
using Tabs;
using TabsComponent = Hviktor.Components.Tabs.Tabs;

namespace Tests.Unit.Components.Tabs;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Tabs")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Cascading)]
public class TabsTests : HviktorBunitContext
{

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void Tabs_RendersAsDIVElement()
    {
        var component = Render<TabsComponent>();

        var element = component.Find("div.ds-tabs");
        Assert.Equal("DIV", element.TagName);
    }

    [Fact]
    public void Tabs_HasDsTabsClass()
    {
        var component = Render<TabsComponent>();

        var element = component.Find("div.ds-tabs");
        Assert.Contains("ds-tabs", element.ClassList);
    }

    [Fact]
    public void Tabs_RendersChildContent()
    {
        var component = Render<TabsComponent>(parameters => parameters
            .AddChildContent("<span>Tab content</span>"));

        var element = component.Find("div.ds-tabs");
        Assert.Contains("Tab content", element.InnerHtml);
    }

    [Fact]
    public void Tabs_AppliesAdditionalAttributes()
    {
        var component = Render<TabsComponent>(parameters => parameters
            .AddUnmatched("data-testid", "tabs-test")
            .AddUnmatched("aria-label", "Navigation tabs"));

        var element = component.Find("div.ds-tabs");
        Assert.Equal("tabs-test", element.GetAttribute("data-testid"));
        Assert.Equal("Navigation tabs", element.GetAttribute("aria-label"));
    }

    [Fact]
    public void Tabs_AppliesCustomCssClass()
    {
        var component = Render<TabsComponent>(parameters => parameters
            .AddUnmatched("class", "custom-tabs"));

        var element = component.Find("div.ds-tabs");
        Assert.Contains("custom-tabs", element.ClassList);
        Assert.Contains("ds-tabs", element.ClassList);
    }

    [Fact]
    public void Tabs_AcceptsDefaultValue()
    {
        var component = Render<TabsComponent>(parameters => parameters
            .AddUnmatched("defaultValue", "tab-1"));

        Assert.Equal("tab-1", component.Instance.internalDefaultValue);
    }

    #endregion

    #region AddPanel Re-registration Tests

    [Fact]
    public void AddPanel_WithDuplicateValue_UpdatesPanelReference()
    {
        var component = Render<TabsComponent>();

        var panelListProperty = typeof(TabsComponent)
            .GetProperty("PanelList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var addPanelMethod = typeof(TabsComponent)
            .GetMethod("AddPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var panelList = (Dictionary<string, Panel>)panelListProperty!.GetValue(component.Instance)!;

#pragma warning disable BL0005 // Component parameter should not be set outside of its component
        var originalPanel = new Panel { InternalValue = "panel-a" };
        var replacementPanel = new Panel { InternalValue = "panel-a" };
#pragma warning restore BL0005

        addPanelMethod!.Invoke(component.Instance, [originalPanel]);
        Assert.Single(panelList);
        Assert.Same(originalPanel, panelList["panel-a"]);

        // Re-register with the same key to test the TryAdd failure path
        addPanelMethod.Invoke(component.Instance, [replacementPanel]);
        Assert.Single(panelList);
        Assert.NotSame(originalPanel, panelList["panel-a"]);
        Assert.Same(replacementPanel, panelList["panel-a"]);
    }

    [Fact]
    public void AddPanel_WithNewValue_AddsPanelToList()
    {
        var component = Render<TabsComponent>();

        var panelListProperty = typeof(TabsComponent)
            .GetProperty("PanelList", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var addPanelMethod = typeof(TabsComponent)
            .GetMethod("AddPanel", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        var panelList = (Dictionary<string, Panel>)panelListProperty!.GetValue(component.Instance)!;

#pragma warning disable BL0005 // Component parameter should not be set outside of its component
        var panelA = new Panel { InternalValue = "panel-a" };
        var panelB = new Panel { InternalValue = "panel-b" };
#pragma warning restore BL0005

        addPanelMethod!.Invoke(component.Instance, [panelA]);
        Assert.Single(panelList);

        addPanelMethod.Invoke(component.Instance, [panelB]);
        Assert.Equal(2, panelList.Count);
        Assert.Same(panelA, panelList["panel-a"]);
        Assert.Same(panelB, panelList["panel-b"]);
    }

    #endregion
}