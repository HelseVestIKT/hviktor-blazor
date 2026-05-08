using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="YamlHighlighter"/> syntax highlighting.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class YamlHighlighterTests
{
    [Fact]
    public void Highlight_Comment_HighlightedAsComment()
    {
        var result = YamlHighlighter.Highlight("# comment");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_DoubleQuotedString_HighlightedAsString()
    {
        var result = YamlHighlighter.Highlight("name: \"hello\"");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_SingleQuotedString_HighlightedAsString()
    {
        var result = YamlHighlighter.Highlight("name: 'hello'");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_BlockScalarIndicator_HighlightedAsKeyword()
    {
        var result = YamlHighlighter.Highlight("description: |");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    public void Highlight_Tag_HighlightedAsType()
    {
        var result = YamlHighlighter.Highlight("value: !!str 42");
        Assert.Contains("codeblock-type", result);
    }

    [Fact]
    public void Highlight_Anchor_HighlightedAsType()
    {
        var result = YamlHighlighter.Highlight("defaults: &defaults");
        Assert.Contains("codeblock-type", result);
    }

    [Fact]
    public void Highlight_AliasRef_HighlightedAsField()
    {
        var result = YamlHighlighter.Highlight("config: *defaults");
        Assert.Contains("codeblock-field", result);
    }

    [Fact]
    public void Highlight_Key_HighlightedAsAttr()
    {
        var result = YamlHighlighter.Highlight("name: value");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    public void Highlight_NestedKey_HighlightedAsAttr()
    {
        var result = YamlHighlighter.Highlight("  nested.key: value");
        Assert.Contains("codeblock-attr", result);
    }

    [Theory]
    [InlineData("enabled: true")]
    [InlineData("enabled: false")]
    [InlineData("value: null")]
    [InlineData("enabled: yes")]
    [InlineData("enabled: no")]
    public void Highlight_BooleanAndNull_HighlightedAsKeyword(string yaml)
    {
        var result = YamlHighlighter.Highlight(yaml);
        Assert.Contains("codeblock-keyword", result);
    }

    [Theory]
    [InlineData("port: 8080")]
    [InlineData("pi: 3.14")]
    [InlineData("hex: 0xFF")]
    public void Highlight_Numbers_HighlightedAsNumber(string yaml)
    {
        var result = YamlHighlighter.Highlight(yaml);
        Assert.Contains("codeblock-number", result);
    }

    [Fact]
    public void Highlight_FlowSequenceValue_HighlightedAsString()
    {
        var result = YamlHighlighter.Highlight("branches: [main, develop]");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_EmptyInput_ReturnsEmpty()
    {
        var result = YamlHighlighter.Highlight("");
        Assert.Equal("", result);
    }
}