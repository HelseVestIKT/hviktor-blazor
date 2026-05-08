using System.Globalization;
using Bunit;
using Hviktor.Abstractions.Enums.Attributes;

namespace Tests.Unit.Components.Dialog;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Dialog")]
public class DialogTests : HviktorBunitContext
{
    public DialogTests()
    {
        CultureInfo.CurrentCulture = new CultureInfo("nb");
        CultureInfo.CurrentUICulture = new CultureInfo("nb");
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    [Fact]
    public void Dialog_RendersWithDefaultValues()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();
        var closeButton = component.Find("button[data-command='close']");
        Assert.NotNull(closeButton);
        Assert.Equal("Lukk dialogvindu", closeButton.GetAttribute("aria-label"));
    }

    [Fact]
    public void Dialog_HasDsDialogClass()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        var dialog = component.Find("dialog");
        Assert.Contains("ds-dialog", dialog.ClassList);
    }

    [Fact]
    public void Dialog_RendersAsDialogElement()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        var dialog = component.Find("dialog");
        Assert.Equal("DIALOG", dialog.TagName);
    }

    [Fact]
    public void Dialog_HasGeneratedId()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        Assert.NotNull(component.Instance.Id);
        Assert.NotEmpty(component.Instance.Id);
    }

    [Fact]
    public void Dialog_AcceptsCustomId()
    {
        const string customId = "my-custom-dialog";
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .Add(p => p.Id, customId));

        Assert.Equal(customId, component.Instance.Id);
        var dialog = component.Find("dialog");
        Assert.Equal(customId, dialog.Id);
    }

    [Fact]
    public void Dialog_GeneratesUniqueIds()
    {
        var component1 = Render<Hviktor.Components.Dialog.Dialog>();
        var component2 = Render<Hviktor.Components.Dialog.Dialog>();

        Assert.NotEqual(component1.Instance.Id, component2.Instance.Id);
    }

    [Fact]
    public void Dialog_RendersChildContent()
    {
        const string content = "Dialog content";
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddChildContent(content));

        var dialog = component.Find("dialog");
        Assert.Contains(content, dialog.InnerHtml);
    }

    #region Modal Tests.Playwright

    [Fact]
    public void Dialog_DataModalDefaultsToFalse()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        var dialog = component.Find("dialog");
        Assert.Equal("false", dialog.GetAttribute("data-modal"));
    }

    [Fact]
    public void Dialog_HasDataModalTrueWhenModalAttributePresent()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("modal", true));

        var dialog = component.Find("dialog");
        Assert.Equal("true", dialog.GetAttribute("data-modal"));
    }

    [Fact]
    public void Dialog_HasDataModalFalseWhenNoModalAttribute()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        var dialog = component.Find("dialog");
        Assert.Equal("false", dialog.GetAttribute("data-modal"));
    }

    #endregion

    #region CloseButton Tests.Playwright

    [Fact]
    public void Dialog_CloseButtonDefaultsToLukkDialogvindu()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();
        var closeButton = component.Find("button[data-command='close']");
        Assert.NotNull(closeButton);
        Assert.Equal("Lukk dialogvindu", closeButton.GetAttribute("aria-label"));
    }

    [Fact]
    public void Dialog_RendersCloseButtonByDefault()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();
        var closeButton = component.Find("button[data-command='close']");
        Assert.NotNull(closeButton);
    }

    [Fact]
    public void Dialog_DoesNotRenderCloseButtonWhenSetToFalse()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .Add(p => p.CloseButton, "false"));

        Assert.Throws<ElementNotFoundException>(() =>
            component.Find("button[data-command='close']"));
    }

    [Fact]
    public void Dialog_CloseButtonHasAriaLabel()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        var closeButton = component.Find("button[data-command='close']");
        Assert.NotNull(closeButton.GetAttribute("aria-label"));
        Assert.NotEmpty(closeButton.GetAttribute("aria-label")!);
    }

    [Fact]
    public void Dialog_CloseButtonUsesCustomAriaLabel()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .Add(p => p.CloseButton, "Close this window"));

        var closeButton = component.Find("button[data-command='close']");
        Assert.Equal("Close this window", closeButton.GetAttribute("aria-label"));
    }

    [Fact]
    public void Dialog_CloseButtonHasAutofocus()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        var closeButton = component.Find("button[data-command='close']");
        Assert.True(closeButton.HasAttribute("autofocus"));
    }

    [Fact]
    public void Dialog_CloseButtonHasDataIcon()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();
        var closeButton = component.Find("button[data-command='close']");
        Assert.True(closeButton.HasAttribute("data-icon"));
    }

    [Fact]
    public void Dialog_CloseButtonHasNeutralColor()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        var closeButton = component.Find("button[data-command='close']");
        Assert.Equal("neutral", closeButton.GetAttribute("data-color"));
    }

    [Fact]
    public void Dialog_CloseButtonHasTertiaryVariant()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        var closeButton = component.Find("button[data-command='close']");
        Assert.Equal("tertiary", closeButton.GetAttribute("data-variant"));
    }

    [Fact]
    public void Dialog_CloseButtonContainsIcon()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        var closeButton = component.Find("button[data-command='close']");
        var icon = closeButton.QuerySelector("svg");
        Assert.NotNull(icon);
    }

    #endregion

    #region Closedby Tests.Playwright

    [Fact]
    public void Dialog_ClosedbyCanBeSetViaAttribute()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("closedby", "closerequest"));

        var dialog = component.Find("dialog");
        Assert.Equal("closerequest", dialog.GetAttribute("closedby"));
    }

    [Fact]
    public void Dialog_ClosedbyAcceptsNone()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("closedby", "none"));

        var dialog = component.Find("dialog");
        Assert.Equal("none", dialog.GetAttribute("closedby"));
    }

    [Fact]
    public void Dialog_ClosedbyAcceptsAny()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("closedby", "any"));

        var dialog = component.Find("dialog");
        Assert.Equal("any", dialog.GetAttribute("closedby"));
    }

    #endregion

    #region Placement Tests.Playwright

    [Fact]
    public void Dialog_NoPlacementByDefault()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();

        var dialog = component.Find("dialog");
        Assert.Null(dialog.GetAttribute("data-placement"));
    }

    [Fact]
    public void Dialog_PlacementCenter_SetsDataAttribute()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("placement", Placement.Center));

        var dialog = component.Find("dialog");
        Assert.Equal("center", dialog.GetAttribute("data-placement"));
    }

    [Fact]
    public void Dialog_PlacementLeft_SetsDataAttribute()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("placement", Placement.Left));

        var dialog = component.Find("dialog");
        Assert.Equal("left", dialog.GetAttribute("data-placement"));
    }

    [Fact]
    public void Dialog_PlacementRight_SetsDataAttribute()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("placement", Placement.Right));

        var dialog = component.Find("dialog");
        Assert.Equal("right", dialog.GetAttribute("data-placement"));
    }

    [Fact]
    public void Dialog_PlacementTop_SetsDataAttribute()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("placement", Placement.Top));

        var dialog = component.Find("dialog");
        Assert.Equal("top", dialog.GetAttribute("data-placement"));
    }

    [Fact]
    public void Dialog_PlacementBottom_SetsDataAttribute()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("placement", Placement.Bottom));

        var dialog = component.Find("dialog");
        Assert.Equal("bottom", dialog.GetAttribute("data-placement"));
    }

    #endregion

    #region Additional Attributes Tests.Playwright

    [Fact]
    public void Dialog_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("data-testid", "dialog-test")
            .AddUnmatched("aria-labelledby", "dialog-title"));

        var dialog = component.Find("dialog");
        Assert.Equal("dialog-test", dialog.GetAttribute("data-testid"));
        Assert.Equal("dialog-title", dialog.GetAttribute("aria-labelledby"));
    }

    [Fact]
    public void Dialog_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("class", "my-custom-dialog"));

        var dialog = component.Find("dialog");
        Assert.Contains("my-custom-dialog", dialog.ClassList);
        Assert.Contains("ds-dialog", dialog.ClassList);
    }

    [Fact]
    public void Dialog_RendersComplexChildContent()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddChildContent("<h2>Dialog Title</h2><p>Dialog body text</p>"));

        var dialog = component.Find("dialog");
        Assert.Contains("<h2>Dialog Title</h2>", dialog.InnerHtml);
        Assert.Contains("<p>Dialog body text</p>", dialog.InnerHtml);
    }

    #endregion

    #region Combined Parameters Tests.Playwright

    [Fact]
    public void Dialog_CombinesAllParameters()
    {
        const string customId = "full-dialog";
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .Add(p => p.Id, customId)
            .Add(p => p.CloseButton, "Close")
            .AddUnmatched("modal", true)
            .AddUnmatched("closedby", "any")
            .AddUnmatched("placement", Placement.Right)
            .AddChildContent("Full dialog content"));

        Assert.Equal(customId, component.Instance.Id);
        Assert.Equal("Close", component.Instance.CloseButton);

        var dialog = component.Find("dialog");
        Assert.Equal(customId, dialog.Id);
        Assert.Equal("true", dialog.GetAttribute("data-modal"));
        Assert.Equal("any", dialog.GetAttribute("closedby"));
        Assert.Equal("right", dialog.GetAttribute("data-placement"));
        Assert.Contains("Full dialog content", dialog.InnerHtml);
    }

    [Fact]
    public void Dialog_NonModalAndNotClosable()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .Add(p => p.CloseButton, "false"));

        var dialog = component.Find("dialog");
        Assert.Equal("false", dialog.GetAttribute("data-modal"));

        Assert.Throws<ElementNotFoundException>(() =>
            component.Find("button[data-command='close']"));
    }

    [Fact]
    public void Dialog_DrawerFromLeft()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("placement", Placement.Left)
            .AddUnmatched("modal", true));

        var dialog = component.Find("dialog");
        Assert.Equal("left", dialog.GetAttribute("data-placement"));
        Assert.Equal("true", dialog.GetAttribute("data-modal"));
    }

    [Fact]
    public void Dialog_LightDismissEnabled()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>(parameters => parameters
            .AddUnmatched("closedby", "any"));

        var dialog = component.Find("dialog");
        Assert.Equal("any", dialog.GetAttribute("closedby"));
    }

    #endregion

    #region Interface Tests.Playwright

    [Fact]
    public void Dialog_ImplementsIAsyncDisposable()
    {
        var component = Render<Hviktor.Components.Dialog.Dialog>();
        Assert.IsType<IAsyncDisposable>(component.Instance, exactMatch: false);
    }

    #endregion
}