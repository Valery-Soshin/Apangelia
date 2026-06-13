using Apangelia.Application;
using Apangelia.Integrations.GitHub;
using Apangelia.Persistence.Postgres;
using Apangelia.Persistence.Postgres.Repositories;
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
        services.AddPostgresPersistenceServices();
        services.AddGitHubIntegration(configuration);

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
        services.AddScoped<INotificationEventHandler, NotificationEventHandler>();

        return services;
    }

    private static IServiceCollection AddPostgresPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationInboxRepository, PostgresNotificationInboxRepository>();
        services.AddScoped<INotificationRepository, PostgresNotificationRepository>();
        services.AddScoped<IUnitOfWork, PostgresUnitOfWork>();

        return services;
    }

    private static IServiceCollection AddGitHubIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<GitHubWebhookOptions>()
            .Bind(configuration.GetSection("Integrations:GitHub"))
            .Validate(options => !string.IsNullOrWhiteSpace(options.WebhookSecret), "GitHub webhook secret must be configured.")
            .ValidateOnStart();

        services.AddScoped<IGitHubWebhookReceiver, GitHubWebhookReceiver>();
        services.AddScoped<IGitHubWebhookHandler, GitHubWebhookHandler>();

        return services;
    }
}
