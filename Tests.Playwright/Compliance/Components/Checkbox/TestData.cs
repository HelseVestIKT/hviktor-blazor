namespace Tests.Playwright.Compliance.Components.Checkbox;

public static class TestData
{
    public sealed class State : TheoryData<string>
    {
        public State()
        {
            Add("unchecked");
            Add("checked");
            Add("indeterminate");
        }
    }

    public sealed class DisabledState : TheoryData<string>
    {
        public DisabledState()
        {
            Add("disabled-unchecked");
            Add("disabled-checked");
        }
    }

    public sealed class ReadOnlyState : TheoryData<string>
    {
        public ReadOnlyState()
        {
            Add("readonly-unchecked");
            Add("readonly-checked");
        }
    }

    public sealed class LabelVariant : TheoryData<string>
    {
        public LabelVariant()
        {
            Add("with-label");
            Add("with-aria-label");
            Add("with-aria-labelledby");
        }
    }

    public sealed class DescriptionVariant : TheoryData<string>
    {
        public DescriptionVariant()
        {
            Add("with-description");
            Add("without-description");
        }
    }
}
