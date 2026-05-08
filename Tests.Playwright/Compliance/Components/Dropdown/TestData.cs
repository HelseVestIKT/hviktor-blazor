namespace Tests.Playwright.Compliance.Components.Dropdown;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("dropdown-with-context");
            Add("dropdown-without-context");
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

    public sealed class Trigger : TheoryData<string>
    {
        public Trigger()
        {
            Add("trigger-primary");
            Add("trigger-secondary");
            Add("trigger-tertiary");
        }
    }

    public sealed class Keyboard : TheoryData<string>
    {
        public Keyboard()
        {
            Add("keyboard-navigation");
            Add("keyboard-multiple");
        }
    }

    public sealed class Items : TheoryData<string>
    {
        public Items()
        {
            Add("items-multiple");
            Add("items-single");
            Add("items-grouped");
        }
    }
}
