using Bunit;
using MarkdownComponent = Hviktor.Components.Markdown.Markdown;

namespace Tests.Unit.Components.Markdown;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Markdown")]
public class MarkdownTests : HviktorBunitContext
{

    #region Component Rendering

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Markdown_RendersWithDefaultValues()
    {
        var component = Render<MarkdownComponent>(p => p.Add(x => x.Value, "Hello"));
        Assert.NotNull(component.Instance);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Markdown_RendersNothingWhenValueIsEmpty()
    {
        var component = Render<MarkdownComponent>(p => p.Add(x => x.Value, ""));

        Assert.Empty(component.Markup);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Markdown_RendersNothingWhenValueIsNull()
    {
        var component = Render<MarkdownComponent>(p => p.Add(x => x.Value, null!));

        Assert.Empty(component.Markup);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Markdown_HasMarkdownCssClass()
    {
        var component = Render<MarkdownComponent>(p => p.Add(x => x.Value, "Hello"));

        var div = component.Find("div");
        Assert.Contains("markdown", div.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Markdown_RendersParagraph()
    {
        var component = Render<MarkdownComponent>(p => p.Add(x => x.Value, "Hello world"));

        var paragraph = component.Find("p");
        Assert.Contains("Hello world", paragraph.InnerHtml);
        Assert.Contains("ds-paragraph", paragraph.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Markdown_RendersHeading()
    {
        var component = Render<MarkdownComponent>(p => p.Add(x => x.Value, "# My Title"));

        var heading = component.Find("h1");
        Assert.Contains("My Title", heading.InnerHtml);
        Assert.Contains("ds-heading", heading.ClassList);
    }

    [Theory]
    [InlineData("# H1", "h1")]
    [InlineData("## H2", "h2")]
    [InlineData("### H3", "h3")]
    [InlineData("#### H4", "h4")]
    [InlineData("##### H5", "h5")]
    [InlineData("###### H6", "h6")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Markdown_RendersAllHeadingLevels(string markdown, string expectedTag)
    {
        var component = Render<MarkdownComponent>(p => p.Add(x => x.Value, markdown));

        var heading = component.Find(expectedTag);
        Assert.NotNull(heading);
    }

    #endregion

    #region Attributes

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Markdown_AppliesAdditionalAttributes()
    {
        var component = Render<MarkdownComponent>(p => p
            .Add(x => x.Value, "Hello")
            .AddUnmatched("data-testid", "md-test")
            .AddUnmatched("id", "my-markdown"));

        var div = component.Find("div");
        Assert.Equal("md-test", div.GetAttribute("data-testid"));
        Assert.Equal("my-markdown", div.GetAttribute("id"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Markdown_AppliesCustomCssClass()
    {
        var component = Render<MarkdownComponent>(p => p
            .Add(x => x.Value, "Hello")
            .AddUnmatched("class", "custom-class"));

        var div = component.Find("div");
        Assert.Contains("custom-class", div.ClassList);
        Assert.Contains("markdown", div.ClassList);
    }

    #endregion
}