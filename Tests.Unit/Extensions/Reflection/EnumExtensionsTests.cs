using System.ComponentModel;
using Hviktor.Extensions.Reflection;

namespace Tests.Unit.Extensions.Reflection;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
public class EnumExtensionsTests
{
    public enum TestEnum
    {
        [Description("First value")] First,
        [Description("Second value")] Second,
        NoDescription
    }

    #region GetDescription

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDescription_Null_ReturnsEmptyString()
    {
        Enum? value = null;
        Assert.Equal("", value.GetDescription());
    }

    [Theory]
    [InlineData(TestEnum.First, "First value")]
    [InlineData(TestEnum.Second, "Second value")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDescription_WithDescription_ReturnsDescription(TestEnum value, string expected)
    {
        Assert.Equal(expected, value.GetDescription());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDescription_WithoutDescription_ReturnsName()
    {
        Assert.Equal("NoDescription", TestEnum.NoDescription.GetDescription());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDescription_InvalidEnumValue_ReturnsToString()
    {
        var invalid = (TestEnum)999;
        Assert.Equal("999", invalid.GetDescription());
    }

    #endregion

    #region GetEnumValue

    [Theory]
    [InlineData("First value", TestEnum.First)]
    [InlineData("Second value", TestEnum.Second)]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetEnumValue_MatchingDescription_ReturnsEnumValue(string description, TestEnum expected)
    {
        Assert.Equal(expected, description.GetEnumValue<TestEnum>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetEnumValue_CaseInsensitive_ReturnsEnumValue()
    {
        Assert.Equal(TestEnum.First, "FIRST VALUE".GetEnumValue<TestEnum>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetEnumValue_NoMatch_ReturnsDefault()
    {
        Assert.Equal(default, "nonexistent".GetEnumValue<TestEnum>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetEnumValue_Null_ReturnsDefault()
    {
        string? description = null;
        Assert.Equal(default, description.GetEnumValue<TestEnum>());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetEnumValue_NonEnumType_ReturnsDefault()
    {
        Assert.Equal(default, "test".GetEnumValue<int>());
    }

    #endregion
}