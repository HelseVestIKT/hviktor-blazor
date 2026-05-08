using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="JsonHighlighter"/> syntax highlighting.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class JsonHighlighterTests
{
    [Fact]
    public void Highlight_Key_HighlightedAsAttr()
    {
        var result = JsonHighlighter.Highlight("{\"name\": \"value\"}");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    public void Highlight_StringValue_HighlightedAsString()
    {
        var result = JsonHighlighter.Highlight("{\"key\": \"value\"}");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_Number_HighlightedAsNumber()
    {
        var result = JsonHighlighter.Highlight("{\"age\": 42}");
        Assert.Contains("codeblock-number", result);
    }

    [Fact]
    public void Highlight_FloatNumber_HighlightedAsNumber()
    {
        var result = JsonHighlighter.Highlight("{\"pi\": 3.14}");
        Assert.Contains("codeblock-number", result);
    }

    [Theory]
    [InlineData("true")]
    [InlineData("false")]
    [InlineData("null")]
    public void Highlight_Keywords_HighlightedAsKeyword(string keyword)
    {
        var result = JsonHighlighter.Highlight($"{{\"key\": {keyword}}}");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_NestedObject_HighlightsKeys()
    {
        var result = JsonHighlighter.Highlight("{\"outer\": {\"inner\": 1}}");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    public void Highlight_EmptyInput_ReturnsEmpty()
    {
        var result = JsonHighlighter.Highlight("");
        Assert.Equal("", result);
    }

    [Fact]
    public void Highlight_HtmlEncodesUnmatchedText()
    {
        var result = JsonHighlighter.Highlight("{\"key\": \"<br>\"}");
        Assert.Contains("&lt;br&gt;", result);
    }
}

