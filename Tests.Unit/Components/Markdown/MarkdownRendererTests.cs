using Bunit;
using MarkdownRenderer = Hviktor.Components.Markdown.MarkdownRenderer;

namespace Tests.Unit.Components.Markdown;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Markdown")]
public class MarkdownRendererTests : HviktorBunitContext
{

    #region Inline

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_Bold_RendersStrongTag()
    {
        var result = MarkdownRenderer.Render("**bold**");

        Assert.Contains("<strong>bold</strong>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_Italic_RendersEmTag()
    {
        var result = MarkdownRenderer.Render("*italic*");

        Assert.Contains("<em>italic</em>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_InlineCode_RendersCodeTag()
    {
        var result = MarkdownRenderer.Render("`code`");

        Assert.Contains("<code class=\"inline\">code</code>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_Strikethrough_RendersDelTag()
    {
        var result = MarkdownRenderer.Render("~~deleted~~");

        Assert.Contains("<del>deleted</del>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_Link_RendersAnchorWithSecurityAttributes()
    {
        var result = MarkdownRenderer.Render("[click](https://example.com)");

        Assert.Contains("href=\"https://example.com\"", result);
        Assert.Contains("target=\"_blank\"", result);
        Assert.Contains("rel=\"noopener noreferrer\"", result);
        Assert.Contains(">click</a>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_Image_RendersImgTag()
    {
        var result = MarkdownRenderer.Render("![alt text](https://example.com/img.png)");

        Assert.Contains("<img src=\"https://example.com/img.png\" alt=\"alt text\"", result);
    }

    #endregion

    #region Block

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_HorizontalRule_RendersHrTag()
    {
        var result = MarkdownRenderer.Render("---");

        Assert.Contains("<hr class=\"ds-divider\" />", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_Blockquote_RendersBlockquoteTag()
    {
        var result = MarkdownRenderer.Render("> quote");

        Assert.Contains("<blockquote class=\"markdown-blockquote\">", result);
        Assert.Contains("quote", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_UnorderedList_RendersUlTag()
    {
        var result = MarkdownRenderer.Render("- item one\n- item two");

        Assert.Contains("<ul class=\"ds-list\">", result);
        Assert.Contains("<li>item one</li>", result);
        Assert.Contains("<li>item two</li>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_OrderedList_RendersOlTag()
    {
        var result = MarkdownRenderer.Render("1. first\n2. second");

        Assert.Contains("<ol class=\"ds-list\">", result);
        Assert.Contains("<li>first</li>", result);
        Assert.Contains("<li>second</li>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_FencedCodeBlock_RendersPreCodeTags()
    {
        var result = MarkdownRenderer.Render("```\nvar x = 1;\n```");

        Assert.Contains("<pre class=\"codeblock-pre\"", result);
        Assert.Contains("<code class=\"codeblock-code\">", result);
        Assert.Contains("var x = 1;", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_Table_RendersTableStructure()
    {
        var markdown = "| Name | Age |\n| --- | --- |\n| Ola | 30 |";
        var result = MarkdownRenderer.Render(markdown);

        Assert.Contains("<table class=\"ds-table\">", result);
        Assert.Contains("<thead>", result);
        Assert.Contains("<th>Name</th>", result);
        Assert.Contains("<th>Age</th>", result);
        Assert.Contains("<tbody>", result);
        Assert.Contains("<td>Ola</td>", result);
        Assert.Contains("<td>30</td>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_Paragraph_WrapsParagraphTag()
    {
        var result = MarkdownRenderer.Render("Some text");

        Assert.Contains("<p class=\"ds-paragraph\">Some text</p>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_MultipleParagraphs_SeparatedByBlankLine()
    {
        var result = MarkdownRenderer.Render("First\n\nSecond");

        Assert.Contains("<p class=\"ds-paragraph\">First</p>", result);
        Assert.Contains("<p class=\"ds-paragraph\">Second</p>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_EmptyInput_ReturnsEmptyString()
    {
        Assert.Equal(string.Empty, MarkdownRenderer.Render(""));
        Assert.Equal(string.Empty, MarkdownRenderer.Render("   "));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_HtmlEncodes_TextContent()
    {
        var result = MarkdownRenderer.Render("Use <div> tags");

        Assert.Contains("&lt;div&gt;", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_RawHtmlBlock_PassesThrough()
    {
        var result = MarkdownRenderer.Render("<div class=\"custom\">content</div>");

        Assert.Contains("<div class=\"custom\">content</div>", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Render_NestedList_RendersNestedStructure()
    {
        var markdown = "- parent\n  - child";
        var result = MarkdownRenderer.Render(markdown);

        Assert.Contains("<ul class=\"ds-list\">", result);
        Assert.Contains("<li>parent</li>", result);
        Assert.Contains("<li>child</li>", result);
    }

    #endregion

    #region Parse

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Parse_EmptyInput_ReturnsEmptyList()
    {
        var segments = MarkdownRenderer.Parse("");

        Assert.Empty(segments);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Parse_PlainText_ReturnsSingleHtmlSegment()
    {
        var segments = MarkdownRenderer.Parse("Hello");

        Assert.Single(segments);
        Assert.False(segments[0].IsCodeBlock);
        Assert.Contains("Hello", segments[0].Content);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Parse_FencedCodeBlock_ReturnsCodeSegment()
    {
        var segments = MarkdownRenderer.Parse("```csharp\nvar x = 1;\n```");

        Assert.Single(segments);
        Assert.True(segments[0].IsCodeBlock);
        Assert.Equal("csharp", segments[0].Language);
        Assert.Equal("var x = 1;", segments[0].Content);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Parse_CodeBlockWithoutLanguage_DefaultsToPlaintext()
    {
        var segments = MarkdownRenderer.Parse("```\nsome code\n```");

        Assert.Single(segments);
        Assert.True(segments[0].IsCodeBlock);
        Assert.Equal("plaintext", segments[0].Language);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Parse_MixedContent_ReturnsSeparateSegments()
    {
        var markdown = "Some text\n\n```csharp\nvar x = 1;\n```\n\nMore text";
        var segments = MarkdownRenderer.Parse(markdown);

        Assert.Equal(3, segments.Count);
        Assert.False(segments[0].IsCodeBlock);
        Assert.True(segments[1].IsCodeBlock);
        Assert.Equal("csharp", segments[1].Language);
        Assert.False(segments[2].IsCodeBlock);
    }

    #endregion
}