using Hviktor.Extensions.Text;

namespace Tests.Unit.Extensions.Text;

[Trait(TestCollections.Traits.Category, "extensions")]
public class StringExtensionsTests
{
    #region SplitByCapitalization

    [Fact]
    public void SplitByCapitalization_NullInput_ReturnsEmptyList()
    {
        var result = ((string)null!).SplitByCapitalization();
        Assert.Empty(result);
    }

    [Fact]
    public void SplitByCapitalization_EmptyString_ReturnsEmptyList()
    {
        var result = string.Empty.SplitByCapitalization();
        Assert.Empty(result);
    }

    [Fact]
    public void SplitByCapitalization_SingleLowercaseWord_ReturnsSingleSegment()
    {
        var result = "hello".SplitByCapitalization();
        Assert.Single(result);
        Assert.Equal("hello", result[0]);
    }

    [Fact]
    public void SplitByCapitalization_SingleUppercaseWord_ReturnsSingleSegment()
    {
        var result = "HELLO".SplitByCapitalization();
        Assert.Single(result);
        Assert.Equal("HELLO", result[0]);
    }

    [Fact]
    public void SplitByCapitalization_SingleCharacter_ReturnsSingleSegment()
    {
        var result = "A".SplitByCapitalization();
        Assert.Single(result);
        Assert.Equal("A", result[0]);
    }

    [Theory]
    [InlineData("HelloWorld", new[] { "Hello", "World" })]
    [InlineData("helloWorld", new[] { "hello", "World" })]
    [InlineData("HelloWorldTest", new[] { "Hello", "World", "Test" })]
    public void SplitByCapitalization_CamelAndPascalCase_SplitsCorrectly(string input, string[] expected)
    {
        var result = input.SplitByCapitalization();
        Assert.Equal(expected, result);
    }

    [Fact]
    public void SplitByCapitalization_AllUppercase_ReturnsSingleSegment()
    {
        var result = "ABC".SplitByCapitalization();
        Assert.Single(result);
        Assert.Equal("ABC", result[0]);
    }

    [Fact]
    public void SplitByCapitalization_AllLowercase_ReturnsSingleSegment()
    {
        var result = "abc".SplitByCapitalization();
        Assert.Single(result);
        Assert.Equal("abc", result[0]);
    }

    [Fact]
    public void SplitByCapitalization_AcronymFollowedByWord_DoesNotSplitWithinUppercaseRun()
    {
        var result = "JSONParser".SplitByCapitalization();
        Assert.Single(result);
        Assert.Equal("JSONParser", result[0]);
    }

    [Fact]
    public void SplitByCapitalization_MixedCase_SplitsAtLowerToUpperTransition()
    {
        var result = "getHTTPResponse".SplitByCapitalization();
        Assert.Equal(["get", "HTTPResponse"], result);
    }

    [Fact]
    public void SplitByCapitalization_TwoCharacterSegments_SplitsCorrectly()
    {
        var result = "AaBb".SplitByCapitalization();
        Assert.Equal(["Aa", "Bb"], result);
    }

    #endregion
}