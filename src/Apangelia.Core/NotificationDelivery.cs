namespace Apangelia.Core;

/// <summary>
/// Конкретная задача доставки уведомления во внешний канал.
/// </summary>
public sealed class NotificationDelivery
{
    /// <summary>
    /// Уникальный идентификатор задачи доставки.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор уведомления, для которого создана доставка.
    /// </summary>
    public Guid NotificationId { get; set; }

    /// <summary>
    /// Идентификатор маршрута, по которому создана доставка.
    /// </summary>
    public Guid RouteId { get; set; }

    /// <summary>
    /// Текущее состояние задачи доставки.
    /// </summary>
    public NotificationDeliveryStatus Status { get; set; }

    /// <summary>
    /// Время следующей попытки доставки, если она отложена.
    /// </summary>
    public DateTimeOffset? NextAttemptAt { get; set; }

    /// <summary>
    /// Время последней начатой попытки доставки.
    /// </summary>
    public DateTimeOffset? LastAttemptAt { get; set; }

    /// <summary>
    /// Время успешной доставки уведомления.
    /// </summary>
    public DateTimeOffset? DeliveredAt { get; set; }

    /// <summary>
    /// Время окончательного сбоя доставки.
    /// </summary>
    public DateTimeOffset? FailedAt { get; set; }

    /// <summary>
    /// Количество выполненных попыток доставки.
    /// </summary>
    public int AttemptCount { get; set; }

    /// <summary>
    /// Максимальное количество попыток доставки.
    /// </summary>
    public int MaxAttempts { get; set; }

    /// <summary>
    /// Время создания задачи доставки.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Время последнего изменения задачи доставки.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }
}
