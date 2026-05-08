using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Table;

/// <summary>
/// WCAG accessibility tests for the Table component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class TableComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Tables)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Table_Basic_WcagCompliant()
    {
        await AssertAllWcagLevelsAsync("/table/basic", "basic");
    }

    [Theory]
    [ClassData<TestData.ZebraVariant>]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Tables)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Table_Zebra_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/table/zebra", testId);
    }

    [Theory]
    [ClassData<TestData.BorderVariant>]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Tables)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Table_Border_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/table/border", testId);
    }

    [Theory]
    [ClassData<TestData.HoverVariant>]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Tables)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Table_Hover_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/table/hover", testId);
    }

    [Theory]
    [ClassData<TestData.StickyHeaderVariant>]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Tables)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Table_StickyHeader_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/table/sticky-header", testId);
    }

    [Theory]
    [ClassData<TestData.SortingVariant>]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Tables)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Table_Sorting_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/table/sorting", testId);
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Tables)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Table_Caption_WcagCompliant()
    {
        await AssertAllWcagLevelsAsync("/table/caption", "with-caption");
    }

    [Theory]
    [ClassData<TestData.ScopeVariant>]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Tables)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Table_Scope_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/table/scope", testId);
    }

    [Fact]
    [Trait(Traits.Component, "Table")]
    [Trait(Traits.Category, Categories.Tables)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Table_EmptyCells_WcagCompliant()
    {
        await AssertAllWcagLevelsAsync("/table/empty-cells", "empty-cells");
    }
}