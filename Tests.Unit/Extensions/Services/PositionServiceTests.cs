using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Extensions.Services;

namespace Tests.Unit.Extensions.Services;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
public class PositionServiceTests
{
    private readonly PositionService sut = new();

    #region GetDataAttribute

    [Theory]
    [InlineData(Position.Start, "start")]
    [InlineData(Position.End, "end")]
    [InlineData(Position.Top, "top")]
    [InlineData(Position.Bottom, "bottom")]
    [InlineData(Position.Left, "left")]
    [InlineData(Position.Right, "right")]
    [InlineData(Position.Center, "center")]
    [InlineData(Position.TopLeft, "top-left")]
    [InlineData(Position.TopRight, "top-right")]
    [InlineData(Position.BottomLeft, "bottom-left")]
    [InlineData(Position.BottomRight, "bottom-right")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_AllValues_ReturnsKebabCase(Position position, string expected)
    {
        Assert.Equal(expected, PositionService.GetDataAttribute(position));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_DefaultOverload_UsesBottomLeftAsDefault()
    {
        Assert.Equal("bottom-left", PositionService.GetDataAttribute(Position.BottomLeft));
    }

    #endregion

    #region GetDataAttribute (EnumValue)

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithTypedValue_ReturnsExpected()
    {
        EnumValue<Position> value = Position.TopRight;
        Assert.Equal("top-right", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithRawString_ReturnsExpected()
    {
        EnumValue<Position> value = "TopRight";
        Assert.Equal("top-right", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithInvalidRawString_ReturnsDefault()
    {
        EnumValue<Position> value = "NotAPosition";
        Assert.Equal("bottom-left", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_Empty_ReturnsDefault()
    {
        EnumValue<Position> value = default;
        Assert.Equal("bottom-left", sut.GetDataAttribute(value));
    }

    #endregion

    #region GetFromString

    [Theory]
    [InlineData("TopLeft", Position.TopLeft)]
    [InlineData("topleft", Position.TopLeft)]
    [InlineData("BOTTOMRIGHT", Position.BottomRight)]
    [InlineData("Center", Position.Center)]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_ValidValue_ReturnsParsedEnum(string input, Position expected)
    {
        Assert.Equal(expected, sut.GetFromString(input));
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_ReturnsDefault(string input)
    {
        Assert.Equal(Position.BottomLeft, sut.GetFromString(input));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_WithCustomDefault_ReturnsCustomDefault()
    {
        Assert.Equal(Position.Top, sut.GetFromString("invalid", Position.Top));
    }

    #endregion
}