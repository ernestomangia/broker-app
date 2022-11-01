namespace Broker.Infrastructure.Integration.Services.Core.Exceptions;

public class ApiServiceException : ApplicationException
{
    public ApiServiceException(string message)
        : base($"Api Service Error: {message}")
    {
    }
}