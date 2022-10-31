using Broker.Common;
using Broker.Infrastructure.Integration.Services.Models.ERA;

namespace Broker.Infrastructure.Integration.Services.Abstractions.ERA;

public interface ITimeSeriesApiService
{
    Task<TimeSeriesApiResponseModel> Get(
        DateTime startDate,
        DateTime endDate,
        CurrencyCodeType? baseCurrencyCodeType = null,
        IEnumerable<CurrencyCodeType>? outputCurrencyCodeTypes = null);
}