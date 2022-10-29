using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Broker.Common;
using Broker.Infrastructure.Integration.Services.Services.ERA;
using Broker.Infrastructure.Integration.Services.UnitTests.Extensions;
using Broker.Infrastructure.Integration.Services.UnitTests.Helpers;
using Broker.Infrastructure.Integration.Services.UnitTests.MockData;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Broker.Infrastructure.Integration.Services.UnitTests.Tests;

public class TimeSeriesApiServiceTest
{
    public TimeSeriesApiServiceTest()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(@"appsettings.json", false, false)
            .AddEnvironmentVariables()
            .Build();

        ApiUrl = configuration.GetSection("Integrations:ExchangeRatesApi")["ApiUrl"] ?? string.Empty;
    }

    private string ApiUrl { get; set; }

    [Fact]
    public async Task GetTimeSeriesReturnsSuccess()
    {
        // Arrange
        var mockData = ExchangeRatesApiMock.GetTimeSeries();
        var mockHttpMessageHandler = HttpClientHelper.GetResults(mockData);

        var mockHttpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(ApiUrl)
        };

        var mockFactory = HttpClientHelper.GetFactory(nameof(ExchangeRatesApiServiceBase), mockHttpClient);

        var timeSeriesApiService = new TimeSeriesApiService(mockFactory.Object);

        // Act
        var startDate = new DateTime(2022, 3, 1);
        var endDate = new DateTime(2022, 3, 3);
        var baseCurrencyCodeType = CurrencyCodeType.USD;
        var outputCurrencyCodeTypes = new List<CurrencyCodeType>
        {
            CurrencyCodeType.RUB,
            CurrencyCodeType.EUR,
            CurrencyCodeType.GBP,
            CurrencyCodeType.JPY
        };

        var response = await timeSeriesApiService.Get(
            startDate,
            endDate,
            baseCurrencyCodeType,
            outputCurrencyCodeTypes);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.IsSuccess);
        Assert.True(response.IsTimeSeries);
        Assert.Equal(startDate, response.StartDate);
        Assert.Equal(endDate, response.EndDate);
        Assert.Equal(baseCurrencyCodeType, response.Base);

        Assert.Equal(3, response.Rates.Count);

        foreach (var rate in response.Rates)
        {
            Assert.True(rate.Value.ContainsKey(CurrencyCodeType.RUB));
            Assert.True(rate.Value.ContainsKey(CurrencyCodeType.EUR));
            Assert.True(rate.Value.ContainsKey(CurrencyCodeType.GBP));
            Assert.True(rate.Value.ContainsKey(CurrencyCodeType.JPY));
        }

        var startDateParamValue = startDate.ToString("yyyy-MM-dd");
        var endDateParamValue = endDate.ToString("yyyy-MM-dd");
        var baseAsString = Enum.GetName(typeof(CurrencyCodeType), baseCurrencyCodeType);
        var symbolsCommaSeparated = string.Join(',', outputCurrencyCodeTypes
            .Select(e => Enum.GetName(typeof(CurrencyCodeType), e)));

        mockHttpMessageHandler
            .Verify(req => req.Method == HttpMethod.Get
                           && req.RequestUri.AbsoluteUri.StartsWith(ApiUrl + "timeseries")
                           && req.RequestUri.Query.Contains($"start_date={startDateParamValue}")
                           && req.RequestUri.Query.Contains($"end_date={endDateParamValue}")
                           && req.RequestUri.Query.Contains($"base={baseAsString}")
                           && req.RequestUri.Query.Contains($"symbols={symbolsCommaSeparated}"));
    }
}