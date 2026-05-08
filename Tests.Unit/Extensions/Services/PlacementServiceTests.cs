using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Extensions.Services;

namespace Tests.Unit.Extensions.Services;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
public class PlacementServiceTests
{
    private readonly PlacementService sut = new();

    #region GetDataAttribute

    [Theory]
    [InlineData(Placement.Center, "center")]
    [InlineData(Placement.Top, "top")]
    [InlineData(Placement.TopStart, "top-start")]
    [InlineData(Placement.TopEnd, "top-end")]
    [InlineData(Placement.Bottom, "bottom")]
    [InlineData(Placement.BottomStart, "bottom-start")]
    [InlineData(Placement.BottomEnd, "bottom-end")]
    [InlineData(Placement.Left, "left")]
    [InlineData(Placement.LeftStart, "left-start")]
    [InlineData(Placement.LeftEnd, "left-end")]
    [InlineData(Placement.Right, "right")]
    [InlineData(Placement.RightStart, "right-start")]
    [InlineData(Placement.RightEnd, "right-end")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_AllValues_ReturnsKebabCase(Placement placement, string expected)
    {
        Assert.Equal(expected, PlacementService.GetDataAttribute(placement));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_DefaultOverload_UsesBottomStartAsDefault()
    {
        Assert.Equal("bottom-start", PlacementService.GetDataAttribute(Placement.BottomStart));
    }

    #endregion

    #region GetDataAttribute (EnumValue)

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithTypedValue_ReturnsExpected()
    {
        EnumValue<Placement> value = Placement.TopEnd;
        Assert.Equal("top-end", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithRawString_ReturnsExpected()
    {
        EnumValue<Placement> value = "TopEnd";
        Assert.Equal("top-end", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithInvalidRawString_ReturnsDefault()
    {
        EnumValue<Placement> value = "NotAPlacement";
        Assert.Equal("bottom-start", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_Empty_ReturnsDefault()
    {
        EnumValue<Placement> value = default;
        Assert.Equal("bottom-start", sut.GetDataAttribute(value));
    }

    #endregion

    #region GetFromString

    [Theory]
    [InlineData("TopStart", Placement.TopStart)]
    [InlineData("topstart", Placement.TopStart)]
    [InlineData("BOTTOMEND", Placement.BottomEnd)]
    [InlineData("Center", Placement.Center)]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_ValidValue_ReturnsParsedEnum(string input, Placement expected)
    {
        Assert.Equal(expected, sut.GetFromString(input));
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_ReturnsDefault(string input)
    {
        Assert.Equal(Placement.BottomStart, sut.GetFromString(input));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_WithCustomDefault_ReturnsCustomDefault()
    {
        Assert.Equal(Placement.Top, sut.GetFromString("invalid", Placement.Top));
    }

    #endregion
}