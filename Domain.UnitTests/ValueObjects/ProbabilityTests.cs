using Domain.ValueObjects;

namespace Domain.UnitTests.ValueObjects;

public class ProbabilityTests
{
    [Theory]
    [InlineData(-1, 0.2, 0.8)]
    [InlineData(0.5, 1.01, 0.5)]
    [InlineData(null, 0.55, null)]
    public void CheckProbability_Throw_When_Probability_Invalid(decimal inputUp, decimal inputDown, decimal inputFlat)
    {

        Assert.ThrowsAny<ArgumentException>(() => new Probability(inputUp, inputDown, inputFlat));
    }

    [Theory]
    [InlineData(0.05, 0.45, 0.03)]
    [InlineData(0.4, 0.5, 0.09)]
    public void Constructor_Throw_When_Probability_Sum_lower_1(decimal inputUp, decimal inputDown, decimal inputFlat)
    {
        Assert.ThrowsAny<ArgumentException>(() => new Probability(inputUp, inputDown, inputFlat));
    }
}
