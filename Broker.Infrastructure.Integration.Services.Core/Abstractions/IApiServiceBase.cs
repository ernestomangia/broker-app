using Broker.Infrastructure.Integration.Services.Core.Models;

namespace Broker.Infrastructure.Integration.Services.Core.Abstractions;

public interface IApiServiceBase
{
    Task<ApiResponseModel<TEntity>> Get<TEntity>(string resourceUrl)
        where TEntity : new();
}