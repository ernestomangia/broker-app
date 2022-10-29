using Broker.Application.Abstractions;
using Broker.Application.Services;
using Broker.Infrastructure.Integration.Services.Abstractions.ERA;
using Broker.Infrastructure.Integration.Services.Services.ERA;

namespace Broker.Services.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Custom services
        services.AddTransient<IRateService, RateService>();

        // Exchange Rates Api services
        services.AddTransient<ITimeSeriesApiService, TimeSeriesApiService>();

        return services;
    }
}