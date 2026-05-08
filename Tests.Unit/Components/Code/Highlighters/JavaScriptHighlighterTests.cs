using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="JavaScriptHighlighter"/> syntax highlighting.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class JavaScriptHighlighterTests
{
    [Fact]
    public void Highlight_SingleLineComment_HighlightedAsComment()
    {
        var result = JavaScriptHighlighter.Highlight("// comment");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_MultiLineComment_HighlightedAsComment()
    {
        var result = JavaScriptHighlighter.Highlight("/* block */");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_JsDocComment_HighlightsTagsAsKeyword()
    {
        var result = JavaScriptHighlighter.Highlight("/** @param {string} name */");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_TemplateLiteral_HighlightsInterpolation()
    {
        var result = JavaScriptHighlighter.Highlight("`Hello ${name}`");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_DoubleQuotedString_HighlightedAsString()
    {
        var result = JavaScriptHighlighter.Highlight("var x = \"hello\";");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_SingleQuotedString_HighlightedAsString()
    {
        var result = JavaScriptHighlighter.Highlight("var x = 'hello';");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_Number_HighlightedAsNumber()
    {
        var result = JavaScriptHighlighter.Highlight("const x = 42;");
        Assert.Contains("codeblock-number", result);
    }

    [Theory]
    [InlineData("const")]
    [InlineData("let")]
    [InlineData("function")]
    [InlineData("return")]
    [InlineData("if")]
    [InlineData("else")]
    [InlineData("class")]
    [InlineData("import")]
    [InlineData("export")]
    public void Highlight_Keywords_HighlightedAsKeyword(string keyword)
    {
        var result = JavaScriptHighlighter.Highlight($"{keyword} ");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_MethodCall_HighlightedAsFunction()
    {
        var result = JavaScriptHighlighter.Highlight("console.log()");
        Assert.Contains("codeblock-function", result);
    }

    [Fact]
    public void Highlight_MemberAccess_HighlightedAsField()
    {
        var result = JavaScriptHighlighter.Highlight("obj.length");
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_DeclaredVariable_RecognizedAsField()
    {
        var result = JavaScriptHighlighter.Highlight("const name = 'x';\nconsole.log(name)");
        // "name" declared via const should be recognized as field in usage
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_DeclaredFunction_RecognizedAsFunction()
    {
        var result = JavaScriptHighlighter.Highlight("function greet() {}\ngreet");
        Assert.Contains("codeblock-function", result);
    }

    [Fact]
    public void Highlight_JsDocWithTypeExpression_HighlightsTypeAsType()
    {
        // {String} triggers HighlightJsDocType with a named type
        var result = JavaScriptHighlighter.Highlight("/** @param {String} name */");
        Assert.Contains("codeblock-type", result);
    }

    [Fact]
    public void Highlight_JsDocWithImportType_HighlightsImportAsKeyword()
    {
        // import('./module') inside JSDoc type braces triggers the import branch
        var result = JavaScriptHighlighter.Highlight("/** @type {import('./module').Default} */");
        Assert.Contains("codeblock-keyword", result);
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_JsDocTypeWithMultipleTypes_HighlightsAll()
    {
        // Multiple type names separated by pipe
        var result = JavaScriptHighlighter.Highlight("/** @param {String|Number} value */");
        Assert.Contains("codeblock-type", result);
    }

    [Fact]
    public void Highlight_JsDocWithParamName_HighlightsAsField()
    {
        var result = JavaScriptHighlighter.Highlight("/** @param {string} myParam description */");
        Assert.Contains("codeblock-field", result);
    }
}