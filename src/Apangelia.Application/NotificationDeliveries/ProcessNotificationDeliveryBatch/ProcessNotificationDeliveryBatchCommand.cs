using Apangelia.Application.Shared.CommandBase;

namespace Apangelia.Application.NotificationDeliveries.ProcessNotificationDeliveryBatch;

/// <summary>
/// Команда обработки одной пачки готовых доставок уведомлений.
/// </summary>
public sealed record ProcessNotificationDeliveryBatchCommand
    : ICommand<int>, INonTransactionalCommand;
