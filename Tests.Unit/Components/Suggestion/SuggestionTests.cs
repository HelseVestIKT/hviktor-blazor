using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SuggestionComponent = Hviktor.Components.Suggestion.Suggestion;

namespace Tests.Unit.Components.Suggestion;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Suggestion")]
public class SuggestionTests : HviktorBunitContext
{
    public SuggestionTests()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.initializeCombobox", _ => true).SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.disposeCombobox", _ => true).SetVoidResult();
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.setSelected", _ => true).SetVoidResult();
    }

    #region Rendering Tests

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Suggestion_RendersComboboxElement()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var combobox = component.Find(".ds-suggestion");
        Assert.Equal("DIV", combobox.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Suggestion_HasDsSuggestionClass()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var combobox = component.Find("div.ds-suggestion");
        Assert.Contains("ds-suggestion", combobox.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Suggestion_AcceptsCustomId()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "my-custom-suggestion"));

        var combobox = component.Find("div.ds-suggestion");
        Assert.Equal("my-custom-suggestion", combobox.Id);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Suggestion_AcceptsHtmlAttributes()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("data-notreal", "true")
            .AddUnmatched("required", true));

        var combobox = component.Find("div.ds-suggestion");
        Assert.Equal("true", combobox.GetAttribute("data-notreal"));
        Assert.True(combobox.HasAttribute("required"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Suggestion_RendersHiddenSelectWhenNameProvided()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("name", "my-field"));

        var select = component.Find("select");
        Assert.Equal("my-field", select.GetAttribute("name"));
        Assert.True(select.HasAttribute("hidden"));
        Assert.Equal("test-suggestion-select", select.Id);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Suggestion_NoHiddenSelectWhenNameNotProvided()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        Assert.Throws<ElementNotFoundException>(() => component.Find("select"));
    }

    #endregion

    #region Attribute Tests

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Suggestion_AppliesMultipleAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("multiple", true));

        var combobox = component.Find("div.ds-suggestion");
        Assert.Equal("true", combobox.GetAttribute("data-multiple"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Suggestion_NoMultipleAttributeWhenNotProvided()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var combobox = component.Find("div.ds-suggestion");
        Assert.Null(combobox.GetAttribute("data-multiple"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Suggestion_AppliesCreatableAttribute()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("creatable", true));

        var combobox = component.Find("div.ds-suggestion");
        Assert.Equal("true", combobox.GetAttribute("data-creatable"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Suggestion_NoCreatableAttributeWhenNotProvided()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var combobox = component.Find("div.ds-suggestion");
        Assert.Null(combobox.GetAttribute("data-creatable"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Suggestion_NoSizeAttributeWhenNull()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var combobox = component.Find("div.ds-suggestion");
        Assert.Null(combobox.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Suggestion_AppliesReadOnly()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("readonly", true));

        var element = component.Find("#test-suggestion");
        Assert.True(element.HasAttribute("readonly"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Suggestion_NoReadOnlyWhenNotProvided()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var element = component.Find("#test-suggestion");
        Assert.Null(element.GetAttribute("readonly"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Suggestion_FilterDefaultsToTrue()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var element = component.Find("#test-suggestion");
        Assert.False(element.HasAttribute("data-nofilter"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Suggestion_FilterDoesNotAddNoFilterAttributeWhenFalse()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("filter", false));

        var element = component.Find("#test-suggestion");
        Assert.True(element.HasAttribute("data-nofilter"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Suggestion_DisabledDefaultsToFalse()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var element = component.Find("#test-suggestion");
        Assert.Null(element.GetAttribute("disabled"));
    }

    #endregion

    #region OnInitialized Tests

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public void OnInitialized_WithNoId_GeneratesNewId()
    {
        var component = Render<SuggestionComponent>();

        var element = component.Find("div.ds-suggestion");
        Assert.NotNull(element.Id);
        Assert.NotEmpty(element.Id);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public void OnInitialized_WithValidId_KeepsProvidedId()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "valid-id"));

        var element = component.Find("div.ds-suggestion");
        Assert.Equal("valid-id", element.Id);
    }

    #endregion

    #region DefaultSelected Tests

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public async Task DefaultSelected_SetsInternalSelected()
    {
        _ = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("defaultSelected", "default-value"));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);

        var invocations = JSInterop.Invocations
            .Where(i => i.Identifier == "globalThis.Hviktor.Suggestion.setSelected")
            .ToList();
        Assert.Single(invocations);
        var selected = invocations[0].Arguments[1] as IEnumerable<string>;
        Assert.NotNull(selected);
        Assert.Equal(["default-value"], selected);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public async Task DefaultSelected_WithEmptySelected_SetsInternalSelected()
    {
        _ = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("selected", "")
            .AddUnmatched("defaultSelected", "default-value"));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);

        var invocations = JSInterop.Invocations
            .Where(i => i.Identifier == "globalThis.Hviktor.Suggestion.setSelected")
            .ToList();
        Assert.Single(invocations);
        var selected = invocations[0].Arguments[1] as IEnumerable<string>;
        Assert.NotNull(selected);
        Assert.Equal(["default-value"], selected);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public async Task Selected_OverridesDefaultSelected()
    {
        _ = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("selected", "existing-value")
            .AddUnmatched("defaultSelected", "default-value"));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);

        var invocations = JSInterop.Invocations
            .Where(i => i.Identifier == "globalThis.Hviktor.Suggestion.setSelected")
            .ToList();
        Assert.Single(invocations);
        var selected = invocations[0].Arguments[1] as IEnumerable<string>;
        Assert.NotNull(selected);
        Assert.Equal(["existing-value"], selected);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public async Task DefaultSelected_WithWhitespace_SyncsWhitespaceValue()
    {
        _ = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("defaultSelected", "   "));

        await Task.Delay(50, Xunit.TestContext.Current.CancellationToken);

        // Whitespace-only strings fall through to the default pattern in ConsumeStringList,
        // where raw.ToString() has Length > 0, so the value is preserved as-is.
        var invocations = JSInterop.Invocations
            .Where(i => i.Identifier == "globalThis.Hviktor.Suggestion.setSelected")
            .ToList();
        Assert.Single(invocations);
        var selected = invocations[0].Arguments[1] as IEnumerable<string>;
        Assert.NotNull(selected);
        Assert.Equal(["   "], selected);
    }

    #endregion

    #region OnAfterRenderAsync Tests

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public void OnAfterRenderAsync_FirstRender_InitializesJsInterop()
    {
        _ = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-js-init"));
        JSInterop.VerifyInvoke("globalThis.Hviktor.Suggestion.initializeCombobox", 1);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.LifeCycle)]
    public void OnAfterRenderAsync_JsInteropFails_DoesNotThrow()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.initializeCombobox", _ => true)
            .SetException(new InvalidOperationException("JS error"));

        var exception = Record.Exception(() =>
        {
            Render<SuggestionComponent>(parameters => parameters
                .AddUnmatched("id", "test-js-fail"));
        });

        Assert.Null(exception);
    }

    #endregion

    #region Callback Tests

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Callbacks)]
    public async Task OnBeforeSelectCallback_WithDelegate_InvokesCallback()
    {
        string? receivedValue = null;
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("onBeforeSelect", EventCallback.Factory.Create<string>(this, value => receivedValue = value)));

        await component.Instance.OnBeforeSelectCallback("test-value");
        Assert.Equal("test-value", receivedValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Callbacks)]
    public async Task OnBeforeSelectCallback_WithoutDelegate_DoesNotThrow()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var exception = await Record.ExceptionAsync(() => component.Instance.OnBeforeSelectCallback("test-value"));
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Callbacks)]
    public async Task OnAfterSelectCallback_WithDelegate_InvokesCallback()
    {
        string? receivedValue = null;
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("onAfterSelect", EventCallback.Factory.Create<string>(this, value => receivedValue = value)));

        await component.Instance.OnAfterSelectCallback(["test-value"]);
        Assert.Equal("test-value", receivedValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Callbacks)]
    public async Task OnAfterSelectCallback_WithOnSelectedChangeDelegate_InvokesBoth()
    {
        string? afterSelectValue = null;
        string[]? selectedChangeValues = null;
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("onAfterSelect", EventCallback.Factory.Create<string>(this, value => afterSelectValue = value))
            .AddUnmatched("onSelectedChange", EventCallback.Factory.Create<string[]>(this, values => selectedChangeValues = values)));

        await component.Instance.OnAfterSelectCallback(["test-value"]);
        Assert.Equal("test-value", afterSelectValue);
        Assert.NotNull(selectedChangeValues);
        Assert.Equal(["test-value"], selectedChangeValues);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Callbacks)]
    public async Task OnAfterSelectCallback_WithoutDelegate_DoesNotThrow()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var exception = await Record.ExceptionAsync(() => component.Instance.OnAfterSelectCallback(["test-value"]));
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Callbacks)]
    public async Task OnBeforeMatchCallback_WithDelegate_InvokesCallback()
    {
        string? receivedValue = null;
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion")
            .AddUnmatched("onBeforeMatch", EventCallback.Factory.Create<string>(this, value => receivedValue = value)));

        await component.Instance.OnBeforeMatchCallback("search-term");
        Assert.Equal("search-term", receivedValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Callbacks)]
    public async Task OnBeforeMatchCallback_WithoutDelegate_DoesNotThrow()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-suggestion"));

        var exception = await Record.ExceptionAsync(() => component.Instance.OnBeforeMatchCallback("search-term"));
        Assert.Null(exception);
    }

    #endregion

    #region DisposeAsync Tests

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task DisposeAsync_CallsJsDispose()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-dispose"));

        await component.Instance.DisposeAsync();
        JSInterop.VerifyInvoke("globalThis.Hviktor.Suggestion.disposeCombobox", 1);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task DisposeAsync_JsDisconnectedException_DoesNotThrow()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.disposeCombobox", _ => true)
            .SetException(new JSDisconnectedException("Circuit disconnected"));

        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-dispose-disconnected"));
        var exception = await Record.ExceptionAsync(() => component.Instance.DisposeAsync().AsTask());
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task DisposeAsync_GeneralException_DoesNotThrow()
    {
        JSInterop.SetupVoid("globalThis.Hviktor.Suggestion.disposeCombobox", _ => true)
            .SetException(new InvalidOperationException("General JS error"));

        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-dispose-error"));
        var exception = await Record.ExceptionAsync(() => component.Instance.DisposeAsync().AsTask());
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public async Task DisposeAsync_CalledMultipleTimes_DoesNotThrow()
    {
        var component = Render<SuggestionComponent>(parameters => parameters
            .AddUnmatched("id", "test-dispose-multiple"));

        await component.Instance.DisposeAsync();
        var exception = await Record.ExceptionAsync(() => component.Instance.DisposeAsync().AsTask());
        Assert.Null(exception);
    }

    #endregion
}