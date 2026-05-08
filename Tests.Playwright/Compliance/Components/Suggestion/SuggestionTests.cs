using Deque.AxeCore.Commons;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Suggestion;

/// <summary>
/// WCAG accessibility tests for the Suggestion component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class SuggestionComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    /// <summary>
    /// Best practices options that exclude the <c>aria-allowed-role</c> rule.
    /// The <c>u-combobox</c> web component adds both <c>list</c> and <c>role="combobox"</c> to
    /// <c>&lt;input type="text"&gt;</c>. This is a known issue in the third-party
    /// <c>@u-elements/u-combobox</c> package and cannot be resolved without an upstream fix.
    /// </summary>
    private static AxeRunOptions SuggestionBestPractices
    {
        get
        {
            var options = new AxeRunOptions
            {
                RunOnly = new RunOnlyOptions
                {
                    Type = "tag", Values =
                    [
                        Tags.BestPractice
                    ]
                },
                Rules = new Dictionary<string, RuleOptions>
                {
                    ["aria-allowed-role"] = new() { Enabled = false }
                }
            };
            return options;
        }
    }

    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    public async Task Suggestion_Basic_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/suggestion/basic", testId);
    }

    [Theory]
    [ClassData<TestData.Basic>]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Suggestion_Basic_BestPracticesCompliant(string testId)
    {
        var result = await RunAccessibilityTestAsync("/suggestion/basic", testId, SuggestionBestPractices);
        Assert.Empty(result.Violations);
    }

    [Theory]
    [ClassData<TestData.Features>]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    public async Task Suggestion_Features_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/suggestion/features", testId);
    }

    [Theory]
    [ClassData<TestData.Features>]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Suggestion_Features_BestPracticesCompliant(string testId)
    {
        var result = await RunAccessibilityTestAsync("/suggestion/features", testId, SuggestionBestPractices);
        Assert.Empty(result.Violations);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    public async Task Suggestion_Accessibility_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/suggestion/accessibility", testId);
    }

    [Theory]
    [ClassData<TestData.Accessibility>]
    [Trait(Traits.Component, "Suggestion")]
    [Trait(Traits.Category, Categories.Aria)]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Suggestion_Accessibility_BestPracticesCompliant(string testId)
    {
        var result = await RunAccessibilityTestAsync("/suggestion/accessibility", testId, SuggestionBestPractices);
        Assert.Empty(result.Violations);
    }
}