using Apangelia.Core;

namespace Apangelia.Application.NotificationRoutes;

/// <summary>
/// Читает маршруты доставки уведомлений, доступные слою приложения.
/// </summary>
public interface INotificationRouteRepository
{
    /// <summary>
    /// Возвращает маршруты пользователя для входящего провайдера уведомления.
    /// </summary>
    /// <param name="userId">Идентификатор владельца маршрутов.</param>
    /// <param name="inputProviderId">Идентификатор входящего провайдера уведомления.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Маршруты, подходящие для входящего уведомления.</returns>
    Task<IReadOnlyCollection<NotificationRoute>> GetByUserAndInputProviderAsync(
        Guid userId,
        string inputProviderId,
        CancellationToken cancellationToken);
}
