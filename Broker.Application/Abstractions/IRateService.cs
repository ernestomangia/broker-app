using Broker.Application.Models;

namespace Broker.Application.Abstractions;

public interface IRateService
{
    Task<BestRevenueModel> FindBestRevenue(
        DateTime startDate, 
        DateTime endDate, 
        decimal moneyUsd);
}