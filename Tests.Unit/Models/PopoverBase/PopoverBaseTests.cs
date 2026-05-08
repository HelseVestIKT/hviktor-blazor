using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using PopoverComponent = Hviktor.Components.Popover.Popover;
using Microsoft.JSInterop;

namespace Tests.Unit.Models.PopoverBase;

/// <summary>
/// Tests for PopoverBase functionality through the Popover component.
/// Since PopoverBase is abstract, we test it via its concrete implementation.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Model, "PopoverBase")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Popover)]
public class PopoverBaseTests : HviktorBunitContext
{
    public PopoverBaseTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Parameter Default Values

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void PopoverBase_PlacementDefaultsToBottomStart()
    {
        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        var element = component.Find("div");
        Assert.Equal("bottom-start", element.GetAttribute("data-placement"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void PopoverBase_OpenDefaultsToNull()
    {
        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        Assert.Null(component.Instance.Open);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void PopoverBase_NoSizeAttributeByDefault()
    {
        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        var element = component.Find("div");
        Assert.Null(element.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void PopoverBase_NoColorAttributeByDefault()
    {
        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        var element = component.Find("div");
        Assert.Null(element.GetAttribute("data-color"));
    }

    #endregion

    #region Attributes and CSS Classes

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void PopoverBase_HasDsPopoverClass()
    {
        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        var element = component.Find("div");
        Assert.Contains("ds-popover", element.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void PopoverBase_HasPopoverManualAttribute()
    {
        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        var element = component.Find("div");
        Assert.Equal("manual", element.GetAttribute("popover"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void PopoverBase_HasDefaultVariantAttribute()
    {
        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        var element = component.Find("div");
        Assert.Equal("primary", element.GetAttribute("data-variant"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void PopoverBase_AppliesIdAttribute()
    {
        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "my-popover-id"));

        var element = component.Find("div");
        Assert.Equal("my-popover-id", element.Id);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void PopoverBase_AppliesAdditionalAttributes()
    {
        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .AddUnmatched("data-testid", "custom-test")
            .AddUnmatched("aria-label", "Custom label"));

        var element = component.Find("div");
        Assert.Equal("custom-test", element.GetAttribute("data-testid"));
        Assert.Equal("Custom label", element.GetAttribute("aria-label"));
    }

    #endregion

    #region Placement Tests

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    [InlineData(Placement.Top, "top")]
    [InlineData(Placement.TopStart, "top-start")]
    [InlineData(Placement.TopEnd, "top-end")]
    [InlineData(Placement.Bottom, "bottom")]
    [InlineData(Placement.BottomStart, "bottom-start")]
    [InlineData(Placement.BottomEnd, "bottom-end")]
    [InlineData(Placement.Left, "left")]
    [InlineData(Placement.Right, "right")]
    public void PopoverBase_AppliesPlacementDataAttribute(Placement placement, string expected)
    {
        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .AddUnmatched("placement", placement));

        var element = component.Find("div");
        Assert.Equal(expected, element.GetAttribute("data-placement"));
    }

    #endregion

    #region Color Tests

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void PopoverBase_NoColorAttributeWhenColorIsNull()
    {
        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        var element = component.Find("div");
        Assert.Null(element.GetAttribute("data-color"));
    }

    #endregion

    #region Child Content

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void PopoverBase_RendersChildContent()
    {
        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .AddChildContent("<span>Popover content</span>"));

        var element = component.Find("div");
        Assert.Contains("Popover content", element.InnerHtml);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void PopoverBase_RendersComplexChildContent()
    {
        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .AddChildContent("<div class='inner'><p>Paragraph 1</p><p>Paragraph 2</p></div>"));

        var inner = component.Find(".inner");
        Assert.Equal(2, inner.QuerySelectorAll("p").Length);
    }

    #endregion

    #region Controlled Mode (Open Parameter)

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public void PopoverBase_AcceptsOpenParameterTrue()
    {
        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true));

        Assert.True(component.Instance.Open);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public void PopoverBase_AcceptsOpenParameterFalse()
    {
        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, false));

        Assert.False(component.Instance.Open);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public async Task PopoverBase_ControlledMode_CallsShowOnFirstRender()
    {
        var registerHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true);
        registerHandler.SetVoidResult();
        var showHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.show", _ => true);
        showHandler.SetVoidResult();

        _ = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        JSInterop.VerifyInvoke("globalThis.Hviktor.Popover.registerControlled");
        JSInterop.VerifyInvoke("globalThis.Hviktor.Popover.show");
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public async Task PopoverBase_ControlledMode_CallsHideWhenOpenChangesToFalse()
    {
        var registerHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true);
        registerHandler.SetVoidResult();
        var showHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.show", _ => true);
        showHandler.SetVoidResult();
        var hideHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.hide", _ => true);
        hideHandler.SetVoidResult();

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        component.Render(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, false));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        JSInterop.VerifyInvoke("globalThis.Hviktor.Popover.hide");
    }

    #endregion

    #region OnCloseFromJs Method

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Callbacks)]
    public async Task PopoverBase_OnCloseFromJs_InvokesOnCloseCallback()
    {
        var onCloseCalled = false;

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true)
            .Add(x => x.OnClose, () =>
            {
                onCloseCalled = true;
                return Task.CompletedTask;
            }));

        await component.Instance.OnCloseFromJs();

        Assert.True(onCloseCalled);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Callbacks)]
    public async Task PopoverBase_OnCloseFromJs_InvokesOpenChangedWithFalse()
    {
        bool? openChangedValue = null;

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true)
            .Add(x => x.OpenChanged, value => { openChangedValue = value; }));

