namespace Tests.Playwright.Compliance.Components.Tooltip;

public static class TestData
{
    public sealed class BasicData : TheoryData<string>
    {
        public BasicData()
        {
            Add("basic-trigger");
            Add("text-trigger-element");
        }
    }

    public sealed class PlacementData : TheoryData<string>
    {
        public PlacementData()
        {
            Add("top-trigger");
            Add("right-trigger");
            Add("bottom-trigger");
            Add("left-trigger");
        }
    }

    public sealed class AriaData : TheoryData<string>
    {
        public AriaData()
        {
            Add("describedby-trigger");
            Add("labelledby-trigger");
        }
    }

    public sealed class InteractionData : TheoryData<string>
    {
        public InteractionData()
        {
            Add("hover-trigger");
            Add("focus-trigger");
        }
    }
}