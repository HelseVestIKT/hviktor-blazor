using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.AvatarStack;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "AvatarStack")]
public class AvatarStackTests : HviktorBunitContext
{

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();
        Assert.NotNull(component.Instance);

        var avatarStack = component.Find(".ds-avatar-stack");

        var styles = avatarStack.GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-overlap: 50;", styles);
        Assert.Contains("--dsc-avatar-stack-size: var(--ds-size-12);", styles);
        Assert.Contains("--dsc-avatar-stack-gap: 2px;", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_RendersSpanElement()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();

        var element = component.Find(".ds-avatar-stack");
        Assert.Equal("SPAN", element.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_HasDsAvatarStackClass()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();

        var element = component.Find("span");
        Assert.Contains("ds-avatar-stack", element.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_RendersChildContent()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddChildContent("<Avatar>AA</Avatar>"));

        var element = component.Find(".ds-avatar-stack");
        Assert.Contains("avatar", element.InnerHtml);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_WithoutChildContent_RendersEmptySpan()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();

        var element = component.Find(".ds-avatar-stack");
        Assert.Empty(element.TextContent.Trim());
    }

    #region Color

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_NoColorAttributeByDefault()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();

        var element = component.Find(".ds-avatar-stack");
        Assert.Null(element.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    [InlineData(Color.Info, "info")]
    [InlineData(Color.Success, "success")]
    [InlineData(Color.Warning, "warning")]
    [InlineData(Color.Danger, "danger")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_AppliesColor(Color color, string expected)
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("color", color));

        var element = component.Find(".ds-avatar-stack");
        Assert.Equal(expected, element.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_AppliesDataColorAttribute(Color color, string expected)
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("data-color", color));

        var element = component.Find(".ds-avatar-stack");
        Assert.Equal(expected, element.GetAttribute("data-color"));
    }

    #endregion

    #region Size

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_NoSizeAttributeByDefault()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();

        var element = component.Find(".ds-avatar-stack");
        Assert.Null(element.GetAttribute("data-size"));
    }

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_AppliesSize(Size size, string expected)
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("size", size));

        var element = component.Find(".ds-avatar-stack");
        Assert.Equal(expected, element.GetAttribute("data-size"));
    }

    #endregion

    #region Gap

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_DefaultGap_Is2px()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-gap: 2px;", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_AppliesCustomGap()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("gap", "4px"));

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-gap: 4px;", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_AppliesGapWithRemUnit()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("gap", "1rem"));

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-gap: 1rem;", styles);
    }

    #endregion

    #region Avatar Size

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_DefaultAvatarSize_IsDesignToken()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-size: var(--ds-size-12);", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_AppliesCustomAvatarSize()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("avatar-size", "48px"));

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-size: 48px;", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_AppliesAvatarSizeViaCamelCase()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("avatarSize", "3rem"));

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-size: 3rem;", styles);
    }

    #endregion

    #region Overlap

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_DefaultOverlap_Is50()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-overlap: 50;", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_AppliesCustomOverlap()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("overlap", "75"));

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-overlap: 75;", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_OverlapClampsToZero_WhenNegative()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("overlap", "-10"));

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-overlap: 0;", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_OverlapClampsTo100_WhenExceeds100()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("overlap", "150"));

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-overlap: 100;", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_OverlapDefaultsTo50_WhenNonNumeric()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("overlap", "abc"));

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-overlap: 50;", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_OverlapAcceptsZero()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("overlap", "0"));

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-overlap: 0;", styles);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_OverlapAccepts100()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("overlap", "100"));

        var styles = component.Find(".ds-avatar-stack").GetAttribute("style");
        Assert.Contains("--dsc-avatar-stack-overlap: 100;", styles);
    }

    #endregion

    #region Suffix

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_NoSuffixByDefault()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();

        var element = component.Find(".ds-avatar-stack");
        Assert.Null(element.GetAttribute("data-suffix"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_AppliesSuffix()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("suffix", "+5"));

        var element = component.Find(".ds-avatar-stack");
        Assert.Equal("+5", element.GetAttribute("data-suffix"));
    }

    #endregion

    #region Expandable

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_NotExpandableByDefault()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>();

        var element = component.Find(".ds-avatar-stack");
        Assert.Null(element.GetAttribute("data-expandable"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void AvatarStack_AppliesExpandable()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("expandable", "true"));

        var element = component.Find(".ds-avatar-stack");
        Assert.NotNull(element.GetAttribute("data-expandable"));
    }

    #endregion

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void AvatarStack_ForwardsCustomAttributes()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("data-testid", "my-stack"));

        var element = component.Find(".ds-avatar-stack");
        Assert.Equal("my-stack", element.GetAttribute("data-testid"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void AvatarStack_ForwardsAriaLabel()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("aria-label", "Team members"));

        var element = component.Find(".ds-avatar-stack");
        Assert.Equal("Team members", element.GetAttribute("aria-label"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void AvatarStack_ForwardsAdditionalCssClasses()
    {
        var component = Render<Hviktor.Components.AvatarStack.AvatarStack>(p => p
            .AddUnmatched("class", "custom-class"));

        var element = component.Find(".ds-avatar-stack");
        Assert.Contains("custom-class", element.ClassList);
        Assert.Contains("ds-avatar-stack", element.ClassList);
    }
}