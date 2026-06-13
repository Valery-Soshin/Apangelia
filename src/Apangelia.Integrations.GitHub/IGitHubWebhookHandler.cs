namespace Apangelia.Integrations.GitHub;

/// <summary>
/// Координирует прием GitHub webhook и передачу нормализованного события в прикладной обработчик.
/// </summary>
public interface IGitHubWebhookHandler
{
    /// <summary>
    /// Обрабатывает доставку GitHub webhook без привязки к HTTP-фреймворку.
    /// </summary>
    /// <param name="request">Входные данные доставки GitHub webhook.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns>Статус обработки, который HTTP-адаптер может преобразовать в ответ.</returns>
    Task<GitHubWebhookHandlingResult> HandleAsync(
        GitHubWebhookReceiveRequest request,
        CancellationToken cancellationToken);
}
