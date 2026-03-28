using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class IndicatorTests
{
    [Fact]
    public void Constructor_Throw_When_IndicatorName_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new Indicator(null, 1.5m, 0.14));
    }


}
