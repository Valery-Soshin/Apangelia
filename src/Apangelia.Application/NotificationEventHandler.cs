using Apangelia.Core;

namespace Apangelia.Application;

/// <summary>
/// Стандартный обработчик входящих событий уведомлений.
/// </summary>
public sealed class NotificationEventHandler : INotificationEventHandler
{
    private readonly INotificationInboxRepository _inboxRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationEventHandler(
        INotificationInboxRepository inboxRepository,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _inboxRepository = inboxRepository;
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<NotificationEventHandlingResult> HandleAsync(
        NotificationEvent notificationEvent,
        CancellationToken cancellationToken)
    {
        return _unitOfWork.ExecuteInTransactionAsync(
            async transactionCancellationToken =>
            {
                var isNewEvent = await _inboxRepository.TryAddAsync(notificationEvent, transactionCancellationToken);

                if (!isNewEvent)
                {
                    return NotificationEventHandlingResult.Duplicate;
                }

                await _notificationRepository.AddAsync(MapToNotification(notificationEvent), transactionCancellationToken);

                return NotificationEventHandlingResult.Accepted;
            },
            cancellationToken);
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
