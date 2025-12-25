namespace Domain.Exceptions;

public class InvalidConfidenceException : Exception
{
    public InvalidConfidenceException(double val)
        :base($"Value {val} is incorrect for level of confidence."){ }
}
