using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Extensions.Services;

namespace Tests.Unit.Extensions.Services;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
public class SizeServiceTests
{
    private readonly SizeService sut = new();

    #region GetDataAttribute

    [Theory]
    [InlineData(Size.ExtraExtraSmall, "2xs")]
    [InlineData(Size.ExtraSmall, "xs")]
    [InlineData(Size.Small, "sm")]
    [InlineData(Size.Medium, "md")]
    [InlineData(Size.Large, "lg")]
    [InlineData(Size.ExtraLarge, "xl")]
    [InlineData(Size.ExtraExtraLarge, "2xl")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_AllValues_ReturnsShortForm(Size size, string expected)
    {
        Assert.Equal(expected, SizeService.GetDataAttribute(size));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_DefaultOverload_UsesMediumAsDefault()
    {
        Assert.Equal("md", SizeService.GetDataAttribute(Size.Medium));
    }

    #endregion

    #region GetDataAttribute (EnumValue)

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithTypedValue_ReturnsExpected()
    {
        EnumValue<Size> value = Size.Large;
        Assert.Equal("lg", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithRawString_ReturnsExpected()
    {
        EnumValue<Size> value = "Large";
        Assert.Equal("lg", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithInvalidRawString_ReturnsDefault()
    {
        EnumValue<Size> value = "NotASize";
        Assert.Equal("md", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_Empty_ReturnsDefault()
    {
        EnumValue<Size> value = default;
        Assert.Equal("md", sut.GetDataAttribute(value));
    }

    #endregion

    #region GetFromString

    [Theory]
    [InlineData("Small", Size.Small)]
    [InlineData("small", Size.Small)]
    [InlineData("LARGE", Size.Large)]
    [InlineData("Medium", Size.Medium)]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_ValidEnumName_ReturnsParsedEnum(string input, Size expected)
    {
        Assert.Equal(expected, sut.GetFromString(input));
    }

    [Theory]
    [InlineData("2xs", Size.ExtraExtraSmall)]
    [InlineData("xs", Size.ExtraSmall)]
    [InlineData("sm", Size.Small)]
    [InlineData("md", Size.Medium)]
    [InlineData("lg", Size.Large)]
    [InlineData("xl", Size.ExtraLarge)]
    [InlineData("2xl", Size.ExtraExtraLarge)]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_ShortForm_ReturnsParsedEnum(string input, Size expected)
    {
        Assert.Equal(expected, sut.GetFromString(input));
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [InlineData("xxxl")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_ReturnsDefault(string input)
    {
        Assert.Equal(Size.Medium, sut.GetFromString(input));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_WithCustomDefault_ReturnsCustomDefault()
    {
        Assert.Equal(Size.Large, sut.GetFromString("invalid", Size.Large));
    }

    #endregion
}