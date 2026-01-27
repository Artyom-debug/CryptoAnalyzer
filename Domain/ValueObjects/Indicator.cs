using Domain.Common;

namespace Domain.ValueObjects;

public class Indicator : ValueObject
{
    public string Name { get; private set; }
    public decimal? Value { get; private set; }
    public double Importance { get; private set; }
    
    public Indicator(string name, decimal? value, double importance)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = value;
        if(importance < 0 || importance > 1) 
            throw new ArgumentOutOfRangeException(nameof(importance));
        Importance = importance;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Value;
        yield return Importance;
    }
}
