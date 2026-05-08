using Bunit;
using Tabs;

namespace Tests.Unit.Components.Tabs;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Tabs.Tab")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class TabsTabTests : HviktorBunitContext
{
    public TabsTabTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void TabsTab_RendersUTabElement()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>(listParams => listParams
                .AddChildContent<Tab>(tabParams => tabParams
                    .AddUnmatched("value", "tab-1")
                    .AddChildContent("Tab 1"))));

        var tab = component.Find("button[role=tab]");
        Assert.Equal("BUTTON", tab.TagName);
    }

    [Fact]
    public void TabsTab_HasRoleTab()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>(listParams => listParams
                .AddChildContent<Tab>(tabParams => tabParams
                    .AddUnmatched("value", "tab-1")
                    .AddChildContent("Tab 1"))));

        var tab = component.Find("button");
        Assert.Equal("tab", tab.GetAttribute("role"));
    }

    [Fact]
    public void TabsTab_HasTypeButton()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>(listParams => listParams
                .AddChildContent<Tab>(tabParams => tabParams
                    .AddUnmatched("value", "tab-1")
                    .AddChildContent("Tab 1"))));

        var tab = component.Find("button[role=tab]");
        Assert.Equal("button", tab.GetAttribute("type"));
    }

    [Fact]
    public void TabsTab_HasDataValue()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>(listParams => listParams
                .AddChildContent<Tab>(tabParams => tabParams
                    .AddUnmatched("value", "my-tab-value")
                    .AddChildContent("Tab 1"))));

        var tab = component.Find("button[role=tab]");
        Assert.Equal("my-tab-value", tab.GetAttribute("data-value"));
    }

    [Fact]
    public void TabsTab_HasDefaultId()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>(listParams => listParams
                .AddChildContent<Tab>(tabParams => tabParams
                    .AddUnmatched("value", "tab-1")
                    .AddChildContent("Tab 1"))));

        var tab = component.Find("button[role=tab]");
        Assert.NotNull(tab.Id);
        Assert.NotEmpty(tab.Id);
    }

    [Fact]
    public void TabsTab_AcceptsCustomId()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>(listParams => listParams
                .AddChildContent<Tab>(tabParams => tabParams
                    .AddUnmatched("value", "tab-1")
                    .AddUnmatched("id", "custom-tab-id")
                    .AddChildContent("Tab 1"))));

        var tab = component.Find("button[role=tab]");
        Assert.Equal("custom-tab-id", tab.Id);
    }

    [Fact]
    public void TabsTab_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>(listParams => listParams
                .AddChildContent<Tab>(tabParams => tabParams
                    .AddUnmatched("value", "tab-1")
                    .AddChildContent("My Tab Label"))));

        var tab = component.Find("button[role=tab]");
        Assert.Contains("My Tab Label", tab.TextContent);
    }

    [Fact]
    public void TabsTab_HasAriaSelectedFalseByDefault()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>(listParams => listParams
                .AddChildContent<Tab>(tabParams => tabParams
                    .AddUnmatched("value", "tab-1")
                    .AddChildContent("Tab 1"))));

        var tab = component.Find("button[role=tab]");
        Assert.Equal("false", tab.GetAttribute("aria-selected"));
    }
}