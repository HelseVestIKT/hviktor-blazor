namespace Tests.Playwright.Compliance.Components.Input;

public static class TestData
{
    /// <summary>
    /// Visible input types that can be tested with axe-core accessibility tests.
    /// Note: InputType.Hidden is excluded as hidden inputs are not visible by design.
    /// </summary>
    private static readonly Hviktor.Abstractions.Enums.Attributes.InputType[] VisibleInputTypes =
    [
        Hviktor.Abstractions.Enums.Attributes.InputType.Number,
        Hviktor.Abstractions.Enums.Attributes.InputType.Color,
        Hviktor.Abstractions.Enums.Attributes.InputType.Checkbox,
        Hviktor.Abstractions.Enums.Attributes.InputType.Date,
        Hviktor.Abstractions.Enums.Attributes.InputType.DateTimeLocal,
        Hviktor.Abstractions.Enums.Attributes.InputType.Email,
        Hviktor.Abstractions.Enums.Attributes.InputType.File,
        Hviktor.Abstractions.Enums.Attributes.InputType.Month,
        Hviktor.Abstractions.Enums.Attributes.InputType.Password,
        Hviktor.Abstractions.Enums.Attributes.InputType.Radio,
        Hviktor.Abstractions.Enums.Attributes.InputType.Search,
        Hviktor.Abstractions.Enums.Attributes.InputType.Tel,
        Hviktor.Abstractions.Enums.Attributes.InputType.Text,
        Hviktor.Abstractions.Enums.Attributes.InputType.Time,
        Hviktor.Abstractions.Enums.Attributes.InputType.Url,
        Hviktor.Abstractions.Enums.Attributes.InputType.Week
    ];

    public sealed class InputTypes : TheoryData<Hviktor.Abstractions.Enums.Attributes.InputType>
    {
        public InputTypes() => AddRange(VisibleInputTypes);
    }

    public sealed class State : TheoryData<string>
    {
        public State()
        {
            Add("state-readonly");
            Add("state-required");
        }
    }

    public sealed class DisabledState : TheoryData<string>
    {
        public DisabledState()
        {
            Add("state-disabled");
        }
    }

    public sealed class Size : TheoryData<string>
    {
        public Size()
        {
            Add("size-small");
            Add("size-medium");
            Add("size-large");
        }
    }

    public sealed class Accessibility : TheoryData<string>
    {
        public Accessibility()
        {
            Add("accessibility-label");
            Add("accessibility-description");
            Add("accessibility-navigation");
            Add("accessibility-error");
        }
    }
}