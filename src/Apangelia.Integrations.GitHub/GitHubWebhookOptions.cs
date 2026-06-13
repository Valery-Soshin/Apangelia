namespace Apangelia.Integrations.GitHub;

/// <summary>
/// Настройки приема webhook-событий GitHub.
/// </summary>
public sealed class GitHubWebhookOptions
{
    /// <summary>
    /// Секрет webhook, который GitHub использует для подписи доставок.
    /// </summary>
    public required string WebhookSecret { get; init; }
}
