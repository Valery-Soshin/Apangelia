namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Настройки интеграции с Telegram Bot API.
/// </summary>
public sealed class TelegramNotificationOptions
{
    /// <summary>
    /// Базовый URL Telegram Bot API.
    /// </summary>
    public required string ApiBaseUrl { get; init; }

    /// <summary>
    /// Токен Telegram-бота, выданный BotFather.
    /// </summary>
    public required string BotToken { get; init; }

    /// <summary>
    /// Секретный токен, который Telegram передает в заголовке webhook-запроса.
    /// </summary>
    public required string WebhookSecretToken { get; init; }
}
