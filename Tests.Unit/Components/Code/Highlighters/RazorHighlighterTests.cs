using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="RazorHighlighter"/> syntax highlighting.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class RazorHighlighterTests
{
    [Fact]
    public void Highlight_HtmlTag_HighlightedAsTag()
    {
        var result = RazorHighlighter.Highlight("<div>hello</div>");
        Assert.Contains("codeblock-tag", result);
    }

    [Fact]
    public void Highlight_ComponentTag_HighlightedAsType()
    {
        var result = RazorHighlighter.Highlight("<Button>Click</Button>");
        Assert.Contains("codeblock-type", result);
    }

    [Fact]
    public void Highlight_AttributeName_HighlightedAsAttr()
    {
        var result = RazorHighlighter.Highlight("<div class=\"x\">test</div>");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    public void Highlight_AttributeValue_HighlightedAsString()
    {
        var result = RazorHighlighter.Highlight("<div class=\"main\">test</div>");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_CodeBlock_HighlightsCSharp()
    {
        var result = RazorHighlighter.Highlight("@code { private int count = 0; }");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_InlineExpression_HighlightsCSharp()
    {
        var result = RazorHighlighter.Highlight("<p>@(DateTime.Now)</p>");
        Assert.Contains("codeblock-type", result);
    }

    [Fact]
    public void Highlight_UsingDirective_HighlightedAsKeyword()
    {
        var result = RazorHighlighter.Highlight("@using Microsoft.AspNetCore");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_ForeachStatement_HighlightsKeyword()
    {
        var result = RazorHighlighter.Highlight("@foreach (var item in items)");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_BindDirective_HighlightedAsAttr()
    {
        var result = RazorHighlighter.Highlight("<input @bind=\"value\" />");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    public void Highlight_EventHandler_HighlightedAsAttr()
    {
        var result = RazorHighlighter.Highlight("<button @onclick=\"HandleClick\">Go</button>");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    public void Highlight_HtmlComment_HighlightedAsComment()
    {
        var result = RazorHighlighter.Highlight("<!-- comment -->");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_CodeBlockMethod_RecognizedInMarkup()
    {
        const string code = "@code { private void HandleClick() { } }\n<button @onclick=\"HandleClick\">Go</button>";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_StructuralDirective_HighlightedAsKeyword()
    {
        var result = RazorHighlighter.Highlight("@page \"/home\"");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_AttributeValueWithRazorExpression_HighlightsCSharp()
    {
        const string code = "@code { private List<string> items = new(); }\n<Component Items=\"@items\" />";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_AttributeValueWithDottedExpression_HighlightsCSharp()
    {
        const string code = "<Icon Name=\"@IconSet.Captions\" />";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_AttributeValueWithMethodFromCodeBlock_HighlightsAsFunction()
    {
        const string code = "@code { private string GetTitle() { return \"\"; } }\n<p>@GetTitle()</p>";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-function", result);
    }

    [Fact]
    public void Highlight_AttributeValueWithFieldFromCodeBlock_HighlightsAsField()
    {
        const string code = "@code { private string title = \"hi\"; }\n<p>@title</p>";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_ImplicitExpression_SimpleField_HighlightsAsField()
    {
        const string code = "@code { private int count = 0; }\n<span>@count</span>";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_ImplicitExpression_UnknownIdentifier_HighlightedViaCSharp()
    {
        var result = RazorHighlighter.Highlight("<p>@DateTime.Now</p>");
        Assert.Contains("codeblock-type", result);
    }

    [Fact]
    public void Highlight_ImplicitExpression_ControlFlowKeyword_HighlightsAsKeyword()
    {
        var result = RazorHighlighter.Highlight("@if (true)");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_ImplicitExpression_DoKeyword_HighlightsKeywordAndRemainder()
    {
        const string code = "<p>@do</p>";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-keyword", result);
        Assert.Contains(">do<", result);
    }

    [Fact]
    public void Highlight_ImplicitExpression_ControlFlowKeywordWithDotAccess_HighlightsKeywordThenCSharp()
    {
        const string code = "<p>@if.Something</p>";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-keyword", result);
        Assert.Contains(">if<", result);
    }

    [Fact]
    public void Highlight_AttributeValue_EmptyExpressionAfterAt_ReturnsEmpty()
    {
        const string code = "<Input Value=\"@ \" />";
        var result = RazorHighlighter.Highlight(code);
        Assert.DoesNotContain("codeblock-function", result);
        Assert.DoesNotContain("codeblock-field", result);
    }

    [Fact]
    public void Highlight_AttributeValue_MethodIdentifier_HighlightsAsFunction()
    {
        const string code = "@code { private string GetName() { return \"\"; } }\n<Input Value=\"@GetName\" />";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-function", result);
    }

    [Fact]
    public void Highlight_AttributeValue_FieldIdentifier_HighlightsAsField()
    {
        const string code = "@code { private string name = \"x\"; }\n<Input Value=\"@name\" />";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_EmptyExpression_NoOutput()
    {
        var result = RazorHighlighter.Highlight("<div class=\"\">test</div>");
        Assert.DoesNotContain("codeblock-function", result);
    }

    [Fact]
    public void Highlight_AttributeValueWithoutClosingQuote_StillHighlights()
    {
        const string code = "@code { private string name = \"\"; }\n<Input Value=\"@name\" />";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_ForeachVariable_RecognizedAsField()
    {
        const string code = "@foreach (var person in people)\n{\n<p>@person</p>\n}";
        var result = RazorHighlighter.Highlight(code);
        Assert.Contains("codeblock-field", result);
    }
}