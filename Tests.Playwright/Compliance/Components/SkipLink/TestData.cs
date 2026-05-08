namespace Tests.Playwright.Compliance.Components.SkipLink;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic");
            Add("custom-text");
            Add("child-content");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("keyboard-navigation");
            Add("visibility-test");
            Add("focus-management");
        }
    }
}