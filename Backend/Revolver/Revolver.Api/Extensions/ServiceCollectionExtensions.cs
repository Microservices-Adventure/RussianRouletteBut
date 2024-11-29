using Revolver.Api.Config;

namespace Revolver.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBaseServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static IServiceCollection AddConfigs(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ServicesParameters>(configuration.GetSection(nameof(ServicesParameters)));
        return services;
    }
}