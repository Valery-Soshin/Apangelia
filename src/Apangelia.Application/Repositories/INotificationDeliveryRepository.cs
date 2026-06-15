using Apangelia.Application.Notifications;
using Apangelia.Core;

namespace Apangelia.Application.Repositories;

/// <summary>
/// Сохраняет и выдает задания доставки уведомлений.
/// </summary>
public interface INotificationDeliveryRepository
{
    /// <summary>
    /// Закрепляет готовые к отправке задания доставки за worker-ом и создает начатые попытки.
    /// </summary>
    /// <param name="batchSize">Максимальное число заданий для закрепления.</param>
    /// <param name="claimedAt">Время закрепления заданий.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Закрепленные задания доставки с данными для отправки.</returns>
    Task<IReadOnlyCollection<ClaimedNotificationDelivery>> ClaimForProcessingAsync(
        int batchSize,
        DateTimeOffset claimedAt,
        CancellationToken cancellationToken);

    /// <summary>
    /// Добавляет набор заданий доставки уведомлений.
    /// </summary>
    /// <param name="deliveries">Задания доставки, созданные для уведомления.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task AddRangeAsync(
        IReadOnlyCollection<NotificationDelivery> deliveries,
        CancellationToken cancellationToken);

    /// <summary>
    /// Завершает попытку и помечает доставку успешно выполненной.
    /// </summary>
    /// <param name="deliveryId">Идентификатор задания доставки.</param>
    /// <param name="attemptId">Идентификатор попытки доставки.</param>
    /// <param name="completedAt">Время завершения попытки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task MarkDeliveredAsync(
        Guid deliveryId,
        Guid attemptId,
        DateTimeOffset completedAt,
        CancellationToken cancellationToken);

    /// <summary>
    /// Завершает попытку ошибкой и планирует следующую повторную отправку.
    /// </summary>
    /// <param name="deliveryId">Идентификатор задания доставки.</param>
    /// <param name="attemptId">Идентификатор попытки доставки.</param>
    /// <param name="completedAt">Время завершения попытки.</param>
    /// <param name="nextAttemptAt">Время следующей попытки доставки.</param>
    /// <param name="errorCode">Машиночитаемый код ошибки.</param>
    /// <param name="errorMessage">Диагностическое сообщение ошибки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task ScheduleRetryAsync(
        Guid deliveryId,
        Guid attemptId,
        DateTimeOffset completedAt,
        DateTimeOffset nextAttemptAt,
        string? errorCode,
        string? errorMessage,
        CancellationToken cancellationToken);

    /// <summary>
    /// Завершает попытку ошибкой и переводит доставку в мертвую очередь.
    /// </summary>
    /// <param name="deliveryId">Идентификатор задания доставки.</param>
    /// <param name="attemptId">Идентификатор попытки доставки.</param>
    /// <param name="completedAt">Время завершения попытки.</param>
    /// <param name="errorCode">Машиночитаемый код ошибки.</param>
    /// <param name="errorMessage">Диагностическое сообщение ошибки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task MoveToDeadLetterAsync(
        Guid deliveryId,
        Guid attemptId,
        DateTimeOffset completedAt,
        string? errorCode,
        string? errorMessage,
        CancellationToken cancellationToken);
}
