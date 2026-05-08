namespace Tests.Playwright.Compliance.Components.Table;

public static class TestData
{
    public sealed class ZebraVariant : TheoryData<string>
    {
        public ZebraVariant()
        {
            Add("no-zebra");
            Add("zebra");
        }
    }

    public sealed class BorderVariant : TheoryData<string>
    {
        public BorderVariant()
        {
            Add("no-border");
            Add("border");
        }
    }

    public sealed class HoverVariant : TheoryData<string>
    {
        public HoverVariant()
        {
            Add("no-hover");
            Add("hover");
        }
    }

    public sealed class StickyHeaderVariant : TheoryData<string>
    {
        public StickyHeaderVariant()
        {
            Add("no-sticky-header");
            Add("sticky-header");
        }
    }

    public sealed class SortingVariant : TheoryData<string>
    {
        public SortingVariant()
        {
            Add("sortable");
            Add("ascending");
            Add("descending");
        }
    }

    public sealed class ScopeVariant : TheoryData<string>
    {
        public ScopeVariant()
        {
            Add("col-scope");
            Add("row-scope");
        }
    }
}