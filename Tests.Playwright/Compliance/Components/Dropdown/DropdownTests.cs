using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Dropdown;

/// <summary>
/// WCAG accessibility tests for the Dropdown component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class DropdownComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Dropdown_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/dropdown/basic", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Heading>]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Structure)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Dropdown_Heading_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/dropdown/heading", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Trigger>]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Dropdown_Trigger_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/dropdown/trigger", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Keyboard>]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Keyboard)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Dropdown_Keyboard_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/dropdown/keyboard", testId, Tags.Wcag2a3);
    }

    [Theory]
    [ClassData<TestData.Items>]
    [Trait(Traits.Component, "Dropdown")]
    [Trait(Traits.Category, Categories.Structure)]
    [Trait(Traits.Category, Categories.Semantics)]  
  [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Dropdown_Items_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/dropdown/items", testId, Tags.Wcag2a3);
    }
}