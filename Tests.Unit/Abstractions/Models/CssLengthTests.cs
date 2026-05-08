using Hviktor.Abstractions.Models;

namespace Tests.Unit.Abstractions.Models;

/// <summary>
/// Unit tests for <see cref="Hviktor.Abstractions.Models.CssLength"/>.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Model, "CssLength")]
public class CssLengthTests
{
    #region Default / Empty

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Default_IsEmpty()
    {
        CssLength length = default;
        Assert.True(length.IsEmpty);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Default_ToCssString_ReturnsNull()
    {
        CssLength length = default;
        Assert.Null(length.ToCssString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Default_ToString_ReturnsNull()
    {
        CssLength length = default;
        Assert.Null(length.ToString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Default_PixelValue_ReturnsNull()
    {
        CssLength length = default;
        Assert.Null(length.PixelValue);
    }

    #endregion

    #region Implicit conversion from int

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData(100, "100px")]
    [InlineData(1, "1px")]
    [InlineData(-50, "-50px")]
    public void FromInt_ProducesPixelString(int pixels, string expected)
    {
        CssLength length = pixels;
        Assert.Equal(expected, length.ToCssString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromInt_Zero_IsEmpty()
    {
        CssLength length = 0;

        Assert.True(length.IsEmpty);
        Assert.Null(length.ToCssString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromInt_PixelValue_MatchesInput()
    {
        CssLength length = 42;
        Assert.Equal(42, length.PixelValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromInt_IsNotEmpty()
    {
        CssLength length = 10;
        Assert.False(length.IsEmpty);
    }

    #endregion

    #region Implicit conversion from double

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData(1.5, "1.5px")]
    [InlineData(0.25, "0.25px")]
    [InlineData(-2.0, "-2px")]
    public void FromDouble_ProducesPixelString(double pixels, string expected)
    {
        CssLength length = pixels;
        Assert.Equal(expected, length.ToCssString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromDouble_Zero_IsEmpty()
    {
        CssLength length = 0.0;

        Assert.True(length.IsEmpty);
        Assert.Null(length.ToCssString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromDouble_UsesInvariantCulture()
    {
        CssLength length = 1.5;

        // Must use '.' not ',' as decimal separator
        Assert.Equal("1.5px", length.ToCssString());
    }

    #endregion

    #region Implicit conversion from string

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData("2rem", "2rem")]
    [InlineData("1em", "1em")]
    [InlineData("100%", "100%")]
    [InlineData("50vw", "50vw")]
    [InlineData("10vh", "10vh")]
    [InlineData("clamp(1rem, 5vw, 3rem)", "clamp(1rem, 5vw, 3rem)")]
    public void FromString_CssUnit_PassesThroughAsIs(string input, string expected)
    {
        CssLength length = input;
        Assert.Equal(expected, length.ToCssString());
    }

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData("200", "200px")]
    [InlineData("1", "1px")]
    public void FromString_BareInteger_ProducesPixelString(string input, string expected)
    {
        CssLength length = input;
        Assert.Equal(expected, length.ToCssString());
    }

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData("1.5", "1.5px")]
    [InlineData("0.75", "0.75px")]
    public void FromString_BareDecimal_ProducesPixelString(string input, string expected)
    {
        CssLength length = input;
        Assert.Equal(expected, length.ToCssString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_BareZeroInteger_IsEmpty()
    {
        CssLength length = "0";
        Assert.True(length.IsEmpty);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_BareZeroDecimal_IsEmpty()
    {
        CssLength length = "0.0";
        Assert.True(length.IsEmpty);
    }

    [Theory]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void FromString_NullOrWhitespace_IsEmpty(string? input)
    {
        CssLength length = input;

        Assert.True(length.IsEmpty);
        Assert.Null(length.ToCssString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void FromString_TrimsLeadingAndTrailingWhitespace()
    {
        CssLength length = "  2rem  ";
        Assert.Equal("2rem", length.ToCssString());
    }

    #endregion

    #region PixelValue property

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void PixelValue_FromRemString_ReturnsNull()
    {
        CssLength length = "2rem";
        Assert.Null(length.PixelValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void PixelValue_FromIntConversion_MatchesInput()
    {
        CssLength length = 64;
        Assert.Equal(64, length.PixelValue);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void PixelValue_FromDoubleConversion_ReturnsNullForNonInteger()
    {
        // "1.5px" — the px suffix is present but int.TryParse("1.5") fails
        CssLength length = 1.5;
        Assert.Null(length.PixelValue);
    }

    #endregion

    #region Equality

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_SamePixelValue_AreEqual()
    {
        CssLength a = 100;
        CssLength b = 100;

        Assert.Equal(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_DifferentPixelValues_AreNotEqual()
    {
        CssLength a = 100;
        CssLength b = 200;

        Assert.NotEqual(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_SameStringValue_AreEqual()
    {
        CssLength a = "2rem";
        CssLength b = "2rem";

        Assert.Equal(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_IsCaseInsensitive()
    {
        CssLength a = "100PX";
        CssLength b = "100px";

        Assert.Equal(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_TwoDefaults_AreEqual()
    {
        CssLength a = default;
        CssLength b = default;

        Assert.Equal(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_DefaultAndEmpty_AreEqual()
    {
        CssLength a = default;
        CssLength b = 0;

        Assert.Equal(a, b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void EqualityOperator_SameValues_ReturnsTrue()
    {
        CssLength a = 50;
        CssLength b = 50;

        Assert.True(a == b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void InequalityOperator_DifferentValues_ReturnsTrue()
    {
        CssLength a = 50;
        CssLength b = "50rem";

        Assert.True(a != b);
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_BoxedObject_AreEqual()
    {
        CssLength a = 100;
        object b = (CssLength)100;

        Assert.True(a.Equals(b));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void Equals_DifferentType_ReturnsTrue()
    {
        CssLength a = 100;
        Assert.True(a.Equals("100px"));
    }

    #endregion

    #region GetHashCode

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetHashCode_SameValues_ProduceSameHash()
    {
        CssLength a = "2rem";
        CssLength b = "2rem";

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetHashCode_CaseInsensitive_ProduceSameHash()
    {
        CssLength a = "100PX";
        CssLength b = "100px";

        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void GetHashCode_Default_ReturnsZero()
    {
        CssLength length = default;
        Assert.Equal(0, length.GetHashCode());
    }

    #endregion

    #region ToString

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ToString_ReturnsUnderlyingCssString()
    {
        CssLength length = "3rem";
        Assert.Equal("3rem", length.ToString());
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Rendering)]
    public void ToString_FromInt_ReturnsPixelString()
    {
        CssLength length = 16;
        Assert.Equal("16px", length.ToString());
    }

    #endregion
}