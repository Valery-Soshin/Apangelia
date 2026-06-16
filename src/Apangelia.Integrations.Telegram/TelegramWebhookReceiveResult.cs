using Apangelia.Application.Notifications.RegisterNotificationRoute;

namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Результат приема Telegram webhook.
/// </summary>
/// <param name="Status">Статус приема webhook-обновления.</param>
/// <param name="Command">Команда регистрации маршрута для успешного webhook-обновления.</param>
public sealed record TelegramWebhookReceiveResult(
    TelegramWebhookReceiveStatus Status,
    RegisterNotificationRouteCommand? Command)
{
    /// <summary>
    /// Создает результат с принятой командой регистрации маршрута.
    /// </summary>
    /// <param name="command">Команда регистрации маршрута доставки.</param>
    /// <returns>Результат успешного приема.</returns>
    public static TelegramWebhookReceiveResult Accepted(RegisterNotificationRouteCommand command)
    {
        return new TelegramWebhookReceiveResult(TelegramWebhookReceiveStatus.Accepted, command);
    }

    /// <summary>
    /// Создает результат с игнорированным обновлением.
    /// </summary>
    /// <returns>Результат приема без команды приложения.</returns>
    public static TelegramWebhookReceiveResult Ignored()
    {
        return new TelegramWebhookReceiveResult(TelegramWebhookReceiveStatus.Ignored, null);
    }

    /// <summary>
    /// Создает результат с отказом в авторизации.
    /// </summary>
    /// <returns>Результат неуспешной проверки секретного токена.</returns>
    public static TelegramWebhookReceiveResult Unauthorized()
    {
        return new TelegramWebhookReceiveResult(TelegramWebhookReceiveStatus.Unauthorized, null);
    }

    /// <summary>
    /// Создает результат с ошибкой JSON-тела.
    /// </summary>
    /// <returns>Результат невалидного payload.</returns>
    public static TelegramWebhookReceiveResult InvalidPayload()
    {
        return new TelegramWebhookReceiveResult(TelegramWebhookReceiveStatus.InvalidPayload, null);
    }
}