        await component.Instance.OnCloseFromJs();

        Assert.False(openChangedValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Callbacks)]
    public async Task PopoverBase_OnCloseFromJs_InvokesBothCallbacks()
    {
        var onCloseCalled = false;
        bool? openChangedValue = null;

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true)
            .Add(x => x.OnClose, () =>
            {
                onCloseCalled = true;
                return Task.CompletedTask;
            })
            .Add(x => x.OpenChanged, value => { openChangedValue = value; }));

        await component.Instance.OnCloseFromJs();

        Assert.True(onCloseCalled);
        Assert.False(openChangedValue);
    }

    [Fact]
    public async Task PopoverBase_OnCloseFromJs_DoesNothingWithoutCallbacks()
    {
        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true));

        var exception = await Record.ExceptionAsync(async () => await component.Instance.OnCloseFromJs());
        Assert.Null(exception);
    }

    #endregion

    #region Disposal

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_DisposeAsync_DisposesComponent()
    {
        var initHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.initialize", _ => true);
        initHandler.SetVoidResult();
        var disposeHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.dispose", _ => true);
        disposeHandler.SetVoidResult();

        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        await component.Instance.DisposeAsync();

        JSInterop.VerifyInvoke("globalThis.Hviktor.Popover.dispose");
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_DisposeAsync_ControlledMode_UnregistersControlled()
    {
        var registerHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true);
        registerHandler.SetVoidResult();
        var showHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.show", _ => true);
        showHandler.SetVoidResult();
        var unregisterHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.unregisterControlled", _ => true);
        unregisterHandler.SetVoidResult();

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true));

        await component.Instance.DisposeAsync();

        JSInterop.VerifyInvoke("globalThis.Hviktor.Popover.unregisterControlled");
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_DisposeAsync_CalledTwice_OnlyDisposesOnce()
    {
        var initHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.initialize", _ => true);
        initHandler.SetVoidResult();
        var disposeHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.dispose", _ => true);
        disposeHandler.SetVoidResult();

        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        await component.Instance.DisposeAsync();
        await component.Instance.DisposeAsync();

        // Should only be called once
        var invocations = JSInterop.Invocations
            .Where(i => i.Identifier == "globalThis.Hviktor.Popover.dispose")
            .ToList();
        Assert.Single(invocations);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_IsDisposed_ReturnsTrueAfterDisposal()
    {
        var initHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.initialize", _ => true);
        initHandler.SetVoidResult();
        var disposeHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.dispose", _ => true);
        disposeHandler.SetVoidResult();

        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));

        await component.Instance.DisposeAsync();

        var initInvocationsBefore = JSInterop.Invocations
            .Count(i => i.Identifier == "globalThis.Hviktor.Popover.initialize");

        component.Render(p => p.Add(x => x.Id, "test"));

        var initInvocationsAfter = JSInterop.Invocations
            .Count(i => i.Identifier == "globalThis.Hviktor.Popover.initialize");

        Assert.Equal(initInvocationsBefore, initInvocationsAfter);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_DisposeAsync_HandlesJSDisconnectedException()
    {
        var initHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.initialize", _ => true);
        initHandler.SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.dispose", _ => true)
            .SetException(new JSDisconnectedException("Test disconnected"));

        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));
        var exception = await Record.ExceptionAsync(async () => await component.Instance.DisposeAsync());
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_DisposeAsync_HandlesTaskCanceledException()
    {
        var initHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.initialize", _ => true);
        initHandler.SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.dispose", _ => true)
            .SetException(new TaskCanceledException("Test canceled"));

        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));
        var exception = await Record.ExceptionAsync(async () => await component.Instance.DisposeAsync());
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_DisposeAsync_HandlesInvalidOperationException()
    {
        var initHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.initialize", _ => true);
        initHandler.SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.dispose", _ => true)
            .SetException(new InvalidOperationException("JS runtime not available"));

        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));
        var exception = await Record.ExceptionAsync(async () => await component.Instance.DisposeAsync());
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_DisposeAsync_ControlledMode_HandlesJSDisconnectedException()
    {
        var registerHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true);
        registerHandler.SetVoidResult();
        var hideHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.hide", _ => true);
        hideHandler.SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.unregisterControlled", _ => true)
            .SetException(new JSDisconnectedException("Test disconnected"));

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, false));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        var exception = await Record.ExceptionAsync(async () => await component.Instance.DisposeAsync());
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_DisposeAsync_ControlledMode_HandlesTaskCanceledException()
    {
        var registerHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true);
        registerHandler.SetVoidResult();
        var hideHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.hide", _ => true);
        hideHandler.SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.unregisterControlled", _ => true)
            .SetException(new TaskCanceledException("Test canceled"));

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, false));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        var exception = await Record.ExceptionAsync(async () => await component.Instance.DisposeAsync());
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_DisposeAsync_ControlledMode_HandlesInvalidOperationException()
    {
        var registerHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true);
        registerHandler.SetVoidResult();
        var hideHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.hide", _ => true);
        hideHandler.SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.unregisterControlled", _ => true)
            .SetException(new InvalidOperationException("JS runtime not available"));

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, false));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        var exception = await Record.ExceptionAsync(async () => await component.Instance.DisposeAsync());
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task PopoverBase_ControlledMode_IsDisposed_PreventsShowAfterDisposal()
    {
        var registerHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true);
        registerHandler.SetVoidResult();
        var showHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.show", _ => true);
        showHandler.SetVoidResult();
        var hideHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.hide", _ => true);
        hideHandler.SetVoidResult();
        var unregisterHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.unregisterControlled", _ => true);
        unregisterHandler.SetVoidResult();

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, false));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        await component.Instance.DisposeAsync();
        var showInvocationsBefore = JSInterop.Invocations
            .Count(i => i.Identifier == "globalThis.Hviktor.Popover.show");

        component.Render(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        var showInvocationsAfter = JSInterop.Invocations
            .Count(i => i.Identifier == "globalThis.Hviktor.Popover.show");
        Assert.Equal(showInvocationsBefore, showInvocationsAfter);
    }

    #endregion

    #region Standalone vs Context Mode

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public void PopoverBase_StandaloneMode_InitializesOnFirstRender()
    {
        var initHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.initialize", _ => true);
        initHandler.SetVoidResult();

        _ = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));
        JSInterop.VerifyInvoke("globalThis.Hviktor.Popover.initialize");
    }

    #endregion

    #region Key Parameter

    [Fact]
    public void PopoverBase_AdditionalAttributesArePassedThrough()
    {
        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .AddUnmatched("data-key", "my-unique-key"));

        var element = component.Find("div");
        Assert.Equal("my-unique-key", element.GetAttribute("data-key"));
    }

    #endregion

    #region JSException Handling

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public void PopoverBase_StandaloneMode_HandlesJSExceptionOnInitialize()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.initialize", _ => true)
            .SetException(new JSException("Test JS error"));

        var component = Render<PopoverComponent>(p => p.Add(x => x.Id, "test"));
        Assert.NotNull(component.Instance);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public async Task PopoverBase_ControlledMode_HandlesJSExceptionOnRegisterControlled()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true)
            .SetException(new JSException("Test JS error"));

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        Assert.NotNull(component.Instance);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public async Task PopoverBase_ControlledMode_HandlesJSExceptionOnShow()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true)
            .SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.show", _ => true)
            .SetException(new JSException("Test JS error"));

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        Assert.NotNull(component.Instance);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public async Task PopoverBase_ControlledMode_HandlesJSExceptionOnHide()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true)
            .SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.show", _ => true)
            .SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Popover.hide", _ => true)
            .SetException(new JSException("Test JS error"));

        var component = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, true));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);

        component.Render(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, false));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        Assert.NotNull(component.Instance);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public async Task PopoverBase_ControlledMode_CallsHideWhenOpenStartsFalse()
    {
        var registerHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.registerControlled", _ => true);
        registerHandler.SetVoidResult();
        var hideHandler = JSInterop.SetupVoid("globalThis.Hviktor.Popover.hide", _ => true);
        hideHandler.SetVoidResult();

        _ = Render<PopoverComponent>(p => p
            .Add(x => x.Id, "test")
            .Add(x => x.Open, false));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);
        JSInterop.VerifyInvoke("globalThis.Hviktor.Popover.registerControlled");
        JSInterop.VerifyInvoke("globalThis.Hviktor.Popover.hide");
    }

    #endregion
}

