using Apangelia.Application.Repositories;
using Apangelia.Core;

namespace Apangelia.Persistence.Postgres.Repositories;

/// <summary>
/// PostgreSQL-репозиторий заданий доставки уведомлений.
/// </summary>
public sealed class PostgresNotificationDeliveryRepository : INotificationDeliveryRepository
{
    private readonly AppDbContext _context;

    public PostgresNotificationDeliveryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(
        IReadOnlyCollection<NotificationDelivery> deliveries,
        CancellationToken cancellationToken)
    {
        if (deliveries.Count == 0)
        {
            return;
        }

        await _context.NotificationDeliveries.AddRangeAsync(deliveries, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
