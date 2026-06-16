using Apangelia.Application.Notifications;
using Apangelia.Application.SeedWork;

namespace Apangelia.Application.Commands.ProcessNotificationDeliveryBatch;

/// <summary>
/// Обработчик команды обработки пачки доставок уведомлений.
/// </summary>
public sealed class ProcessNotificationDeliveryBatchCommandHandler
    : ICommandHandler<ProcessNotificationDeliveryBatchCommand, int>
{
    private readonly NotificationDeliveryProcessor _processor;

    public ProcessNotificationDeliveryBatchCommandHandler(NotificationDeliveryProcessor processor)
    {
        _processor = processor;
    }

    public Task<int> HandleAsync(
        ProcessNotificationDeliveryBatchCommand command,
        CancellationToken cancellationToken)
    {
        return _processor.ProcessNextBatchAsync(cancellationToken);
    }
}
