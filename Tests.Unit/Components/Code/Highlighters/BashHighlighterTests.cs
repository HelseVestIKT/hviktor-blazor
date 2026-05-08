using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="BashHighlighter"/> syntax highlighting.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class BashHighlighterTests
{
    [Fact]
    public void Highlight_Shebang_HighlightedAsType()
    {
        var result = BashHighlighter.Highlight("#!/bin/bash");
        Assert.Contains("codeblock-type", result);
        Assert.Contains("#!/bin/bash", result);
    }

    [Fact]
    public void Highlight_Comment_HighlightedAsComment()
    {
        var result = BashHighlighter.Highlight("# this is a comment");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_SingleQuotedString_HighlightedAsString()
    {
        var result = BashHighlighter.Highlight("echo 'hello'");
        Assert.Contains("codeblock-string", result);
        Assert.Contains("hello", result);
    }

    [Fact]
    public void Highlight_DoubleQuotedStringWithVariable_HighlightsInterpolation()
    {
        var result = BashHighlighter.Highlight("echo \"Hello $name\"");
        Assert.Contains("codeblock-field", result);
        Assert.Contains("$name", result);
    }

    [Fact]
    public void Highlight_VariableAssignment_HighlightedAsField()
    {
        var result = BashHighlighter.Highlight("value=\"World\"");
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_Variable_HighlightedAsField()
    {
        var result = BashHighlighter.Highlight("echo $HOME");
        Assert.Contains("codeblock-field", result);
        Assert.Contains("$HOME", result);
    }

    [Fact]
    public void Highlight_BracedVariable_HighlightedAsField()
    {
        var result = BashHighlighter.Highlight("echo ${HOME}");
        Assert.Contains("codeblock-field", result);
        Assert.Contains("${HOME}", result);
    }

    [Theory]
    [InlineData("if")]
    [InlineData("then")]
    [InlineData("else")]
    [InlineData("for")]
    [InlineData("while")]
    [InlineData("do")]
    [InlineData("done")]
    [InlineData("echo")]
    public void Highlight_Keywords_HighlightedAsKeyword(string keyword)
    {
        var result = BashHighlighter.Highlight(keyword);
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_ShebangNotTreatedAsComment()
    {
        var result = BashHighlighter.Highlight("#!/bin/bash\n# comment");
        // Shebang should be type, regular comment should be comment
        Assert.Contains("codeblock-type", result);
        Assert.Contains("codeblock-comment", result);
    }
}