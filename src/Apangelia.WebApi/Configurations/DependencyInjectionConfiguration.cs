using Apangelia.Application.Commands.AcceptNotificationEvent;
using Apangelia.Application.Commands.ProcessNotificationDeliveryBatch;
using Apangelia.Application.PipelineBehaviors;
using Apangelia.Application.NotificationDeliveries;
using Apangelia.Application.NotificationProviders;
using Apangelia.Application.Notifications;
using Apangelia.Application.NotificationRoutes;
using Apangelia.Application.SeedWork;
using Apangelia.Integrations.GitHub;
using Apangelia.Integrations.Telegram;
using Apangelia.Persistence.Postgres;
using Apangelia.Persistence.Postgres.Repositories;
using Apangelia.WebApi.Workers;
using Microsoft.AspNetCore.HttpLogging;
using Apangelia.Application.NotificationInboxes;

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
        services.AddScoped<ICommandHandler<AcceptNotificationEventCommand, AcceptNotificationEventResult>, AcceptNotificationEventCommandHandler>();
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
        services.AddScoped<INotificationProvider, TelegramNotificationProvider>();

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
