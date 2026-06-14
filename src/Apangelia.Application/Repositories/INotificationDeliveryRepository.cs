using Apangelia.Core;

namespace Apangelia.Application.Repositories;

/// <summary>
/// Сохраняет задания доставки уведомлений.
/// </summary>
public interface INotificationDeliveryRepository
{
    /// <summary>
    /// Добавляет набор заданий доставки уведомлений.
    /// </summary>
    /// <param name="deliveries">Задания доставки, созданные для уведомления.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task AddRangeAsync(
        IReadOnlyCollection<NotificationDelivery> deliveries,
        CancellationToken cancellationToken);
}
