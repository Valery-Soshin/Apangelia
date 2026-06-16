using Apangelia.Application.Abstractions.Providers;

namespace Apangelia.Application.Abstractions.Persistence.Models;

/// <summary>
/// Задание доставки, закрепленное за worker-ом вместе с начатой попыткой отправки.
/// </summary>
/// <param name="AttemptId">Идентификатор начатой попытки доставки.</param>
/// <param name="SendRequest">Данные для отправки уведомления исходящим провайдером.</param>
public sealed record NotificationDeliveryClaim(
    Guid AttemptId,
    NotificationProviderRequest SendRequest);
