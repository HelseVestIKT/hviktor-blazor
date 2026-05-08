using Bunit;

namespace Tests.Unit.Components.Divider;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Divider")]
public class DividerTests : HviktorBunitContext
{

    [Fact]
    public void Divider_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Divider.Divider>();

        Assert.NotNull(component.Instance);
    }

    [Fact]
    public void Divider_HasDsDividerClass()
    {
        var component = Render<Hviktor.Components.Divider.Divider>();

        var hr = component.Find("hr");
        Assert.Contains("ds-divider", hr.ClassList);
    }

    [Fact]
    public void Divider_RendersAsHrElement()
    {
        var component = Render<Hviktor.Components.Divider.Divider>();

        var hr = component.Find("hr");
        Assert.Equal("HR", hr.TagName);
    }

    [Fact]
    public void Divider_IsHiddenFromAccessibility()
    {
        var component = Render<Hviktor.Components.Divider.Divider>();

        var hr = component.Find("hr");
        Assert.Equal("true", hr.GetAttribute("aria-hidden"));
    }

    [Fact]
    public void Divider_HasAriaHiddenTrue()
    {
        var component = Render<Hviktor.Components.Divider.Divider>();

        var hr = component.Find("hr");
        Assert.Equal("true", hr.GetAttribute("aria-hidden"));
    }

    [Fact]
    public void Divider_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Divider.Divider>(parameters => parameters
            .AddUnmatched("data-testid", "divider-test")
            .AddUnmatched("id", "my-divider"));

        var hr = component.Find("hr");
        Assert.Equal("divider-test", hr.GetAttribute("data-testid"));
        Assert.Equal("my-divider", hr.GetAttribute("id"));
    }

    [Fact]
    public void Divider_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Divider.Divider>(parameters => parameters
            .AddUnmatched("class", "my-custom-divider"));

        var hr = component.Find("hr");
        Assert.Contains("my-custom-divider", hr.ClassList);
        Assert.Contains("ds-divider", hr.ClassList);
    }

    [Fact]
    public void Divider_AppliesMultipleAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Divider.Divider>(parameters => parameters
            .AddUnmatched("data-testid", "divider-1")
            .AddUnmatched("data-section", "header")
            .AddUnmatched("title", "Section divider"));

        var hr = component.Find("hr");
        Assert.Equal("divider-1", hr.GetAttribute("data-testid"));
        Assert.Equal("header", hr.GetAttribute("data-section"));
        Assert.Equal("Section divider", hr.GetAttribute("title"));
    }

    [Fact]
    public void Divider_IsSelfClosingElement()
    {
        var component = Render<Hviktor.Components.Divider.Divider>();

        var hr = component.Find("hr");
        Assert.Empty(hr.InnerHtml);
    }

    [Fact]
    public void Divider_MaintainsAccessibilityHidingWithAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Divider.Divider>(parameters => parameters
            .AddUnmatched("class", "custom-class")
            .AddUnmatched("data-testid", "test"));

        var hr = component.Find("hr");
        Assert.Equal("true", hr.GetAttribute("aria-hidden"));
    }

    [Fact]
    public void Divider_RendersOnlyOneElement()
    {
        var component = Render<Hviktor.Components.Divider.Divider>();

        var elements = component.FindAll("*");
        Assert.Single(elements);
    }
}
