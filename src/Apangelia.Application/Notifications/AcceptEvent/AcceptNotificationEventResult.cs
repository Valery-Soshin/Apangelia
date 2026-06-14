namespace Apangelia.Application.Notifications.AcceptEvent;

/// <summary>
/// Результат выполнения команды приема события уведомления.
/// </summary>
public enum AcceptNotificationEventResult
{
    /// <summary>
    /// Событие принято впервые и преобразовано в уведомление.
    /// </summary>
    Accepted = 0,

    /// <summary>
    /// Событие уже было принято ранее и повторно не обработано.
    /// </summary>
    Duplicate = 1
}
