using Hviktor.Extensions.Services;
using Microsoft.Extensions.Localization;

namespace Tests.Unit.Extensions.Services;

/// <summary>
/// Unit tests for <see cref="StringLocalizerService{TResource}"/>.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class StringLocalizerServiceTests
{
    #region GetValue(key)

    [Fact]
    public void GetValue_ExistingKey_ReturnsLocalizedValue()
    {
        var localizer = new MockStringLocalizer<StringLocalizerServiceTests>(
            new Dictionary<string, string> { ["greeting"] = "Hello" });
        var service = new StringLocalizerService<StringLocalizerServiceTests>(localizer);

        var result = service.GetValue("greeting");

        Assert.Equal("Hello", result);
    }

    [Fact]
    public void GetValue_MissingKey_ReturnsErrorMessage()
    {
        var localizer = new MockStringLocalizer<StringLocalizerServiceTests>(new Dictionary<string, string>());
        var service = new StringLocalizerService<StringLocalizerServiceTests>(localizer);

        var result = service.GetValue("missing");

        Assert.Equal("missing", result);
    }

    #endregion

    #region GetValue(key, defaultValue)

    [Fact]
    public void GetValue_ExistingKey_WithDefault_ReturnsLocalizedValue()
    {
        var localizer = new MockStringLocalizer<StringLocalizerServiceTests>(
            new Dictionary<string, string> { ["key"] = "Value" });
        var service = new StringLocalizerService<StringLocalizerServiceTests>(localizer);

        var result = service.GetValue("key");
        Assert.Equal("Value", result);
    }

    [Fact]
    public void GetValue_MissingKey_ReturnsDefaultValue()
    {
        var localizer = new MockStringLocalizer<StringLocalizerServiceTests>(new Dictionary<string, string>());
        var service = new StringLocalizerService<StringLocalizerServiceTests>(localizer);

        var result = service.GetValue("missing");
        Assert.Equal("missing", result);
    }

    [Fact]
    public void GetValue_MissingKey_EmptyDefault_ReturnsKey()
    {
        var localizer = new MockStringLocalizer<StringLocalizerServiceTests>(new Dictionary<string, string>());
        var service = new StringLocalizerService<StringLocalizerServiceTests>(localizer);

        var result = service.GetValue("someKey");
        Assert.Equal("someKey", result);
    }

    [Fact]
    public void GetValue_MissingKey_WhitespaceDefault_ReturnsKey()
    {
        var localizer = new MockStringLocalizer<StringLocalizerServiceTests>(new Dictionary<string, string>());
        var service = new StringLocalizerService<StringLocalizerServiceTests>(localizer);

        var result = service.GetValue("someKey");
        Assert.Equal("someKey", result);
    }

    #endregion

    /// <summary>
    /// Minimal mock <see cref="IStringLocalizer{T}"/> for testing.
    /// </summary>
    private sealed class MockStringLocalizer<T>(Dictionary<string, string> resources) : IStringLocalizer<T>
    {
        public LocalizedString this[string name]
        {
            get
            {
                var found = resources.TryGetValue(name, out var value);
                return new LocalizedString(name, value ?? name, !found);
            }
        }

        public LocalizedString this[string name, params object[] arguments] => this[name];

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
            resources.Select(kvp => new LocalizedString(kvp.Key, kvp.Value));
    }
}