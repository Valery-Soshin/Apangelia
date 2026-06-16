using Apangelia.Core;

namespace Apangelia.Application.NotificationDeliveries;

/// <summary>
/// Состояние доставки и ее попытки, загруженное для сохранения результата обработки.
/// </summary>
/// <param name="Delivery">Задача доставки, состояние которой обновляется.</param>
/// <param name="Attempt">Попытка доставки, результат которой фиксируется.</param>
public sealed record NotificationDeliveryAttemptState(
    NotificationDelivery Delivery,
    NotificationDeliveryAttempt Attempt);
