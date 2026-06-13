namespace Apangelia.Core;

/// <summary>
/// Историческая запись о попытке доставки уведомления.
/// </summary>
public sealed class NotificationDeliveryAttempt
{
    /// <summary>
    /// Уникальный идентификатор попытки доставки.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор задачи доставки, к которой относится попытка.
    /// </summary>
    public Guid NotificationDeliveryId { get; set; }

    /// <summary>
    /// Время начала попытки доставки.
    /// </summary>
    public DateTimeOffset StartedAt { get; set; }

    /// <summary>
    /// Время завершения попытки доставки.
    /// </summary>
    public DateTimeOffset? FinishedAt { get; set; }

    /// <summary>
    /// Код ошибки попытки доставки.
    /// </summary>
    public string? ErrorCode { get; set; }

    /// <summary>
    /// Сообщение об ошибке попытки доставки.
    /// </summary>
    public string? ErrorMessage { get; set; }
}
