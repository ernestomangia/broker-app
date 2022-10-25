using Broker.Application.Models;

namespace Broker.Application.Abstractions;

public interface IRateService
{
    Task<ICollection<RateModel>> FindAll();

    Task<BestRateModel> FindBest();
}