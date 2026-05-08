namespace Tests.Playwright.Compliance.Components.Chip;

/// <summary>
/// Chip-specific test data. Standard colors and variants come from <see cref="SharedTestData"/>.
/// </summary>
public static class TestData
{
    /// <summary>Chip sizes include ExtraSmall in addition to standard sizes.</summary>
    public sealed class Size : TheoryData<Hviktor.Abstractions.Enums.Attributes.Size>
    {
        public Size()
        {
            Add(Hviktor.Abstractions.Enums.Attributes.Size.ExtraSmall);
            Add(Hviktor.Abstractions.Enums.Attributes.Size.Small);
            Add(Hviktor.Abstractions.Enums.Attributes.Size.Medium);
            Add(Hviktor.Abstractions.Enums.Attributes.Size.Large);
        }
    }

    public sealed class ButtonType : TheoryData<string>
    {
        public ButtonType()
        {
            Add("button-default");
            Add("button-with-value");
            Add("button-with-name");
        }
    }

    public sealed class RadioState : TheoryData<string>
    {
        public RadioState()
        {
            Add("radio-unchecked");
            Add("radio-checked");
            Add("radio-default-checked");
        }
    }

    public sealed class CheckboxState : TheoryData<string>
    {
        public CheckboxState()
        {
            Add("checkbox-unchecked");
            Add("checkbox-checked");
            Add("checkbox-default-checked");
        }
    }

    public sealed class RemovableType : TheoryData<string>
    {
        public RemovableType()
        {
            Add("removable-default");
            Add("removable-with-value");
        }
    }
}