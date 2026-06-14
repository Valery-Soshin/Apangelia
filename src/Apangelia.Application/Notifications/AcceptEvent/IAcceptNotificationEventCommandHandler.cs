namespace Apangelia.Application.Notifications.AcceptEvent;

/// <summary>
/// Обрабатывает команду приема нормализованного события уведомления.
/// </summary>
public interface IAcceptNotificationEventCommandHandler
{
    /// <summary>
    /// Проводит идемпотентный прием события и создает уведомление для новых событий.
    /// </summary>
    /// <param name="command">Команда приема события уведомления.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат приема события уведомления.</returns>
    Task<AcceptNotificationEventResult> HandleAsync(
        AcceptNotificationEventCommand command,
        CancellationToken cancellationToken);
}
