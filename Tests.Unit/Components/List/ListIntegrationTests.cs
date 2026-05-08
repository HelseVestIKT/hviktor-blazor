using Bunit;
using Microsoft.AspNetCore.Components;

namespace Tests.Unit.Components.List;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "List")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class ListIntegrationTests : HviktorBunitContext
{

    [Fact]
    public void UnorderedList_RendersMultipleItems()
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

                builder.OpenComponent<global::List.Item>(4);
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Item 3")));
                builder.CloseComponent();

                builder.OpenComponent<global::List.Item>(6);
                builder.AddAttribute(7, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Item 4")));
                builder.CloseComponent();

                builder.OpenComponent<global::List.Item>(8);
                builder.AddAttribute(9, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Item 5")));
                builder.CloseComponent();
            }));

        var items = component.FindAll("li");
        Assert.Equal(5, items.Count);

        for (var i = 0; i < 5; i++)
        {
            Assert.Contains($"Item {i + 1}", items[i].InnerHtml);
        }
    }

    [Fact]
    public void OrderedList_RendersMultipleItems()
    {
        var component = Render<global::List.Ordered>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::List.Item>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "First")));
                builder.CloseComponent();

                builder.OpenComponent<global::List.Item>(2);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Second")));
                builder.CloseComponent();

                builder.OpenComponent<global::List.Item>(4);
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Third")));
                builder.CloseComponent();
            }));

        var list = component.Find("ol");
        var items = component.FindAll("li");

        Assert.Contains("ds-list", list.ClassList);
        Assert.Equal(3, items.Count);
    }

    [Fact]
    public void NestedLists_RenderCorrectly()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::List.Item>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b =>
                {
                    b.AddContent(0, "Parent item");
                    b.OpenComponent<global::List.Unordered>(1);
                    b.AddAttribute(2, "ChildContent", (RenderFragment)(nested =>
                    {
                        nested.OpenComponent<global::List.Item>(0);
                        nested.AddAttribute(1, "ChildContent", (RenderFragment)(ni => ni.AddContent(0, "Nested item")));
                        nested.CloseComponent();
                    }));
                    b.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var lists = component.FindAll("ul");
        var items = component.FindAll("li");

        Assert.Equal(2, lists.Count);
        Assert.Equal(2, items.Count);
    }

    [Fact]
    public void List_ItemsInheritParentContext()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddUnmatched("data-parent", "true")
            .AddChildContent<global::List.Item>(itemParams => itemParams
                .AddChildContent("Child item")));

        var list = component.Find("ul");
        var item = component.Find("li");

        Assert.Equal("true", list.GetAttribute("data-parent"));
        Assert.NotNull(item);
    }

    [Fact]
    public void List_CombinesListAndItemAttributes()
    {
        var component = Render<global::List.Unordered>(parameters => parameters
            .AddUnmatched("id", "my-list")
            .AddUnmatched("class", "custom-list")
            .AddChildContent<global::List.Item>(itemParams => itemParams
                .AddUnmatched("id", "my-item")
                .AddUnmatched("class", "custom-item")
                .AddChildContent("Styled item")));

        var list = component.Find("ul");
        var item = component.Find("li");

        Assert.Equal("my-list", list.Id);
        Assert.Contains("ds-list", list.ClassList);
        Assert.Contains("custom-list", list.ClassList);

        Assert.Equal("my-item", item.Id);
        Assert.Contains("custom-item", item.ClassList);
    }
}