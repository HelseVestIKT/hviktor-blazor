using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Microsoft.AspNetCore.Components;

namespace Tests.Unit.Components.Dropdown;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Dropdown")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class DropdownIntegrationTests : HviktorBunitContext
{
    public DropdownIntegrationTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Dropdown_RendersListAndItems()
    {
        const string item1 = "Item 1";
        const string item2 = "Item 2";

        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Dropdown.List>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b =>
                {
                    b.OpenComponent<global::Dropdown.Item>(0);
                    b.AddAttribute(1, "ChildContent", (RenderFragment)(ib => ib.AddContent(0, item1)));
                    b.CloseComponent();

                    b.OpenComponent<global::Dropdown.Item>(2);
                    b.AddAttribute(3, "ChildContent", (RenderFragment)(ib => ib.AddContent(0, item2)));
                    b.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var list = component.Find("ul");
        var items = component.FindAll("li");

        Assert.NotNull(list);
        Assert.Equal(2, items.Count);
        Assert.Contains(item1, items[0].InnerHtml);
        Assert.Contains(item2, items[1].InnerHtml);
    }

    [Fact]
    public void Dropdown_ListInheritsParentContext()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "context-dropdown")
            .AddChildContent<global::Dropdown.List>(listParams => listParams
                .AddChildContent("List content")));

        var list = component.Find("ul");
        Assert.NotNull(list);
    }

    [Fact]
    public void Dropdown_ItemInheritsParentContext()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "context-dropdown")
            .AddUnmatched("placement", Placement.TopEnd)
            .AddChildContent<global::Dropdown.Item>(itemParams => itemParams
                .AddChildContent("Item content")));

        var item = component.Find("li");
        Assert.NotNull(item);
    }

    [Fact]
    public void Dropdown_FullStructureRendersCorrectly()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "full-dropdown")
            .AddUnmatched("placement", Placement.BottomEnd)
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::Dropdown.List>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b =>
                {
                    b.OpenComponent<global::Dropdown.Item>(0);
                    b.AddAttribute(1, "ChildContent", (RenderFragment)(ib => ib.AddContent(0, "Option 1")));
                    b.CloseComponent();

                    b.OpenComponent<global::Dropdown.Item>(2);
                    b.AddAttribute(3, "ChildContent", (RenderFragment)(ib => ib.AddContent(0, "Option 2")));
                    b.CloseComponent();

                    b.OpenComponent<global::Dropdown.Item>(4);
                    b.AddAttribute(5, "ChildContent", (RenderFragment)(ib => ib.AddContent(0, "Option 3")));
                    b.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var dropdown = component.Find("div");
        Assert.Equal("full-dropdown", dropdown.Id);
        Assert.Equal("bottom-end", dropdown.GetAttribute("data-placement"));

        var list = component.Find("ul");
        Assert.NotNull(list);

        var items = component.FindAll("li");
        Assert.Equal(3, items.Count);
        Assert.Contains("Option 1", items[0].InnerHtml);
        Assert.Contains("Option 2", items[1].InnerHtml);
        Assert.Contains("Option 3", items[2].InnerHtml);
    }
}