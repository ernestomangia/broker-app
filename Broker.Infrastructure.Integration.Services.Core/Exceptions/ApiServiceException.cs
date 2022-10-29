namespace Broker.Infrastructure.Integration.Services.Core.Exceptions;

public class ApiServiceException : ApplicationException
{
    public ApiServiceException() { }

    public ApiServiceException(string message) 
        : base(message)
    {
    }
}