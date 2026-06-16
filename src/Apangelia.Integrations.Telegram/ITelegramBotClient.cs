namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Клиент используемых методов Telegram Bot API.
/// </summary>
public interface ITelegramBotClient
{
    /// <summary>
    /// Отправляет текстовое сообщение в Telegram-чат.
    /// </summary>
    /// <param name="chatId">Идентификатор Telegram-чата.</param>
    /// <param name="text">Текст сообщения.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат запроса к Telegram Bot API.</returns>
    Task<TelegramBotApiRequestResult> SendMessageAsync(
        string chatId,
        string text,
        CancellationToken cancellationToken);

    /// <summary>
    /// Регистрирует URL для доставки Telegram webhook-обновлений.
    /// </summary>
    /// <param name="request">Параметры регистрации webhook.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат запроса к Telegram Bot API.</returns>
    Task<TelegramBotApiRequestResult> SetWebhookAsync(
        TelegramWebhookRegistrationRequest request,
        CancellationToken cancellationToken);
}
