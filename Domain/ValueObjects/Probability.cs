using Domain.Common;

namespace Domain.ValueObjects;

public class Probability : ValueObject
{
    public decimal ProbabilityUp { get; private set; }
    public decimal ProbabilityDown { get; private set; }
    public decimal ProbabilityFlat { get; private set; }

    public Probability(decimal probabilityUp, decimal probabilityDown, decimal probabilityFlat)
    {
        CheckProbability(probabilityUp);
        CheckProbability(probabilityDown);
        CheckProbability(probabilityFlat);
        var sum = probabilityUp + probabilityDown + probabilityFlat;
        if (Math.Abs(sum - 1m) > 0.001m)
            throw new ArgumentException($"Probabilities must sum ~1. Sum: {sum}.");
        ProbabilityUp = probabilityUp;
        ProbabilityDown = probabilityDown;
        ProbabilityFlat = probabilityFlat;
    }

    private void CheckProbability(decimal value)
    {
        if (value > 1 || value < 0)
            throw new ArgumentException("Invalid probability.");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProbabilityUp;
        yield return ProbabilityDown;
        yield return ProbabilityFlat;
    }
}
