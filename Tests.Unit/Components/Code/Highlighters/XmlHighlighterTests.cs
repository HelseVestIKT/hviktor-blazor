using Hviktor.Components.Code.Highlighters;

namespace Tests.Unit.Components.Code.Highlighters;

/// <summary>
/// Unit tests for <see cref="XmlHighlighter"/> syntax highlighting.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Code")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
public class XmlHighlighterTests
{
    [Fact]
    public void Highlight_Comment_HighlightedAsComment()
    {
        var result = XmlHighlighter.Highlight("<!-- comment -->");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    public void Highlight_CData_HighlightedAsString()
    {
        var result = XmlHighlighter.Highlight("<![CDATA[content]]>");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_AttributeValue_HighlightedAsString()
    {
        var result = XmlHighlighter.Highlight("<div class=\"main\">");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    public void Highlight_TagName_HighlightedAsTag()
    {
        var result = XmlHighlighter.Highlight("<div>");
        Assert.Contains("codeblock-tag", result);
    }

    [Fact]
    public void Highlight_ClosingTag_HighlightedAsTag()
    {
        var result = XmlHighlighter.Highlight("</div>");
        Assert.Contains("codeblock-tag", result);
    }

    [Fact]
    public void Highlight_AttributeName_HighlightedAsAttr()
    {
        var result = XmlHighlighter.Highlight("<div class=\"x\">");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    public void Highlight_SelfClosingTag_HighlightedAsTag()
    {
        var result = XmlHighlighter.Highlight("<br/>");
        Assert.Contains("codeblock-tag", result);
    }

    [Fact]
    public void Highlight_NamespacedTag_HighlightedAsTag()
    {
        var result = XmlHighlighter.Highlight("<ns:element>");
        Assert.Contains("codeblock-tag", result);
    }

    [Fact]
    public void Highlight_HtmlEncodesContent()
    {
        var result = XmlHighlighter.Highlight("<div>a & b</div>");
        Assert.Contains("&amp;", result);
    }
}