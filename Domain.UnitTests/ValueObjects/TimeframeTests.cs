using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class TimeframeTests
{
    [Fact]
    public void Constructor_Throw_When_Value_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new Timeframe(null!));
    }

    [Theory]
    [InlineData("2")]
    [InlineData("")]
    [InlineData("1k")]
    [InlineData("@l")]
    [InlineData("-1s")]
    [InlineData("0d")]
    [InlineData("_5m")]
    [InlineData("0.2s")]
    [InlineData("5.0m")]
    [InlineData("      ")]
    [InlineData("15v")]
    public void Constructor_Throw_When_Timeframe_Invalid(string input)
    {
        Assert.ThrowsAny<ArgumentException>(() => new Timeframe(input));
    }

    [Theory]
    [InlineData("   5h", "5h")]
    [InlineData("15M", "15m")]
    [InlineData("   1D  ", "1d")]
    public void Constructor_Normolize_Timeframe(string input, string expected)
    {
        var timeframe = new Timeframe(input);
        Assert.Equal(expected, timeframe.Value);
    }
}
