using Hviktor.Components.Code;

namespace Tests.Unit.Components.Code;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "SyntaxHighlighter")]
public class SyntaxHighlighterTests
{
    #region GetLanguageDisplayName

    [Theory]
    [InlineData("csharp", "C#")]
    [InlineData("cs", "C#")]
    [InlineData("c#", "C#")]
    [InlineData("razor", "Razor")]
    [InlineData("cshtml", "Razor")]
    [InlineData("html", "HTML")]
    [InlineData("xml", "XML")]
    [InlineData("svg", "SVG")]
    [InlineData("css", "CSS")]
    [InlineData("scss", "SCSS")]
    [InlineData("json", "JSON")]
    [InlineData("javascript", "JavaScript")]
    [InlineData("js", "JavaScript")]
    [InlineData("typescript", "TypeScript")]
    [InlineData("ts", "TypeScript")]
    [InlineData("bash", "Bash")]
    [InlineData("shell", "Bash")]
    [InlineData("sh", "Bash")]
    [InlineData("sql", "SQL")]
    [InlineData("yaml", "YAML")]
    [InlineData("yml", "YAML")]
    [InlineData("powershell", "PowerShell")]
    [InlineData("plaintext", "Text")]
    [InlineData("text", "Text")]
    [InlineData("", "Text")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetLanguageDisplayName_ReturnsExpected(string input, string expected)
    {
        var result = SyntaxHighlighter.GetLanguageDisplayName(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetLanguageDisplayName_UnknownLanguage_ReturnsUpperCase()
    {
        var result = SyntaxHighlighter.GetLanguageDisplayName("ruby");
        Assert.Equal("RUBY", result);
    }

    #endregion

    #region Highlight

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_EmptyCode_ReturnsEmpty()
    {
        var result = SyntaxHighlighter.Highlight("", "csharp");
        Assert.Empty(result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_WhitespaceOnly_ReturnsEmpty()
    {
        var result = SyntaxHighlighter.Highlight("   ", "csharp");
        Assert.Empty(result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_CSharp_HighlightsKeywords()
    {
        var result = SyntaxHighlighter.Highlight("public class Foo { }", "csharp");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_CSharp_HighlightsStrings()
    {
        var result = SyntaxHighlighter.Highlight("var x = \"hello\";", "csharp");
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_CSharp_HighlightsComments()
    {
        var result = SyntaxHighlighter.Highlight("// comment", "csharp");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_CSharp_HighlightsNumbers()
    {
        var result = SyntaxHighlighter.Highlight("var x = 42;", "csharp");
        Assert.Contains("codeblock-number", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_CSharp_HighlightsTypes()
    {
        var result = SyntaxHighlighter.Highlight("String x = null;", "csharp");
        Assert.Contains("codeblock-type", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_Html_HighlightsTags()
    {
        var result = SyntaxHighlighter.Highlight("<div class=\"test\">Hello</div>", "html");
        Assert.Contains("codeblock-tag", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_Json_HighlightsPropertyKeys()
    {
        var result = SyntaxHighlighter.Highlight("{ \"name\": \"value\" }", "json");
        Assert.Contains("codeblock-attr", result);
        Assert.Contains("codeblock-string", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_Json_HighlightsKeywords()
    {
        var result = SyntaxHighlighter.Highlight("{ \"active\": true, \"count\": null }", "json");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_Json_HighlightsNumbers()
    {
        var result = SyntaxHighlighter.Highlight("{ \"count\": 42 }", "json");
        Assert.Contains("codeblock-number", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_JavaScript_HighlightsKeywords()
    {
        var result = SyntaxHighlighter.Highlight("const x = 1;", "javascript");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_Bash_HighlightsComments()
    {
        var result = SyntaxHighlighter.Highlight("# comment\necho hello", "bash");
        Assert.Contains("codeblock-comment", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_Sql_HighlightsKeywords()
    {
        var result = SyntaxHighlighter.Highlight("SELECT * FROM users", "sql");
        Assert.Contains("codeblock-keyword", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_Yaml_HighlightsKeys()
    {
        var result = SyntaxHighlighter.Highlight("name: value", "yaml");
        Assert.Contains("codeblock-attr", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_UnknownLanguage_EncodesWithoutHighlighting()
    {
        var result = SyntaxHighlighter.Highlight("<script>alert('xss')</script>", "unknown");
        Assert.DoesNotContain("codeblock-", result);
        // Should HTML-encode the content
        Assert.Contains("&lt;script&gt;", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_HtmlEncodesContent()
    {
        var result = SyntaxHighlighter.Highlight("x < 5 && y > 3", "csharp");
        Assert.Contains("&lt;", result);
        Assert.Contains("&amp;&amp;", result);
    }

    #endregion

    #region GetLanguageCssClass

    [Theory]
    [InlineData("csharp", "lang-csharp")]
    [InlineData("cs", "lang-csharp")]
    [InlineData("c#", "lang-csharp")]
    [InlineData("razor", "lang-razor")]
    [InlineData("cshtml", "lang-razor")]
    [InlineData("xml", "lang-xml")]
    [InlineData("html", "lang-xml")]
    [InlineData("svg", "lang-xml")]
    [InlineData("css", "lang-css")]
    [InlineData("scss", "lang-css")]
    [InlineData("json", "lang-json")]
    [InlineData("javascript", "lang-js")]
    [InlineData("js", "lang-js")]
    [InlineData("typescript", "lang-js")]
    [InlineData("ts", "lang-js")]
    [InlineData("bash", "lang-bash")]
    [InlineData("shell", "lang-bash")]
    [InlineData("sh", "lang-bash")]
    [InlineData("sql", "lang-sql")]
    [InlineData("yaml", "lang-yaml")]
    [InlineData("yml", "lang-yaml")]
    [InlineData("powershell", "lang-powershell")]
    [InlineData("ps1", "lang-powershell")]
    [InlineData("ps", "lang-powershell")]
    [InlineData("markdown", "lang-markdown")]
    [InlineData("md", "lang-markdown")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetLanguageCssClass_ReturnsExpected(string input, string expected)
    {
        var result = SyntaxHighlighter.GetLanguageCssClass(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetLanguageCssClass_UnknownLanguage_ReturnsEmpty()
    {
        var result = SyntaxHighlighter.GetLanguageCssClass("ruby");
        Assert.Equal("", result);
    }

    #endregion

    #region Highlight - additional language coverage

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_Razor_HighlightsTags()
    {
        var result = SyntaxHighlighter.Highlight("<div>@code { }</div>", "razor");
        Assert.Contains("codeblock-tag", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_Xml_HighlightsTags()
    {
        var result = SyntaxHighlighter.Highlight("<root><child/></root>", "xml");
        Assert.Contains("codeblock-tag", result);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Highlight_PowerShell_HighlightsKeywords()
    {
        var result = SyntaxHighlighter.Highlight("if ($true) { Write-Output 'yes' }", "powershell");
        Assert.Contains("codeblock-keyword", result);
    }

    #endregion
}