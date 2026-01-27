using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;
using Domain.Exceptions;

namespace Domain.Entities;

public class AnalyticsReport : BaseEntity
{
    private readonly List<Indicator> _indicators = new();
    public Guid CoinPairId { get; private set; }
    public CoinPair? CoinPair { get; private set; }
    public Probability? Probability { get; private set; }
    public IReadOnlyList<Indicator> Indicators => _indicators.AsReadOnly();

    public Indicator? GetIndicator(string name)
    {
        var indicator = _indicators.FirstOrDefault(ind => ind.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return indicator ?? throw new AbsenceIndicatorException(name);
    }

}
