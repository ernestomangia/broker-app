using Broker.Application.Abstractions;
using Broker.Application.Core;
using Broker.Application.Core.Exceptions;
using Broker.Application.Models;
using Broker.Common;
using Broker.Domain;
using Broker.Infrastructure;
using Broker.Infrastructure.Integration.Services.Abstractions.ERA;
using Broker.Infrastructure.Integration.Services.Models.ERA;

namespace Broker.Application.Services;

public class RateService : ServiceBase, IRateService
{
    private readonly ITimeSeriesApiService _timeSeriesApiService;

    #region Constructor(s)

    public RateService(
        AppDbContext context,
        ITimeSeriesApiService timeSeriesApiService)
        : base(context)
    {
        _timeSeriesApiService = timeSeriesApiService;
    }

    #endregion

    #region Private Member(s)

    private static CurrencyCodeType DefaultBaseCurrencyCodeType => CurrencyCodeType.USD;

    private static IEnumerable<CurrencyCodeType> DefaultOutputCurrencyCodeTypes => new List<CurrencyCodeType>
    {
        CurrencyCodeType.RUB,
        CurrencyCodeType.EUR,
        CurrencyCodeType.GBP,
        CurrencyCodeType.JPY
    };

    #endregion

    public async Task<BestRevenueModel> FindBestRevenue(
        DateTime startDate,
        DateTime endDate,
        decimal moneyUsd)
    {
        if (endDate <= startDate)
            throw new DataValidationException("'endDate' must be greater than 'startDate'");

        if (moneyUsd < 0)
            throw new DataValidationException("'moneyUsd' must be greater than or equal to 0");

        var rateEntities = await GetRatesByDateRange(startDate, endDate);

        var result = new BestRevenueModel
        {
            Rates = MapToRateModels(rateEntities)
                .ToList()
        };

        var buyDates = new Dictionary<CurrencyCodeType, DateTime>();
        var buyValues = new Dictionary<CurrencyCodeType, decimal>();

        var firstRate = rateEntities[0];

        foreach (var rateValue in firstRate.Values)
        {
            buyDates[rateValue.TargetCurrencyCodeType] = firstRate.Date;
            buyValues[rateValue.TargetCurrencyCodeType] = rateValue.Value;
        }

        decimal maxRevenueAmount = 0;

        // Calculate best revenue
        for (var i = 1; i < rateEntities.Count; i++)
        {
            var rate = rateEntities[i];

            // Loop over each rate pair (RUB/USD, EUR/USD, etc) 
            foreach (var targetCurrencyCode in DefaultOutputCurrencyCodeTypes)
            {
                var rateValue = rate.Values
                    .Single(e => e.SourceCurrencyCodeType == DefaultBaseCurrencyCodeType
                                 && e.TargetCurrencyCodeType == targetCurrencyCode);

                var revenuePercent = buyValues[rateValue.TargetCurrencyCodeType] / rateValue.Value;
                var fee = (int)Math.Ceiling(rate.Date.Subtract(buyDates[rateValue.TargetCurrencyCodeType]).TotalDays);
                var revenueAmount = moneyUsd * (revenuePercent - 1) - fee;

                if (rateValue.Value > buyValues[rateValue.TargetCurrencyCodeType])
                {
                    // If the current Rate Value is greater than the previous Buy Value,
                    // then it means it's a better value to buy
                    buyDates[rateValue.TargetCurrencyCodeType] = rate.Date;
                    buyValues[rateValue.TargetCurrencyCodeType] = rateValue.Value;
                }
                else if (revenueAmount > maxRevenueAmount)
                {
                    // If the revenue amount is greater than the previous max revenue,
                    // then keep the data associated to the current rate
                    maxRevenueAmount = revenueAmount;

                    result.BuyDate = buyDates[rateValue.TargetCurrencyCodeType];
                    result.SellDate = rate.Date;
                    result.Tool = rateValue.TargetCurrencyCodeType;
                }
            }
        }

        result.Revenue = maxRevenueAmount;

        return result;
    }

