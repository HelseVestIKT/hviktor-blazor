namespace Tests.Playwright.Compliance.Components.Pagination;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic");
            Add("many-pages");
        }
    }

    public sealed class Variants : TheoryData<string>
    {
        public Variants()
        {
            Add("variant-primary");
            Add("variant-secondary");
            Add("variant-tertiary");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("aria-label");
            Add("aria-current");
            Add("keyboard-navigation");
        }
    }
}