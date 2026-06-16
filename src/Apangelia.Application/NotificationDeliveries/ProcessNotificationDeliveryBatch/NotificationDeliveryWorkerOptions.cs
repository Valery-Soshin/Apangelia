namespace Apangelia.Application.NotificationDeliveries.ProcessNotificationDeliveryBatch;

/// <summary>
/// Настройки фонового worker-а доставки уведомлений.
/// </summary>
public sealed class NotificationDeliveryWorkerOptions
{
    /// <summary>
    /// Максимальное число доставок, закрепляемых за один проход.
    /// </summary>
    public required int BatchSize { get; init; }

    /// <summary>
    /// Интервал между проходами worker-а.
    /// </summary>
    public required TimeSpan PollingInterval { get; init; }

    /// <summary>
    /// Максимальное число попыток отправки перед переводом в мертвую очередь.
    /// </summary>
    public required int MaxAttempts { get; init; }

    /// <summary>
    /// Базовая задержка перед первой повторной попыткой отправки.
    /// </summary>
    public required TimeSpan InitialRetryDelay { get; init; }
}