    #region Private Method(s)

    private async Task<IList<Rate>> GetRatesByDateRange(
        DateTime startDate,
        DateTime endDate,
        bool cacheNewData = true)
    {
        var needReordering = false;

        // Get rates data from db
        var ratesEntities = Context.Rates
            .Where(e => e.Date >= startDate
                        && e.Date <= endDate)
            .OrderBy(e => e.Date)
            .ToList();

        if (ratesEntities.Any())
        {
            // Check rates data retrieved from db and get missing dates from API if needed
            var firstRate = ratesEntities.FirstOrDefault();
            var lastRate = ratesEntities.LastOrDefault();

            if (firstRate?.Date > startDate)
            {
                // Get date interval [startDate; firstRate.Date - 1 day] from API
                var newRates = (await GetRatesFromExternalDataSource(
                    startDate,
                    firstRate.Date.AddDays(-1)))
                    .ToList();

                ratesEntities.AddRange(newRates);
                needReordering = true;

                // Cache data into db
                if (cacheNewData)
                    await SaveRates(newRates);
            }

            if (lastRate?.Date < endDate)
            {
                // Get date interval [lastRate.Date + 1 day; endDate] from API
                var newRates = (await GetRatesFromExternalDataSource(
                    lastRate.Date.AddDays(1),
                    endDate))
                    .ToList();

                ratesEntities.AddRange(newRates);
                needReordering = true;

                // Cache data into db
                if (cacheNewData)
                    await SaveRates(newRates);
            }
        }
        else
        {
            // Get all rates data from API
            var newRates = (await GetRatesFromExternalDataSource(
                startDate,
                endDate))
                .ToList();

            ratesEntities.AddRange(newRates);

            // Cache data into db
            if (cacheNewData)
                await SaveRates(newRates);
        }

        return needReordering
            ? ratesEntities
                .OrderBy(e => e.Date)
                .ToList()
            : ratesEntities;
    }

    private async Task<IEnumerable<Rate>> GetRatesFromExternalDataSource(
        DateTime startDate,
        DateTime endDate)
    {
        var apiTimeSeries = await _timeSeriesApiService.Get(
            startDate,
            endDate,
            DefaultBaseCurrencyCodeType,
            DefaultOutputCurrencyCodeTypes);

        return MapToRateEntities(apiTimeSeries);
    }

    private static IEnumerable<Rate> MapToRateEntities(TimeSeriesApiResponseModel timeSeries)
    {
        // TODO: improve this by using a mapper lib like AutoMapper
        return timeSeries.Rates
            .Select(e => new Rate
            {
                Date = e.Key,
                Values = e.Value.Select(v => new RateValue
                {
                    SourceCurrencyCodeType = timeSeries.Base,
                    TargetCurrencyCodeType = v.Key,
                    Value = v.Value
                }).ToList()
            });
    }

    private async Task SaveRates(IEnumerable<Rate> rates)
    {
        await Context.Rates.AddRangeAsync(rates);
        await Context.SaveChangesAsync();
    }

    private static IEnumerable<RateModel> MapToRateModels(IEnumerable<Rate> rates)
    {
        // TODO: improve this by using a mapper lib like AutoMapper
        return rates.Select(e => new RateModel
        {
            Date = e.Date,
            Rub = e.Values.Single(v => v.TargetCurrencyCodeType == CurrencyCodeType.RUB).Value,
            Eur = e.Values.Single(v => v.TargetCurrencyCodeType == CurrencyCodeType.EUR).Value,
            Gbp = e.Values.Single(v => v.TargetCurrencyCodeType == CurrencyCodeType.GBP).Value,
            Jpy = e.Values.Single(v => v.TargetCurrencyCodeType == CurrencyCodeType.JPY).Value
        });
    }

    #endregion
}