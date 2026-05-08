using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="CssHighlighter"/> syntax highlighting.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class CssHighlighterTests
{
    [Fact]
    public void Highlight_Comment_HighlightedAsComment()
    {
        var result = CssHighlighter.Highlight("/* comment */");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_StringLiteral_HighlightedAsString()
    {
        var result = CssHighlighter.Highlight("content: \"hello\"");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_AtRule_HighlightedAsKeyword()
    {
        var result = CssHighlighter.Highlight("@media screen");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_CssFunction_HighlightedAsFunction()
    {
        var result = CssHighlighter.Highlight("color: var(--x)");
        Assert.Contains("codeblock-function", result);
    }

    [Fact]
    public void Highlight_CssVariable_HighlightedAsField()
    {
        var result = CssHighlighter.Highlight("--ds-border-radius-md");
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_PropertyName_HighlightedAsAttr()
    {
        var result = CssHighlighter.Highlight("{ color: red }");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    public void Highlight_Number_HighlightedAsNumber()
    {
        var result = CssHighlighter.Highlight("{ margin: 10px }");
        Assert.Contains("codeblock-number", result);
    }

    [Fact]
    public void Highlight_Pseudo_HighlightedAsKeyword()
    {
        var result = CssHighlighter.Highlight("a:hover");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_ClassSelector_HighlightedAsTag()
    {
        var result = CssHighlighter.Highlight(".button { }");
        Assert.Contains("codeblock-tag", result);
    }

    [Fact]
    public void Highlight_IdSelector_HighlightedAsTag()
    {
        var result = CssHighlighter.Highlight("#main { }");
        Assert.Contains("codeblock-tag", result);
    }

    [Fact]
    public void Highlight_HtmlElement_HighlightedAsTag()
    {
        var result = CssHighlighter.Highlight("div { }");
        Assert.Contains("codeblock-tag", result);
    }
}