using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Extensions.Services;
using Tests.Playwright.Fixtures;
using static Tests.Playwright.TestCollections;

namespace Tests.Playwright.Compliance.Components.Avatar;

/// <summary>
/// WCAG accessibility tests for the Avatar component.
/// Uses axe-core to validate WCAG 2.0/2.1/2.2 compliance at all levels.
/// </summary>
[Trait(Traits.Collection, TestCollections.Compliance)]
public class AvatarComplianceCollection(TestsFixture fixture) : WcagTestBase<TestsFixture>(fixture)
{
    #region Avatar Variant Tests

    [Theory]
    [ClassData<TestData.Variant>]
    [Trait(Traits.Component, "Avatar")]
    [Trait(Traits.Category, Categories.Semantics)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Avatar_Variant_WcagCompliant(Variant variant)
    {
        await AssertAllWcagLevelsAsync("/avatar/variant", variant, VariantService.GetDataAttribute);
    }

    #endregion

    #region Avatar Color Tests

    [Theory]
    [ClassData<SharedTestData.AllColorsData>]
    [Trait(Traits.Component, "Avatar")]
    [Trait(Traits.Category, Categories.Color)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Avatar_Color_WcagCompliant(Color color)
    {
        await AssertAllWcagLevelsAsync("/avatar/color", color, ColorService.GetDataAttribute);
    }

    #endregion

    #region Avatar Size Tests

    [Theory]
    [ClassData<TestData.Size>]
    [Trait(Traits.Component, "Avatar")]
    [Trait(Traits.Category, Categories.SensoryAndVisualCues)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Avatar_Size_WcagCompliant(Size size)
    {
        await AssertAllWcagLevelsAsync("/avatar/size", size, SizeService.GetDataAttribute);
    }

    #endregion

    #region Avatar Initials Tests

    [Theory]
    [ClassData<TestData.Initials>]
    [Trait(Traits.Component, "Avatar")]
    [Trait(Traits.Category, Categories.TextAlternatives)]
    [Trait(Traits.Tag, Tags.Wcag)]
    [Trait(Traits.Tag, Tags.BestPractice)]
    public async Task Avatar_Initials_WcagCompliant(string testId)
    {
        await AssertAllWcagLevelsAsync("/avatar/initials", testId);
    }

    #endregion
}