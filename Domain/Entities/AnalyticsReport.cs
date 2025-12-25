using Domain.Common;
using Domain.Events;
using Domain.ValueObjects;
using Domain.Exceptions;

namespace Domain.Entities;

public class AnalyticsReport : BaseAuditableEntity
{
    public string Coin { get; set; }
    public decimal CurrentPrice { get; set; }
    public double ChangePercent { get; set; }
    public Recommendation Summary { get; set; }

    private readonly List<Indicator> _indicators;
    public IReadOnlyList<Indicator> Indicators => _indicators.AsReadOnly();

    public Indicator? GetIndicator(string name)
    {
        var indicator = _indicators.FirstOrDefault(ind => ind.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return indicator ?? throw new AbsenceIndicatorException(name);
    }
    public AnalyticsReport(string coin,
                           decimal price,
                           double percent,
                           Recommendation summary,
                           List<Indicator> list)
    {
        Coin = coin ?? throw new ArgumentNullException(nameof(coin));
        CurrentPrice = price;
        ChangePercent = percent;
        Summary = summary ?? throw new ArgumentNullException(nameof(summary));
        _indicators = list ?? throw new ArgumentNullException(nameof(list));
    }
}
