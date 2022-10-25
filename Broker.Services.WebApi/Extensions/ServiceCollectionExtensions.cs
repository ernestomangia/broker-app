using Broker.Application.Abstractions;
using Broker.Application.Services;

namespace Broker.Services.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(
        this IServiceCollection services)
    {
        services.AddTransient<IRateService, RateService>();

        return services;
    }
}