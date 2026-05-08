using Bunit;

namespace Tests.Unit.Components.Tabs;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Tabs.List")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class TabsListTests : HviktorBunitContext
{
    public TabsListTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void TabsList_RendersTabListWcElement()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>());
        var tablist = component.Find("[role=tablist]");
        Assert.Equal("DIV", tablist.TagName);
    }

    [Fact]
    public void TabsList_HasRoleTablist()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>());

        var tablist = component.Find("div[role=tablist]");
        Assert.Equal("tablist", tablist.GetAttribute("role"));
    }

    [Fact]
    public void TabsList_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>(p => p
                .AddChildContent("<span>List content</span>")));

        var tablist = component.Find("div[role=tablist]");
        Assert.Contains("List content", tablist.InnerHtml);
    }

    [Fact]
    public void TabsList_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Tabs.Tabs>(parameters => parameters
            .AddChildContent<global::Tabs.List>(p => p
                .AddUnmatched("data-testid", "tablist-test")));

        var tablist = component.Find("div[role=tablist]");
        Assert.Equal("tablist-test", tablist.GetAttribute("data-testid"));
    }
}