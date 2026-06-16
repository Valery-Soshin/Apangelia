using Apangelia.Application.SeedWork;

namespace Apangelia.Application.Commands.ProcessNotificationDeliveryBatch;

/// <summary>
/// Команда обработки одной пачки готовых доставок уведомлений.
/// </summary>
public sealed record ProcessNotificationDeliveryBatchCommand
    : ICommand<int>, INonTransactionalCommand;
