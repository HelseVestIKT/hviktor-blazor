namespace Tests.Playwright.Compliance.Components.Fieldset;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic");
            Add("with-description");
        }
    }

    public sealed class Legend : TheoryData<string>
    {
        public Legend()
        {
            Add("legend-visible");
            Add("legend-sr-only");
        }
    }

    public sealed class Description : TheoryData<string>
    {
        public Description()
        {
            Add("description-default");
            Add("description-long");
            Add("description-short");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("accessibility-legend");
            Add("accessibility-description");
            Add("accessibility-nested");
        }
    }
}