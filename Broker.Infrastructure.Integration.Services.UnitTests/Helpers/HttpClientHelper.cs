using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace Broker.Infrastructure.Integration.Services.UnitTests.Helpers;

public class HttpClientHelper
{
    public static Mock<IHttpClientFactory> GetFactory(string name, HttpClient httpClient)
    {
        var mockFactory = new Mock<IHttpClientFactory>();

        mockFactory
            .Setup(_ => _.CreateClient(name))
            .Returns(httpClient);

        return mockFactory;
    }

    public static Mock<HttpMessageHandler> GetResults(string response)
    {
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(response),
        };

        mockResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var mockHandler = new Mock<HttpMessageHandler>();

        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        return mockHandler;
    }
}