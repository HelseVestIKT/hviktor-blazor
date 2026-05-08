using Hviktor.Abstractions.Enums.Attributes;
using Microsoft.Playwright;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Input;

/// <summary>
/// WCAG accessibility tests for the Input component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class InputComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    /// <summary>
    /// Converts InputType enum to test ID format (e.g., DateTimeLocal -> datetime-local).
    /// </summary>
    private static string GetTestIdForInputType(InputType inputType)
    {
        return inputType switch
        {
            InputType.DateTimeLocal => "input-type-datetime-local",
            _ => $"input-type-{inputType.ToString().ToLowerInvariant()}"
        };
    }

    [Theory]
    [InlineData(InputType.Hidden)]
    [Trait(Traits.Component, "Input")]
    public async Task Input_HiddenType_IsNotVisible(InputType inputType)
    {
        await GoToPageAsync("/input/types");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        var element = await GetElementByTestIdAsync(GetTestIdForInputType(inputType));
        await element.WaitForSelectorStateAsync(WaitForSelectorState.Hidden);

        Assert.True(await element.IsHiddenAsync());
    }

    [Theory]
    [ClassData<TestData.InputTypes>]
    [Trait(Traits.Component, "Input")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Input_AllTypes_WcagCompliant(InputType inputType)
    {
        var testId = GetTestIdForInputType(inputType);
        await AssertAllWcagLevelsAsync("/input/types", testId);
    }

    [Theory]
    [ClassData<TestData.State>]
    [Trait(Traits.Component, "Input")]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Input_State_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/input/state", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Size>]
    [Trait(Traits.Component, "Input")]
    [Trait(Traits.Category, Categories.Structure)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Input_Size_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/input/size", testId);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Input")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Input_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/input/accessibility", testId, Tags.Wcag2a3);
    }
}