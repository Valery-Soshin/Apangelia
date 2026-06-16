namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Проверяет и преобразует входящие Telegram webhook-обновления.
/// </summary>
public interface ITelegramWebhookReceiver
{
    /// <summary>
    /// Принимает webhook-обновление Telegram и извлекает команду приложения.
    /// </summary>
    /// <param name="request">Входные данные Telegram webhook.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат приема webhook-обновления.</returns>
    ValueTask<TelegramWebhookReceiveResult> ReceiveAsync(
        TelegramWebhookReceiveRequest request,
        CancellationToken cancellationToken);
}
