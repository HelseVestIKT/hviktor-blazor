using Bunit;
using Hviktor.Models.List;
using Microsoft.AspNetCore.Components;

namespace Tests.Unit.Components.List;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "List.Unordered")]
public class ListUnorderedTests : HviktorBunitContext
{

    [Fact]
    public void Unordered_RendersWithDefaultValues()
    {
        var component = Render<global::List.Unordered>();

        Assert.NotNull(component.Instance);
    }

    [Fact]
    public void Unordered_HasDsListClass()
    {
        var component = Render<global::List.Unordered>();

        var list = component.Find("ul");
        Assert.Contains("ds-list", list.ClassList);
    }

    [Fact]
    public void Unordered_RendersAsUlElement()
    {
        var component = Render<global::List.Unordered>();

        var list = component.Find("ul");
        Assert.Equal("UL", list.TagName);
    }

    [Fact]
    public void Unordered_RendersChildContent()
    {
        const string content = "List content";
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddChildContent(content));

        var list = component.Find("ul");
        Assert.Contains(content, list.InnerHtml);
    }

    [Fact]
    public void Unordered_RendersEmptyWhenNoChildContent()
    {
        var component = Render<global::List.Unordered>();

        var list = component.Find("ul");
        Assert.Empty(list.InnerHtml.Trim());
    }

    [Fact]
    public void Unordered_AppliesAdditionalAttributes()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddUnmatched("data-testid", "list-test")
            .AddUnmatched("aria-label", "Navigation list"));

        var list = component.Find("ul");
        Assert.Equal("list-test", list.GetAttribute("data-testid"));
        Assert.Equal("Navigation list", list.GetAttribute("aria-label"));
    }

    [Fact]
    public void Unordered_AppliesCustomCssClass()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddUnmatched("class", "my-custom-list"));

        var list = component.Find("ul");
        Assert.Contains("my-custom-list", list.ClassList);
        Assert.Contains("ds-list", list.ClassList);
    }

    [Fact]
    public void Unordered_InheritsFromListBase()
    {
        var component = Render<global::List.Unordered>();
        Assert.IsType<ListBase>(component.Instance, exactMatch: false);
    }

    [Fact]
    public void Unordered_RendersWithItems()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::List.Item>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Item 1")));
                builder.CloseComponent();

                builder.OpenComponent<global::List.Item>(2);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Item 2")));
                builder.CloseComponent();
            }));

        var items = component.FindAll("li");
        Assert.Equal(2, items.Count);
        Assert.Contains("Item 1", items[0].InnerHtml);
        Assert.Contains("Item 2", items[1].InnerHtml);
    }

    [Fact]
    public void Unordered_AppliesRoleAttribute()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddUnmatched("role", "navigation"));

        var list = component.Find("ul");
        Assert.Equal("navigation", list.GetAttribute("role"));
    }
}