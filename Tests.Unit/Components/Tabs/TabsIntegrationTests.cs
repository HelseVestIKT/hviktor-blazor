using Bunit;
using Tabs;

namespace Tests.Unit.Components.Tabs;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Tabs")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Cascading)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class TabsIntegrationTests : HviktorBunitContext
{
    public TabsIntegrationTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Tabs_WithDefaultValue_ShowsCorrectPanel()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddUnmatched("defaultValue", "second")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Tabs.List>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(listBuilder =>
                {
                    listBuilder.OpenComponent<Tab>(0);
                    listBuilder.AddAttribute(1, "Value", "first");
                    listBuilder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "First")));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<Tab>(3);
                    listBuilder.AddAttribute(4, "Value", "second");
                    listBuilder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Second")));
                    listBuilder.CloseComponent();
                }));
                builder.CloseComponent();

                builder.OpenComponent<Panel>(2);
                builder.AddAttribute(3, "Value", "first");
                builder.AddAttribute(4, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "First Panel Content")));
                builder.CloseComponent();

                builder.OpenComponent<Panel>(5);
                builder.AddAttribute(6, "Value", "second");
                builder.AddAttribute(7, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Second Panel Content")));
                builder.CloseComponent();
            }));

        var panel = component.Find("div[role=tabpanel]:not([hidden])");
        Assert.Contains("Second Panel Content", panel.TextContent);
    }

    [Fact]
    public void Tabs_ClickingTab_SwitchesPanel()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddUnmatched("defaultValue", "first")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Tabs.List>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(listBuilder =>
                {
                    listBuilder.OpenComponent<Tab>(0);
                    listBuilder.AddAttribute(1, "Value", "first");
                    listBuilder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "First")));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<Tab>(3);
                    listBuilder.AddAttribute(4, "Value", "second");
                    listBuilder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Second")));
                    listBuilder.CloseComponent();
                }));
                builder.CloseComponent();

                builder.OpenComponent<Panel>(2);
                builder.AddAttribute(3, "Value", "first");
                builder.AddAttribute(4, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "First Panel Content")));
                builder.CloseComponent();

                builder.OpenComponent<Panel>(5);
                builder.AddAttribute(6, "Value", "second");
                builder.AddAttribute(7, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Second Panel Content")));
                builder.CloseComponent();
            }));

        var panel = component.Find("div[role=tabpanel]:not([hidden])");
        Assert.Contains("First Panel Content", panel.TextContent);

        var buttons = component.FindAll("button[role=tab]");
        buttons[1].Click();

        panel = component.Find("div[role=tabpanel]:not([hidden])");
        Assert.Contains("Second Panel Content", panel.TextContent);
    }

    [Fact]
    public void Tabs_SelectedTab_HasAriaSelectedTrue()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddUnmatched("defaultValue", "first")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Tabs.List>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(listBuilder =>
                {
                    listBuilder.OpenComponent<Tab>(0);
                    listBuilder.AddAttribute(1, "Value", "first");
                    listBuilder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "First")));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<Tab>(3);
                    listBuilder.AddAttribute(4, "Value", "second");
                    listBuilder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Second")));
                    listBuilder.CloseComponent();
                }));
                builder.CloseComponent();

                builder.OpenComponent<Panel>(2);
                builder.AddAttribute(3, "Value", "first");
                builder.AddAttribute(4, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "First Panel")));
                builder.CloseComponent();

                builder.OpenComponent<Panel>(5);
                builder.AddAttribute(6, "Value", "second");
                builder.AddAttribute(7, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Second Panel")));
                builder.CloseComponent();
            }));

        var buttons = component.FindAll("button[role=tab]");
        Assert.Equal("true", buttons[0].GetAttribute("aria-selected"));
        Assert.Equal("false", buttons[1].GetAttribute("aria-selected"));
    }

    [Fact]
    public void Tabs_MultipleTabs_AllRenderCorrectly()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Tabs.List>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(listBuilder =>
                {
                    listBuilder.OpenComponent<Tab>(0);
                    listBuilder.AddAttribute(1, "Value", "tab1");
                    listBuilder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Tab 1")));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<Tab>(3);
                    listBuilder.AddAttribute(4, "Value", "tab2");
                    listBuilder.AddAttribute(5, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Tab 2")));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<Tab>(6);
                    listBuilder.AddAttribute(7, "Value", "tab3");
                    listBuilder.AddAttribute(8, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Tab 3")));
                    listBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var tabs = component.FindAll("button[role=tab]");
        Assert.Equal(3, tabs.Count);
        Assert.Contains("Tab 1", tabs[0].TextContent);
        Assert.Contains("Tab 2", tabs[1].TextContent);
        Assert.Contains("Tab 3", tabs[2].TextContent);
    }

    [Fact]
    public void Tabs_TabHasAriaControlsMatchingPanel()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddUnmatched("defaultValue", "panel-a")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Panel>(0);
                builder.AddAttribute(1, "Value", "panel-a");
                builder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Panel A Content")));
                builder.CloseComponent();

                builder.OpenComponent<global::Tabs.List>(3);
                builder.AddAttribute(4, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(listBuilder =>
                {
                    listBuilder.OpenComponent<Tab>(0);
                    listBuilder.AddAttribute(1, "Value", "panel-a");
                    listBuilder.AddAttribute(2, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Tab A")));
                    listBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var tab = component.Find("button[role=tab]");
        var panel = component.Find("div[role=tabpanel]");

        var ariaControls = tab.GetAttribute("aria-controls");
        Assert.Equal(panel.Id, ariaControls);
    }

    [Fact]
    public void Tabs_CompleteStructure_HasCorrectAccessibility()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddUnmatched("defaultValue", "home")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Tabs.List>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(listBuilder =>
                {
                    listBuilder.OpenComponent<Tab>(0);
                    listBuilder.AddAttribute(1, "Value", "home");
                    listBuilder.AddAttribute(2, "Id", "tab-home");
                    listBuilder.AddAttribute(3, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Home")));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<Tab>(4);
                    listBuilder.AddAttribute(5, "Value", "profile");
                    listBuilder.AddAttribute(6, "Id", "tab-profile");
                    listBuilder.AddAttribute(7, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Profile")));
                    listBuilder.CloseComponent();
                }));
                builder.CloseComponent();

                builder.OpenComponent<Panel>(2);
                builder.AddAttribute(3, "Value", "home");
                builder.AddAttribute(4, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Home Content")));
                builder.CloseComponent();

                builder.OpenComponent<Panel>(5);
                builder.AddAttribute(6, "Value", "profile");
                builder.AddAttribute(7, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Profile Content")));
                builder.CloseComponent();
            }));

        var tabsContainer = component.Find("div.ds-tabs");
        var tablist = component.Find("div[role=tablist]");
        var tabs = component.FindAll("button[role=tab]");
        var panel = component.Find("div[role=tabpanel]");

        Assert.NotNull(tabsContainer);
        Assert.NotNull(tablist);
        Assert.Equal(2, tabs.Count);
        Assert.NotNull(panel);

        Assert.Equal("0", panel.GetAttribute("tabindex"));
        Assert.Single(tabs, tab => tab.GetAttribute("aria-selected") == "true");
        Assert.All(tabs.Where(tab => tab.GetAttribute("aria-selected") != "true"),
            tab => Assert.Equal("false", tab.GetAttribute("aria-selected")));
    }
}