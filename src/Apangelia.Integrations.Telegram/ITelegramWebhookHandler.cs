namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Обрабатывает входящие Telegram webhook-обновления.
/// </summary>
public interface ITelegramWebhookHandler
{
    /// <summary>
    /// Обрабатывает webhook-обновление Telegram.
    /// </summary>
    /// <param name="request">Входные данные Telegram webhook.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат обработки webhook-обновления.</returns>
    Task<TelegramWebhookHandlingResult> HandleAsync(
        TelegramWebhookReceiveRequest request,
        CancellationToken cancellationToken);
}
