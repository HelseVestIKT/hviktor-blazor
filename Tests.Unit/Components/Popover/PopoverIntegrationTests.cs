using Bunit;
using Popover;

namespace Tests.Unit.Components.Popover;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Popover")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Popover)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class PopoverIntegrationTests : HviktorBunitContext
{
    public PopoverIntegrationTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void PopoverWithTriggerContext_TriggerHasPopovertarget()
    {
        var component = Render<TriggerContext>(parameters => parameters
            .Add(p => p.Id, "my-popover")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Trigger>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Open")));
                builder.CloseComponent();
            }));

        var button = component.Find("button");
        Assert.Equal("my-popover", button.GetAttribute("popovertarget"));
    }

    [Fact]
    public void PopoverWithTriggerContext_PopoverUsesContextId()
    {
        var component = Render<TriggerContext>(parameters => parameters
            .Add(p => p.Id, "context-popover-id")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Hviktor.Components.Popover.Popover>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Content")));
                builder.CloseComponent();
            }));

        var popover = component.Find(".ds-popover");
        Assert.Equal("context-popover-id", popover.Id);
    }

    [Fact]
    public void PopoverWithTriggerContext_BothComponentsRender()
    {
        var component = Render<TriggerContext>(parameters => parameters
            .Add(p => p.Id, "test-context")
            .AddChildContent(builder =>
            {
                builder.OpenComponent<Trigger>(0);
                builder.AddAttribute(1, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Trigger")));
                builder.CloseComponent();

                builder.OpenComponent<Hviktor.Components.Popover.Popover>(2);
                builder.AddAttribute(3, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)(b => b.AddContent(0, "Popover content")));
                builder.CloseComponent();
            }));

        var button = component.Find("button");
        var popover = component.Find(".ds-popover");

        Assert.NotNull(button);
        Assert.NotNull(popover);
        Assert.Contains("Trigger", button.InnerHtml);
        Assert.Contains("Popover content", popover.InnerHtml);
    }
}