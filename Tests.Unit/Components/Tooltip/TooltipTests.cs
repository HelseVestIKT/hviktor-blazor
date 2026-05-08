using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Components.Tooltip;

namespace Tests.Unit.Components.Tooltip;

/// <summary>
/// Unit tests for the <see cref="Tooltip"/> component.
/// The Tooltip renders a hidden <c>&lt;template&gt;</c> marker with tooltip configuration
/// followed by its ChildContent. JavaScript applies the data attributes to the next sibling.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Tooltip")]
public class TooltipTests : HviktorBunitContext
{

    #region Basic Rendering Tests

    [Fact]
    public void Tooltip_RendersMarkerTemplate()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .AddChildContent("<button>Hover me</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.NotNull(marker);
    }

    [Fact]
    public void Tooltip_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .AddChildContent("<button>Trigger element</button>"));

        var button = component.Find("button");
        Assert.Contains("Trigger element", button.TextContent);
    }

    [Fact]
    public void Tooltip_DoesNotRenderWrapperElement()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .AddChildContent("<button>Click me</button>"));

        // No wrapper span should exist
        Assert.Throws<ElementNotFoundException>(() => component.Find("span.ds-tooltip-anchor"));
    }

    #endregion

    #region Content Tests

    [Fact]
    public void Tooltip_ContentParameter_IsStored()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "This is helpful text"));

        Assert.Equal("This is helpful text", component.Instance.Content);
    }

    [Fact]
    public void Tooltip_ContentParameter_AppliedToMarker()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Equal("Tooltip text", marker.GetAttribute("data-tooltip-content"));
    }

    [Fact]
    public void Tooltip_WithNullContent_ContentIsNull()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>();

        Assert.Null(component.Instance.Content);
    }

    [Fact]
    public void Tooltip_WithEmptyContent_MarkerHasEmptyContent()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "")
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Equal("", marker.GetAttribute("data-tooltip-content"));
    }

    #endregion

    #region Placement Tests

    [Fact]
    public void Tooltip_DefaultPlacementIsNull()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text"));

        Assert.Null(component.Instance.Placement);
    }

    [Fact]
    public void Tooltip_DefaultPlacement_MarkerHasTopPlacement()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Equal("top", marker.GetAttribute("data-tooltip-placement"));
    }

    [Fact]
    public void Tooltip_BottomPlacement_MarkerHasBottomPlacement()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .Add(p => p.Placement, Placement.Bottom)
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Equal("bottom", marker.GetAttribute("data-tooltip-placement"));
    }

    [Fact]
    public void Tooltip_LeftPlacement_MarkerHasLeftPlacement()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .Add(p => p.Placement, Placement.Left)
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Equal("left", marker.GetAttribute("data-tooltip-placement"));
    }

    [Fact]
    public void Tooltip_RightPlacement_MarkerHasRightPlacement()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .Add(p => p.Placement, Placement.Right)
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Equal("right", marker.GetAttribute("data-tooltip-placement"));
    }

    #endregion

    #region AutoPlacement Tests

    [Fact]
    public void Tooltip_DefaultAutoPlacementIsTrue()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text"));

        Assert.True(component.Instance.AutoPlacement);
    }

    [Fact]
    public void Tooltip_AutoPlacementTrue_MarkerHasAutoplacementAttribute()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .Add(p => p.AutoPlacement, true)
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Equal("true", marker.GetAttribute("data-tooltip-autoplacement"));
    }

    [Fact]
    public void Tooltip_AutoPlacementFalse_MarkerLacksAutoplacementAttribute()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .Add(p => p.AutoPlacement, false)
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Null(marker.GetAttribute("data-tooltip-autoplacement"));
    }

    #endregion

    #region ARIA Type Tests

    [Fact]
    public void Tooltip_DefaultType_MarkerHasNoTypeAttribute()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Null(marker.GetAttribute("data-tooltip-type"));
    }

    [Fact]
    public void Tooltip_TypeLabelledBy_MarkerHasLabelledbyType()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .Add(p => p.Type, InputType.LabelledBy)
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Equal("labelledby", marker.GetAttribute("data-tooltip-type"));
    }

    [Fact]
    public void Tooltip_TypeDescribedBy_MarkerHasNoTypeAttribute()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .Add(p => p.Type, InputType.DescribedBy)
            .AddChildContent("<button>Hover</button>"));

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Null(marker.GetAttribute("data-tooltip-type"));
    }

    #endregion

    #region Combined Parameters Tests

    [Fact]
    public void Tooltip_WithAllParameters_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Complete tooltip content")
            .Add(p => p.Placement, Placement.Bottom)
            .Add(p => p.AutoPlacement, false)
            .Add(p => p.Type, InputType.LabelledBy)
            .AddChildContent("<button>Hover for info</button>"));

        var marker = component.Find("template[data-tooltip-marker]");

        Assert.Equal("Complete tooltip content", marker.GetAttribute("data-tooltip-content"));
        Assert.Equal("bottom", marker.GetAttribute("data-tooltip-placement"));
        Assert.Null(marker.GetAttribute("data-tooltip-autoplacement"));
        Assert.Equal("labelledby", marker.GetAttribute("data-tooltip-type"));

        var button = component.Find("button");
        Assert.Contains("Hover for info", button.TextContent);
    }

    #endregion

    #region Real-World Usage Tests

    [Fact]
    public void Tooltip_HelpIcon_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Click here for more information")
            .Add(p => p.Placement, Placement.Right)
            .AddChildContent("<span class=\"help-icon\">?</span>"));

        Assert.Equal("Click here for more information", component.Instance.Content);
        Assert.Equal(Placement.Right, component.Instance.Placement);

        var marker = component.Find("template[data-tooltip-marker]");
        Assert.Equal("Click here for more information", marker.GetAttribute("data-tooltip-content"));
        Assert.Equal("right", marker.GetAttribute("data-tooltip-placement"));
    }

    [Fact]
    public void Tooltip_ButtonWithHint_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Save your changes")
            .Add(p => p.Placement, Placement.Top)
            .Add(p => p.AutoPlacement, true)
            .AddChildContent("<button>Save</button>"));

        Assert.Equal("Save your changes", component.Instance.Content);

        var button = component.Find("button");
        Assert.Contains("Save", button.TextContent);
    }

    [Fact]
    public void Tooltip_DisabledAutoPlacement_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Fixed position tooltip")
            .Add(p => p.Placement, Placement.Left)
            .Add(p => p.AutoPlacement, false)
            .AddChildContent("<button>Fixed tooltip</button>"));

        Assert.Equal(Placement.Left, component.Instance.Placement);
        Assert.False(component.Instance.AutoPlacement);
    }

    #endregion

    #region Size Tests

    [Fact]
    public void Tooltip_AcceptsSizeParameter()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .Add(p => p.Size, Size.Small));

        Assert.Equal(Size.Small, component.Instance.Size);
    }

    [Fact]
    public void Tooltip_SizeIsNullByDefault()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text"));

        Assert.Null(component.Instance.Size);
    }

    #endregion

    #region Children (ChildContent) Tests

    [Fact]
    public void Tooltip_StringChildContent_RenderedAfterMarker()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .AddChildContent("Simple text trigger"));

        Assert.Contains("Simple text trigger", component.Markup);
        Assert.NotNull(component.Find("template[data-tooltip-marker]"));
    }

    [Fact]
    public void Tooltip_ElementChildContent_RenderedAfterMarker()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "Tooltip text")
            .AddChildContent("<button>Click me</button>"));

        var button = component.Find("button");
        Assert.Contains("Click me", button.TextContent);
    }

    [Fact]
    public void Tooltip_ComplexChildContent_RendersCorrectly()
    {
        var component = Render<Hviktor.Components.Tooltip.Tooltip>(parameters => parameters
            .Add(p => p.Content, "More information")
            .AddChildContent("<span class=\"icon\"><svg></svg></span>"));

        Assert.Contains("icon", component.Markup);
        Assert.Contains("<svg>", component.Markup);
    }

    #endregion
}