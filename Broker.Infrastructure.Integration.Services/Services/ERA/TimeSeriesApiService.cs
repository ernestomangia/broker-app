using Broker.Common;
using Broker.Common.Core.Extensions;
using Broker.Infrastructure.Integration.Services.Abstractions.ERA;
using Broker.Infrastructure.Integration.Services.Models.ERA;

namespace Broker.Infrastructure.Integration.Services.Services.ERA;

public class TimeSeriesApiService : ExchangeRatesApiServiceBase, ITimeSeriesApiService
{
    private const string TimeSeriesResourceUrl = "timeseries";

    public TimeSeriesApiService(IHttpClientFactory httpClientFactory)
        : base(httpClientFactory)
    {
    }

    public async Task<TimeSeriesApiResponseModel> Get(
        DateTime startDate,
        DateTime endDate,
        CurrencyCodeType? baseCurrencyCodeType = null,
        IEnumerable<CurrencyCodeType>? outputCurrencyCodeTypes = null)
    {
        var startDateParamValue = startDate.ToString("yyyy-MM-dd");
        var endDateParamValue = endDate.ToString("yyyy-MM-dd");

        var url = @$"{TimeSeriesResourceUrl}?start_date={startDateParamValue}&end_date={endDateParamValue}";

        if (baseCurrencyCodeType != null)
        {
            url += $"&base={Enum.GetName(typeof(CurrencyCodeType), baseCurrencyCodeType)}";
        }

        if (outputCurrencyCodeTypes != null)
        {
            var values = string.Join(',', outputCurrencyCodeTypes
                .Select(e => Enum.GetName(typeof(CurrencyCodeType), e)));

            url += $"&symbols={values}";
        }

        var entity = await Get<TimeSeriesApiResponseModel>(url);

        entity.ThrowIfNull();

        return entity.Result;
    }
}