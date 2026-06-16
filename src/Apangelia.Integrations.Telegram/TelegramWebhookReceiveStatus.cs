namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Статус приема Telegram webhook.
/// </summary>
public enum TelegramWebhookReceiveStatus
{
    /// <summary>
    /// Обновление принято и преобразовано в команду регистрации маршрута.
    /// </summary>
    Accepted = 0,

    /// <summary>
    /// Обновление корректно принято, но не требует действий приложения.
    /// </summary>
    Ignored = 1,

    /// <summary>
    /// Секретный токен отсутствует или не совпадает с ожидаемым.
    /// </summary>
    Unauthorized = 2,

    /// <summary>
    /// Тело доставки не является корректным JSON-объектом.
    /// </summary>
    InvalidPayload = 3
}
