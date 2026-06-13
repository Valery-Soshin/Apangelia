namespace Apangelia.Integrations.GitHub;

/// <summary>
/// Результат полной обработки доставки GitHub webhook на уровне интеграции.
/// </summary>
/// <param name="Status">Статус доставки, достаточный для HTTP-маппинга.</param>
public sealed record GitHubWebhookHandlingResult(GitHubWebhookReceiveStatus Status)
{
    /// <summary>
    /// Создает успешный результат обработки доставки.
    /// </summary>
    /// <returns>Результат успешной обработки.</returns>
    public static GitHubWebhookHandlingResult Accepted()
    {
        return new GitHubWebhookHandlingResult(GitHubWebhookReceiveStatus.Accepted);
    }

    /// <summary>
    /// Создает результат обработки из статуса приема доставки.
    /// </summary>
    /// <param name="status">Статус приема доставки.</param>
    /// <returns>Результат обработки с указанным статусом.</returns>
    public static GitHubWebhookHandlingResult FromReceiveStatus(GitHubWebhookReceiveStatus status)
    {
        return new GitHubWebhookHandlingResult(status);
    }
}
