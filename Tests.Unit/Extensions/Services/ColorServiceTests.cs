using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Extensions.Services;

namespace Tests.Unit.Extensions.Services;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
public class ColorServiceTests
{
    private readonly ColorService sut = new();

    #region GetDataAttribute

    [Theory]
    [InlineData(Color.Accent, "accent")]
    [InlineData(Color.Neutral, "neutral")]
    [InlineData(Color.Brand1, "brand1")]
    [InlineData(Color.Brand2, "brand2")]
    [InlineData(Color.Brand3, "brand3")]
    [InlineData(Color.Success, "success")]
    [InlineData(Color.Danger, "danger")]
    [InlineData(Color.Info, "info")]
    [InlineData(Color.Warning, "warning")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_AllValues_ReturnsLowercaseString(Color color, string expected)
    {
        Assert.Equal(expected, ColorService.GetDataAttribute(color));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_DefaultOverload_UsesAccentAsDefault()
    {
        Assert.Equal("accent", ColorService.GetDataAttribute(Color.Accent));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_WithDefaultValue_ReturnsExpected()
    {
        Assert.Equal("danger", ColorService.GetDataAttribute(Color.Danger, Color.Accent));
    }

    #endregion

    #region GetDataAttribute (EnumValue)

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithTypedValue_ReturnsExpected()
    {
        EnumValue<Color> value = Color.Danger;
        Assert.Equal("danger", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithRawString_ReturnsExpected()
    {
        EnumValue<Color> value = "Danger";
        Assert.Equal("danger", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithInvalidRawString_ReturnsDefault()
    {
        EnumValue<Color> value = "NotAColor";
        Assert.Equal("accent", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_Empty_ReturnsDefault()
    {
        EnumValue<Color> value = default;
        Assert.Equal("accent", sut.GetDataAttribute(value));
    }

    #endregion

    #region GetFromString

    [Theory]
    [InlineData("Accent", Color.Accent)]
    [InlineData("accent", Color.Accent)]
    [InlineData("DANGER", Color.Danger)]
    [InlineData("Info", Color.Info)]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_ValidValue_ReturnsParsedEnum(string input, Color expected)
    {
        Assert.Equal(expected, sut.GetFromString(input));
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData("NotAColor")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_ReturnsDefault(string input)
    {
        Assert.Equal(Color.Accent, sut.GetFromString(input));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_WithCustomDefault_ReturnsCustomDefault()
    {
        Assert.Equal(Color.Danger, sut.GetFromString("invalid", Color.Danger));
    }

    #endregion
}