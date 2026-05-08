using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Select;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Select")]
public class SelectTests : HviktorBunitContext
{

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void Select_RendersSelectElement()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select"));

        var select = component.Find("select");
        Assert.Equal("SELECT", select.TagName);
    }

    [Fact]
    public void Select_HasDsInputClass()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select"));

        var select = component.Find("select");
        Assert.Contains("ds-input", select.ClassList);
    }

    [Fact]
    public void Select_HasAriaInvalidFalse()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select"));

        var select = component.Find("select");
        Assert.Equal("false", select.GetAttribute("aria-invalid"));
    }

    [Fact]
    public void Select_AcceptsCustomId()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "my-custom-select"));

        var select = component.Find("select");
        Assert.Equal("my-custom-select", select.Id);
    }

    [Fact]
    public void Select_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddChildContent("<option value='test'>Test Option</option>"));

        var select = component.Find("select");
        Assert.Contains("Test Option", select.InnerHtml);
    }

    #endregion

    #region Width Tests.Playwright

    [Fact]
    public void Select_HasDefaultWidthFull()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select"));

        var select = component.Find("select");
        Assert.Equal("full", select.GetAttribute("data-width"));
    }

    [Fact]
    public void Select_AppliesAutoWidth()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("width", Width.Auto));

        var select = component.Find("select");
        Assert.Equal("auto", select.GetAttribute("data-width"));
    }

    #endregion

    #region Size Tests.Playwright

    [Fact]
    public void Select_AppliesSmallSize()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("size", Size.Small));

        var select = component.Find("select");
        Assert.Equal("sm", select.GetAttribute("data-size"));
    }

    [Fact]
    public void Select_AppliesMediumSize()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("size", Size.Medium));

        var select = component.Find("select");
        Assert.Equal("md", select.GetAttribute("data-size"));
    }

    [Fact]
    public void Select_AppliesLargeSize()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("size", Size.Large));

        var select = component.Find("select");
        Assert.Equal("lg", select.GetAttribute("data-size"));
    }

    [Fact]
    public void Select_NoSizeAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select"));

        var select = component.Find("select");
        Assert.Null(select.GetAttribute("data-size"));
    }

    #endregion

    #region ReadOnly Tests.Playwright

    [Fact]
    public void Select_CanBeReadOnly()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("readOnly", true));

        var select = component.Find("select");
        Assert.True(select.HasAttribute("readonly"));
    }

    [Fact]
    public void Select_NotReadOnlyByDefault()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select"));

        var select = component.Find("select");
        Assert.False(select.HasAttribute("readonly"));
    }

    #endregion

    #region Additional Attributes Tests.Playwright

    [Fact]
    public void Select_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("data-testid", "select-test")
            .AddUnmatched("name", "my-select"));

        var select = component.Find("select");
        Assert.Equal("select-test", select.GetAttribute("data-testid"));
        Assert.Equal("my-select", select.GetAttribute("name"));
    }

    [Fact]
    public void Select_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("class", "custom-select"));

        var select = component.Find("select");
        Assert.Contains("custom-select", select.ClassList);
        Assert.Contains("ds-input", select.ClassList);
    }

    [Fact]
    public void Select_CanBeDisabled()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("disabled", true));

        var select = component.Find("select");
        Assert.True(select.HasAttribute("disabled"));
    }

    [Fact]
    public void Select_CanBeRequired()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("required", true));

        var select = component.Find("select");
        Assert.True(select.HasAttribute("required"));
    }

    [Fact]
    public void Select_AppliesAriaLabel()
    {
        var component = Render<Hviktor.Components.Select.Select>(parameters => parameters
            .AddUnmatched("id", "test-select")
            .AddUnmatched("aria-label", "Select an option"));

        var select = component.Find("select");
        Assert.Equal("Select an option", select.GetAttribute("aria-label"));
    }

    #endregion
}