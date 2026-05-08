namespace Tests.Playwright.Compliance.Components.Radio;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic-group");
            Add("with-description");
            Add("single-radio");
        }
    }

    public sealed class State : TheoryData<string>
    {
        public State()
        {
            Add("state-readonly");
            Add("state-checked");
            Add("state-required");
            Add("state-error");
        }
    }

    public sealed class DisabledState : TheoryData<string>
    {
        public DisabledState()
        {
            Add("state-disabled");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("accessible-group");
            Add("aria-label");
            Add("keyboard-navigation");
            Add("multiple-groups");
            Add("aria-labelledby");
        }
    }
}