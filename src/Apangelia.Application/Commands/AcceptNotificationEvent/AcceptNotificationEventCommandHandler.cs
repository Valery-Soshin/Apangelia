using Apangelia.Application.NotificationDeliveries;
using Apangelia.Application.Notifications;
using Apangelia.Application.NotificationRoutes;
using Apangelia.Application.SeedWork;
using Apangelia.Core;
using Apangelia.Application.NotificationInboxes;

namespace Apangelia.Application.Commands.AcceptNotificationEvent;

/// <summary>
/// Стандартный обработчик команды приема нормализованного события уведомления.
/// </summary>
public sealed class AcceptNotificationEventCommandHandler
    : ICommandHandler<AcceptNotificationEventCommand, AcceptNotificationEventResult>
{
    private const int DefaultMaxDeliveryAttempts = 5;

    // Временная заглушка до появления сущности User и контекста текущего пользователя.
    private static readonly Guid TemporaryUserId = Guid.Parse("019ec79d-b1ec-7dc7-a9f7-e96899574d7b");

    private readonly INotificationDeliveryRepository _deliveryRepository;
    private readonly INotificationInboxRepository _inboxRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly INotificationRouteRepository _routeRepository;

    public AcceptNotificationEventCommandHandler(
        INotificationInboxRepository inboxRepository,
        INotificationRepository notificationRepository,
        INotificationRouteRepository routeRepository,
        INotificationDeliveryRepository deliveryRepository)
    {
        _inboxRepository = inboxRepository;
        _notificationRepository = notificationRepository;
        _routeRepository = routeRepository;
        _deliveryRepository = deliveryRepository;
    }

    public async Task<AcceptNotificationEventResult> HandleAsync(
        AcceptNotificationEventCommand command,
        CancellationToken cancellationToken)
    {
        var notificationInbox = new NotificationInbox(
            command.Source,
            command.EventType,
            command.ExternalEventId,
            command.RawPayloadJson,
            command.OccurredAt);

        var isNewEvent = await _inboxRepository.TryAddAsync(notificationInbox, cancellationToken);
        if (!isNewEvent)
        {
            return AcceptNotificationEventResult.Duplicate;
        }

        var currentDate = DateTimeOffset.UtcNow;

        var notification = MapToNotification(command, currentDate);

        await _notificationRepository.AddAsync(notification, cancellationToken);

        var routes = await _routeRepository.GetByUserAndInputProviderAsync(
            TemporaryUserId,
            command.Source,
            cancellationToken);

        if (routes.Count > 0)
        {
            var deliveries = routes
                .Select(route => MapToDelivery(notification.Id, route.Id, currentDate))
                .ToArray();

            await _deliveryRepository.AddRangeAsync(deliveries, cancellationToken);
        }

        return AcceptNotificationEventResult.Accepted;
    }

    private static Notification MapToNotification(AcceptNotificationEventCommand command, DateTimeOffset createdAt)
    {
        return new Notification
        {
            Source = command.Source,
            EventType = command.EventType,
            ExternalEventId = command.ExternalEventId,
            Title = command.Title,
            Message = command.Message,
            OccurredAt = command.OccurredAt,
            CreatedAt = createdAt
        };
    }

    private static NotificationDelivery MapToDelivery(
        Guid notificationId,
        Guid routeId,
        DateTimeOffset createdAt)
    {
        return new NotificationDelivery
        {
            NotificationId = notificationId,
            RouteId = routeId,
            Status = NotificationDeliveryStatus.Pending,
            MaxAttempts = DefaultMaxDeliveryAttempts,
            CreatedAt = createdAt
        };
    }
}
