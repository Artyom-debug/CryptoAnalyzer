namespace Domain.Exceptions;

public class AbsenceIndicatorException : Exception
{
    public AbsenceIndicatorException(string name)
    : base($"Indicator {name} dont exist or not supported") { }
}
