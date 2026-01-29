using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;

namespace Domain.Entities;

public class AnalyticsReport : BaseEntity
{
    private readonly List<Indicator> _indicators = new();
    public Guid CoinPairId { get; private set; }
    public CoinPair CoinPair { get; private set; } //navigation for ef
    public Probability Probability { get; private set; }
    public Timeframe Timeframe { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset CandleOpen { get; private set; }
    public DateTimeOffset CandleClose { get; private set; }
    public IReadOnlyList<Indicator> Indicators => _indicators.AsReadOnly();

    public AnalyticsReport(Guid coinPairId,
                           Timeframe timeframe,
                           Probability probability,
                           DateTimeOffset createdAt,
                           DateTimeOffset candleOpen,
                           DateTimeOffset candleClose,
                           List<Indicator> indicators)
    {
        CoinPairId = coinPairId;
        Timeframe = timeframe ?? throw new ArgumentNullException(nameof(timeframe));
        Probability = probability ?? throw new ArgumentNullException(nameof(probability));
        CreatedAt = createdAt;
        if (candleOpen > candleClose)
            throw new ArgumentException("Invalid Candle timeframe");
        CandleOpen = candleOpen;
        CandleClose = candleClose;
        if(indicators is null)
            throw new ArgumentNullException(nameof(indicators));
        _indicators = indicators;
    }

    public Indicator? GetIndicator(string name)
    {
        var indicator = _indicators.FirstOrDefault(ind => ind.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return indicator ?? throw new ArgumentException($"Invalid indicator {nameof(name)}");
    }

}
