using Apangelia.Application.Shared.CommandBase;

namespace Apangelia.Application.Notifications.RegisterNotificationRoute;

/// <summary>
/// Команда регистрации маршрута доставки уведомлений для текущего пользователя.
/// </summary>
/// <param name="InputProviderId">Идентификатор входящего провайдера уведомлений.</param>
/// <param name="OutputProviderId">Идентификатор исходящего провайдера уведомлений.</param>
/// <param name="DestinationId">Идентификатор назначения внутри исходящего провайдера.</param>
public sealed record RegisterNotificationRouteCommand(
    string InputProviderId,
    string OutputProviderId,
    string DestinationId)
    : ICommand<RegisterNotificationRouteResult>;
