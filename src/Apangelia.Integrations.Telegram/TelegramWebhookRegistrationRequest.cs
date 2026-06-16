namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Параметры регистрации Telegram webhook.
/// </summary>
/// <param name="Url">Публичный HTTPS URL для доставки webhook-обновлений.</param>
/// <param name="AllowedUpdates">Типы обновлений, которые Telegram должен доставлять.</param>
/// <param name="DropPendingUpdates">Признак удаления ожидающих обновлений при регистрации webhook.</param>
/// <param name="SecretToken">Секретный токен заголовка webhook-запросов.</param>
public sealed record TelegramWebhookRegistrationRequest(
    Uri Url,
    IReadOnlyCollection<string> AllowedUpdates,
    bool DropPendingUpdates,
    string SecretToken);
