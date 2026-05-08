using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Alert;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Alert")]
public class AlertTests : HviktorBunitContext
{

    [Fact]
    public void Alert_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Alert.Alert>();
        var alert = component.Find<Hviktor.Components.Alert.Alert>(".ds-alert");
        Assert.Equal("info", alert.GetAttribute("data-color"));
    }

    [Fact]
    public void Alert_HasDsAlertClass()
    {
        var component = Render<Hviktor.Components.Alert.Alert>();

        var alert = component.Find("div");
        Assert.Contains("ds-alert", alert.ClassList);
    }

    [Fact]
    public void Alert_RendersAsDivElement()
    {
        var component = Render<Hviktor.Components.Alert.Alert>();

        var alert = component.Find("div");
        Assert.Equal("DIV", alert.TagName);
    }

    [Fact]
    public void Alert_RendersChildContent()
    {
        const string alertText = "This is an alert message";
        var component = Render<Hviktor.Components.Alert.Alert>(parameters => parameters
            .AddChildContent(alertText));

        var alert = component.Find("div");
        Assert.Contains(alertText, alert.InnerHtml);
    }

    [Fact]
    public void Alert_DefaultColorIsInfo()
    {
        var component = Render<Hviktor.Components.Alert.Alert>();

        var alert = component.Find("div");
        Assert.Equal("info", alert.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData(Color.Info, "info")]
    [InlineData(Color.Success, "success")]
    [InlineData(Color.Warning, "warning")]
    [InlineData(Color.Danger, "danger")]
    public void Alert_AppliesAllColors(Color color, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Alert.Alert>(parameters => parameters
            .AddUnmatched("color", color));

        var alert = component.Find("div");
        Assert.Equal(expectedDataAttribute, alert.GetAttribute("data-color"));
    }

    [Fact]
    public void Alert_HasNoSizeAttributeWhenNull()
    {
        var component = Render<Hviktor.Components.Alert.Alert>();

        var alert = component.Find("div");
        Assert.Null(alert.GetAttribute("data-size"));
    }

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    public void Alert_AppliesAllSizes(Size size, string expectedDataAttribute)
    {
        var component = Render<Hviktor.Components.Alert.Alert>(parameters => parameters
            .AddUnmatched("size", size));

        var alert = component.Find("div");
        Assert.Equal(expectedDataAttribute, alert.GetAttribute("data-size"));
    }

    [Fact]
    public void Alert_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Alert.Alert>(parameters => parameters
            .AddUnmatched("class", "my-custom-class"));

        var alert = component.Find("div");
        Assert.Contains("my-custom-class", alert.ClassList);
        Assert.Contains("ds-alert", alert.ClassList);
    }

    [Fact]
    public void Alert_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Alert.Alert>(parameters => parameters
            .AddUnmatched("aria-label", "Alert message")
            .AddUnmatched("role", "alert"));

        var alert = component.Find("div");
        Assert.Equal("Alert message", alert.GetAttribute("aria-label"));
        Assert.Equal("alert", alert.GetAttribute("role"));
    }

    [Fact]
    public void Alert_RendersComplexChildContent()
    {
        var component = Render<Hviktor.Components.Alert.Alert>(parameters => parameters
            .AddChildContent("<strong>Warning!</strong> Something went wrong."));

        var alert = component.Find("div");
        Assert.Contains("Warning!", alert.InnerHtml);
        Assert.Contains("strong", alert.InnerHtml);
    }

    [Fact]
    public void Alert_AppliesMultipleUnmatchedAttributes()
    {
        var component = Render<Hviktor.Components.Alert.Alert>(parameters => parameters
            .AddUnmatched("data-testid", "alert-box")
            .AddUnmatched("id", "my-alert")
            .AddUnmatched("tabindex", "0"));

        var alert = component.Find("div");
        Assert.Equal("alert-box", alert.GetAttribute("data-testid"));
        Assert.Equal("my-alert", alert.GetAttribute("id"));
        Assert.Equal("0", alert.GetAttribute("tabindex"));
    }

    [Fact]
    public void Alert_ColorAndSizeCanBeCombined()
    {
        var component = Render<Hviktor.Components.Alert.Alert>(parameters => parameters
            .AddUnmatched("color", Color.Warning)
            .AddUnmatched("size", Size.Large));

        var alert = component.Find("div");
        Assert.Equal("warning", alert.GetAttribute("data-color"));
        Assert.Equal("lg", alert.GetAttribute("data-size"));
    }
}