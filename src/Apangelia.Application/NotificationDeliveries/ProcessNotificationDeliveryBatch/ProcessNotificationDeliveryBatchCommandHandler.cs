using Apangelia.Application.Shared.CommandBase;

namespace Apangelia.Application.NotificationDeliveries.ProcessNotificationDeliveryBatch;

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
