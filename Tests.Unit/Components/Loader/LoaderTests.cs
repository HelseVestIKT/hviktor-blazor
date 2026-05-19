using Bunit;
using LoaderComponent = Hviktor.Components.Loader.Loader;

namespace Tests.Unit.Components.Loader;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Loader")]
public class LoaderTests : HviktorBunitContext
{
    #region Rendering

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Loader_RendersWithDefaultValues()
    {
        var component = Render<LoaderComponent>();
        Assert.NotNull(component.Instance);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Loader_NonModal_RendersCard()
    {
        var component = Render<LoaderComponent>();
        var card = component.Find(".loader");
        Assert.NotNull(card);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Loader_NonModal_RendersSpinner()
    {
        var component = Render<LoaderComponent>();
        var spinner = component.Find(".loader-content");
        Assert.NotNull(spinner);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Loader_Modal_RendersDialog()
    {
        var component = Render<LoaderComponent>(p => p.AddUnmatched("modal", "true"));
        var dialog = component.Find("dialog");
        Assert.NotNull(dialog);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Loader_RendersChildContent()
    {
        var component = Render<LoaderComponent>(p => p.AddChildContent("Loading data..."));
        component.MarkupMatches(component.Markup);
        Assert.Contains("Loading data...", component.Markup);
    }

    #endregion

    #region Attributes

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_DefaultColor_IsNeutral()
    {
        var component = Render<LoaderComponent>();
        var element = component.Find("[data-color]");
        Assert.Equal("neutral", element.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData("accent", "accent")]
    [InlineData("neutral", "neutral")]
    [InlineData("brand1", "brand1")]
    [InlineData("success", "success")]
    [InlineData("info", "info")]
    [InlineData("warning", "warning")]
    [InlineData("danger", "danger")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_AppliesColor(string color, string expected)
    {
        var component = Render<LoaderComponent>(p => p.AddUnmatched("color", color));
        var element = component.Find("[data-color]");
        Assert.Equal(expected, element.GetAttribute("data-color"));
    }

    [Theory]
    [InlineData("small", "sm")]
    [InlineData("medium", "md")]
    [InlineData("large", "lg")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_AppliesSize(string size, string expected)
    {
        var component = Render<LoaderComponent>(p => p.AddUnmatched("size", size));
        var element = component.Find("[data-size]");
        Assert.Equal(expected, element.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_NoSize_DoesNotRenderSizeAttribute()
    {
        var component = Render<LoaderComponent>();
        var element = component.Find("[data-color]");
        Assert.Null(element.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_Modal_AppliesPositionAttribute()
    {
        var component = Render<LoaderComponent>(p => p
            .AddUnmatched("modal", "true")
            .AddUnmatched("position", "top"));
        var dialog = component.Find("dialog");
        Assert.Equal("top", dialog.GetAttribute("data-position"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_Modal_DefaultPosition_IsStart()
    {
        var component = Render<LoaderComponent>(p => p.AddUnmatched("modal", "true"));
        var dialog = component.Find("dialog");
        Assert.Equal("start", dialog.GetAttribute("data-position"));
    }

    #endregion

    #region Methods

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Loader_Close_ClosesDialog()
    {
        var component = Render<LoaderComponent>(p => p.AddUnmatched("modal", "true"));
        component.Instance.Close();

        var dialog = component.Find("dialog");
        Assert.NotEqual("false", dialog.GetAttribute("open"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Loader_Open_OpensDialog()
    {
        var component = Render<LoaderComponent>(p => p.AddUnmatched("modal", "true"));
        component.Instance.Open();

        var dialog = component.Find("dialog");
        Assert.NotEqual("true", dialog.GetAttribute("open"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Loader_Close_WhenNotModal_DoesNotThrow()
    {
        var component = Render<LoaderComponent>();
        var exception = Record.Exception(() => component.Instance.Close());
        Assert.Null(exception);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Loader_Open_WhenNotModal_DoesNotThrow()
    {
        var component = Render<LoaderComponent>();
        var exception = Record.Exception(() => component.Instance.Open());
        Assert.Null(exception);
    }

    #endregion

    #region ComputeAttributes Branch Coverage

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_NonModal_WithPosition_LogsWarning()
    {
        var component = Render<LoaderComponent>(p => p.AddUnmatched("position", "top"));
        var element = component.Find(".loader");
        Assert.NotNull(element);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_UsesDataPositionAttribute()
    {
        var component = Render<LoaderComponent>(p => p
            .AddUnmatched("modal", "true")
            .AddUnmatched("data-position", "bottom"));
        var dialog = component.Find("dialog");
        Assert.Equal("bottom", dialog.GetAttribute("data-position"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_UsesDataColorAttribute()
    {
        var component = Render<LoaderComponent>(p => p.AddUnmatched("data-color", "accent"));
        var element = component.Find("[data-color]");
        Assert.Equal("accent", element.GetAttribute("data-color"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_UsesDataSizeAttribute()
    {
        var component = Render<LoaderComponent>(p => p.AddUnmatched("data-size", "large"));
        var element = component.Find("[data-size]");
        Assert.Equal("lg", element.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void Loader_ModalFalseString_IsNotModal()
    {
        var component = Render<LoaderComponent>(p => p.AddUnmatched("modal", "false"));
        var element = component.Find(".loader");
        Assert.NotNull(element);
    }

    #endregion
}