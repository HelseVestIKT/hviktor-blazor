namespace Tests.Playwright.Compliance.Components.List;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("unordered-basic");
            Add("ordered-basic");
        }
    }

    public sealed class Nested : TheoryData<string>
    {
        public Nested()
        {
            Add("nested-unordered");
            Add("nested-ordered");
            Add("nested-mixed");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("navigation-list");
            Add("interactive-list");
            Add("instructions-list");
        }
    }
}