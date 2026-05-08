using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Components.Code;

namespace Tests.Unit.Components.Code;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "CodeBlock")]
public class CodeBlockTests : HviktorBunitContext
{
    public CodeBlockTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Rendering

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_RendersWithDefaultValues()
    {
        var component = Render<CodeBlock>();
        Assert.NotNull(component.Instance);

        var figure = component.Find("figure");
        Assert.Equal("FIGURE", figure.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_HasRoleFigure()
    {
        var component = Render<CodeBlock>();

        var figure = component.Find("figure");
        Assert.Equal("figure", figure.GetAttribute("role"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_HasAriaLabel()
    {
        var component = Render<CodeBlock>();

        var figure = component.Find("figure");
        Assert.NotNull(figure.GetAttribute("aria-label"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_HasCodeblockClass()
    {
        var component = Render<CodeBlock>();

        var figure = component.Find("figure");
        Assert.Contains("codeblock", figure.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_RendersCodeContent()
    {
        var component = Render<CodeBlock>(p => p.Add(x => x.Code, "var x = 1;"));

        var code = component.Find("code");
        Assert.Contains("var", code.InnerHtml);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_RendersPreElement()
    {
        var component = Render<CodeBlock>(p => p.Add(x => x.Code, "hello"));

        var pre = component.Find("pre");
        Assert.Equal("PRE", pre.TagName);
        Assert.Equal("0", pre.GetAttribute("tabindex"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_ShowsHeaderByDefault()
    {
        var component = Render<CodeBlock>();

        var header = component.Find(".codeblock-header");
        Assert.NotNull(header);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_ShowsLanguageDisplay()
    {
        var component = Render<CodeBlock>(p => p.Add(x => x.Language, "csharp"));

        var lang = component.Find(".codeblock-language");
        Assert.Equal("C#", lang.TextContent);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_ShowsFilename()
    {
        var component = Render<CodeBlock>(p => p.Add(x => x.Filename, "Button.razor.cs"));

        var filename = component.Find(".codeblock-filename");
        Assert.Equal("Button.razor.cs", filename.TextContent);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_ShowsCopyButtonByDefault()
    {
        var component = Render<CodeBlock>();

        var copyBtn = component.Find(".codeblock-copy");
        Assert.NotNull(copyBtn);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_HidesCopyButtonWhenCopyableFalse()
    {
        var component = Render<CodeBlock>(p => p.Add(x => x.Copyable, false));

        Assert.Throws<Bunit.ElementNotFoundException>(() => component.Find(".codeblock-copy"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_ShowsLineNumbers()
    {
        var component = Render<CodeBlock>(p => p
            .Add(x => x.Code, "line1\nline2\nline3")
            .Add(x => x.LineNumbers, true));

        var lineNumbers = component.Find(".codeblock-line-numbers");
        Assert.NotNull(lineNumbers);
        Assert.Equal("true", lineNumbers.GetAttribute("aria-hidden"));

        var numbers = component.FindAll(".codeblock-line-number");
        Assert.Equal(3, numbers.Count);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_HidesLineNumbersByDefault()
    {
        var component = Render<CodeBlock>(p => p.Add(x => x.Code, "hello"));

        Assert.Throws<Bunit.ElementNotFoundException>(() => component.Find(".codeblock-line-numbers"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_HasScreenReaderLiveRegion()
    {
        var component = Render<CodeBlock>();

        var srOnly = component.Find("[role='status']");
        Assert.Equal("polite", srOnly.GetAttribute("aria-live"));
        Assert.Equal("true", srOnly.GetAttribute("aria-atomic"));
    }

    #endregion

    #region Parameters

    [Theory]
    [InlineData("scroll")]
    [InlineData("wrap")]
    [InlineData("auto")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void CodeBlock_AppliesOverflow(string overflow)
    {
        var component = Render<CodeBlock>(p => p.Add(x => x.Overflow, overflow));

        var figure = component.Find("figure");
        Assert.Contains($"codeblock-overflow-{overflow}", figure.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void CodeBlock_DefaultOverflowIsScroll()
    {
        var component = Render<CodeBlock>();

        var figure = component.Find("figure");
        Assert.Contains("codeblock-overflow-scroll", figure.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void CodeBlock_AppliesColorDataAttribute()
    {
        var component = Render<CodeBlock>(p => p.Add(x => x.Color, Color.Neutral));

        var figure = component.Find("figure");
        Assert.Equal("neutral", figure.GetAttribute("data-color"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void CodeBlock_AppliesSizeDataAttribute()
    {
        var component = Render<CodeBlock>(p => p.Add(x => x.Size, Size.Small));

        var figure = component.Find("figure");
        Assert.Equal("sm", figure.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void CodeBlock_HasNoSizeWhenNull()
    {
        var component = Render<CodeBlock>();

        var figure = component.Find("figure");
        Assert.Null(figure.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void CodeBlock_NotCopyableAddsClass()
    {
        var component = Render<CodeBlock>(p => p.Add(x => x.Copyable, false));

        var figure = component.Find("figure");
        Assert.Contains("codeblock-not-copyable", figure.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void CodeBlock_NoHighlightRendersPlainText()
    {
        var component = Render<CodeBlock>(p => p
            .Add(x => x.Code, "var x = 1;")
            .Add(x => x.NoHighlight, true));

        var code = component.Find("code");
        // Should not contain span elements for syntax highlighting
        Assert.DoesNotContain("codeblock-keyword", code.InnerHtml);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void CodeBlock_AppliesAdditionalAttributes()
    {
        var component = Render<CodeBlock>(p => p
            .AddUnmatched("data-testid", "code-test"));

        var figure = component.Find("figure");
        Assert.Equal("code-test", figure.GetAttribute("data-testid"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void CodeBlock_AppliesCustomCssClass()
    {
        var component = Render<CodeBlock>(p => p
            .AddUnmatched("class", "my-code-class"));

        var figure = component.Find("figure");
        Assert.Contains("my-code-class", figure.ClassList);
        Assert.Contains("codeblock", figure.ClassList);
    }

    #endregion

    #region Indentation Normalization

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_NormalizesIndentation()
    {
        var code = "    line1\n    line2";
        var component = Render<CodeBlock>(p => p
            .Add(x => x.Code, code)
            .Add(x => x.NoHighlight, true));

        var codeEl = component.Find("code");
        Assert.Contains("line1", codeEl.InnerHtml);
        // Should have removed leading indentation
        Assert.DoesNotContain("    line1", codeEl.TextContent);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void CodeBlock_TrimsLeadingAndTrailingEmptyLines()
    {
        var code = "\n\n  hello\n\n";
        var component = Render<CodeBlock>(p => p
            .Add(x => x.Code, code)
            .Add(x => x.NoHighlight, true));

        var codeEl = component.Find("code");
        Assert.Equal("hello", codeEl.TextContent);
    }

    #endregion
}