using Hviktor.Extensions.Services;
using Microsoft.JSInterop;

namespace Tests.Unit.Extensions.Services;

/// <summary>
/// Unit tests for <see cref="JsObjectReferenceService"/>.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class JsObjectReferenceServiceTests
{
    private readonly JsObjectReferenceService sut = new();

    #region InvokeVoidAsync

    [Fact]
    public async Task InvokeVoidAsync_NullReference_DoesNotThrow()
    {
        var exception = await Record.ExceptionAsync(() => sut.InvokeVoidAsync<JsObjectReferenceServiceTests>(null, "test"));
        Assert.Null(exception);
    }

    [Fact]
    public async Task InvokeVoidAsync_ValidReference_InvokesWithPrefixedMethodName()
    {
        var jsRef = new MockJsObjectReference();

        await sut.InvokeVoidAsync<JsObjectReferenceServiceTests>(jsRef, "doSomething", "arg1");

        Assert.Single(jsRef.Calls);
        Assert.Equal("JsObjectReferenceServiceTests.doSomething", jsRef.Calls[0].Method);
    }

    [Fact]
    public async Task InvokeVoidAsync_UsesTypeNameAsPrefix()
    {
        var jsRef = new MockJsObjectReference();

        await sut.InvokeVoidAsync<string>(jsRef, "init");

        Assert.Equal("String.init", jsRef.Calls[0].Method);
    }

    #endregion

    #region InvokeAsync

    [Fact]
    public async Task InvokeAsync_NullReference_ReturnsDefaultForValueType()
    {
        var result = await sut.InvokeAsync<JsObjectReferenceServiceTests, int>(null, "getValue");

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task InvokeAsync_NullReference_ReturnsNullForReferenceType()
    {
        var result = await sut.InvokeAsync<JsObjectReferenceServiceTests, string>(null, "getText");

        Assert.Null(result);
    }

    [Fact]
    public async Task InvokeAsync_ValidReference_InvokesWithPrefixedMethodName()
    {
        var jsRef = new MockJsObjectReference();

        await sut.InvokeAsync<JsObjectReferenceServiceTests, string>(jsRef, "getText");

        // Two calls: one from InvokeAsync itself. Filter by method name.
        var call = Assert.Single(jsRef.Calls, c => c.Method == "JsObjectReferenceServiceTests.getText");
        Assert.Equal("JsObjectReferenceServiceTests.getText", call.Method);
    }

    #endregion

    /// <summary>
    /// Minimal mock for <see cref="IJSObjectReference"/> that records all invocations.
    /// </summary>
    private sealed class MockJsObjectReference : IJSObjectReference
    {
        public List<(string Method, object?[]? Args)> Calls { get; } = [];

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            Calls.Add((identifier, args));
            return new ValueTask<TValue>(default(TValue)!);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            Calls.Add((identifier, args));
            return new ValueTask<TValue>(default(TValue)!);
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}