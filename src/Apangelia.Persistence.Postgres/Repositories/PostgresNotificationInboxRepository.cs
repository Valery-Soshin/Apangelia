using Apangelia.Application.Repositories;
using Apangelia.Core;
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

    public async Task<bool> TryAddAsync(NotificationEvent notificationEvent, CancellationToken cancellationToken)
    {
        var messageId = PostgresGuidV7ValueGenerator.Create();

        var affectedRows = await _context.Database.ExecuteSqlInterpolatedAsync($"""
            INSERT INTO "NotificationInboxMessages"
                ("Id", "Source", "EventType", "ExternalEventId", "RawPayloadJson", "OccurredAt")
            VALUES
                ({messageId}, {notificationEvent.Source}, {notificationEvent.EventType}, {notificationEvent.ExternalEventId},
                 CAST({notificationEvent.RawPayloadJson} AS jsonb), {notificationEvent.OccurredAt})
            ON CONFLICT ("Source", "ExternalEventId") DO NOTHING
            """, cancellationToken);

        return affectedRows == 1;
    }
}
