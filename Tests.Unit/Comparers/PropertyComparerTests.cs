using System.Reflection;
using Hviktor.Comparers;

namespace Tests.Unit.Comparers;

/// <summary>
/// Unit tests for <see cref="PropertyComparer{TItem}"/>.
/// </summary>
[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
[Trait(TestCollections.Traits.Category, TestCollections.Categories.Integrations)]
public class PropertyComparerTests
{
    private sealed class TestItem
    {
        public int IntValue { get; set; }
        public double DoubleValue { get; set; }
        public DateTime DateValue { get; set; }
        public decimal DecimalValue { get; set; }
        public int? NullableIntValue { get; set; }
        public string StringValue { get; set; } = string.Empty;
        public List<string> ListValue { get; set; } = [];
    }

    private static PropertyInfo GetProperty(string name)
        => typeof(TestItem).GetProperty(name)!;

    #region Null Item Handling

    [Fact]
    public void Compare_BothNull_ReturnsZero()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.IntValue)));
        Assert.Equal(0, comparer.Compare(null, null));
    }

    [Fact]
    public void Compare_XNull_ReturnsNegativeOne()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.IntValue)));
        Assert.Equal(-1, comparer.Compare(null, new TestItem()));
    }

    [Fact]
    public void Compare_YNull_ReturnsPositiveOne()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.IntValue)));
        Assert.Equal(1, comparer.Compare(new TestItem(), null));
    }

    #endregion

    #region Int Comparison

    [Fact]
    public void Compare_IntProperty_LessThan()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.IntValue)));
        var x = new TestItem { IntValue = 1 };
        var y = new TestItem { IntValue = 5 };
        Assert.True(comparer.Compare(x, y) < 0);
    }

    [Fact]
    public void Compare_IntProperty_GreaterThan()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.IntValue)));
        var x = new TestItem { IntValue = 10 };
        var y = new TestItem { IntValue = 3 };
        Assert.True(comparer.Compare(x, y) > 0);
    }

    [Fact]
    public void Compare_IntProperty_Equal()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.IntValue)));
        var x = new TestItem { IntValue = 7 };
        var y = new TestItem { IntValue = 7 };
        Assert.Equal(0, comparer.Compare(x, y));
    }

    #endregion

    #region Double Comparison

    [Fact]
    public void Compare_DoubleProperty_LessThan()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.DoubleValue)));
        var x = new TestItem { DoubleValue = 1.5 };
        var y = new TestItem { DoubleValue = 2.5 };
        Assert.True(comparer.Compare(x, y) < 0);
    }

    [Fact]
    public void Compare_DoubleProperty_Equal()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.DoubleValue)));
        var x = new TestItem { DoubleValue = 3.14 };
        var y = new TestItem { DoubleValue = 3.14 };
        Assert.Equal(0, comparer.Compare(x, y));
    }

    #endregion

    #region DateTime Comparison

    [Fact]
    public void Compare_DateTimeProperty_LessThan()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.DateValue)));
        var x = new TestItem { DateValue = new DateTime(2020, 1, 1) };
        var y = new TestItem { DateValue = new DateTime(2025, 1, 1) };
        Assert.True(comparer.Compare(x, y) < 0);
    }

    [Fact]
    public void Compare_DateTimeProperty_Equal()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.DateValue)));
        var date = new DateTime(2023, 6, 15);
        var x = new TestItem { DateValue = date };
        var y = new TestItem { DateValue = date };
        Assert.Equal(0, comparer.Compare(x, y));
    }

    #endregion

    #region Decimal Comparison

    [Fact]
    public void Compare_DecimalProperty_GreaterThan()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.DecimalValue)));
        var x = new TestItem { DecimalValue = 99.99m };
        var y = new TestItem { DecimalValue = 10.00m };
        Assert.True(comparer.Compare(x, y) > 0);
    }

    #endregion

    #region Nullable Property Values

    [Fact]
    public void Compare_NullableInt_BothNull_ReturnsZero()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.NullableIntValue)));
        var x = new TestItem { NullableIntValue = null };
        var y = new TestItem { NullableIntValue = null };
        Assert.Equal(0, comparer.Compare(x, y));
    }

    [Fact]
    public void Compare_NullableInt_XNull_ReturnsNegativeOne()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.NullableIntValue)));
        var x = new TestItem { NullableIntValue = null };
        var y = new TestItem { NullableIntValue = 5 };
        Assert.Equal(-1, comparer.Compare(x, y));
    }

    [Fact]
    public void Compare_NullableInt_YNull_ReturnsPositiveOne()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.NullableIntValue)));
        var x = new TestItem { NullableIntValue = 5 };
        var y = new TestItem { NullableIntValue = null };
        Assert.Equal(1, comparer.Compare(x, y));
    }

    #endregion

    #region String Comparison

    [Fact]
    public void Compare_StringProperty_LessThan()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.StringValue)));
        var x = new TestItem { StringValue = "Apple" };
        var y = new TestItem { StringValue = "Banana" };
        Assert.True(comparer.Compare(x, y) < 0);
    }

    [Fact]
    public void Compare_StringProperty_GreaterThan()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.StringValue)));
        var x = new TestItem { StringValue = "Banana" };
        var y = new TestItem { StringValue = "Apple" };
        Assert.True(comparer.Compare(x, y) > 0);
    }

    [Fact]
    public void Compare_StringProperty_Equal()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.StringValue)));
        var x = new TestItem { StringValue = "Same" };
        var y = new TestItem { StringValue = "Same" };
        Assert.Equal(0, comparer.Compare(x, y));
    }

    #endregion

    #region Enumerable Comparison

    [Fact]
    public void Compare_ListProperty_LessThan()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.ListValue)));
        var x = new TestItem { ListValue = ["A", "B"] };
        var y = new TestItem { ListValue = ["C", "D"] };
        Assert.True(comparer.Compare(x, y) < 0);
    }

    [Fact]
    public void Compare_ListProperty_Equal()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.ListValue)));
        var x = new TestItem { ListValue = ["A", "B"] };
        var y = new TestItem { ListValue = ["A", "B"] };
        Assert.Equal(0, comparer.Compare(x, y));
    }

    [Fact]
    public void Compare_ListProperty_GreaterThan()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.ListValue)));
        var x = new TestItem { ListValue = ["X", "Y"] };
        var y = new TestItem { ListValue = ["A", "B"] };
        Assert.True(comparer.Compare(x, y) > 0);
    }

    #endregion

    #region Value Type TItem Bug

    [Fact]
    public void Compare_IntProperty_BothDefaultValue_ComparesAsEqual()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.IntValue)));
        var x = new TestItem { IntValue = 0 };
        var y = new TestItem { IntValue = 0 };
        Assert.Equal(0, comparer.Compare(x, y));
    }

    [Fact]
    public void Compare_IntProperty_XDefaultValue_YNonDefault_ComparesCorrectly()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.IntValue)));
        var x = new TestItem { IntValue = 0 };
        var y = new TestItem { IntValue = 5 };
        Assert.True(comparer.Compare(x, y) < 0);
    }

    [Fact]
    public void Compare_IntProperty_XNonDefault_YDefaultValue_ComparesCorrectly()
    {
        var comparer = new PropertyComparer<TestItem>(GetProperty(nameof(TestItem.IntValue)));
        var x = new TestItem { IntValue = 5 };
        var y = new TestItem { IntValue = 0 };
        Assert.True(comparer.Compare(x, y) > 0);
    }

    #endregion
}