namespace Tests.Playwright.Compliance.Components.Select;

public static class TestData
{
    public sealed class Basic : TheoryData<string>
    {
        public Basic()
        {
            Add("basic");
            Add("many-options");
            Add("preselected");
        }
    }

    public sealed class State : TheoryData<string>
    {
        public State()
        {
            Add("state-readonly");
            Add("width-full");
            Add("width-auto");
            Add("state-required");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("accessible-label");
            Add("keyboard-navigation");
            Add("multiple-selects");
            Add("aria-labelledby");
            Add("character-jump");
        }
    }
}
