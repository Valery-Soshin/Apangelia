using Apangelia.Application.Commands;
using Apangelia.Core;

namespace Apangelia.Application.Notifications.AcceptEvent;

/// <summary>
/// Команда приема нормализованного события уведомления из внешней интеграции.
/// </summary>
/// <param name="NotificationEvent">Событие уведомления, приведенное к доменному формату.</param>
public sealed record AcceptNotificationEventCommand(NotificationEvent NotificationEvent)
    : ICommand<AcceptNotificationEventResult>;
