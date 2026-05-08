using Bunit;

namespace Tests.Unit.Components.Dropdown;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Dropdown.List")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class DropdownListTests : HviktorBunitContext
{
    public DropdownListTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void List_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.List>());

        var list = component.Find("ul");
        Assert.NotNull(list);
    }

    [Fact]
    public void List_RendersAsUlElement()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.List>());

        var list = component.Find("ul");
        Assert.Equal("UL", list.TagName);
    }

    [Fact]
    public void List_RendersChildContent()
    {
        const string listContent = "List item content";
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.List>(listParams => listParams
                .AddChildContent(listContent)));

        var list = component.Find("ul");
        Assert.Contains(listContent, list.InnerHtml);
    }

    [Fact]
    public void List_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.List>(listParams => listParams
                .AddUnmatched("data-testid", "list-test")
                .AddUnmatched("aria-label", "Menu list")));

        var list = component.Find("ul");
        Assert.Equal("list-test", list.GetAttribute("data-testid"));
        Assert.Equal("Menu list", list.GetAttribute("aria-label"));
    }

    [Fact]
    public void List_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.List>(listParams => listParams
                .AddUnmatched("class", "my-list-class")));

        var list = component.Find("ul");
        Assert.Contains("my-list-class", list.ClassList);
    }
}