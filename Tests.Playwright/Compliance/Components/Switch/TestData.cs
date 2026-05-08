namespace Tests.Playwright.Compliance.Components.Switch;

public static class TestData
{
    public sealed class State : TheoryData<string>
    {
        public State()
        {
            Add("unchecked");
            Add("checked");
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

    public sealed class PositionVariant : TheoryData<string>
    {
        public PositionVariant()
        {
            Add("position-start");
            Add("position-end");
        }
    }
}
