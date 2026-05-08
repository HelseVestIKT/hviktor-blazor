using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Switch;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Switch")]
public class SwitchTests : HviktorBunitContext
{
    public SwitchTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void Switch_RendersFieldContainer()
    {
        var component = Render<Hviktor.Components.Switch.Switch>();

        var field = component.Find(".ds-field");
        Assert.NotNull(field);
    }

    [Fact]
    public void Switch_RendersCheckboxElement()
    {
        var component = Render<Hviktor.Components.Switch.Switch>();

        var checkbox = component.Find("input[type='checkbox']");
        Assert.NotNull(checkbox);
    }

    [Fact]
    public void Switch_HasRoleSwitchAttribute()
    {
        var component = Render<Hviktor.Components.Switch.Switch>();

        var checkbox = component.Find("input[type='checkbox']");
        Assert.Equal("switch", checkbox.GetAttribute("role"));
    }

    [Fact]
    public void Switch_HasDefaultId()
    {
        var component = Render<Hviktor.Components.Switch.Switch>();
        var checkbox = component.Find("input[type='checkbox']");
        Assert.NotNull(checkbox.Id);
        Assert.NotEmpty(checkbox.Id);
    }

    [Fact]
    public void Switch_AcceptsCustomId()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("id", "my-switch"));

        var checkbox = component.Find("input[type='checkbox']");
        Assert.Equal("my-switch", checkbox.Id);
    }

    #endregion

    #region Label Tests.Playwright

    [Fact]
    public void Switch_WithLabel_RendersLabelElement()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("label", "Enable notifications"));

        var label = component.Find("label");
        Assert.NotNull(label);
        Assert.Contains("Enable notifications", label.TextContent);
    }

    [Fact]
    public void Switch_WithLabel_LabelHasForAttribute()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("id", "switch-1")
            .AddUnmatched("label", "Dark mode"));

        var label = component.Find("label");
        Assert.Equal("switch-1", label.GetAttribute("for"));
    }

    [Fact]
    public void Switch_WithoutLabel_DoesNotRenderLabelElement()
    {
        var component = Render<Hviktor.Components.Switch.Switch>();

        var labels = component.FindAll("label");
        Assert.Empty(labels);
    }

    #endregion

    #region Description Tests.Playwright

    [Fact]
    public void Switch_WithDescription_RendersDescriptionElement()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("description", "Toggle this to enable feature"));

        var description = component.Find("[data-field='description']");
        Assert.NotNull(description);
        Assert.Contains("Toggle this to enable feature", description.TextContent);
    }

    [Fact]
    public void Switch_WithoutDescription_DoesNotRenderDescriptionElement()
    {
        var component = Render<Hviktor.Components.Switch.Switch>();

        var descriptions = component.FindAll("[data-field='description']");
        Assert.Empty(descriptions);
    }

    #endregion

    #region Position Tests.Playwright

    [Fact]
    public void Switch_DefaultPositionIsStart()
    {
        var component = Render<Hviktor.Components.Switch.Switch>();
        var field = component.Find("div.ds-field");
        Assert.Equal("start", field.GetAttribute("data-position"));
    }

    [Fact]
    public void Switch_AcceptsEndPosition()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("position", Position.End));

        var field = component.Find("div.ds-field");
        Assert.Equal("end", field.GetAttribute("data-position"));
    }

    #endregion

    #region Additional Attributes Tests.Playwright

    [Fact]
    public void Switch_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("data-testid", "switch-test"));

        var checkbox = component.Find("input[type='checkbox']");
        Assert.Equal("switch-test", checkbox.GetAttribute("data-testid"));
    }

    [Fact]
    public void Switch_CanBeDisabled()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("disabled", true));

        var checkbox = component.Find("input[type='checkbox']");
        Assert.True(checkbox.HasAttribute("disabled"));
    }

    [Fact]
    public void Switch_CanBeCheckedByDefault()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("checked", true));

        var checkbox = component.Find("input[type='checkbox']");
        Assert.True(checkbox.HasAttribute("checked"));
    }

    [Fact]
    public void Switch_CanBeRequired()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("required", true));

        var checkbox = component.Find("input[type='checkbox']");
        Assert.True(checkbox.HasAttribute("required"));
    }

    [Fact]
    public void Switch_AppliesAriaLabel()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("aria-label", "Toggle setting"));

        var checkbox = component.Find("input[type='checkbox']");
        Assert.Equal("Toggle setting", checkbox.GetAttribute("aria-label"));
    }

    [Fact]
    public void Switch_AppliesName()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("name", "notification-switch"));

        var checkbox = component.Find("input[type='checkbox']");
        Assert.Equal("notification-switch", checkbox.GetAttribute("name"));
    }

    #endregion

    #region Complete Switch Tests.Playwright

    [Fact]
    public void Switch_WithAllParameters_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("id", "complete-switch")
            .AddUnmatched("label", "Enable feature")
            .AddUnmatched("description", "This enables the feature")
            .AddUnmatched("position", Position.Start));

        var checkbox = component.Find("input[type='checkbox']");
        var label = component.Find("label");
        var description = component.Find("[data-field='description']");

        Assert.Equal("complete-switch", checkbox.Id);
        Assert.Equal("switch", checkbox.GetAttribute("role"));
        Assert.Contains("Enable feature", label.TextContent);
        Assert.Contains("This enables the feature", description.TextContent);
    }

    [Fact]
    public void Switch_WithLabelAndDescription_HasCorrectAssociation()
    {
        var component = Render<Hviktor.Components.Switch.Switch>(parameters => parameters
            .AddUnmatched("id", "assoc-switch")
            .AddUnmatched("label", "Test Label")
            .AddUnmatched("description", "Test Description"));

        var checkbox = component.Find("input[type='checkbox']");
        var label = component.Find("label");

        Assert.Equal("assoc-switch", checkbox.Id);
        Assert.Equal("assoc-switch", label.GetAttribute("for"));
    }

    #endregion
}