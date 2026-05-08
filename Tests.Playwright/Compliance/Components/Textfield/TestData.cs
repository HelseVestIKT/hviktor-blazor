namespace Tests.Playwright.Compliance.Components.Textfield;

public static class TestData
{
    public sealed class BasicData : TheoryData<string>
    {
        public BasicData()
        {
            Add("basic");
            Add("with-description");
            Add("with-value");
        }
    }

    public sealed class AffixesData : TheoryData<string>
    {
        public AffixesData()
        {
            Add("with-prefix");
            Add("with-suffix");
            Add("with-both");
        }
    }

    public sealed class ErrorData : TheoryData<string>
    {
        public ErrorData()
        {
            Add("with-error");
            Add("with-error-description");
        }
    }

    public sealed class CounterData : TheoryData<string>
    {
        public CounterData()
        {
            Add("counter-under");
            Add("counter-at");
            Add("counter-over");
        }
    }

    public sealed class MultilineData : TheoryData<string>
    {
        public MultilineData()
        {
            Add("multiline");
            Add("multiline-description");
            Add("multiline-error");
        }
    }

    public sealed class TypesData : TheoryData<string>
    {
        public TypesData()
        {
            Add("type-text");
            Add("type-email");
            Add("type-password");
            Add("type-number");
            Add("type-tel");
            Add("type-url");
            Add("type-search");
            Add("type-date");
        }
    }

    public sealed class AccessibilityData : TheoryData<string>
    {
        public AccessibilityData()
        {
            Add("aria-label");
            Add("aria-labelledby");
        }
    }
}