using Bunit;

namespace Tests.Unit.Components.Dropdown;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Dropdown.Item")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class DropdownItemTests : HviktorBunitContext
{
    public DropdownItemTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Item_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Item>());

        var item = component.Find("li");
        Assert.NotNull(item);
    }

    [Fact]
    public void Item_RendersAsLiElement()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Item>());

        var item = component.Find("li");
        Assert.Equal("LI", item.TagName);
    }

    [Fact]
    public void Item_RendersChildContent()
    {
        const string itemContent = "Menu item";
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Item>(itemParams => itemParams
                .AddChildContent(itemContent)));

        var item = component.Find("li");
        Assert.Contains(itemContent, item.InnerHtml);
    }

    [Fact]
    public void Item_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Item>(itemParams => itemParams
                .AddUnmatched("data-testid", "item-test")
                .AddUnmatched("role", "menuitem")));

        var item = component.Find("li");
        Assert.Equal("item-test", item.GetAttribute("data-testid"));
        Assert.Equal("menuitem", item.GetAttribute("role"));
    }

    [Fact]
    public void Item_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Item>(itemParams => itemParams
                .AddUnmatched("class", "my-item-class")));

        var item = component.Find("li");
        Assert.Contains("my-item-class", item.ClassList);
    }
}