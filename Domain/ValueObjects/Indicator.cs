using Domain.Common;
using Domain.Exceptions;

namespace Domain.ValueObjects;

public class Indicator : ValueObject
{
    public string Name { get; }
    public double Value { get; }
    public string Status { get; }
    public string Explanation { get; } = string.Empty;

    private static readonly Dictionary<string, Func<double, bool>> _validationRules =
        new(StringComparer.OrdinalIgnoreCase)
    {
        ["RSI"] = val => val is >= 0 and <= 100,
        ["FearGreed"] = val => val is >= 0 and <= 100,
        ["MACD"] = _ => true 
    };

    public Indicator(string name, double value, string status, string explanation)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Value = ValidateValue(name, value);
        Status = status;
        Explanation = explanation;
    }

    private double ValidateValue(string name, double val)
    {
        if(!_validationRules.TryGetValue(name, out var validator))
            throw new UnsupportedIndicatorException(name);
        if (!validator(val))
            throw new InvalidIndexException(name, val);
        return val;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Value;
        yield return Status;
        yield return Explanation;
    }
}
