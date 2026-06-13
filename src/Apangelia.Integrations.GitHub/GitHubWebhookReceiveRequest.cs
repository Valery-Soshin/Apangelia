namespace Apangelia.Integrations.GitHub;

/// <summary>
/// Входные данные доставки GitHub webhook, извлеченные из HTTP-запроса.
/// </summary>
/// <param name="Signature">Значение заголовка X-Hub-Signature-256.</param>
/// <param name="DeliveryId">Значение заголовка X-GitHub-Delivery.</param>
/// <param name="EventType">Значение заголовка X-GitHub-Event.</param>
/// <param name="Body">Исходное тело запроса.</param>
/// <param name="BodyBytes">Исходные байты тела запроса для проверки подписи.</param>
public sealed record GitHubWebhookReceiveRequest(
    string? Signature,
    string? DeliveryId,
    string? EventType,
    string Body,
    ReadOnlyMemory<byte> BodyBytes);
