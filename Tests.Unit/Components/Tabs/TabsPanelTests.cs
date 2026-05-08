using Bunit;
using Tabs;

namespace Tests.Unit.Components.Tabs;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Tabs.Panel")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class TabsPanelTests : HviktorBunitContext
{

    [Fact]
    public void TabsPanel_NotVisibleByDefault()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<Panel>(panelParams => panelParams
                .AddUnmatched("value", "panel-1")
                .AddChildContent("Panel Content")));

        var panels = component.FindAll("div[role=tabpanel]:not([hidden])");
        Assert.Empty(panels);
    }

    [Fact]
    public void TabsPanel_VisibleWhenSelected()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddUnmatched("defaultValue", "panel-1")
            .AddChildContent<Panel>(panelParams => panelParams
                .AddUnmatched("value", "panel-1")
                .AddChildContent("Panel Content")));

        var panel = component.Find("div[role=tabpanel]:not([hidden])");
        Assert.NotNull(panel);
        Assert.Contains("Panel Content", panel.TextContent);
    }

    [Fact]
    public void TabsPanel_HasRoleTabpanel()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddUnmatched("defaultValue", "panel-1")
            .AddChildContent<Panel>(panelParams => panelParams
                .AddUnmatched("value", "panel-1")
                .AddChildContent("Content")));

        var panel = component.Find("div[role=tabpanel]");
        Assert.Equal("tabpanel", panel.GetAttribute("role"));
    }

    [Fact]
    public void TabsPanel_HasTabIndex()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddUnmatched("defaultValue", "panel-1")
            .AddChildContent<Panel>(panelParams => panelParams
                .AddUnmatched("value", "panel-1")
                .AddChildContent("Content")));

        var panel = component.Find("div[role=tabpanel]");
        Assert.Equal("0", panel.GetAttribute("tabindex"));
    }

    [Fact]
    public void TabsPanel_HasGeneratedPanelId()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddUnmatched("defaultValue", "panel-1")
            .AddChildContent<Panel>(panelParams => panelParams
                .AddUnmatched("value", "panel-1")
                .AddChildContent("Content")));

        var panel = component.Find("div[role=tabpanel]");
        Assert.NotNull(panel.Id);
        Assert.StartsWith("tabpanel-", panel.Id);
    }

    [Fact]
    public void TabsPanel_HasAriaLabelledby()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddUnmatched("defaultValue", "panel-1")
            .AddChildContent<Panel>(panelParams => panelParams
                .AddUnmatched("value", "panel-1")
                .AddChildContent("Content")));

        var panel = component.Find("div[role=tabpanel]");
        Assert.NotNull(panel.GetAttribute("aria-labelledby"));
    }
}