using Apangelia.Application.Abstractions.Providers;
using Apangelia.Application.NotificationDeliveries.ProcessNotificationDeliveryBatch;
using Apangelia.Application.NotificationProviders;
using Apangelia.Application.Notifications.AcceptNotification;
using Apangelia.Application.Notifications.RegisterNotificationRoute;
using Apangelia.Application.Shared.CommandBase;
using Apangelia.Application.Shared.PipelineBehaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Apangelia.Application;

/// <summary>
/// Регистрирует зависимости слоя сценариев приложения.
/// </summary>
public static class ApplicationModule
{
    /// <summary>
    /// Добавляет обработчики команд, конвейер команд и прикладные сервисы.
    /// </summary>
    public static IServiceCollection AddApplicationModule(this IServiceCollection services)
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
}
