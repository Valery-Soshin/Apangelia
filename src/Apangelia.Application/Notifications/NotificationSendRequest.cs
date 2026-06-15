using Apangelia.Core;

namespace Apangelia.Application.Notifications;

/// <summary>
/// Данные, необходимые исходящему провайдеру для отправки уведомления.
/// </summary>
/// <param name="Delivery">Задание доставки, в рамках которого выполняется отправка.</param>
/// <param name="Notification">Отправляемое уведомление.</param>
/// <param name="Route">Маршрут, определяющий исходящий провайдер и назначение.</param>
public sealed record NotificationSendRequest(
    NotificationDelivery Delivery,
    Notification Notification,
    NotificationRoute Route);
