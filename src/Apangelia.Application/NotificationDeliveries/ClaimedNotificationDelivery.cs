using Apangelia.Application.NotificationProviders;

namespace Apangelia.Application.NotificationDeliveries;

/// <summary>
/// Задание доставки, закрепленное за worker-ом вместе с начатой попыткой отправки.
/// </summary>
/// <param name="AttemptId">Идентификатор начатой попытки доставки.</param>
/// <param name="SendRequest">Данные для отправки уведомления исходящим провайдером.</param>
public sealed record ClaimedNotificationDelivery(
    Guid AttemptId,
    NotificationProviderRequest SendRequest);
