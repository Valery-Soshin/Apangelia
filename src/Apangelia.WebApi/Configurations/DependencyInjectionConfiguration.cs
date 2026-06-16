using Apangelia.Application.NotificationProviders;
using Apangelia.Integrations.GitHub;
using Apangelia.Integrations.Telegram;
using Apangelia.Persistence.Postgres;
using Apangelia.Persistence.Postgres.Repositories;
using Apangelia.WebApi.Workers;
using Microsoft.AspNetCore.HttpLogging;
using Apangelia.Application.NotificationDeliveries.ProcessNotificationDeliveryBatch;
using Apangelia.Application.Abstractions.Persistence;
using Apangelia.Application.Abstractions.Providers;
using Apangelia.Application.Notifications.AcceptNotification;
using Apangelia.Application.Notifications.RegisterNotificationRoute;
using Apangelia.Application.Shared.PipelineBehaviors;
using Apangelia.Application.Shared.CommandBase;

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
        services.AddTelegramIntegration(configuration);
        services.AddNotificationDeliveryWorker(configuration);

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
        services.AddScoped<ICommandDispatcher, CommandDispatcher>();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));
        services.AddScoped<ICommandHandler<AcceptNotificationCommand, AcceptNotificationEventResult>, AcceptNotificationEventCommandHandler>();
        services.AddScoped<ICommandHandler<RegisterNotificationRouteCommand, RegisterNotificationRouteResult>, RegisterNotificationRouteCommandHandler>();
        services.AddScoped<ICommandHandler<ProcessNotificationDeliveryBatchCommand, int>, ProcessNotificationDeliveryBatchCommandHandler>();
        services.AddScoped<INotificationProviderResolver, NotificationProviderResolver>();
        services.AddScoped<NotificationDeliveryProcessor>();

        return services;
    }

    private static IServiceCollection AddPostgresPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<INotificationInboxRepository, PostgresNotificationInboxRepository>();
        services.AddScoped<INotificationRepository, PostgresNotificationRepository>();
        services.AddScoped<INotificationRouteRepository, PostgresNotificationRouteRepository>();
        services.AddScoped<INotificationDeliveryRepository, PostgresNotificationDeliveryRepository>();
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

    private static IServiceCollection AddTelegramIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<TelegramNotificationOptions>()
            .Bind(configuration.GetSection("Integrations:Telegram"))
            .Validate(TelegramNotificationOptions.IsValidApiBaseUrl, "Telegram API base URL must be an absolute HTTPS URL.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.BotToken), "Telegram bot token must be configured.")
            .Validate(TelegramNotificationOptions.IsValidWebhookSecretToken, "Telegram webhook secret token must contain 1-256 characters: A-Z, a-z, 0-9, _ or -.")
            .ValidateOnStart();

        services.AddOptions<TelegramWebhookRegistrationOptions>()
            .Bind(configuration.GetSection("Integrations:Telegram:WebhookRegistration"))
            .Validate(TelegramWebhookRegistrationOptions.IsValidRegistration, "Telegram webhook registration requires an absolute HTTPS public URL and at least one allowed update type when enabled.")
            .ValidateOnStart();

        services.AddHttpClient<ITelegramBotClient, TelegramBotClient>()
            .RemoveAllLoggers();

        services.AddScoped<INotificationProvider, TelegramNotificationProvider>();
        services.AddScoped<ITelegramWebhookReceiver, TelegramWebhookReceiver>();
        services.AddScoped<ITelegramWebhookHandler, TelegramWebhookHandler>();
        services.AddHostedService<TelegramWebhookRegistrationWorker>();

        return services;
    }

    private static IServiceCollection AddNotificationDeliveryWorker(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<NotificationDeliveryWorkerOptions>()
            .Bind(configuration.GetSection("BackgroundWorkers:NotificationDelivery"))
            .Validate(options => options.BatchSize > 0, "Notification delivery worker batch size must be positive.")
            .Validate(options => options.PollingInterval > TimeSpan.Zero, "Notification delivery worker polling interval must be positive.")
            .Validate(options => options.MaxAttempts > 0, "Notification delivery worker max attempts must be positive.")
            .Validate(options => options.InitialRetryDelay > TimeSpan.Zero, "Notification delivery worker initial retry delay must be positive.")
            .ValidateOnStart();

        services.AddHostedService<NotificationDeliveryWorker>();

        return services;
    }
}
