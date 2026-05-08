using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Components.Link;

namespace Tests.Unit.Components.Link;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "ActionLink")]
public class ActionLinkTests : HviktorBunitContext
{

    #region Rendering

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ActionLink_RendersWithDefaultValues()
    {
        var component = Render<ActionLink>();
        Assert.NotNull(component.Instance);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ActionLink_RendersAsAnchorElement()
    {
        var component = Render<ActionLink>();
        var link = component.Find("a");
        Assert.Equal("A", link.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ActionLink_HasDsLinkClass()
    {
        var component = Render<ActionLink>();

        var link = component.Find("a");
        Assert.Contains("ds-link", link.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ActionLink_HasRoleButton()
    {
        var component = Render<ActionLink>();

        var link = component.Find("a");
        Assert.Equal("button", link.GetAttribute("role"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ActionLink_HasGeneratedId()
    {
        var component = Render<ActionLink>();

        var link = component.Find("a");
        Assert.NotNull(link.Id);
        Assert.NotEmpty(link.Id);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ActionLink_RendersChildContent()
    {
        const string content = "Do something";
        var component = Render<ActionLink>(p => p
            .AddChildContent(content));

        var link = component.Find("a");
        Assert.Contains(content, link.InnerHtml);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ActionLink_RendersComplexChildContent()
    {
        var component = Render<ActionLink>(p => p
            .AddChildContent("<span>Action</span>"));

        var link = component.Find("a");
        Assert.Contains("<span>Action</span>", link.InnerHtml);
    }

    #endregion

    #region Attributes

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void ActionLink_AppliesAdditionalAttributes()
    {
        var component = Render<ActionLink>(p => p
            .AddUnmatched("data-testid", "action-link-test")
            .AddUnmatched("title", "Action title"));

        var link = component.Find("a");
        Assert.Equal("action-link-test", link.GetAttribute("data-testid"));
        Assert.Equal("Action title", link.GetAttribute("title"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void ActionLink_AppliesCustomCssClass()
    {
        var component = Render<ActionLink>(p => p
            .AddUnmatched("class", "my-action-class"));

        var link = component.Find("a");
        Assert.Contains("my-action-class", link.ClassList);
        Assert.Contains("ds-link", link.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void ActionLink_AppliesAriaLabel()
    {
        const string ariaLabel = "Perform action";
        var component = Render<ActionLink>(p => p
            .AddUnmatched("aria-label", ariaLabel));

        var link = component.Find("a");
        Assert.Equal(ariaLabel, link.GetAttribute("aria-label"));
    }

    #endregion

    #region Size

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void ActionLink_AppliesAllSizes(Size size, string expected)
    {
        var component = Render<ActionLink>(p => p
            .AddUnmatched("size", size));

        var link = component.Find("a");
        Assert.Equal(expected, link.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void ActionLink_HasNoSizeWhenNotProvided()
    {
        var component = Render<ActionLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("data-size"));
    }

    #endregion

    #region Color

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void ActionLink_AppliesAllColors(Color color, string expected)
    {
        var component = Render<ActionLink>(p => p
            .AddUnmatched("color", color));

        var link = component.Find("a");
        Assert.Equal(expected, link.GetAttribute("data-color"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void ActionLink_HasNoColorWhenNotProvided()
    {
        var component = Render<ActionLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("data-color"));
    }

    #endregion

    #region Combined

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ActionLink_CombinesAllParameters()
    {
        var component = Render<ActionLink>(p => p
            .AddUnmatched("size", Size.Large)
            .AddUnmatched("color", Color.Neutral)
            .AddUnmatched("aria-label", "Do action")
            .AddChildContent("Action text"));

        var link = component.Find("a");
        Assert.Equal("button", link.GetAttribute("role"));
        Assert.Equal("lg", link.GetAttribute("data-size"));
        Assert.Equal("neutral", link.GetAttribute("data-color"));
        Assert.Equal("Do action", link.GetAttribute("aria-label"));
        Assert.Contains("Action text", link.InnerHtml);
    }

    #endregion
}