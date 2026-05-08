namespace Tests.Playwright.Compliance.Components.Divider;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("divider-basic");
            Add("divider-paragraphs");
        }
    }

    public sealed class Sections : TheoryData<string>
    {
        public Sections()
        {
            Add("divider-article");
            Add("divider-group");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("divider-with-headings");
            Add("divider-with-list");
            Add("divider-accessible-sections");
        }
    }
}
