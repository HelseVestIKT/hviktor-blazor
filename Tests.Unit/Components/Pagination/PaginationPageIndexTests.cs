using Bunit;
using Hviktor.Abstractions.Enums.Attributes;
using Microsoft.AspNetCore.Components;
using PaginationComponent = Hviktor.Components.Pagination.Pagination;
using PaginationList = Pagination.List;
using PaginationItem = Pagination.Item;
using PaginationButton = Pagination.Button;

namespace Tests.Unit.Components.Pagination;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Pagination")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class PaginationPageIndexTests : HviktorBunitContext
{

    #region Pagination Container Tests.Playwright

    [Fact]
    public void Pagination_RendersAsNavElement()
    {
        var component = Render<PaginationComponent>();

        var nav = component.Find("nav");
        Assert.Equal("NAV", nav.TagName);
    }

    [Fact]
    public void Pagination_HasDsPaginationClass()
    {
        var component = Render<PaginationComponent>();

        var nav = component.Find("nav");
        Assert.Contains("ds-pagination", nav.ClassList);
    }

    [Fact]
    public void Pagination_HasAriaLabel()
    {
        var component = Render<PaginationComponent>();

        var nav = component.Find("nav");
        Assert.NotNull(nav.GetAttribute("aria-label"));
    }

    [Fact]
    public void Pagination_RendersChildContent()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent("<span>Test Content</span>"));

        var nav = component.Find("nav");
        Assert.Contains("Test Content", nav.InnerHtml);
    }

    #endregion

    #region Pagination.List Tests.Playwright

    [Fact]
    public void PaginationList_RendersAsUlElement()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>());

        var ul = component.Find("ul");
        Assert.Equal("UL", ul.TagName);
    }

    [Fact]
    public void PaginationList_HasNoListClass()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>());

        var ul = component.Find("ul");
        Assert.Empty(ul.ClassList);
    }

    [Fact]
    public void PaginationList_RendersChildContent()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent("<li>Item</li>")));

        var ul = component.Find("ul");
        Assert.Contains("Item", ul.InnerHtml);
    }

    #endregion

    #region Pagination.Item Tests.Playwright

    [Fact]
    public void PaginationItem_RendersAsLiElement()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>()));

        var li = component.Find("li");
        Assert.Equal("LI", li.TagName);
    }

    [Fact]
    public void PaginationItem_HasNoItemClass()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>()));

        var li = component.Find("li");
        Assert.Empty(li.ClassList);
    }

    [Fact]
    public void PaginationItem_WithoutChildContent_RendersEmptyLi()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>()));

        var li = component.Find("li");
        Assert.Empty(li.InnerHtml.Trim());
    }

    [Fact]
    public void PaginationItem_WithChildContent_RendersContent()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>(itemParams => itemParams
                    .AddChildContent("Page 1"))));

        var li = component.Find("li");
        Assert.Contains("Page 1", li.InnerHtml);
    }

    #endregion

    #region Pagination.Button Tests.Playwright

    [Fact]
    public void PaginationButton_RendersAsButtonElement()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>(itemParams => itemParams
                    .AddChildContent<PaginationButton>(buttonParams => buttonParams
                        .AddChildContent("1")))));

        var button = component.Find("button");
        Assert.Equal("BUTTON", button.TagName);
    }

    [Fact]
    public void PaginationButton_HasDsPaginationButtonClass()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>(itemParams => itemParams
                    .AddChildContent<PaginationButton>(buttonParams => buttonParams
                        .AddChildContent("1")))));

        var button = component.Find("button");
        Assert.Contains("ds-button", button.ClassList);
    }

    [Fact]
    public void PaginationButton_DefaultVariantIsTertiary()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>(itemParams => itemParams
                    .AddChildContent<PaginationButton>(buttonParams => buttonParams
                        .AddChildContent("1")))));

        var button = component.Find("button");
        Assert.Equal("tertiary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void PaginationButton_AppliesPrimaryVariant()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>(itemParams => itemParams
                    .AddChildContent<PaginationButton>(buttonParams => buttonParams
                        .AddUnmatched("variant", Variant.Primary)
                        .AddChildContent("1")))));

        var button = component.Find("button");
        Assert.Equal("primary", button.GetAttribute("data-variant"));
    }

    [Fact]
    public void PaginationButton_CanBeDisabled()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>(itemParams => itemParams
                    .AddChildContent<PaginationButton>(buttonParams => buttonParams
                        .AddUnmatched("disabled", "true")
                        .AddChildContent("Previous")))));

        var button = component.Find("button");
        Assert.True(button.HasAttribute("disabled"));
    }

    [Fact]
    public void PaginationButton_TriggersOnClickCallback()
    {
        var clicked = false;
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>(itemParams => itemParams
                    .AddChildContent<PaginationButton>(buttonParams => buttonParams
                        .AddUnmatched("onclick", EventCallback.Factory.Create(this, () => clicked = true))
                        .AddChildContent("1")))));

        var button = component.Find("button");
        button.Click();

        Assert.True(clicked);
    }

    [Fact]
    public void PaginationButton_RendersChildContent()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>(itemParams => itemParams
                    .AddChildContent<PaginationButton>(buttonParams => buttonParams
                        .AddChildContent("Next")))));

        var button = component.Find("button");
        Assert.Equal("Next", button.TextContent.Trim());
    }

    #endregion

    #region Full Pagination Structure Tests.Playwright

    [Fact]
    public void Pagination_FullStructure_RendersCorrectly()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<PaginationList>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(listBuilder =>
                {
                    listBuilder.OpenComponent<PaginationItem>(0);
                    listBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "Disabled", true);
                        itemBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Previous")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(2);
                    listBuilder.AddAttribute(3, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "Variant", Variant.Primary);
                        itemBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "1")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(4);
                    listBuilder.AddAttribute(5, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "2")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(6);
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(7);
                    listBuilder.AddAttribute(8, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "10")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(9);
                    listBuilder.AddAttribute(10, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "Next")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var nav = component.Find("nav");
        var ul = component.Find("ul");
        var items = component.FindAll("li");
        var buttons = component.FindAll("button");

        Assert.NotNull(nav);
        Assert.NotNull(ul);
        Assert.Equal(6, items.Count);
        Assert.Equal(5, buttons.Count);
    }

    [Fact]
    public void Pagination_WithMultiplePages_RendersAllPageButtons()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<PaginationList>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(listBuilder =>
                {
                    listBuilder.OpenComponent<PaginationItem>(0);
                    listBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "1")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(2);
                    listBuilder.AddAttribute(3, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "2")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(4);
                    listBuilder.AddAttribute(5, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "3")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(6);
                    listBuilder.AddAttribute(7, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "4")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(8);
                    listBuilder.AddAttribute(9, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(b => b.AddContent(0, "5")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var buttons = component.FindAll("button");
        var pageNumbers = buttons
            .Where(b => int.TryParse(b.TextContent.Trim(), out _))
            .Select(b => int.Parse(b.TextContent.Trim()))
            .ToList();

        Assert.Equal(5, pageNumbers.Count);
        Assert.Contains(1, pageNumbers);
        Assert.Contains(2, pageNumbers);
        Assert.Contains(3, pageNumbers);
        Assert.Contains(4, pageNumbers);
        Assert.Contains(5, pageNumbers);
    }

    [Fact]
    public void Pagination_CurrentPage_HasPrimaryVariant()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent(builder =>
            {
                builder.OpenComponent<PaginationList>(0);
                builder.AddAttribute(1, "ChildContent", (RenderFragment)(listBuilder =>
                {
                    listBuilder.OpenComponent<PaginationItem>(0);
                    listBuilder.AddAttribute(1, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "Variant", Variant.Tertiary);
                        itemBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "1")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(2);
                    listBuilder.AddAttribute(3, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "Variant", Variant.Tertiary);
                        itemBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "2")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(4);
                    listBuilder.AddAttribute(5, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "Variant", Variant.Primary);
                        itemBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "3")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(6);
                    listBuilder.AddAttribute(7, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "Variant", Variant.Tertiary);
                        itemBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "4")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();

                    listBuilder.OpenComponent<PaginationItem>(8);
                    listBuilder.AddAttribute(9, "ChildContent", (RenderFragment)(itemBuilder =>
                    {
                        itemBuilder.OpenComponent<PaginationButton>(0);
                        itemBuilder.AddAttribute(1, "Variant", Variant.Tertiary);
                        itemBuilder.AddAttribute(2, "ChildContent", (RenderFragment)(b => b.AddContent(0, "5")));
                        itemBuilder.CloseComponent();
                    }));
                    listBuilder.CloseComponent();
                }));
                builder.CloseComponent();
            }));

        var buttons = component.FindAll("button");
        var currentPageButton = buttons.First(b => b.TextContent.Trim() == "3");

        Assert.Equal("primary", currentPageButton.GetAttribute("data-variant"));
    }

    #endregion

    #region Accessibility Tests.Playwright

    [Fact]
    public void Pagination_AppliesAriaLabelToButtons()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>(itemParams => itemParams
                    .AddChildContent<PaginationButton>(buttonParams => buttonParams
                        .AddUnmatched("aria-label", "Go to page 1")
                        .AddChildContent("1")))));

        var button = component.Find("button");
        Assert.Equal("Go to page 1", button.GetAttribute("aria-label"));
    }

    [Fact]
    public void Pagination_AppliesAriaCurrentToCurrentPage()
    {
        var component = Render<PaginationComponent>(parameters => parameters
            .AddChildContent<PaginationList>(listParams => listParams
                .AddChildContent<PaginationItem>(itemParams => itemParams
                    .AddChildContent<PaginationButton>(buttonParams => buttonParams
                        .AddUnmatched("variant", Variant.Primary)
                        .AddUnmatched("aria-current", "page")
                        .AddChildContent("1")))));

        var button = component.Find("button");
        Assert.Equal("page", button.GetAttribute("aria-current"));
    }

    #endregion
}