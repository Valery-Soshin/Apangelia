using Apangelia.Application.Abstractions.Persistence.Models;

namespace Apangelia.Application.Abstractions.Persistence;

/// <summary>
/// Сохраняет входящие события уведомлений с проверкой идемпотентности.
/// </summary>
public interface INotificationInboxRepository
{
    /// <summary>
    /// Пытается зарегистрировать входящее событие во входящем журнале.
    /// </summary>
    /// <param name="notificationInbox">Данные входящего события для проверки идемпотентности.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns><see langword="true"/>, если событие новое; иначе <see langword="false"/>.</returns>
    Task<bool> TryAddAsync(NotificationInbox notificationInbox, CancellationToken cancellationToken);
}
