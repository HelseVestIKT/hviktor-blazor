using Hviktor.Abstractions.Models;

namespace Tests.Unit.Abstractions.Models;

/// <summary>
/// Unit tests for <see cref="EnumValue{TEnum}"/>.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Model, "EnumValue")]
public class EnumValueTests
{
    private enum Color
    {
        Red,
        Green,
        Blue
    }

    // ReSharper disable UnusedMember.Local
    private enum Size
    {
        Small,
        Medium,
        Large
    }
    // ReSharper restore UnusedMember.Local

    #region Default / Empty

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Default_IsEmpty_ReturnsTrue()
    {
        EnumValue<Color> value = default;
        Assert.True(value.IsEmpty);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Default_IsRaw_ReturnsFalse()
    {
        EnumValue<Color> value = default;
        Assert.False(value.IsRaw);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Default_EnumValueOrNull_ReturnsNull()
    {
        EnumValue<Color> value = default;
        Assert.Null(value.EnumValueOrNull);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Default_RawValue_ReturnsNull()
    {
        EnumValue<Color> value = default;
        Assert.Null(value.RawValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Default_ToString_ReturnsEmptyString()
    {
        EnumValue<Color> value = default;
        Assert.Equal(string.Empty, value.ToString());
    }

    #endregion

    #region Implicit conversion from enum

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromEnum_IsEmpty_ReturnsFalse()
    {
        EnumValue<Color> value = Color.Red;
        Assert.False(value.IsEmpty);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromEnum_IsRaw_ReturnsFalse()
    {
        EnumValue<Color> value = Color.Red;
        Assert.False(value.IsRaw);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromEnum_EnumValueOrNull_ReturnsEnumValue()
    {
        EnumValue<Color> value = Color.Green;
        Assert.Equal(Color.Green, value.EnumValueOrNull);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromEnum_RawValue_ReturnsNull()
    {
        EnumValue<Color> value = Color.Blue;
        Assert.Null(value.RawValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromEnum_ToString_ReturnsEnumName()
    {
        EnumValue<Color> value = Color.Red;
        Assert.Equal("Red", value.ToString());
    }

    #endregion

    #region Implicit conversion from nullable enum

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromNullableEnum_WithValue_IsNotEmpty()
    {
        Color? input = Color.Blue;
        EnumValue<Color> value = input;
        Assert.False(value.IsEmpty);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromNullableEnum_WithValue_EnumValueOrNull_MatchesInput()
    {
        Color? input = Color.Blue;
        EnumValue<Color> value = input;
        Assert.Equal(Color.Blue, value.EnumValueOrNull);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromNullableEnum_Null_IsEmpty()
    {
        Color? input = null;
        EnumValue<Color> value = input;
        Assert.True(value.IsEmpty);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromNullableEnum_Null_EqualsDefault()
    {
        Color? input = null;
        EnumValue<Color> value = input;
        Assert.Equal(default, value);
    }

    #endregion

    #region Implicit conversion from string

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_ValidEnumName_IsRaw_ReturnsTrue()
    {
        EnumValue<Color> value = "Red";
        Assert.True(value.IsRaw);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_ValidEnumName_IsEmpty_ReturnsFalse()
    {
        EnumValue<Color> value = "Red";
        Assert.False(value.IsEmpty);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_ValidEnumName_RawValue_ReturnsInput()
    {
        EnumValue<Color> value = "Green";
        Assert.Equal("Green", value.RawValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_ValidEnumName_EnumValueOrNull_ReturnsNull()
    {
        // IsRaw path: EnumValueOrNull is always null when constructed from string
        EnumValue<Color> value = "Blue";
        Assert.Null(value.EnumValueOrNull);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_ValidEnumName_ToString_ReturnsRawString()
    {
        EnumValue<Color> value = "Red";
        Assert.Equal("Red", value.ToString());
    }

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void FromString_NullOrWhitespace_IsEmpty(string? input)
    {
        EnumValue<Color> value = input;
        Assert.True(value.IsEmpty);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_TrimsWhitespace()
    {
        EnumValue<Color> value = "  Red  ";
        Assert.Equal("Red", value.RawValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_UnknownValue_IsRaw_ReturnsTrue()
    {
        EnumValue<Color> value = "Purple";
        Assert.True(value.IsRaw);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_UnknownValue_IsEmpty_ReturnsFalse()
    {
        EnumValue<Color> value = "Purple";
        Assert.False(value.IsEmpty);
    }

    #endregion

    #region Equality — same source type

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_SameEnumValue_ReturnsTrue()
    {
        EnumValue<Color> a = Color.Red;
        EnumValue<Color> b = Color.Red;
        Assert.Equal(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_DifferentEnumValues_ReturnsFalse()
    {
        EnumValue<Color> a = Color.Red;
        EnumValue<Color> b = Color.Blue;
        Assert.NotEqual(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_SameRawString_ReturnsTrue()
    {
        EnumValue<Color> a = "Purple";
        EnumValue<Color> b = "Purple";
        Assert.Equal(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_RawString_IsCaseInsensitive()
    {
        EnumValue<Color> a = "purple";
        EnumValue<Color> b = "PURPLE";
        Assert.Equal(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_DifferentRawStrings_ReturnsFalse()
    {
        EnumValue<Color> a = "Purple";
        EnumValue<Color> b = "Orange";
        Assert.NotEqual(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_TwoDefaults_ReturnsTrue()
    {
        EnumValue<Color> a = default;
        EnumValue<Color> b = default;
        Assert.Equal(a, b);
    }

    #endregion

    #region Equality — cross-source (enum vs parseable string)

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_EnumAndMatchingString_ReturnsTrue()
    {
        EnumValue<Color> fromEnum = Color.Red;
        EnumValue<Color> fromString = "Red";
        Assert.Equal(fromEnum, fromString);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_EnumAndMatchingString_CaseInsensitive()
    {
        EnumValue<Color> fromEnum = Color.Green;
        EnumValue<Color> fromString = "green";
        Assert.Equal(fromEnum, fromString);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_EnumAndNonMatchingString_ReturnsFalse()
    {
        EnumValue<Color> fromEnum = Color.Red;
        EnumValue<Color> fromString = "Blue";
        Assert.NotEqual(fromEnum, fromString);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_EnumAndUnknownString_ReturnsFalse()
    {
        EnumValue<Color> fromEnum = Color.Red;
        EnumValue<Color> fromString = "Purple";
        Assert.NotEqual(fromEnum, fromString);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_DefaultAndNullString_ReturnsTrue()
    {
        EnumValue<Color> fromDefault = default;
        EnumValue<Color> fromNullString = (string?)null;
        Assert.Equal(fromDefault, fromNullString);
    }

    #endregion

    #region Equality operators

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void EqualityOperator_SameEnumValues_ReturnsTrue()
    {
        EnumValue<Color> a = Color.Blue;
        EnumValue<Color> b = Color.Blue;
        Assert.True(a == b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void InequalityOperator_DifferentEnumValues_ReturnsTrue()
    {
        EnumValue<Color> a = Color.Red;
        EnumValue<Color> b = Color.Green;
        Assert.True(a != b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_BoxedObject_ReturnsTrue()
    {
        EnumValue<Color> a = Color.Red;
        object b = (EnumValue<Color>)Color.Red;
        Assert.True(a.Equals(b));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_DifferentType_ReturnsTrue()
    {
        EnumValue<Color> a = Color.Red;
        Assert.True(a.Equals("Red"));
    }

    #endregion

    #region GetHashCode

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetHashCode_SameEnumValues_ProduceSameHash()
    {
        EnumValue<Color> a = Color.Red;
        EnumValue<Color> b = Color.Red;
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetHashCode_EnumAndMatchingString_ProduceSameHash()
    {
        EnumValue<Color> fromEnum = Color.Green;
        EnumValue<Color> fromString = "Green";
        Assert.Equal(fromEnum.GetHashCode(), fromString.GetHashCode());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetHashCode_UnknownStrings_CaseInsensitive_ProduceSameHash()
    {
        EnumValue<Color> a = "purple";
        EnumValue<Color> b = "PURPLE";
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetHashCode_TwoDefaults_ProduceSameHash()
    {
        EnumValue<Color> a = default;
        EnumValue<Color> b = default;
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    #endregion

    #region ToString

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ToString_FromEnum_ReturnsEnumName()
    {
        EnumValue<Size> value = Size.Large;
        Assert.Equal("Large", value.ToString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ToString_FromString_ReturnsRawString()
    {
        EnumValue<Size> value = "ExtraLarge";
        Assert.Equal("ExtraLarge", value.ToString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ToString_Default_ReturnsEmptyString()
    {
        EnumValue<Size> value = default;
        Assert.Equal(string.Empty, value.ToString());
    }

    #endregion
}