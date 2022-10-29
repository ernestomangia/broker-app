using System;
using System.Net.Http;
using System.Threading;
using Moq;
using Moq.Protected;

namespace Broker.Infrastructure.Integration.Services.UnitTests.Extensions;

public static class HttpMessageHandlerExtensions
{
    public static void Verify(this Mock<HttpMessageHandler> mock, Func<HttpRequestMessage, bool> match)
    {
        mock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req => match(req)),
            ItExpr.IsAny<CancellationToken>()
        );
    }
}