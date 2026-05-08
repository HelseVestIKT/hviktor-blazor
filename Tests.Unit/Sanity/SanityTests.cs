namespace Tests.Unit.Sanity;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
public class SanityTests
{
    [Fact]
    public void True_IsTrue()
    {
        Assert.True(true);
    }

    [Fact]
    public void False_IsFalse()
    {
        Assert.False(false);
    }

    [Fact]
    public void One_Equals_One()
    {
        Assert.Equal(1, 1);
    }

    [Fact]
    public void String_NotNull_OrEmpty()
    {
        const string value = "sanity";
        Assert.False(string.IsNullOrEmpty(value));
    }

    [Fact]
    public async Task Async_CompletesSuccessfully()
    {
        await Task.Delay(1, TestContext.Current.CancellationToken);
        Assert.True(true);
    }

    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(0, 0, 0)]
    [InlineData(-1, 1, 0)]
    public void Addition_Works(int a, int b, int expected)
    {
        Assert.Equal(expected, a + b);
    }
}