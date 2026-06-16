namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Результат обработки Telegram webhook.
/// </summary>
/// <param name="Status">Статус обработки webhook-обновления.</param>
public sealed record TelegramWebhookHandlingResult(TelegramWebhookReceiveStatus Status)
{
    /// <summary>
    /// Создает результат успешной обработки.
    /// </summary>
    /// <returns>Результат успешной обработки.</returns>
    public static TelegramWebhookHandlingResult Accepted()
    {
        return new TelegramWebhookHandlingResult(TelegramWebhookReceiveStatus.Accepted);
    }

    /// <summary>
    /// Создает результат обработки из статуса приема.
    /// </summary>
    /// <param name="status">Статус приема webhook-обновления.</param>
    /// <returns>Результат обработки.</returns>
    public static TelegramWebhookHandlingResult FromReceiveStatus(TelegramWebhookReceiveStatus status)
    {
        return new TelegramWebhookHandlingResult(status);
    }
}
