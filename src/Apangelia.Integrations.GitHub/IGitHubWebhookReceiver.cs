namespace Apangelia.Integrations.GitHub;

/// <summary>
/// Принимает и нормализует webhook-доставки GitHub.
/// </summary>
public interface IGitHubWebhookReceiver
{
    /// <summary>
    /// Проверяет доставку GitHub и преобразует ее в унифицированное событие уведомления.
    /// </summary>
    /// <param name="request">Данные webhook-доставки.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Результат приема доставки.</returns>
    ValueTask<GitHubWebhookReceiveResult> ReceiveAsync(
        GitHubWebhookReceiveRequest request,
        CancellationToken cancellationToken);
}
