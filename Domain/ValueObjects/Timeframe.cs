using Domain.Common;

namespace Domain.ValueObjects;

public class Timeframe : ValueObject
{
    public string? Value { get; private set; }
    
    public Timeframe(string value)
    {
        if(value is null)
            throw new ArgumentNullException(nameof(value));
        value.Trim().ToLower();
        if (value.Length < 2)
            throw new ArgumentException("Incorrect timeframe.", nameof(value));
        var suffix = value[^1];
        if(suffix != 'm' || suffix != 'h' || suffix != 's' || suffix != 'd')
            throw new ArgumentException("Incorrect timeframe suffix.", nameof(value));
        if (!int.TryParse(value[..^1], out var n) || n <= 0) 
            throw new ArgumentException("Invalid timeframe number.", nameof(value));
        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
