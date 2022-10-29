using System.Text.Json.Serialization;
using Broker.Common;

namespace Broker.Infrastructure.Integration.Services.Models.ERA;

public class TimeSeriesApiResponseModel
{
    [JsonPropertyName("success")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("timeseries")]
    public bool IsTimeSeries { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("base")]
    public CurrencyCodeType Base { get; set; }

    [JsonPropertyName("rates")]
    public Dictionary<DateTime, Dictionary<CurrencyCodeType, decimal>> Rates { get; set; }
}