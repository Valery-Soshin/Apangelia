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

    /// <summary>
    /// Проверяет, что базовый URL Telegram Bot API задан абсолютным HTTPS URL.
    /// </summary>
    /// <param name="options">Настройки интеграции с Telegram Bot API.</param>
    /// <returns><see langword="true" />, если базовый URL корректен.</returns>
    public static bool IsValidApiBaseUrl(TelegramNotificationOptions options)
    {
        return Uri.TryCreate(options.ApiBaseUrl, UriKind.Absolute, out var apiBaseUrl)
            && string.Equals(apiBaseUrl.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Проверяет секретный токен Telegram webhook.
    /// </summary>
    /// <param name="options">Настройки интеграции с Telegram Bot API.</param>
    /// <returns><see langword="true" />, если токен соответствует ограничениям Telegram.</returns>
    public static bool IsValidWebhookSecretToken(TelegramNotificationOptions options)
    {
        var token = options.WebhookSecretToken;

        return !string.IsNullOrWhiteSpace(token)
            && token.Length is >= 1 and <= 256
            && token.All(IsWebhookSecretTokenCharacter);
    }

    private static bool IsWebhookSecretTokenCharacter(char value)
    {
        return value is >= 'A' and <= 'Z'
            or >= 'a' and <= 'z'
            or >= '0' and <= '9'
            or '_'
            or '-';
    }
}
