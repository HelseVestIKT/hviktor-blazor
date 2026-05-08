using Bunit;
using Popover;

namespace Tests.Unit.Components.Popover;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Popover.Trigger")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Context)]
public class TriggerContextTests : HviktorBunitContext
{

    [Fact]
    public void TriggerContext_RendersChildContent()
    {
        var component = Render<TriggerContext>(parameters => parameters
            .AddChildContent("<span>Context content</span>"));

        Assert.Contains("Context content", component.Markup);
    }

    [Fact]
    public void TriggerContext_HasDefaultId()
    {
        var component = Render<TriggerContext>();

        Assert.NotNull(component.Instance.Id);
        Assert.NotEmpty(component.Instance.Id);
    }

    [Fact]
    public void TriggerContext_AcceptsCustomId()
    {
        var component = Render<TriggerContext>(parameters => parameters
            .Add(p => p.Id, "custom-context-id"));

        Assert.Equal("custom-context-id", component.Instance.Id);
    }

    [Fact]
    public void TriggerContext_WithoutChildContent_RendersEmpty()
    {
        var component = Render<TriggerContext>();

        Assert.Empty(component.Markup.Trim());
    }
}