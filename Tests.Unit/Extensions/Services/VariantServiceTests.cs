using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Extensions.Services;

namespace Tests.Unit.Extensions.Services;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
public class VariantServiceTests
{
    private readonly VariantService sut = new();

    #region GetDataAttribute

    [Theory]
    [InlineData(Variant.Default, "default")]
    [InlineData(Variant.Base, "base")]
    [InlineData(Variant.Primary, "primary")]
    [InlineData(Variant.Secondary, "secondary")]
    [InlineData(Variant.Tertiary, "tertiary")]
    [InlineData(Variant.Tinted, "tinted")]
    [InlineData(Variant.Long, "long")]
    [InlineData(Variant.Short, "short")]
    [InlineData(Variant.Square, "square")]
    [InlineData(Variant.Rectangle, "rectangle")]
    [InlineData(Variant.Circle, "circle")]
    [InlineData(Variant.Text, "text")]
    [InlineData(Variant.Outline, "outline")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_AllValues_ReturnsLowercaseString(Variant variant, string expected)
    {
        Assert.Equal(expected, VariantService.GetDataAttribute(variant));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_DefaultOverload_UsesDefaultAsDefault()
    {
        Assert.Equal("default", VariantService.GetDataAttribute(Variant.Default));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_WithCustomDefault_ReturnsExpected()
    {
        Assert.Equal("primary", VariantService.GetDataAttribute(Variant.Primary, Variant.Default));
    }

    #endregion

    #region GetDataAttribute (EnumValue)

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithTypedValue_ReturnsExpected()
    {
        EnumValue<Variant> value = Variant.Primary;
        Assert.Equal("primary", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithRawString_ReturnsExpected()
    {
        EnumValue<Variant> value = "Primary";
        Assert.Equal("primary", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithInvalidRawString_ReturnsDefault()
    {
        EnumValue<Variant> value = "NotAVariant";
        Assert.Equal("default", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_Empty_ReturnsDefault()
    {
        EnumValue<Variant> value = default;
        Assert.Equal("default", sut.GetDataAttribute(value));
    }

    #endregion

    #region GetFromString

    [Theory]
    [InlineData("Primary", Variant.Primary)]
    [InlineData("primary", Variant.Primary)]
    [InlineData("SECONDARY", Variant.Secondary)]
    [InlineData("Tertiary", Variant.Tertiary)]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_ValidValue_ReturnsParsedEnum(string input, Variant expected)
    {
        Assert.Equal(expected, sut.GetFromString(input));
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_ReturnsDefault(string input)
    {
        Assert.Equal(Variant.Default, sut.GetFromString(input));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_WithCustomDefault_ReturnsCustomDefault()
    {
        Assert.Equal(Variant.Primary, sut.GetFromString("invalid", Variant.Primary));
    }

    #endregion
}