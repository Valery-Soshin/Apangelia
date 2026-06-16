namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Входные данные Telegram webhook, извлеченные из HTTP-запроса.
/// </summary>
/// <param name="SecretToken">Значение заголовка X-Telegram-Bot-Api-Secret-Token.</param>
/// <param name="BodyBytes">Исходные байты тела запроса.</param>
public sealed record TelegramWebhookReceiveRequest(
    string? SecretToken,
    ReadOnlyMemory<byte> BodyBytes);
