namespace Tests.Playwright.Compliance.Components.Tabs;

public static class TestData
{
    public sealed class BasicTabs : TheoryData<string>
    {
        public BasicTabs()
        {
            Add("basic-tabs");
        }
    }

    public sealed class DefaultValueVariant : TheoryData<string>
    {
        public DefaultValueVariant()
        {
            Add("tabs-first-default");
            Add("tabs-second-default");
        }
    }

    public sealed class AriaVariant : TheoryData<string>
    {
        public AriaVariant()
        {
            Add("aria-tabs");
        }
    }

    public sealed class InteractiveVariant : TheoryData<string>
    {
        public InteractiveVariant()
        {
            Add("interactive-tabs");
        }
    }
}