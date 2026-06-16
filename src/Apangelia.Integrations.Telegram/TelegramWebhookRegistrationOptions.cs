namespace Apangelia.Integrations.Telegram;

/// <summary>
/// Настройки автоматической регистрации Telegram webhook.
/// </summary>
public sealed class TelegramWebhookRegistrationOptions
{
    /// <summary>
    /// Признак включенной регистрации webhook при старте приложения.
    /// </summary>
    public bool Enabled { get; init; }

    /// <summary>
    /// Публичный HTTPS URL endpoint-а, на который Telegram должен отправлять webhook-обновления.
    /// </summary>
    public required string PublicUrl { get; init; }

    /// <summary>
    /// Типы обновлений, которые Telegram должен доставлять.
    /// </summary>
    public required string[] AllowedUpdates { get; init; }

    /// <summary>
    /// Признак удаления ожидающих обновлений при регистрации webhook.
    /// </summary>
    public bool DropPendingUpdates { get; init; }
}
