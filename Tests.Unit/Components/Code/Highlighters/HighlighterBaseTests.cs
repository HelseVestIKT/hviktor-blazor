using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="HighlighterBase"/> token collection, sorting, and rendering.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class HighlighterBaseTests
{
    [Fact]
    public void ApplyPatterns_EmptyCode_ReturnsEmpty()
    {
        var result = HighlighterBase.ApplyPatterns("", []);
        Assert.Equal("", result);
    }

    [Fact]
    public void ApplyPatterns_NoMatches_ReturnsHtmlEncodedInput()
    {
        var result = HighlighterBase.ApplyPatterns("hello", []);
        Assert.Equal("hello", result);
    }

    [Fact]
    public void ApplyPatterns_HtmlEncodes_UnmatchedText()
    {
        var result = HighlighterBase.ApplyPatterns("<div>", []);
        Assert.Equal("&lt;div&gt;", result);
    }

    [Fact]
    public void ApplyPatterns_WrapsMatchInSpan()
    {
        var patterns = new (System.Text.RegularExpressions.Regex, string)[]
        {
            (SharedPatterns.NumberLiteral(), "number")
        };

        var result = HighlighterBase.ApplyPatterns("x = 42;", patterns);
        Assert.Contains("<span class=\"codeblock-number\">42</span>", result);
    }

    [Fact]
    public void ApplyPatterns_OverlappingTokens_CommentWinsOverNumber()
    {
        var patterns = new (System.Text.RegularExpressions.Regex, string)[]
        {
            (SharedPatterns.SingleLineComment(), "comment"),
            (SharedPatterns.NumberLiteral(), "number")
        };

        var result = HighlighterBase.ApplyPatterns("// 42", patterns);
        Assert.Contains("<span class=\"codeblock-comment\">// 42</span>", result);
        Assert.DoesNotContain("codeblock-number", result);
    }

    [Fact]
    public void CollectTokens_ReturnsAllMatches()
    {
        var patterns = new (System.Text.RegularExpressions.Regex, string)[]
        {
            (SharedPatterns.NumberLiteral(), "number")
        };

        var tokens = HighlighterBase.CollectTokens("1 + 2", patterns);
        Assert.Equal(2, tokens.Count);
    }

    [Fact]
    public void SortAndFilter_RemovesOverlappingTokens()
    {
        var tokens = new List<(int Start, int Length, string CssClass)>
        {
            (0, 5, "comment"),
            (2, 3, "number"),
        };

        var filtered = HighlighterBase.SortAndFilter(tokens);
        Assert.Single(filtered);
        Assert.Equal("comment", filtered[0].CssClass);
    }

    [Fact]
    public void SortAndFilter_KeepsNonOverlappingTokens()
    {
        var tokens = new List<(int Start, int Length, string CssClass)>
        {
            (0, 3, "keyword"),
            (5, 2, "number"),
        };

        var filtered = HighlighterBase.SortAndFilter(tokens);
        Assert.Equal(2, filtered.Count);
    }

    [Fact]
    public void Encode_HtmlEncodesSpecialCharacters()
    {
        Assert.Equal("&lt;", HighlighterBase.Encode("<"));
        Assert.Equal("&gt;", HighlighterBase.Encode(">"));
        Assert.Equal("&amp;", HighlighterBase.Encode("&"));
        Assert.Equal("&quot;", HighlighterBase.Encode("\""));
    }
}
