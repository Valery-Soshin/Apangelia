using Apangelia.Core;

namespace Apangelia.Application.Repositories;

/// <summary>
/// Сохраняет созданные уведомления.
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// Добавляет новое уведомление.
    /// </summary>
    /// <param name="notification">Созданное уведомление.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    Task AddAsync(Notification notification, CancellationToken cancellationToken);
}
