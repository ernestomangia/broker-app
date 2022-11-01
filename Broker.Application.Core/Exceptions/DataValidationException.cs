namespace Broker.Application.Core.Exceptions;

public class DataValidationException : Exception
{
    public DataValidationException(string message)
        : base($"Data Validation Error: {message}")
    {
    }
}