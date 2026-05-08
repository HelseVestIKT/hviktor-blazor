namespace Tests.Playwright.Compliance.Components.Search;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic");
            Add("no-clear");
            Add("icon-button");
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
            Add("with-label");
            Add("keyboard-navigation");
            Add("with-description");
            Add("multiple-searches");
        }
    }
}
