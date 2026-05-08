using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="CSharpHighlighter"/> syntax highlighting.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class CSharpHighlighterTests
{
    [Fact]
    public void Highlight_SingleLineComment_HighlightedAsComment()
    {
        var result = CSharpHighlighter.Highlight("// comment");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_MultiLineComment_HighlightedAsComment()
    {
        var result = CSharpHighlighter.Highlight("/* block */");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_XmlDocComment_HighlightedAsComment()
    {
        var result = CSharpHighlighter.Highlight("/// <summary>Docs</summary>");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_StringLiteral_HighlightedAsString()
    {
        var result = CSharpHighlighter.Highlight("var x = \"hello\";");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_VerbatimString_HighlightedAsString()
    {
        var result = CSharpHighlighter.Highlight("var x = @\"path\\to\";");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_InterpolatedString_HighlightsExpressionHole()
    {
        var result = CSharpHighlighter.Highlight("var x = $\"Hello {name}\";");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_CharLiteral_HighlightedAsString()
    {
        var result = CSharpHighlighter.Highlight("var c = 'a';");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_Number_HighlightedAsNumber()
    {
        var result = CSharpHighlighter.Highlight("int x = 42;");
        Assert.Contains("codeblock-number", result);
    }

    [Theory]
    [InlineData("public")]
    [InlineData("class")]
    [InlineData("void")]
    [InlineData("return")]
    [InlineData("if")]
    [InlineData("else")]
    [InlineData("var")]
    [InlineData("namespace")]
    [InlineData("using")]
    public void Highlight_Keywords_HighlightedAsKeyword(string keyword)
    {
        var result = CSharpHighlighter.Highlight(keyword);
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_TypeName_HighlightedAsType()
    {
        var result = CSharpHighlighter.Highlight("StringBuilder sb");
        Assert.Contains("codeblock-type", result);
    }

    [Fact]
    public void Highlight_MethodCall_HighlightedAsFunction()
    {
        var result = CSharpHighlighter.Highlight("Console.WriteLine()");
        Assert.Contains("codeblock-function", result);
    }

    [Fact]
    public void Highlight_Attribute_HighlightedAsAttr()
    {
        var result = CSharpHighlighter.Highlight("[Fact]");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    public void Highlight_MemberAccess_HighlightedAsField()
    {
        var result = CSharpHighlighter.Highlight("obj.name");
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_InterpolatedString_WithNestedStringInHole_HighlightsCorrectly()
    {
        // The nested string "world" inside the interpolation hole triggers SkipNestedString
        var result = CSharpHighlighter.Highlight("var x = $\"Hello {name.Replace(\"world\", \"earth\")}\";");
        Assert.Contains("codeblock-string", result);
        Assert.Contains("codeblock-function", result);
    }

    [Fact]
    public void Highlight_InterpolatedString_WithEscapedQuoteInNestedString_SkipsCorrectly()
    {
        // Escaped quote inside nested string in interpolation hole
        var result = CSharpHighlighter.Highlight("var x = $\"Result: {GetValue(\"test\\\"val\")}\";");
        Assert.Contains("codeblock-string", result);
    }
}
