namespace Apangelia.Integrations.GitHub;

/// <summary>
/// Статус приема GitHub webhook.
/// </summary>
public enum GitHubWebhookReceiveStatus
{
    /// <summary>
    /// Доставка принята и преобразована в унифицированное событие.
    /// </summary>
    Accepted = 0,

    /// <summary>
    /// Подпись доставки отсутствует или не совпадает с ожидаемой.
    /// </summary>
    Unauthorized = 1,

    /// <summary>
    /// В доставке отсутствуют обязательные заголовки GitHub.
    /// </summary>
    MissingRequiredHeaders = 2,

    /// <summary>
    /// Тело доставки не является корректным JSON-объектом.
    /// </summary>
    InvalidPayload = 3
}
