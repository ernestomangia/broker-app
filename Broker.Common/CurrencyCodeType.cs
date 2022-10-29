using System.Text.Json.Serialization;

namespace Broker.Common;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CurrencyCodeType
{
    USD = 1,
    EUR = 2,
    RUB = 3,
    GBP = 4,
    JPY = 5
}