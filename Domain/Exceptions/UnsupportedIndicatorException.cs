namespace Domain.Exceptions;

public class UnsupportedIndicatorException : Exception
{
    public UnsupportedIndicatorException(string name)
        : base($"Indicator {name} is unsupported.")
    { }
}
