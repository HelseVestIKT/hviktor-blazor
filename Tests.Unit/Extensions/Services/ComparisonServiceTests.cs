using Hviktor.Extensions.Services;

namespace Tests.Unit.Extensions.Services;

/// <summary>
/// Unit tests for <see cref="ComparisonService"/>.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class ComparisonServiceTests
{
    private readonly ComparisonService sut = new();

    #region StringComparison

    [Fact]
    public void StringComparison_ReturnsCaseInsensitive()
    {
        Assert.Equal(StringComparison.CurrentCultureIgnoreCase, sut.StringComparison);
    }

    [Fact]
    public void StringComparison_MatchesCaseInsensitively()
    {
        Assert.Equal(0, string.Compare("hello", "HELLO", sut.StringComparison));
    }

    [Fact]
    public void StringComparison_DistinguishesDifferentStrings()
    {
        Assert.NotEqual(0, string.Compare("hello", "world", sut.StringComparison));
    }

    #endregion

    #region StringComparer

    [Fact]
    public void StringComparer_ReturnsCaseInsensitive()
    {
        Assert.Equal(StringComparer.CurrentCultureIgnoreCase, sut.StringComparer);
    }

    [Fact]
    public void StringComparer_TreatsCaseAsEqual()
    {
        Assert.True(sut.StringComparer.Equals("Test", "test"));
    }

    [Fact]
    public void StringComparer_DistinguishesDifferentStrings()
    {
        Assert.False(sut.StringComparer.Equals("foo", "bar"));
    }

    [Fact]
    public void StringComparer_WorksInDictionary()
    {
        var dict = new Dictionary<string, int>(sut.StringComparer) { ["Key"] = 1 };
        Assert.Equal(1, dict["key"]);
    }

    #endregion
}