using Bunit;

namespace Tests.Unit.Components.RequiredTag;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "RequiredTag")]
public class RequiredTagTests : HviktorBunitContext
{
    #region Required State Tests

    [Fact]
    public void RequiredTag_WithoutRequiredAttribute_RendersOptional()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>();

        var span = component.Find("span");
        Assert.Contains("Optional", span.TextContent);
    }

    [Fact]
    public void RequiredTag_WithoutRequiredAttribute_UsesInfoColor()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>();

        var span = component.Find("span");
        Assert.Equal("info", span.GetAttribute("data-color"));
    }

    [Fact]
    public void RequiredTag_WithRequiredTrue_RendersRequired()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>(parameters => parameters
            .AddUnmatched("required", "true"));

        var span = component.Find("span");
        Assert.Contains("Must be filled out", span.TextContent);
    }

    [Fact]
    public void RequiredTag_WithRequiredTrue_UsesWarningColor()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>(parameters => parameters
            .AddUnmatched("required", "true"));

        var span = component.Find("span");
        Assert.Equal("warning", span.GetAttribute("data-color"));
    }

    [Fact]
    public void RequiredTag_WithRequiredFalse_RendersOptional()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>(parameters => parameters
            .AddUnmatched("required", "false"));

        var span = component.Find("span");
        Assert.Contains("Optional", span.TextContent);
    }

    [Fact]
    public void RequiredTag_WithRequiredFalse_UsesInfoColor()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>(parameters => parameters
            .AddUnmatched("required", "false"));

        var span = component.Find("span");
        Assert.Equal("info", span.GetAttribute("data-color"));
    }

    [Fact]
    public void RequiredTag_WithRequiredAnyOtherValue_TreatsAsRequired()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>(parameters => parameters
            .AddUnmatched("required", "any_value_not_false"));

        var span = component.Find("span");
        Assert.Equal("warning", span.GetAttribute("data-color"));
        Assert.Contains("Must be filled out", span.TextContent);
    }

    #endregion

    #region Mode Tests

    [Fact]
    public void RequiredTag_AllMode_Required_RendersAllRequiredText()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>(parameters => parameters
            .AddUnmatched("required", "true")
            .AddUnmatched("mode", "all"));

        var span = component.Find("span");
        Assert.Contains("All fields must be filled out", span.TextContent);
    }

    [Fact]
    public void RequiredTag_AllMode_Optional_RendersAllOptionalText()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>(parameters => parameters
            .AddUnmatched("mode", "all"));

        var span = component.Find("span");
        Assert.Contains("All fields are optional", span.TextContent);
    }

    [Fact]
    public void RequiredTag_ModeAttribute_IsNotPassedToDOM()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>(parameters => parameters
            .AddUnmatched("mode", "all"));

        var span = component.Find("span");
        Assert.Null(span.GetAttribute("mode"));
    }

    #endregion

    #region Additional Attributes Tests

    [Fact]
    public void RequiredTag_HasDsTagClass()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>();

        var span = component.Find("span");
        Assert.Contains("ds-tag", span.ClassList);
    }

    [Fact]
    public void RequiredTag_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>(parameters => parameters
            .AddUnmatched("data-testid", "required-tag-test")
            .AddUnmatched("id", "my-required-tag"));

        var span = component.Find("span");
        Assert.Equal("required-tag-test", span.GetAttribute("data-testid"));
        Assert.Equal("my-required-tag", span.Id);
    }

    [Fact]
    public void RequiredTag_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.RequiredTag.RequiredTag>(parameters => parameters
            .AddUnmatched("class", "custom-class"));

        var span = component.Find("span");
        Assert.Contains("custom-class", span.ClassList);
        Assert.Contains("ds-tag", span.ClassList);
    }

    #endregion
}