using Apangelia.Application.SeedWork;
using Apangelia.Core;

namespace Apangelia.Application.Commands.Notifications;

/// <summary>
/// Команда приема нормализованного события уведомления из внешней интеграции.
/// </summary>
/// <param name="NotificationEvent">Событие уведомления, приведенное к доменному формату.</param>
public sealed record AcceptNotificationEventCommand(NotificationEvent NotificationEvent)
    : ICommand<AcceptNotificationEventResult>;
