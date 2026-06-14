using Apangelia.Application.Repositories;
using Apangelia.Application.SeedWork;
using Apangelia.Core;

namespace Apangelia.Application.Commands.Notifications;

/// <summary>
/// Стандартный обработчик команды приема нормализованного события уведомления.
/// </summary>
public sealed class AcceptNotificationEventCommandHandler
    : ICommandHandler<AcceptNotificationEventCommand, AcceptNotificationEventResult>
{
    private readonly INotificationInboxRepository _inboxRepository;
    private readonly INotificationRepository _notificationRepository;

    public AcceptNotificationEventCommandHandler(
        INotificationInboxRepository inboxRepository,
        INotificationRepository notificationRepository)
    {
        _inboxRepository = inboxRepository;
        _notificationRepository = notificationRepository;
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

        await _notificationRepository.AddAsync(MapToNotification(notificationEvent), cancellationToken);

        return AcceptNotificationEventResult.Accepted;
    }

    private static Notification MapToNotification(NotificationEvent notificationEvent)
    {
        return new Notification
        {
            Source = notificationEvent.Source,
            EventType = notificationEvent.EventType,
            ExternalEventId = notificationEvent.ExternalEventId,
            Title = notificationEvent.Title,
            Message = notificationEvent.Message,
            OccurredAt = notificationEvent.OccurredAt,
            CreatedAt = DateTimeOffset.UtcNow
        };
    }
}
