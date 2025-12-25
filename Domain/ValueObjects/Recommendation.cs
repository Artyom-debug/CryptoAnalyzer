using Domain.Common;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public class Recommendation : ValueObject
{
    public string Action { get; }
    public double Confidence { get; }
    public string Risk { get; }
    public string Summary { get; } = string.Empty;

    public Recommendation(string action, double confidence, string risk, string summary)
    {
        if (confidence > 1 || confidence < 0)
            throw new InvalidConfidenceException(confidence);
        Action = action;
        Confidence = confidence;
        Risk = risk;
        Summary = summary;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Action;
        yield return Confidence;
        yield return Risk;
        yield return Summary;
    }
}
