using Bunit;
using Hviktor.Models.List;
using Microsoft.AspNetCore.Components;

namespace Tests.Unit.Components.List;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "List.Ordered")]
public class ListOrderedTests : HviktorBunitContext
{

    [Fact]
    public void Ordered_RendersWithDefaultValues()
    {
        var component = Render<global::List.Ordered>();

        Assert.NotNull(component.Instance);
    }

    [Fact]
    public void Ordered_HasDsListClass()
    {
        var component = Render<global::List.Ordered>();

        var list = component.Find("ol");
        Assert.Contains("ds-list", list.ClassList);
    }

    [Fact]
    public void Ordered_RendersAsOlElement()
    {
        var component = Render<global::List.Ordered>();

        var list = component.Find("ol");
        Assert.Equal("OL", list.TagName);
    }

    [Fact]
    public void Ordered_RendersChildContent()
    {
        const string content = "Ordered list content";
        var component = Render<global::List.Ordered>(parameters => parameters
            .AddChildContent(content));

        var list = component.Find("ol");
        Assert.Contains(content, list.InnerHtml);
    }

    [Fact]
    public void Ordered_RendersEmptyWhenNoChildContent()
    {
        var component = Render<global::List.Ordered>();

        var list = component.Find("ol");
        Assert.Empty(list.InnerHtml.Trim());
    }

    [Fact]
    public void Ordered_AppliesAdditionalAttributes()
    {
        var component = Render<global::List.Ordered>(parameters => parameters
            .AddUnmatched("data-testid", "ordered-list-test")
            .AddUnmatched("aria-label", "Steps list"));

        var list = component.Find("ol");
        Assert.Equal("ordered-list-test", list.GetAttribute("data-testid"));
        Assert.Equal("Steps list", list.GetAttribute("aria-label"));
    }

    [Fact]
    public void Ordered_AppliesCustomCssClass()
    {
        var component = Render<global::List.Ordered>(parameters => parameters
            .AddUnmatched("class", "my-ordered-list"));

        var list = component.Find("ol");
        Assert.Contains("my-ordered-list", list.ClassList);
        Assert.Contains("ds-list", list.ClassList);
    }

    [Fact]
    public void Ordered_InheritsFromListBase()
    {
        var component = Render<global::List.Ordered>();
        Assert.IsType<ListBase>(component.Instance, exactMatch: false);
    }

    [Fact]
    public void Ordered_RendersWithItems()
    {
        var component = Render<global::List.Ordered>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<global::List.Item>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Step 1")));
                builder.CloseComponent();

                builder.OpenComponent<global::List.Item>(2);
                builder.AddAttribute(3, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Step 2")));
                builder.CloseComponent();

                builder.OpenComponent<global::List.Item>(4);
                builder.AddAttribute(5, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Step 3")));
                builder.CloseComponent();
            }));

        var items = component.FindAll("li");
        Assert.Equal(3, items.Count);
        Assert.Contains("Step 1", items[0].InnerHtml);
        Assert.Contains("Step 2", items[1].InnerHtml);
        Assert.Contains("Step 3", items[2].InnerHtml);
    }

    [Fact]
    public void Ordered_AppliesStartAttribute()
    {
        var component = Render<global::List.Ordered>(parameters => parameters
            .AddUnmatched("start", "5"));

        var list = component.Find("ol");
        Assert.Equal("5", list.GetAttribute("start"));
    }

    [Fact]
    public void Ordered_AppliesReversedAttribute()
    {
        var component = Render<global::List.Ordered>(parameters => parameters
            .AddUnmatched("reversed", "reversed"));

        var list = component.Find("ol");
        Assert.True(list.HasAttribute("reversed"));
    }

    [Fact]
    public void Ordered_AppliesTypeAttribute()
    {
        var component = Render<global::List.Ordered>(parameters => parameters
            .AddUnmatched("type", "A"));

        var list = component.Find("ol");
        Assert.Equal("A", list.GetAttribute("type"));
    }
}