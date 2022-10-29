using Broker.Application.Abstractions;
using Broker.Application.Core;
using Broker.Application.Models;
using Broker.Common;
using Broker.Infrastructure;
using Broker.Infrastructure.Integration.Services.Abstractions.ERA;
using Microsoft.EntityFrameworkCore;

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

    public async Task<ICollection<RateModel>> FindAll()
    {
        return new List<RateModel>();
    }

    public async Task<BestRevenueModel> FindBestRevenue(DateTime startDate, DateTime endDate, decimal moneyUsd)
    {
        var outputCurrencyCodeTypes = new List<CurrencyCodeType>
        {
            CurrencyCodeType.RUB,
            CurrencyCodeType.EUR,
            CurrencyCodeType.GBP,
            CurrencyCodeType.JPY
        };

        // TODO: Get rates from db
        var dbEntities = await Context.Rates.ToListAsync();

        // Get rates from Api
        var apiTimeSeries = await _timeSeriesApiService.Get(
            startDate,
            endDate,
            CurrencyCodeType.USD,
            outputCurrencyCodeTypes);

        // TODO: Save rates to db

        // TODO: Consolidate db and api data

        var rates = dbEntities
            .Select(e => new RateModel())
            .ToList();

        rates = rates.OrderBy(e => e.Date).ToList();

        // Calculate best revenue
        var result = new BestRevenueModel
        {
            Rates = rates
        };

        if (!rates.Any())
            return result;

        var buyRubValue = rates[0].Rub;
        var buyEurValue = rates[0].Eur;
        var buyGbpValue = rates[0].Gbp;
        var buyJpyValue = rates[0].Jpy;

        result.BuyDate = rates[0].Date;
        result.SellDate = rates[0].Date;

        decimal maxRevenue = 0;

        for (var i = 1; i < rates.Count; i++)
        {
            // RUB
            var revenue = Math.Round(buyRubValue / rates[i].Rub, 2);

            if (rates[i].Rub > buyRubValue)
            {
                result.BuyDate = rates[i].Date;
                result.Tool = "RUB";

                buyRubValue = rates[i].Rub;
            }
            else if (revenue > maxRevenue)
            {
                result.SellDate = rates[i].Date;
                result.Tool = "RUB";

                maxRevenue = revenue;
            }

            // EUR
            revenue = Math.Round(buyEurValue / rates[i].Eur, 2);

            if (rates[i].Eur > buyEurValue)
            {
                result.BuyDate = rates[i].Date;
                result.Tool = "EUR";

                buyEurValue = rates[i].Eur;
            }
            else if (revenue > maxRevenue)
            {
                result.SellDate = rates[i].Date;
                result.Tool = "EUR";

                maxRevenue = revenue;
            }

            // GBP
            revenue = Math.Round(buyGbpValue / rates[i].Gbp, 2);

            if (rates[i].Gbp > buyGbpValue)
            {
                result.BuyDate = rates[i].Date;
                result.Tool = "GBP";

                buyGbpValue = rates[i].Gbp;
            }
            else if (revenue > maxRevenue)
            {
                result.SellDate = rates[i].Date;
                result.Tool = "GBP";

                maxRevenue = revenue;
            }

            // JPY
            revenue = Math.Round(buyJpyValue / rates[i].Jpy, 2);

            if (rates[i].Jpy > buyJpyValue)
            {
                result.BuyDate = rates[i].Date;
                result.Tool = "JPY";

                buyJpyValue = rates[i].Jpy;
            }
            else if (revenue > maxRevenue)
            {
                result.SellDate = rates[i].Date;
                result.Tool = "JPY";

                maxRevenue = revenue;
            }
        }

        result.Revenue = moneyUsd * (maxRevenue - 1);

        return result;
    }
}