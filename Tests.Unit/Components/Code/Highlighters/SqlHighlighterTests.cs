using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="SqlHighlighter"/> syntax highlighting.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class SqlHighlighterTests
{
    [Fact]
    public void Highlight_LineComment_HighlightedAsComment()
    {
        var result = SqlHighlighter.Highlight("-- comment");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_BlockComment_HighlightedAsComment()
    {
        var result = SqlHighlighter.Highlight("/* block */");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_StringLiteral_HighlightedAsString()
    {
        var result = SqlHighlighter.Highlight("'hello'");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_Number_HighlightedAsNumber()
    {
        var result = SqlHighlighter.Highlight("SELECT 42");
        Assert.Contains("codeblock-number", result);
    }

    [Fact]
    public void Highlight_Variable_HighlightedAsVariable()
    {
        var result = SqlHighlighter.Highlight("@userId");
        Assert.Contains("codeblock-variable", result);
    }

    [Theory]
    [InlineData("INT")]
    [InlineData("VARCHAR")]
    [InlineData("DATETIME")]
    [InlineData("BOOLEAN")]
    public void Highlight_DataTypes_HighlightedAsType(string dataType)
    {
        var result = SqlHighlighter.Highlight(dataType);
        Assert.Contains("codeblock-type", result);
    }

    [Theory]
    [InlineData("SELECT")]
    [InlineData("FROM")]
    [InlineData("WHERE")]
    [InlineData("INSERT")]
    [InlineData("UPDATE")]
    [InlineData("DELETE")]
    [InlineData("JOIN")]
    public void Highlight_Keywords_HighlightedAsKeyword(string keyword)
    {
        var result = SqlHighlighter.Highlight(keyword);
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_BuiltInFunction_HighlightedAsFunction()
    {
        var result = SqlHighlighter.Highlight("COUNT(*)");
        Assert.Contains("codeblock-function", result);
    }

    [Fact]
    public void Highlight_Operator_HighlightedAsOperator()
    {
        var result = SqlHighlighter.Highlight("a >= b");
        Assert.Contains("codeblock-operator", result);
    }

    [Fact]
    public void Highlight_CaseInsensitiveKeywords()
    {
        var result = SqlHighlighter.Highlight("select from where");
        Assert.Contains("codeblock-keyword", result);
    }
}
