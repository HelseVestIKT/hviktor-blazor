namespace Tests.Playwright.Compliance.Components.ToggleGroup;

public static class TestData
{
    public sealed class BasicData : TheoryData<string>
    {
        public BasicData()
        {
            Add("basic");
        }
    }

    public sealed class VariantData : TheoryData<string>
    {
        public VariantData()
        {
            Add("primary");
            Add("secondary");
        }
    }

    public sealed class SizeData : TheoryData<string>
    {
        public SizeData()
        {
            Add("sm");
            Add("md");
            Add("lg");
        }
    }

    public sealed class AriaData : TheoryData<string>
    {
        public AriaData()
        {
            Add("aria-group");
        }
    }
}