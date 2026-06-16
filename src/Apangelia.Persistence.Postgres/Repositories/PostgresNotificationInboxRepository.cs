using Apangelia.Application.NotificationInboxes;
using Apangelia.Persistence.Postgres;
using Apangelia.Persistence.Postgres.ValueGenerators;
using Microsoft.EntityFrameworkCore;

namespace Apangelia.Persistence.Postgres.Repositories;

/// <summary>
/// PostgreSQL-репозиторий входящих сообщений уведомлений.
/// </summary>
public sealed class PostgresNotificationInboxRepository : INotificationInboxRepository
{
    private readonly AppDbContext _context;

    public PostgresNotificationInboxRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> TryAddAsync(NotificationInbox notificationInbox, CancellationToken cancellationToken)
    {
        var messageId = PostgresGuidV7ValueGenerator.Create();

        var affectedRows = await _context.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO "NotificationInboxMessages"
                ("Id", "Source", "EventType", "ExternalEventId", "RawPayloadJson", "OccurredAt")
            VALUES
                ({messageId}, {notificationInbox.Source}, {notificationInbox.EventType}, {notificationInbox.ExternalEventId},
                 CAST({notificationInbox.RawPayloadJson} AS jsonb), {notificationInbox.OccurredAt})
            ON CONFLICT ("Source", "ExternalEventId") DO NOTHING
            """, cancellationToken);

        return affectedRows == 1;
    }
}
