namespace Tests.Playwright.Compliance.Components.ErrorSummary;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic");
            Add("single-error");
        }
    }

    public sealed class Heading : TheoryData<string>
    {
        public Heading()
        {
            Add("heading-level-2");
            Add("heading-level-3");
            Add("heading-level-4");
        }
    }

    public sealed class Links : TheoryData<string>
    {
        public Links()
        {
            Add("multiple-links");
            Add("descriptive-links");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("focus-test");
            Add("labelledby");
        }
    }
}
