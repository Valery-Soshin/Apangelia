using Apangelia.Core;

namespace Apangelia.Application.Notifications.AcceptEvent;

/// <summary>
/// Стандартный обработчик команды приема нормализованного события уведомления.
/// </summary>
public sealed class AcceptNotificationEventCommandHandler : IAcceptNotificationEventCommandHandler
{
    private readonly INotificationInboxRepository _inboxRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AcceptNotificationEventCommandHandler(
        INotificationInboxRepository inboxRepository,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _inboxRepository = inboxRepository;
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<AcceptNotificationEventResult> HandleAsync(
        AcceptNotificationEventCommand command,
        CancellationToken cancellationToken)
    {
        return _unitOfWork.ExecuteInTransactionAsync(
            async transactionCancellationToken =>
            {
                var notificationEvent = command.NotificationEvent;
                var isNewEvent = await _inboxRepository.TryAddAsync(notificationEvent, transactionCancellationToken);

                if (!isNewEvent)
                {
                    return AcceptNotificationEventResult.Duplicate;
                }

                await _notificationRepository.AddAsync(MapToNotification(notificationEvent), transactionCancellationToken);

                return AcceptNotificationEventResult.Accepted;
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
