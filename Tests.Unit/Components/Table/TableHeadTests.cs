using Bunit;
using Table;

namespace Tests.Unit.Components.Table;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Component, "Table.Head")]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Nested)]
public class TableHeadTests : HviktorBunitContext
{

    [Fact]
    public void TableHead_RendersTheadElement()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>());

        var thead = component.Find("thead");
        Assert.Equal("THEAD", thead.TagName);
    }

    [Fact]
    public void TableHead_RendersChildContent()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(p => p
                .AddChildContent("<tr><th>Header</th></tr>")));

        var thead = component.Find("thead");
        Assert.Contains("Header", thead.InnerHtml);
    }

    [Fact]
    public void TableHead_AppliesAdditionalAttributes()
    {
        var component = Render<Hviktor.Components.Table.Table>(parameters => parameters
            .AddChildContent<Head>(p => p
                .AddUnmatched("data-testid", "thead-test")));

        var thead = component.Find("thead");
        Assert.Equal("thead-test", thead.GetAttribute("data-testid"));
    }
}