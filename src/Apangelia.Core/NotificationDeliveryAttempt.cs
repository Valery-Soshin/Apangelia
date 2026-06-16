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

    /// <summary>
    /// Помечает попытку доставки успешно завершенной.
    /// </summary>
    /// <param name="completedAt">Время завершения попытки доставки.</param>
    public void MarkSucceeded(DateTimeOffset completedAt)
    {
        FinishedAt = completedAt;
        ErrorCode = null;
        ErrorMessage = null;
    }

    /// <summary>
    /// Помечает попытку доставки завершенной с ошибкой.
    /// </summary>
    /// <param name="completedAt">Время завершения попытки доставки.</param>
    /// <param name="errorCode">Машиночитаемый код ошибки.</param>
    /// <param name="errorMessage">Диагностическое сообщение ошибки.</param>
    public void MarkFailed(DateTimeOffset completedAt, string? errorCode, string? errorMessage)
    {
        FinishedAt = completedAt;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}
