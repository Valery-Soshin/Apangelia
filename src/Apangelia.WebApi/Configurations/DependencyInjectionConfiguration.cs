using Apangelia.Core;
using Apangelia.WebApi.Configurations;
using Microsoft.AspNetCore.HttpLogging;

namespace Apangelia.WebApi.Configurations;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddHttpLogging();
        services.AddSwagger();
        services.AddEntityFramework(configuration);
        services.AddApplicationServices();

        return services;
    }

    private static IServiceCollection AddHttpLogging(this IServiceCollection services)
    {
        return services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.Duration;
        });
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services;
    }
}