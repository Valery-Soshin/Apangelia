using Apangelia.Application.Repositories;
using Apangelia.Core;
using Apangelia.Persistence.Postgres;
using Microsoft.EntityFrameworkCore;

namespace Apangelia.Persistence.Postgres.Repositories;

/// <summary>
/// PostgreSQL-репозиторий уведомлений.
/// </summary>
public sealed class PostgresNotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    public PostgresNotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Notification notification, CancellationToken cancellationToken)
    {
        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
