namespace Tests.Playwright.Compliance.Components.Field;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic-textfield");
            Add("field-with-description");
            Add("field-complete");
        }
    }

    public sealed class Position : TheoryData<string>
    {
        public Position()
        {
            Add("position-start");
            Add("position-end");
        }
    }

    public sealed class Counter : TheoryData<string>
    {
        public Counter()
        {
            Add("counter-basic");
            Add("counter-custom-labels");
            Add("counter-small-limit");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("aria-connections");
            Add("counter-accessibility");
            Add("multiple-fields");
        }
    }
}
