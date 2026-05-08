using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Extensions.Services;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Alert;

/// <summary>
/// WCAG accessibility tests for the Alert component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class AlertComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    [Theory]
    [ClassData<SharedTestData.AllColorsData>]
    [Trait(Traits.Component, "Alert")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Alert_Color_WcagCompliant(Color color)
    {
        await AssertAllWcagLevelsAsync("/alert/color", color, ColorService.GetDataAttribute);
    }

    [Theory]
    [ClassData<SharedTestData.StandardSizesData>]
    [Trait(Traits.Component, "Alert")]
    [Trait(Traits.Category, Categories.SensoryAndVisualCues)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Alert_Size_WcagCompliant(Size size)
    {
        await AssertAllWcagLevelsAsync("/alert/size", size, SizeService.GetDataAttribute);
    }
}