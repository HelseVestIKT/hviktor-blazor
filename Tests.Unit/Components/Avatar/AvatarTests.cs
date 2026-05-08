using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Avatar;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Avatar")]
public class AvatarTests : HviktorBunitContext
{

    [Fact]
    public void Avatar_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>();

        Assert.NotNull(component.Instance);
        Assert.Null(component.Instance.Initials);
    }

    [Fact]
    public void Avatar_HasDsAvatarClass()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>();

        var avatar = component.Find("span");
        Assert.Contains("ds-avatar", avatar.ClassList);
    }

    [Fact]
    public void Avatar_RendersAsSpanElement()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>();

        var avatar = component.Find("span");
        Assert.Equal("SPAN", avatar.TagName);
    }

    [Fact]
    public void Avatar_DefaultVariantIsCircle()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>();

        var avatar = component.Find("span");
        Assert.Equal("circle", avatar.GetAttribute("data-variant"));
    }

    [Theory]
    [InlineData(Variant.Circle, "circle")]
    [InlineData(Variant.Square, "square")]
    public void Avatar_AppliesAllowedVariants(Variant variant, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>(parameters => parameters
            .AddUnmatched("variant", variant));

        var avatar = component.Find("span");
        Assert.Equal(expectedDataAttribute, avatar.GetAttribute("data-variant"));
    }

    [Fact]
    public void Avatar_HasNoColorAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>();

        var avatar = component.Find("span");
        Assert.Null(avatar.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    public void Avatar_AppliesColors(Color color, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>(parameters => parameters
            .AddUnmatched("color", color));

        var avatar = component.Find("span");
        Assert.Equal(expectedDataAttribute, avatar.GetAttribute("data-color"));
    }

    [Fact]
    public void Avatar_HasNoSizeAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>();

        var avatar = component.Find("span");
        Assert.Null(avatar.GetAttribute("data-size"));
    }

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    public void Avatar_AppliesAllSizes(Size size, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>(parameters => parameters
            .AddUnmatched("size", size));

        var avatar = component.Find("span");
        Assert.Equal(expectedDataAttribute, avatar.GetAttribute("data-size"));
    }

    [Fact]
    public void Avatar_AppliesInitials()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>(parameters => parameters
            .Add(p => p.Initials, "JD"));

        var avatar = component.Find("span");
        Assert.Equal("JD", avatar.GetAttribute("data-initials"));
    }

    [Fact]
    public void Avatar_HasNoInitialsAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>();

        var avatar = component.Find("span");
        Assert.Null(avatar.GetAttribute("data-initials"));
    }

    [Fact]
    public void Avatar_RendersChildContent()
    {
        const string content = "<img src='avatar.png' alt='User' />";
        var component = Render<Hviktor.Components.Avatar.Avatar>(parameters => parameters
            .AddChildContent(content));

        var avatar = component.Find("span");
        Assert.Contains("img", avatar.InnerHtml);
    }

    [Fact]
    public void Avatar_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var avatar = component.Find("span");
        Assert.Contains("my-custom-class", avatar.ClassList);
        Assert.Contains("ds-avatar", avatar.ClassList);
    }

    [Fact]
    public void Avatar_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>(parameters => parameters
            .AddUnmatched("aria-label", "User avatar")
            .AddUnmatched("title", "John Doe"));

        var avatar = component.Find("span");
        Assert.Equal("User avatar", avatar.GetAttribute("aria-label"));
        Assert.Equal("John Doe", avatar.GetAttribute("title"));
    }

    [Fact]
    public void Avatar_AllPropertiesCanBeCombined()
    {
        var component = Render<Hviktor.Components.Avatar.Avatar>(parameters => parameters
            .AddUnmatched("variant", Variant.Square)
            .AddUnmatched("color", Color.Accent)
            .AddUnmatched("size", Size.Large)
            .Add(p => p.Initials, "AB"));

        var avatar = component.Find("span");
        Assert.Equal("square", avatar.GetAttribute("data-variant"));
        Assert.Equal("accent", avatar.GetAttribute("data-color"));
        Assert.Equal("lg", avatar.GetAttribute("data-size"));
        Assert.Equal("AB", avatar.GetAttribute("data-initials"));
    }
}