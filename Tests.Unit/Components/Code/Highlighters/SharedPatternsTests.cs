using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="SharedPatterns"/> regex patterns.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class SharedPatternsTests
{
    [Theory]
    [InlineData("42", "42")]
    [InlineData("3.14", "3.14")]
    [InlineData("1e10", "1e10")]
    [InlineData("2.5f", "2.5f")]
    [InlineData("100L", "100L")]
    public void NumberLiteral_MatchesVariousFormats(string input, string expected)
    {
        var match = SharedPatterns.NumberLiteral().Match(input);
        Assert.True(match.Success);
        Assert.Equal(expected, match.Value);
    }

    [Theory]
    [InlineData("foo(", "foo")]
    [InlineData("bar (", "bar")]
    public void MethodCall_MatchesIdentifierBeforeParen(string input, string expected)
    {
        var match = SharedPatterns.MethodCall().Match(input);
        Assert.True(match.Success);
        Assert.Equal(expected, match.Value);
    }

    [Fact]
    public void MultiLineComment_MatchesBlockComment()
    {
        const string input = "/* hello\nworld */";
        var match = SharedPatterns.MultiLineComment().Match(input);
        Assert.True(match.Success);
        Assert.Equal(input, match.Value);
    }

    [Fact]
    public void SingleLineComment_MatchesLineComment()
    {
        const string input = "// this is a comment";
        var match = SharedPatterns.SingleLineComment().Match(input);
        Assert.True(match.Success);
        Assert.Equal(input, match.Value);
    }

    [Theory]
    [InlineData("\"hello\"", "\"hello\"")]
    [InlineData("\"esc\\\"ape\"", "\"esc\\\"ape\"")]
    public void DoubleQuotedString_MatchesStrings(string input, string expected)
    {
        var match = SharedPatterns.DoubleQuotedString().Match(input);
        Assert.True(match.Success);
        Assert.Equal(expected, match.Value);
    }
}
