using Hviktor.Comparers;

namespace Tests.Unit.Comparers;

/// <summary>
/// Unit tests for <see cref="PropertyComparerHelpers"/>.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class PropertyComparerHelpersTests
{
    #region CompareNullPair

    [Fact]
    public void CompareNullPair_BothNull_ReturnsZero()
    {
        var result = PropertyComparerHelpers.CompareNullPair(true, true);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareNullPair_XNull_ReturnsNegativeOne()
    {
        var result = PropertyComparerHelpers.CompareNullPair(true, false);
        Assert.Equal(-1, result);
    }

    [Fact]
    public void CompareNullPair_YNull_ReturnsPositiveOne()
    {
        var result = PropertyComparerHelpers.CompareNullPair(false, true);
        Assert.Equal(1, result);
    }

    [Fact]
    public void CompareNullPair_NeitherNull_ReturnsNull()
    {
        var result = PropertyComparerHelpers.CompareNullPair(false, false);
        Assert.Null(result);
    }

    #endregion

    #region TypeComparers

    [Fact]
    public void TypeComparers_ContainsInt()
    {
        Assert.True(PropertyComparerHelpers.TypeComparers.ContainsKey(typeof(int)));
    }

    [Fact]
    public void TypeComparers_ContainsNullableInt()
    {
        Assert.True(PropertyComparerHelpers.TypeComparers.ContainsKey(typeof(int?)));
    }

    [Fact]
    public void TypeComparers_ContainsDouble()
    {
        Assert.True(PropertyComparerHelpers.TypeComparers.ContainsKey(typeof(double)));
    }

    [Fact]
    public void TypeComparers_ContainsDecimal()
    {
        Assert.True(PropertyComparerHelpers.TypeComparers.ContainsKey(typeof(decimal)));
    }

    [Fact]
    public void TypeComparers_ContainsDateTime()
    {
        Assert.True(PropertyComparerHelpers.TypeComparers.ContainsKey(typeof(DateTime)));
    }

    [Fact]
    public void TypeComparers_ContainsDateTimeOffset()
    {
        Assert.True(PropertyComparerHelpers.TypeComparers.ContainsKey(typeof(DateTimeOffset)));
    }

    [Theory]
    [InlineData(1, 2, -1)]
    [InlineData(2, 1, 1)]
    [InlineData(5, 5, 0)]
    public void TypeComparers_Int_ComparesCorrectly(int x, int y, int expectedSign)
    {
        var comparer = PropertyComparerHelpers.TypeComparers[typeof(int)];
        var result = comparer(x, y);
        Assert.Equal(expectedSign, Math.Sign(result));
    }

    [Theory]
    [InlineData(1.0, 2.0, -1)]
    [InlineData(2.0, 1.0, 1)]
    [InlineData(3.14, 3.14, 0)]
    public void TypeComparers_Double_ComparesCorrectly(double x, double y, int expectedSign)
    {
        var comparer = PropertyComparerHelpers.TypeComparers[typeof(double)];
        var result = comparer(x, y);
        Assert.Equal(expectedSign, Math.Sign(result));
    }

    [Fact]
    public void TypeComparers_DateTime_ComparesCorrectly()
    {
        var comparer = PropertyComparerHelpers.TypeComparers[typeof(DateTime)];
        var earlier = new DateTime(2020, 1, 1);
        var later = new DateTime(2025, 1, 1);

        Assert.True(comparer(earlier, later) < 0);
        Assert.True(comparer(later, earlier) > 0);
        Assert.Equal(0, comparer(earlier, earlier));
    }

    [Fact]
    public void TypeComparers_Decimal_ComparesCorrectly()
    {
        var comparer = PropertyComparerHelpers.TypeComparers[typeof(decimal)];
        Assert.True(comparer(1.5m, 2.5m) < 0);
        Assert.True(comparer(2.5m, 1.5m) > 0);
        Assert.Equal(0, comparer(10.0m, 10.0m));
    }

    #endregion
}