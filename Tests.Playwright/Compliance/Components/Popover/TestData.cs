namespace Tests.Playwright.Compliance.Components.Popover;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic");
            Add("standalone");
        }
    }

    public sealed class Placement : TheoryData<string>
    {
        public Placement()
        {
            Add("placement-top");
            Add("placement-bottom");
            Add("placement-left");
            Add("placement-right");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("keyboard-navigation");
            Add("multiple-popovers");
            Add("focus-management");
        }
    }
}
