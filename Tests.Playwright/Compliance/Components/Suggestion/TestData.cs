namespace Tests.Playwright.Compliance.Components.Suggestion;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic");
            Add("default-selected");
            Add("many-options");
        }
    }

    public sealed class Features : TheoryData<string>
    {
        public Features()
        {
            Add("filter-enabled");
            Add("creatable");
            Add("multiple");
            Add("form-name");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("keyboard-navigation");
            Add("autocomplete");
            Add("disabled-options");
            Add("readonly");
            Add("clearable");
            Add("empty-state");
        }
    }
}
