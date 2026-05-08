using Hviktor.Abstractions.Enums.Attributes;
using Hviktor.Abstractions.Models;
using Hviktor.Extensions.Services;

namespace Tests.Unit.Extensions.Services;

[Trait(TestCollections.Traits.Collection, TestCollections.Generic)]
public class InputTypeServiceTests
{
    private readonly InputTypeService sut = new();

    #region GetDataAttribute

    [Theory]
    [InlineData(InputType.Text, "text")]
    [InlineData(InputType.TextArea, "textarea")]
    [InlineData(InputType.Checkbox, "checkbox")]
    [InlineData(InputType.Radio, "radio")]
    [InlineData(InputType.Button, "button")]
    [InlineData(InputType.File, "file")]
    [InlineData(InputType.Password, "password")]
    [InlineData(InputType.Email, "email")]
    [InlineData(InputType.Number, "number")]
    [InlineData(InputType.Hidden, "hidden")]
    [InlineData(InputType.Tel, "tel")]
    [InlineData(InputType.Url, "url")]
    [InlineData(InputType.Search, "search")]
    [InlineData(InputType.Date, "date")]
    [InlineData(InputType.Time, "time")]
    [InlineData(InputType.DateTimeLocal, "datetime-local")]
    [InlineData(InputType.Month, "month")]
    [InlineData(InputType.Week, "week")]
    [InlineData(InputType.Color, "color")]
    [InlineData(InputType.Submit, "submit")]
    [InlineData(InputType.Reset, "reset")]
    [InlineData(InputType.DescribedBy, "describedby")]
    [InlineData(InputType.LabelledBy, "labelledby")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_AllValues_ReturnsExpectedString(InputType type, string expected)
    {
        Assert.Equal(expected, InputTypeService.GetDataAttribute(type));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_DateTimeLocal_ReturnsHyphenatedForm()
    {
        Assert.Equal("datetime-local", InputTypeService.GetDataAttribute(InputType.DateTimeLocal));
    }

    #endregion

    #region GetDataAttribute (EnumValue)

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithTypedValue_ReturnsExpected()
    {
        EnumValue<InputType> value = InputType.Email;
        Assert.Equal("email", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithRawString_ReturnsExpected()
    {
        EnumValue<InputType> value = "Email";
        Assert.Equal("email", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_WithInvalidRawString_ReturnsDefault()
    {
        EnumValue<InputType> value = "NotAType";
        Assert.Equal("datetime-local", sut.GetDataAttribute(value));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetDataAttribute_EnumValue_Empty_ReturnsDefault()
    {
        EnumValue<InputType> value = default;
        Assert.Equal("datetime-local", sut.GetDataAttribute(value));
    }

    #endregion

    #region GetFromString

    [Theory]
    [InlineData("Text", InputType.Text)]
    [InlineData("text", InputType.Text)]
    [InlineData("EMAIL", InputType.Email)]
    [InlineData("DateTimeLocal", InputType.DateTimeLocal)]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_ValidValue_ReturnsParsedEnum(string input, InputType expected)
    {
        Assert.Equal(expected, sut.GetFromString(input));
    }

    [Theory]
    [InlineData("")]
    [InlineData("invalid")]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_ReturnsDefault(string input)
    {
        Assert.Equal(InputType.DateTimeLocal, sut.GetFromString(input));
    }

    [Fact]
    [Trait(TestCollections.Traits.Category, TestCollections.Categories.Attributes)]
    public void GetFromString_InvalidValue_WithCustomDefault_ReturnsCustomDefault()
    {
        Assert.Equal(InputType.Text, sut.GetFromString("invalid", InputType.Text));
    }

    #endregion
}