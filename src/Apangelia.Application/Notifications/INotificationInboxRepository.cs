using Apangelia.Core;

namespace Apangelia.Application.Notifications;

/// <summary>
/// Сохраняет входящие события уведомлений с проверкой идемпотентности.
/// </summary>
public interface INotificationInboxRepository
{
    /// <summary>
    /// Пытается зарегистрировать входящее событие во входящем журнале.
    /// </summary>
    /// <param name="notificationEvent">Событие внешней интеграции.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns><see langword="true"/>, если событие новое; иначе <see langword="false"/>.</returns>
    Task<bool> TryAddAsync(NotificationEvent notificationEvent, CancellationToken cancellationToken);
}
