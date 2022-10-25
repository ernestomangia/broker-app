using Broker.Application.Abstractions;
using Broker.Application.Core;
using Broker.Application.Models;
using Broker.Infrastructure;

namespace Broker.Application.Services;

public class RateService : ServiceBase, IRateService
{
    #region Constructor(s)

    public RateService(AppDbContext context)
        : base(context)
    {
    }

    #endregion

    public async Task<ICollection<RateModel>> FindAll()
    {
        return new List<RateModel>();
    }

    public async Task<BestRateModel> FindBest()
    {
        return new BestRateModel();
    }
}