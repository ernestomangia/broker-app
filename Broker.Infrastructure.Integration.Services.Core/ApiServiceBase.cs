using System.Text.Json;
using Broker.Infrastructure.Integration.Services.Core.Abstractions;
using Broker.Infrastructure.Integration.Services.Core.Exceptions;
using Broker.Infrastructure.Integration.Services.Core.Models;

namespace Broker.Infrastructure.Integration.Services.Core;

public abstract class ApiServiceBase : IApiServiceBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    protected ApiServiceBase(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    protected abstract string ServiceName { get; }

    public async Task<ApiResponseModel<TEntity>?> Get<TEntity>(string resourceUrl)
    {
        var client = _httpClientFactory.CreateClient(ServiceName);

        try
        {
            var response = await client.GetAsync(resourceUrl);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStreamAsync();

            var data = await JsonSerializer.DeserializeAsync<TEntity>(result);

            return new ApiResponseModel<TEntity>
            {
                Result = data,
                Response = response
            };
        }
        catch (HttpRequestException ex)
        {
            throw new ApiServiceException(ex.Message);
        }
    }
}