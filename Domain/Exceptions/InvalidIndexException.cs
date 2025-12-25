namespace Domain.Exceptions;

public class InvalidIndexException : Exception
{
    public InvalidIndexException(string name, double value) 
        : base($"Value {value} is incorrect for index {name}.") { }
}
