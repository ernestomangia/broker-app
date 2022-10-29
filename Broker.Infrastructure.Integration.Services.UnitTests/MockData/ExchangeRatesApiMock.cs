using System.IO;

namespace Broker.Infrastructure.Integration.Services.UnitTests.MockData;

public class ExchangeRatesApiMock
{
    public static string GetTimeSeries()
    {
        string path = Path.Combine(Directory.GetCurrentDirectory(), @"MockData\timeseries-api-response.json");

        using var reader = new StreamReader(path);

        var json = reader.ReadToEnd();

        return json;
    }
}