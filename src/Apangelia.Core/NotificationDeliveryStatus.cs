namespace Apangelia.Core;

/// <summary>
/// Состояние задачи доставки уведомления.
/// </summary>
public enum NotificationDeliveryStatus
{
    /// <summary>
    /// Доставка ожидает обработки.
    /// </summary>
    Pending,

    /// <summary>
    /// Доставка сейчас обрабатывается worker-ом.
    /// </summary>
    Processing,

    /// <summary>
    /// Уведомление успешно доставлено.
    /// </summary>
    Delivered,

    /// <summary>
    /// Следующая попытка доставки запланирована на будущее.
    /// </summary>
    RetryScheduled,

    /// <summary>
    /// Доставка завершилась ошибкой.
    /// </summary>
    Failed,

    /// <summary>
    /// Доставка переведена в хранилище необрабатываемых задач.
    /// </summary>
    DeadLetter
}
