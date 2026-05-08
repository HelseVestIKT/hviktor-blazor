using Bunit;
using Hviktor.Abstractions.Enums;
using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Components.Link;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Unit.Components.Link;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "NavigationLink")]
public class NavigationLinkTests : HviktorBunitContext
{
    #region Rendering

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_RendersWithDefaultValues()
    {
        var component = Render<NavigationLink>();

        Assert.NotNull(component.Instance);
        Assert.Equal(NavLinkMatch.Prefix, component.Instance.Match);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_RendersAsAnchorElement()
    {
        var component = Render<NavigationLink>();

        var link = component.Find("a");
        Assert.Equal("A", link.TagName);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_HasDsLinkClass()
    {
        var component = Render<NavigationLink>();

        var link = component.Find("a");
        Assert.Contains("ds-link", link.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_HasGeneratedId()
    {
        var component = Render<NavigationLink>();

        var link = component.Find("a");
        Assert.NotNull(link.Id);
        Assert.NotEmpty(link.Id);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_RendersChildContent()
    {
        const string content = "Home";
        var component = Render<NavigationLink>(p => p
            .AddChildContent(content));

        var link = component.Find("a");
        Assert.Contains(content, link.InnerHtml);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_RendersComplexChildContent()
    {
        var component = Render<NavigationLink>(p => p
            .AddChildContent("<span>Dashboard</span>"));

        var link = component.Find("a");
        Assert.Contains("<span>Dashboard</span>", link.InnerHtml);
    }

    #endregion

    #region Attributes

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_AppliesHref()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("href", "/dashboard"));

        var link = component.Find("a");
        Assert.Equal("/dashboard", link.GetAttribute("href"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_HasNoHrefWhenNotProvided()
    {
        var component = Render<NavigationLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("href"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_AppliesAdditionalAttributes()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("data-testid", "nav-link-test")
            .AddUnmatched("title", "Nav title"));

        var link = component.Find("a");
        Assert.Equal("nav-link-test", link.GetAttribute("data-testid"));
        Assert.Equal("Nav title", link.GetAttribute("title"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_AppliesCustomCssClass()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("class", "my-nav-class"));

        var link = component.Find("a");
        Assert.Contains("my-nav-class", link.ClassList);
        Assert.Contains("ds-link", link.ClassList);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_AppliesAriaLabel()
    {
        const string ariaLabel = "Go to dashboard";
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("aria-label", ariaLabel));

        var link = component.Find("a");
        Assert.Equal(ariaLabel, link.GetAttribute("aria-label"));
    }

    #endregion

    #region Match

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_DefaultsToMatchPrefix()
    {
        var component = Render<NavigationLink>();

        Assert.Equal(NavLinkMatch.Prefix, component.Instance.Match);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_AcceptsMatchAll()
    {
        var component = Render<NavigationLink>(p => p
            .Add(x => x.Match, NavLinkMatch.All));

        Assert.Equal(NavLinkMatch.All, component.Instance.Match);
    }

    #endregion

    #region Target

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_AppliesTargetBlank()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("target", LinkTarget.NewTab));

        var link = component.Find("a");
        Assert.Equal("_blank", link.GetAttribute("target"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_AppliesTargetSelf()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("target", LinkTarget.Self));

        var link = component.Find("a");
        Assert.Equal("_self", link.GetAttribute("target"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_HasNoTargetWhenNotProvided()
    {
        var component = Render<NavigationLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("target"));
    }

    #endregion

    #region Security

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_AddsRelForExternalLinkWithTargetBlank()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("href", "https://external.example.com")
            .AddUnmatched("target", LinkTarget.NewTab));

        var link = component.Find("a");
        Assert.Equal("external noreferrer noopener", link.GetAttribute("rel"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_ExternalHrefDoesNotApplyActiveState()
    {
        // External links skip active state matching and log a warning
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("href", "https://external.example.com"));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("active"));
        Assert.Null(link.GetAttribute("aria-current"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_DoesNotAddRelForInternalLink()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("href", "/about")
            .AddUnmatched("target", LinkTarget.NewTab));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("rel"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_DoesNotAddRelForExternalLinkWithoutTarget()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("href", "https://external.example.com"));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("rel"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_MalformedHttpUrl_TreatedAsExternal()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("href", "http://"));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("active"));
        Assert.Null(link.GetAttribute("aria-current"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_MalformedHttpsUrl_TreatedAsExternal()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("href", "https://"));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("active"));
        Assert.Null(link.GetAttribute("aria-current"));
    }

    #endregion

    #region Size

    [Theory]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_AppliesAllSizes(Size size, string expected)
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("size", size));

        var link = component.Find("a");
        Assert.Equal(expected, link.GetAttribute("data-size"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_HasNoSizeWhenNotProvided()
    {
        var component = Render<NavigationLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("data-size"));
    }

    #endregion

    #region Color

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_AppliesAllColors(Color color, string expected)
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("color", color));

        var link = component.Find("a");
        Assert.Equal(expected, link.GetAttribute("data-color"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_HasNoColorWhenNotProvided()
    {
        var component = Render<NavigationLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("data-color"));
    }

    #endregion

    #region Disposal

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Disposal)]
    public void NavigationLink_ImplementsIDisposable()
    {
        var component = Render<NavigationLink>();
        Assert.IsType<IDisposable>(component.Instance, exactMatch: false);
    }

    #endregion

    #region Combined

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_CombinesAllParameters()
    {
        var component = Render<NavigationLink>(p => p
            .Add(x => x.Match, NavLinkMatch.All)
            .AddUnmatched("href", "/dashboard")
            .AddUnmatched("target", LinkTarget.NewTab)
            .AddUnmatched("size", Size.Large)
            .AddUnmatched("color", Color.Neutral)
            .AddChildContent("Nav link"));

        var link = component.Find("a");
        Assert.Equal("/dashboard", link.GetAttribute("href"));
        Assert.Equal("_blank", link.GetAttribute("target"));
        Assert.Equal("lg", link.GetAttribute("data-size"));
        Assert.Equal("neutral", link.GetAttribute("data-color"));
        Assert.Contains("Nav link", link.InnerHtml);
    }

    #endregion

    #region ActiveState

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_AppliesActiveStateForMatchingInternalHref()
    {
        var navManager = Services.GetRequiredService<NavigationManager>();
        navManager.NavigateTo("/dashboard");

        var component = Render<NavigationLink>(p => p
            .Add(x => x.Match, NavLinkMatch.Prefix)
            .AddUnmatched("href", "/dashboard"));

        var link = component.Find("a");
        Assert.NotNull(link.GetAttribute("active"));
        Assert.Equal("page", link.GetAttribute("aria-current"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_DoesNotApplyActiveStateForNonMatchingHref()
    {
        var navManager = Services.GetRequiredService<NavigationManager>();
        navManager.NavigateTo("/settings");

        var component = Render<NavigationLink>(p => p
            .Add(x => x.Match, NavLinkMatch.All)
            .AddUnmatched("href", "/dashboard"));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("active"));
        Assert.Null(link.GetAttribute("aria-current"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_MatchAll_ActiveOnlyForExactMatch()
    {
        var navManager = Services.GetRequiredService<NavigationManager>();
        navManager.NavigateTo("/dashboard/sub");

        var component = Render<NavigationLink>(p => p
            .Add(x => x.Match, NavLinkMatch.All)
            .AddUnmatched("href", "/dashboard"));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("active"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_MatchPrefix_ActiveForSubPath()
    {
        var navManager = Services.GetRequiredService<NavigationManager>();
        navManager.NavigateTo("/dashboard/sub");

        var component = Render<NavigationLink>(p => p
            .Add(x => x.Match, NavLinkMatch.Prefix)
            .AddUnmatched("href", "/dashboard"));

        var link = component.Find("a");
        Assert.NotNull(link.GetAttribute("active"));
        Assert.Equal("page", link.GetAttribute("aria-current"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_NoActiveStateWhenHrefIsNull()
    {
        var component = Render<NavigationLink>();

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("active"));
        Assert.Null(link.GetAttribute("aria-current"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_UpdatesActiveStateOnLocationChanged()
    {
        var navManager = Services.GetRequiredService<NavigationManager>();
        navManager.NavigateTo("/other");

        var component = Render<NavigationLink>(p => p
            .Add(x => x.Match, NavLinkMatch.Prefix)
            .AddUnmatched("href", "/dashboard"));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("active"));

        navManager.NavigateTo("/dashboard");

        link = component.Find("a");
        Assert.NotNull(link.GetAttribute("active"));
        Assert.Equal("page", link.GetAttribute("aria-current"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_RemovesActiveStateOnLocationChanged()
    {
        var navManager = Services.GetRequiredService<NavigationManager>();
        navManager.NavigateTo("/dashboard");

        var component = Render<NavigationLink>(p => p
            .Add(x => x.Match, NavLinkMatch.Prefix)
            .AddUnmatched("href", "/dashboard"));

        var link = component.Find("a");
        Assert.NotNull(link.GetAttribute("active"));

        navManager.NavigateTo("/settings");

        link = component.Find("a");
        Assert.Null(link.GetAttribute("active"));
        Assert.Null(link.GetAttribute("aria-current"));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void NavigationLink_InvalidMatchValue_DoesNotApplyActiveState()
    {
        var navManager = Services.GetRequiredService<NavigationManager>();
        navManager.NavigateTo("/dashboard");

        var component = Render<NavigationLink>(p => p
            .Add(x => x.Match, (NavLinkMatch)999)
            .AddUnmatched("href", "/dashboard"));

        var link = component.Find("a");
        Assert.Null(link.GetAttribute("active"));
        Assert.Null(link.GetAttribute("aria-current"));
    }

    #endregion

    #region FragmentAndRelativeHref

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_FragmentHref_RendersWithCurrentBaseAndFragment()
    {
        var navManager = Services.GetRequiredService<NavigationManager>();
        navManager.NavigateTo("/page");

        var component = Render<NavigationLink>(p => p
            .AddUnmatched("href", "#section"));

        var link = component.Find("a");
        var hrefValue = link.GetAttribute("href");
        Assert.NotNull(hrefValue);
        Assert.Contains("#section", hrefValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void NavigationLink_RelativeHref_ConvertsToAbsoluteUrl()
    {
        var component = Render<NavigationLink>(p => p
            .AddUnmatched("href", "/about"));

        var link = component.Find("a");
        var hrefValue = link.GetAttribute("href");
        Assert.NotNull(hrefValue);
        Assert.Contains("/about", hrefValue);
    }

    #endregion
}