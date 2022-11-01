namespace Broker.Infrastructure.Integration.Services.Core.Models;

public class ApiResponseModel<TEntity> : ApiResponseModelBase
{
    public TEntity Result { get; set; }
}