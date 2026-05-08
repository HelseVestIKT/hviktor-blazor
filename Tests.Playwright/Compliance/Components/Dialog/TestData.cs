namespace Tests.Playwright.Compliance.Components.Dialog;

public static class TestData
{
    public sealed class Placement : TheoryData<string>
    {
        public Placement()
        {
            Add("center");
            Add("left");
            Add("right");
            Add("top");
            Add("bottom");
        }
    }

    public sealed class Modal : TheoryData<string>
    {
        public Modal()
        {
            Add("modal-true");
            Add("modal-false");
        }
    }

    public sealed class Closedby : TheoryData<string>
    {
        public Closedby()
        {
            Add("closedby-none");
            Add("closedby-closerequest");
            Add("closedby-any");
        }
    }

    public sealed class CloseButton : TheoryData<string>
    {
        public CloseButton()
        {
            Add("closebutton-default");
            Add("closebutton-custom");
            Add("closebutton-hidden");
        }
    }

    public sealed class Keyboard : TheoryData<string>
    {
        public Keyboard()
        {
            Add("keyboard-navigation");
            Add("focus-trap");
        }
    }
}