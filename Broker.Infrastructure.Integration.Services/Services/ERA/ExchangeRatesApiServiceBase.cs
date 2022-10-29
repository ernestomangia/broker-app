using Broker.Infrastructure.Integration.Services.Core;

namespace Broker.Infrastructure.Integration.Services.Services.ERA;

public abstract class ExchangeRatesApiServiceBase : ApiServiceBase
{
    protected ExchangeRatesApiServiceBase(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }

    protected override string ServiceName => nameof(ExchangeRatesApiServiceBase);
}