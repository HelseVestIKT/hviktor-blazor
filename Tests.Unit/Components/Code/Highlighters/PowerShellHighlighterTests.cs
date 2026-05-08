using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="PowerShellHighlighter"/> syntax highlighting.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class PowerShellHighlighterTests
{
    [Fact]
    public void Highlight_BlockComment_HighlightedAsComment()
    {
        var result = PowerShellHighlighter.Highlight("<# block comment #>");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_LineComment_HighlightedAsComment()
    {
        var result = PowerShellHighlighter.Highlight("# line comment");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_SingleQuotedString_HighlightedAsString()
    {
        var result = PowerShellHighlighter.Highlight("'hello'");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_DoubleQuotedStringWithVariable_HighlightsInterpolation()
    {
        var result = PowerShellHighlighter.Highlight("\"Hello $name\"");
        Assert.Contains("codeblock-field", result);
        Assert.Contains("$name", result);
    }

    [Fact]
    public void Highlight_Variable_HighlightedAsField()
    {
        var result = PowerShellHighlighter.Highlight("$Server");
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_Parameter_HighlightedAsAttr()
    {
        var result = PowerShellHighlighter.Highlight("-ComputerName");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    public void Highlight_Number_HighlightedAsNumber()
    {
        var result = PowerShellHighlighter.Highlight("$x = 42");
        Assert.Contains("codeblock-number", result);
    }

    [Theory]
    [InlineData("if")]
    [InlineData("else")]
    [InlineData("foreach")]
    [InlineData("function")]
    [InlineData("return")]
    [InlineData("try")]
    [InlineData("catch")]
    public void Highlight_Keywords_HighlightedAsKeyword(string keyword)
    {
        var result = PowerShellHighlighter.Highlight(keyword);
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_Cmdlet_HighlightedAsFunction()
    {
        var result = PowerShellHighlighter.Highlight("Get-Process");
        Assert.Contains("codeblock-function", result);
    }

    [Fact]
    public void Highlight_WriteCmdlet_HighlightedAsFunction()
    {
        var result = PowerShellHighlighter.Highlight("Write-Output");
        Assert.Contains("codeblock-function", result);
    }
}
