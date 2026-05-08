namespace Tests.Playwright.Compliance.Components.Textarea;

public static class TestData
{
    public sealed class BasicData : TheoryData<string>
    {
        public BasicData()
        {
            Add("basic");
            Add("empty");
        }
    }

    public sealed class StateData : TheoryData<string>
    {
        public StateData()
        {
            Add("disabled");
            Add("readonly");
            Add("required");
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

    public sealed class DimensionsData : TheoryData<string>
    {
        public DimensionsData()
        {
            Add("with-rows");
            Add("with-cols");
            Add("with-both");
        }
    }

    public sealed class AccessibilityData : TheoryData<string>
    {
        public AccessibilityData()
        {
            Add("labeled");
            Add("aria-labeled");
            Add("described");
            Add("maxlength");
        }
    }
}
