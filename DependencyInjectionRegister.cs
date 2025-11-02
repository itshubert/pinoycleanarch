using Mapster;
using Microsoft.Extensions.DependencyInjection;
using PinoyCleanArch.Infrastructure.Interceptors;

namespace PinoyCleanArch;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        services.AddMappings();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<PublishDomainEventsInterceptor>();

        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(AppDomain.CurrentDomain.GetAssemblies());

        services.AddSingleton(config);
        services.AddMapster();

        return services;
    }
}