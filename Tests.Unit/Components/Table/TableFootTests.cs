using Bunit;
using Table;

namespace Tests.Unit.Components.Table;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Table.Foot")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class TableFootTests : HviktorBunitContext
{

    [Fact]
    public void TableFoot_RendersTfootElement()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Foot>());

        var tfoot = component.Find("tfoot");
        Assert.Equal("TFOOT", tfoot.TagName);
    }

    [Fact]
    public void TableFoot_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Foot>(p => p
                .AddChildContent("<tr><td>Footer</td></tr>")));

        var tfoot = component.Find("tfoot");
        Assert.Contains("Footer", tfoot.InnerHtml);
    }

    [Fact]
    public void TableFoot_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Foot>(p => p
                .AddUnmatched("data-testid", "tfoot-test")));

        var tfoot = component.Find("tfoot");
        Assert.Equal("tfoot-test", tfoot.GetAttribute("data-testid"));
    }
}