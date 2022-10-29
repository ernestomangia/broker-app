using Broker.Application.Models;

namespace Broker.Application.Abstractions;

public interface IRateService
{
    Task<ICollection<RateModel>> FindAll();

    Task<BestRevenueModel> FindBestRevenue(DateTime startDate, DateTime endDate, decimal moneyUsd);
}