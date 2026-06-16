using Apangelia.Application.Repositories;
using Apangelia.Application.SeedWork;
using Apangelia.Core;

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
        var notificationEvent = command.NotificationEvent;

        var isNewEvent = await _inboxRepository.TryAddAsync(notificationEvent, cancellationToken);
        if (!isNewEvent)
        {
            return AcceptNotificationEventResult.Duplicate;
        }

        var currentDate = DateTimeOffset.UtcNow;

        var notification = MapToNotification(notificationEvent, currentDate);

        await _notificationRepository.AddAsync(notification, cancellationToken);

        var routes = await _routeRepository.GetByUserAndInputProviderAsync(
            TemporaryUserId,
            notificationEvent.Source,
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

    private static Notification MapToNotification(NotificationEvent notificationEvent, DateTimeOffset createdAt)
    {
        return new Notification
        {
            Source = notificationEvent.Source,
            EventType = notificationEvent.EventType,
            ExternalEventId = notificationEvent.ExternalEventId,
            Title = notificationEvent.Title,
            Message = notificationEvent.Message,
            OccurredAt = notificationEvent.OccurredAt,
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
