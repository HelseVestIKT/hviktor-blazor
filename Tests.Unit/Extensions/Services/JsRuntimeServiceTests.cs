using Hviktor.Extensions.Services;
using Microsoft.JSInterop;

namespace Tests.Unit.Extensions.Services;

/// <summary>
/// Unit tests for <see cref="JsRuntimeService"/>.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class JsRuntimeServiceTests
{
    [Fact]
    public async Task ImportAsync_InvokesJsRuntimeWithImport()
    {
        var mockRuntime = new MockJsRuntime();

        var service = new JsRuntimeService(mockRuntime);
        await service.ImportAsync<JsRuntimeServiceTests>();

        Assert.Single(mockRuntime.Calls);
        Assert.Equal("import", mockRuntime.Calls[0].Identifier);
    }

    [Fact]
    public async Task ImportAsync_PathContainsContentPrefix()
    {
        var mockRuntime = new MockJsRuntime();

        var service = new JsRuntimeService(mockRuntime);
        await service.ImportAsync<JsRuntimeService>();

        var path = (string)mockRuntime.Calls[0].Args![0]!;
        Assert.StartsWith("./_content/", path);
    }

    [Fact]
    public async Task ImportAsync_PathEndsWithRazorJs()
    {
        var mockRuntime = new MockJsRuntime();

        var service = new JsRuntimeService(mockRuntime);
        await service.ImportAsync<JsRuntimeServiceTests>();

        var path = (string)mockRuntime.Calls[0].Args![0]!;
        Assert.Contains(".razor.js", path);
    }

    [Fact]
    public async Task ImportAsync_PathContainsVersionParameter()
    {
        var mockRuntime = new MockJsRuntime();

        var service = new JsRuntimeService(mockRuntime);
        await service.ImportAsync<JsRuntimeServiceTests>();

        var path = (string)mockRuntime.Calls[0].Args![0]!;
        Assert.Contains("?v=", path);
    }

    [Fact]
    public async Task ImportAsync_PathContainsTypeName()
    {
        var mockRuntime = new MockJsRuntime();

        var service = new JsRuntimeService(mockRuntime);
        await service.ImportAsync<JsRuntimeServiceTests>();

        var path = (string)mockRuntime.Calls[0].Args![0]!;
        Assert.Contains("JsRuntimeServiceTests", path);
    }

    /// <summary>
    /// Mock <see cref="IJSRuntime"/> that records invocations and returns a mock reference.
    /// </summary>
    private sealed class MockJsRuntime : IJSRuntime
    {
        public List<(string Identifier, object?[]? Args)> Calls { get; } = [];

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            Calls.Add((identifier, args));
            // Return a mock IJSObjectReference when TValue is IJSObjectReference
            if (typeof(TValue) == typeof(IJSObjectReference))
            {
                return new ValueTask<TValue>((TValue)(object)new MockJsObjectReference());
            }

            return new ValueTask<TValue>(default(TValue)!);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            return InvokeAsync<TValue>(identifier, args);
        }
    }

    /// <summary>
    /// Minimal mock for <see cref="IJSObjectReference"/>.
    /// </summary>
    private sealed class MockJsObjectReference : IJSObjectReference
    {
        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) =>
            new(default(TValue)!);

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) =>
            new(default(TValue)!);

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}
