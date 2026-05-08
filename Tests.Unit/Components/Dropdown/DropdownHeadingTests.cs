using Bunit;

namespace Tests.Unit.Components.Dropdown;

/// <summary>
/// Tests.Playwright for the Dropdown.Heading component.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Dropdown.Heading")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class DropdownHeadingTests : HviktorBunitContext
{
    public DropdownHeadingTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    #region Basic Rendering Tests.Playwright

    [Fact]
    public void DropdownHeading_RendersHeadingElement()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddChildContent("Menu Title")));

        var heading = component.Find("h2");
        Assert.Equal("H2", heading.TagName);
    }

    [Fact]
    public void DropdownHeading_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddChildContent("Dropdown Menu")));

        var heading = component.Find("h2");
        Assert.Contains("Dropdown Menu", heading.TextContent);
    }

    [Fact]
    public void DropdownHeading_DefaultLevelIs2()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddChildContent("Title")));

        var heading = component.Find("h2");
        Assert.NotNull(heading);
    }

    #endregion

    #region Level Tests.Playwright

    [Fact]
    public void DropdownHeading_Level1_RendersH1()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddUnmatched("level", 1)
                .AddChildContent("Title")));

        var heading = component.Find("h1");
        Assert.Equal("H1", heading.TagName);
    }

    [Fact]
    public void DropdownHeading_Level2_RendersH2()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddUnmatched("level", 2)
                .AddChildContent("Title")));

        var heading = component.Find("h2");
        Assert.Equal("H2", heading.TagName);
    }

    [Fact]
    public void DropdownHeading_Level3_RendersH3()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddUnmatched("level", 3)
                .AddChildContent("Title")));

        var heading = component.Find("h3");
        Assert.Equal("H3", heading.TagName);
    }

    [Fact]
    public void DropdownHeading_Level4_RendersH4()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddUnmatched("level", 4)
                .AddChildContent("Title")));

        var heading = component.Find("h4");
        Assert.Equal("H4", heading.TagName);
    }

    [Fact]
    public void DropdownHeading_Level5_RendersH5()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddUnmatched("level", 5)
                .AddChildContent("Title")));

        var heading = component.Find("h5");
        Assert.Equal("H5", heading.TagName);
    }

    [Fact]
    public void DropdownHeading_Level6_RendersH6()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddUnmatched("level", 6)
                .AddChildContent("Title")));

        var heading = component.Find("h6");
        Assert.Equal("H6", heading.TagName);
    }

    #endregion

    #region Additional Attributes Tests.Playwright

    [Fact]
    public void DropdownHeading_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddUnmatched("data-testid", "heading-test")
                .AddChildContent("Title")));

        var heading = component.Find("h2");
        Assert.Equal("heading-test", heading.GetAttribute("data-testid"));
    }

    [Fact]
    public void DropdownHeading_AppliesCustomCssClass()
    {
        var component = Render<Hviktor.Components.Dropdown.Dropdown>(parameters => parameters
            .Add(p => p.Id, "test-dropdown")
            .AddChildContent<global::Dropdown.Heading>(headingParams => headingParams
                .AddUnmatched("class", "custom-heading")
                .AddChildContent("Title")));

        var heading = component.Find("h2");
        Assert.Contains("custom-heading", heading.ClassList);
    }

    #endregion
}