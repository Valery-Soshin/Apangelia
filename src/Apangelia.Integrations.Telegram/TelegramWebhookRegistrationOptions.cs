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

    /// <summary>
    /// Проверяет настройки автоматической регистрации Telegram webhook.
    /// </summary>
    /// <param name="options">Настройки регистрации Telegram webhook.</param>
    /// <returns><see langword="true" />, если регистрация выключена или включена с корректными параметрами.</returns>
    public static bool IsValidRegistration(TelegramWebhookRegistrationOptions options)
    {
        if (!options.Enabled)
        {
            return true;
        }

        return Uri.TryCreate(options.PublicUrl, UriKind.Absolute, out var publicUrl)
            && string.Equals(publicUrl.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)
            && options.AllowedUpdates is { Length: > 0 }
            && options.AllowedUpdates.All(updateType => !string.IsNullOrWhiteSpace(updateType));
    }
}
