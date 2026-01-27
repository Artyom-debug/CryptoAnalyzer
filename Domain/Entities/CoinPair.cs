using Domain.Common;

namespace Domain.Entities;

public class CoinPair : BaseEntity
{
    private CoinPair() {}

    public string? Symbol { get; private set; }

    public CoinPair(string symbol)
    {
        if (symbol is null )
            throw new ArgumentNullException(nameof(symbol));
        Symbol = symbol.Trim().ToUpper();
    }
}
