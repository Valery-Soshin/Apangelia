namespace Apangelia.Application;

/// <summary>
/// Результат обработки входящего события уведомления.
/// </summary>
public enum NotificationEventHandlingResult
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
