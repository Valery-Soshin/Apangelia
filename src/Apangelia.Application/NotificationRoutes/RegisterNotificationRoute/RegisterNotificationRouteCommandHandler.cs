using Apangelia.Application.Abstractions.Persistence;
using Apangelia.Application.Shared.CommandBase;
using Apangelia.Core;

namespace Apangelia.Application.Notifications.RegisterNotificationRoute;

/// <summary>
/// Регистрирует маршрут доставки уведомлений для текущего пользователя.
/// </summary>
public sealed class RegisterNotificationRouteCommandHandler
    : ICommandHandler<RegisterNotificationRouteCommand, RegisterNotificationRouteResult>
{
    private readonly INotificationRouteRepository _routeRepository;

    public RegisterNotificationRouteCommandHandler(INotificationRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;
    }

    public async Task<RegisterNotificationRouteResult> HandleAsync(
        RegisterNotificationRouteCommand command,
        CancellationToken cancellationToken)
    {
        var route = new NotificationRoute
        {
            UserId = TemporaryNotificationUser.Id,
            InputProviderId = command.InputProviderId,
            OutputProviderId = command.OutputProviderId,
            DestinationId = command.DestinationId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _routeRepository.RegisterAsync(route, cancellationToken);

        return RegisterNotificationRouteResult.Registered;
    }
}
