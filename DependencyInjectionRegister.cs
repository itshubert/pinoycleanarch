using System.Reflection;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using PinoyCleanArch.Infrastructure.Interceptors;

namespace PinoyCleanArch;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddApplication(this IServiceCollection services, params Assembly[] assemblies)
    {
        // If no assemblies provided, scan all loaded assemblies to find handlers
        var assembliesToScan = assemblies.Length > 0
            ? assemblies.Concat(new[] { typeof(DependencyInjectionRegister).Assembly }).Distinct().ToArray()
            : AppDomain.CurrentDomain.GetAssemblies().ToArray();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assembliesToScan));
        services.AddMappings(assembliesToScan);
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<PublishDomainEventsInterceptor>();

        return services;
    }

    private static IServiceCollection AddMappings(this IServiceCollection services, Assembly[] assemblies)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(assemblies);

        services.AddSingleton(config);
        services.AddMapster();

        return services;
    }
}