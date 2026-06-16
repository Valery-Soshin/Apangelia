using Apangelia.Application.Abstractions.Persistence;
using Apangelia.Core;
using Microsoft.EntityFrameworkCore;

namespace Apangelia.Persistence.Postgres.Repositories;

/// <summary>
/// PostgreSQL-репозиторий маршрутов доставки уведомлений.
/// </summary>
public sealed class PostgresNotificationRouteRepository : INotificationRouteRepository
{
    private readonly AppDbContext _context;

    public PostgresNotificationRouteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegisterAsync(NotificationRoute route, CancellationToken cancellationToken)
    {
        var currentRoute = await _context.NotificationRoutes
            .FirstOrDefaultAsync(
                existingRoute =>
                    existingRoute.UserId == route.UserId
                    && existingRoute.InputProviderId == route.InputProviderId
                    && existingRoute.OutputProviderId == route.OutputProviderId
                    && existingRoute.ConditionsJson == route.ConditionsJson,
                cancellationToken);

        if (currentRoute is null)
        {
            await _context.NotificationRoutes.AddAsync(route, cancellationToken);
        }
        else
        {
            currentRoute.DestinationId = route.DestinationId;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<NotificationRoute>> GetByUserAndInputProviderAsync(
        Guid userId,
        string inputProviderId,
        CancellationToken cancellationToken)
    {
        return await _context.NotificationRoutes
            .AsNoTracking()
            .Where(route => route.UserId == userId && route.InputProviderId == inputProviderId)
            .ToArrayAsync(cancellationToken);
    }
}
