using Hviktor.Extensions.Conversion;

namespace Tests.Unit.Extensions.Conversion;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class ConverterExtensionsTests
{
    #region ToValue

    [Fact]
    public void ToValue_CastsObjectToSpecifiedType()
    {
        object value = 42;
        var result = Converter<int>.ToValue(value);
        Assert.Equal(42, result);
    }

    [Fact]
    public void ToValue_CastsStringCorrectly()
    {
        object value = "hello";
        var result = Converter<string>.ToValue(value);
        Assert.Equal("hello", result);
    }

    [Fact]
    public void ToValue_InvalidCast_ThrowsException()
    {
        object value = "not an int";
        Assert.Throws<InvalidCastException>(() => Converter<int>.ToValue(value));
    }

    #endregion

    #region ToString

    [Fact]
    public void ToString_WithValue_ReturnsString()
    {
        var result = Converter<object>.ToString(42);
        Assert.Equal("42", result);
    }

    [Fact]
    public void ToString_WithNull_ReturnsEmptyString()
    {
        var result = Converter<object>.ToString(null);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void ToString_WithStringValue_ReturnsSameString()
    {
        var result = Converter<object>.ToString("test");
        Assert.Equal("test", result);
    }

    #endregion

    #region ToInt

    [Fact]
    public void ToInt_WithValidInt_ReturnsInt()
    {
        var result = Converter<object>.ToInt(42);
        Assert.Equal(42, result);
    }

    [Fact]
    public void ToInt_WithNull_ReturnsZero()
    {
        var result = Converter<object>.ToInt(null);
        Assert.Equal(0, result);
    }

    [Fact]
    public void ToInt_WithNull_ReturnsFallback()
    {
        var result = Converter<object>.ToInt(null, -1);
        Assert.Equal(-1, result);
    }

    [Fact]
    public void ToInt_WithInvalidValue_ReturnsFallback()
    {
        var result = Converter<object>.ToInt("not a number", 99);
        Assert.Equal(99, result);
    }

    [Fact]
    public void ToInt_WithStringNumber_ConvertsCorrectly()
    {
        var result = Converter<object>.ToInt("123");
        Assert.Equal(123, result);
    }

    [Fact]
    public void ToInt_WithOverflow_ReturnsFallback()
    {
        var result = Converter<object>.ToInt(long.MaxValue, 5);
        Assert.Equal(5, result);
    }

    #endregion

    #region ToLong

    [Fact]
    public void ToLong_WithValidValue_ReturnsLong()
    {
        var result = Converter<object>.ToLong(100L);
        Assert.Equal(100L, result);
    }

    [Fact]
    public void ToLong_WithNull_ReturnsZero()
    {
        var result = Converter<object>.ToLong(null);
        Assert.Equal(0L, result);
    }

    [Fact]
    public void ToLong_WithNull_ReturnsFallback()
    {
        var result = Converter<object>.ToLong(null, -1L);
        Assert.Equal(-1L, result);
    }

    [Fact]
    public void ToLong_WithInvalidValue_ReturnsFallback()
    {
        var result = Converter<object>.ToLong("invalid", 50L);
        Assert.Equal(50L, result);
    }

    [Fact]
    public void ToLong_WithLargeValue_ConvertsCorrectly()
    {
        var result = Converter<object>.ToLong(long.MaxValue);
        Assert.Equal(long.MaxValue, result);
    }

    #endregion

    #region ToDouble

    [Fact]
    public void ToDouble_WithValidValue_ReturnsDouble()
    {
        var result = Converter<object>.ToDouble(3.14);
        Assert.Equal(3.14, result);
    }

    [Fact]
    public void ToDouble_WithNull_ReturnsZero()
    {
        var result = Converter<object>.ToDouble(null);
        Assert.Equal(0D, result);
    }

    [Fact]
    public void ToDouble_WithNull_ReturnsFallback()
    {
        var result = Converter<object>.ToDouble(null, 1.5);
        Assert.Equal(1.5, result);
    }

    [Fact]
    public void ToDouble_WithInvalidValue_ReturnsFallback()
    {
        var result = Converter<object>.ToDouble("not a number", 9.9);
        Assert.Equal(9.9, result);
    }

    [Fact]
    public void ToDouble_WithIntValue_ConvertsCorrectly()
    {
        var result = Converter<object>.ToDouble(10);
        Assert.Equal(10D, result);
    }

    #endregion

    #region ToDecimal

    [Fact]
    public void ToDecimal_WithValidValue_ReturnsDecimal()
    {
        var result = Converter<object>.ToDecimal(9.99M);
        Assert.Equal(9.99M, result);
    }

    [Fact]
    public void ToDecimal_WithNull_ReturnsZero()
    {
        var result = Converter<object>.ToDecimal(null);
        Assert.Equal(0M, result);
    }

    [Fact]
    public void ToDecimal_WithNull_ReturnsFallback()
    {
        var result = Converter<object>.ToDecimal(null, 5.5M);
        Assert.Equal(5.5M, result);
    }

    [Fact]
    public void ToDecimal_WithInvalidValue_ReturnsFallback()
    {
        var result = Converter<object>.ToDecimal("invalid", 1.1M);
        Assert.Equal(1.1M, result);
    }

    [Fact]
    public void ToDecimal_WithDoubleOverflow_ReturnsFallback()
    {
        var result = Converter<object>.ToDecimal(double.MaxValue, 0M);
        Assert.Equal(0M, result);
    }

    #endregion

    #region ToBool

    [Fact]
    public void ToBool_WithTrue_ReturnsTrue()
    {
        var result = Converter<object>.ToBool(true);
        Assert.True(result);
    }

    [Fact]
    public void ToBool_WithFalse_ReturnsFalse()
    {
        var result = Converter<object>.ToBool(false);
        Assert.False(result);
    }

    [Fact]
    public void ToBool_WithNull_ReturnsFalseDefault()
    {
        var result = Converter<object>.ToBool(null);
        Assert.False(result);
    }

    [Fact]
    public void ToBool_WithNull_ReturnsFallback()
    {
        var result = Converter<object>.ToBool(null, true);
        Assert.True(result);
    }

    [Fact]
    public void ToBool_WithInvalidValue_ReturnsFallback()
    {
        var result = Converter<object>.ToBool("not a bool", true);
        Assert.True(result);
    }

    #endregion

    #region ToDateTime

    [Fact]
    public void ToDateTime_WithValidDate_ReturnsDateTime()
    {
        var expected = new DateTime(2024, 1, 15);
        var result = Converter<object>.ToDateTime("2024-01-15");
        Assert.Equal(expected.Date, result.Date);
    }

    [Fact]
    public void ToDateTime_WithNull_ReturnsApproximateNow()
    {
        var before = DateTime.Now;
        var result = Converter<object>.ToDateTime(null);
        var after = DateTime.Now;
        Assert.InRange(result, before, after);
    }

    [Fact]
    public void ToDateTime_WithNull_ReturnsFallback()
    {
        var fallback = new DateTime(2000, 6, 15);
        var result = Converter<object>.ToDateTime(null, fallback);
        Assert.Equal(fallback, result);
    }

    [Fact]
    public void ToDateTime_WithInvalidValue_ReturnsFallback()
    {
        var fallback = new DateTime(2000, 1, 1);
        var result = Converter<object>.ToDateTime("not a date", fallback);
        Assert.Equal(fallback, result);
    }

    #endregion
}