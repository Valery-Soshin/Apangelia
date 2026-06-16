using Apangelia.Application;
using Apangelia.Application.NotificationDeliveries.ProcessNotificationDeliveryBatch;
using Apangelia.Integrations.GitHub;
using Apangelia.Integrations.Telegram;
using Apangelia.Persistence.Postgres;
using Apangelia.WebApi.Workers;
using Microsoft.AspNetCore.HttpLogging;

namespace Apangelia.WebApi.Configurations;

public static class DependencyInjectionConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddHttpLogging();
        services.AddSwagger();

        services.AddApplicationModule();
        services.AddPostgresPersistenceModule(configuration.GetConnectionString("Postgres")!);
        services.AddGitHubIntegrationModule(configuration.GetSection("Integrations:GitHub"));
        services.AddTelegramIntegrationModule(configuration.GetSection("Integrations:Telegram"));

        services.AddTelegramWebhookRegistrationWorker(configuration.GetSection("Integrations:Telegram:WebhookRegistration"));
        services.AddNotificationDeliveryWorker(configuration.GetSection("BackgroundWorkers:NotificationDelivery"));

        return services;
    }

    private static IServiceCollection AddHttpLogging(this IServiceCollection services)
    {
        return services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.Duration;
        });
    }

    private static IServiceCollection AddTelegramWebhookRegistrationWorker(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        services.AddOptions<TelegramWebhookRegistrationOptions>()
            .Bind(configurationSection)
            .Validate(TelegramWebhookRegistrationOptions.IsValidRegistration, "Telegram webhook registration requires an absolute HTTPS public URL and at least one allowed update type when enabled.")
            .ValidateOnStart();

        services.AddHostedService<TelegramWebhookRegistrationWorker>();

        return services;
    }

    private static IServiceCollection AddNotificationDeliveryWorker(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        services.AddOptions<NotificationDeliveryWorkerOptions>()
            .Bind(configurationSection)
            .Validate(options => options.BatchSize > 0, "Notification delivery worker batch size must be positive.")
            .Validate(options => options.PollingInterval > TimeSpan.Zero, "Notification delivery worker polling interval must be positive.")
            .Validate(options => options.MaxAttempts > 0, "Notification delivery worker max attempts must be positive.")
            .Validate(options => options.InitialRetryDelay > TimeSpan.Zero, "Notification delivery worker initial retry delay must be positive.")
            .ValidateOnStart();

        services.AddHostedService<NotificationDeliveryWorker>();

        return services;
    }
}
