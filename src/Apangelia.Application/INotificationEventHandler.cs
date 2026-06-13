using Apangelia.Core;

namespace Apangelia.Application;

/// <summary>
/// Обрабатывает унифицированные события уведомлений из любых внешних интеграций.
/// </summary>
public interface INotificationEventHandler
{
    /// <summary>
    /// Проводит идемпотентную обработку входящего события и создает уведомление для новых событий.
    /// </summary>
    /// <param name="notificationEvent">Входящее нормализованное событие.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат обработки события.</returns>
    Task<NotificationEventHandlingResult> HandleAsync(
        NotificationEvent notificationEvent,
        CancellationToken cancellationToken);
}
