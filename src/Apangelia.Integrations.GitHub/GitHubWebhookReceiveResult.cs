using Apangelia.Application.Notifications.AcceptNotificationEvent;

namespace Apangelia.Integrations.GitHub;

/// <summary>
/// Результат приема доставки GitHub webhook.
/// </summary>
/// <param name="Status">Статус приема доставки.</param>
/// <param name="Command">Команда приема нормализованного события для успешной доставки.</param>
public sealed record GitHubWebhookReceiveResult(
    GitHubWebhookReceiveStatus Status,
    AcceptNotificationEventCommand? Command)
{
    /// <summary>
    /// Создает успешный результат приема.
    /// </summary>
    /// <param name="command">Команда приема нормализованного события уведомления.</param>
    /// <returns>Результат успешного приема.</returns>
    public static GitHubWebhookReceiveResult Accepted(AcceptNotificationEventCommand command)
    {
        return new GitHubWebhookReceiveResult(GitHubWebhookReceiveStatus.Accepted, command);
    }

    /// <summary>
    /// Создает результат с отказом в авторизации.
    /// </summary>
    /// <returns>Результат неуспешной проверки подписи.</returns>
    public static GitHubWebhookReceiveResult Unauthorized()
    {
        return new GitHubWebhookReceiveResult(GitHubWebhookReceiveStatus.Unauthorized, null);
    }

    /// <summary>
    /// Создает результат с ошибкой обязательных заголовков.
    /// </summary>
    /// <returns>Результат невалидного запроса.</returns>
    public static GitHubWebhookReceiveResult MissingRequiredHeaders()
    {
        return new GitHubWebhookReceiveResult(GitHubWebhookReceiveStatus.MissingRequiredHeaders, null);
    }

    /// <summary>
    /// Создает результат с ошибкой JSON-тела.
    /// </summary>
    /// <returns>Результат невалидного payload.</returns>
    public static GitHubWebhookReceiveResult InvalidPayload()
    {
        return new GitHubWebhookReceiveResult(GitHubWebhookReceiveStatus.InvalidPayload, null);
    }
}
