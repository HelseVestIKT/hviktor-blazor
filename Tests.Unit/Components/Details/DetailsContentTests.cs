using Bunit;

namespace Tests.Unit.Components.Details;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Details.Content")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class DetailsContentTests : HviktorBunitContext
{

    [Fact]
    public void Content_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Content>());

        var divs = component.FindAll("div");
        Assert.NotEmpty(divs);
    }

    [Fact]
    public void Content_RendersAsDivElement()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Content>());

        var content = component.Find("details div");
        Assert.Equal("DIV", content.TagName);
    }

    [Fact]
    public void Content_RendersChildContent()
    {
        const string contentText = "This is the details content";
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Content>(contentParams => contentParams
                .AddChildContent(contentText)));

        var content = component.Find("details div");
        Assert.Contains(contentText, content.InnerHtml);
    }

    [Fact]
    public void Content_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Content>(contentParams => contentParams
                .AddUnmatched("data-testid", "content-test")
                .AddUnmatched("aria-label", "Content section")));

        var content = component.Find("details div");
        Assert.Equal("content-test", content.GetAttribute("data-testid"));
        Assert.Equal("Content section", content.GetAttribute("aria-label"));
    }

    [Fact]
    public void Content_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Details.Details>(parameters => parameters
            .AddChildContent<global::Details.Content>(contentParams => contentParams
                .AddUnmatched("class", "my-content-class")));

        var content = component.Find("details div");
        Assert.Contains("my-content-class", content.ClassList);
    }
}