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
    /// Загружает состояние доставки и попытки для фиксации результата обработки.
    /// </summary>
    /// <param name="deliveryId">Идентификатор задания доставки.</param>
    /// <param name="attemptId">Идентификатор попытки доставки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Состояние доставки и попытки доставки.</returns>
    Task<NotificationDeliveryAttemptState> GetAttemptStateAsync(
        Guid deliveryId,
        Guid attemptId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Сохраняет измененное состояние доставки и попытки.
    /// </summary>
    /// <param name="state">Состояние доставки и попытки доставки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task SaveAttemptStateAsync(
        NotificationDeliveryAttemptState state,
        CancellationToken cancellationToken);
}
