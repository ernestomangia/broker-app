using System.Net;

namespace Broker.Services.WebApi.Models;

public class ExceptionModel
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
    public DateTime DateTime { get; set; }
}