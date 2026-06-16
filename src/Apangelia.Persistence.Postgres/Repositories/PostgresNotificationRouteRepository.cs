using Apangelia.Application.NotificationRoutes;
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
