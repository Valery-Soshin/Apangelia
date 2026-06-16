using Apangelia.Application.NotificationDeliveries;
using Apangelia.Application.NotificationProviders;
using Apangelia.Core;
using Microsoft.EntityFrameworkCore;

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

    public async Task<IReadOnlyCollection<ClaimedNotificationDelivery>> ClaimForProcessingAsync(
        int batchSize,
        DateTimeOffset claimedAt,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        var deliveries = await ClaimDeliveriesAsync(batchSize, claimedAt, cancellationToken);
        if (deliveries.Count == 0)
        {
            await transaction.CommitAsync(cancellationToken);

            return [];
        }

        var attempts = deliveries
            .Select(delivery => new NotificationDeliveryAttempt
            {
                NotificationDeliveryId = delivery.Id,
                StartedAt = claimedAt
            })
            .ToArray();

        _context.NotificationDeliveryAttempts.AddRange(attempts);
        await _context.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return await LoadClaimedDeliveriesAsync(deliveries, attempts, cancellationToken);
    }

    public async Task AddRangeAsync(
        IReadOnlyCollection<NotificationDelivery> deliveries,
        CancellationToken cancellationToken)
    {
        if (deliveries.Count == 0)
        {
            return;
        }

        _context.NotificationDeliveries.AddRange(deliveries);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<NotificationDeliveryAttemptState> GetAttemptStateAsync(
        Guid deliveryId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var delivery = await _context.NotificationDeliveries
            .FirstAsync(currentDelivery => currentDelivery.Id == deliveryId, cancellationToken);

        var attempt = await _context.NotificationDeliveryAttempts
            .FirstAsync(currentAttempt =>
                currentAttempt.Id == attemptId
                && currentAttempt.NotificationDeliveryId == deliveryId,
                cancellationToken);

        return new NotificationDeliveryAttemptState(delivery, attempt);
    }

    public async Task SaveAttemptStateAsync(
        NotificationDeliveryAttemptState state,
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<IReadOnlyCollection<NotificationDelivery>> ClaimDeliveriesAsync(
        int batchSize,
        DateTimeOffset claimedAt,
        CancellationToken cancellationToken)
    {
        var pendingStatus = NotificationDeliveryStatus.Pending.ToString();
        var retryScheduledStatus = NotificationDeliveryStatus.RetryScheduled.ToString();
        var processingStatus = NotificationDeliveryStatus.Processing.ToString();

        return await _context.NotificationDeliveries
            .FromSql($"""
                WITH claimed AS (
                    SELECT delivery."Id"
                    FROM "NotificationDeliveries" AS delivery
                    WHERE delivery."Status" = {pendingStatus}
                        OR (
                            delivery."Status" = {retryScheduledStatus}
                            AND (
                                delivery."NextAttemptAt" IS NULL
                                OR delivery."NextAttemptAt" <= {claimedAt}
                            )
                        )
                    ORDER BY COALESCE(delivery."NextAttemptAt", delivery."CreatedAt"), delivery."CreatedAt", delivery."Id"
                    LIMIT {batchSize}
                    FOR UPDATE SKIP LOCKED
                ),
                updated AS (
                    UPDATE "NotificationDeliveries" AS delivery
                    SET "Status" = {processingStatus},
                        "AttemptCount" = delivery."AttemptCount" + 1,
                        "LastAttemptAt" = {claimedAt},
                        "NextAttemptAt" = NULL,
                        "UpdatedAt" = {claimedAt}
                    FROM claimed
                    WHERE delivery."Id" = claimed."Id"
                    RETURNING
                        delivery."Id",
                        delivery."NotificationId",
                        delivery."RouteId",
                        delivery."Status",
                        delivery."NextAttemptAt",
                        delivery."LastAttemptAt",
                        delivery."DeliveredAt",
                        delivery."FailedAt",
                        delivery."AttemptCount",
                        delivery."MaxAttempts",
                        delivery."CreatedAt",
                        delivery."UpdatedAt"
                )
                SELECT
                    updated."Id",
                    updated."NotificationId",
                    updated."RouteId",
                    updated."Status",
                    updated."NextAttemptAt",
                    updated."LastAttemptAt",
                    updated."DeliveredAt",
                    updated."FailedAt",
                    updated."AttemptCount",
                    updated."MaxAttempts",
                    updated."CreatedAt",
                    updated."UpdatedAt"
                FROM updated
                """)
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);
    }

    private async Task<IReadOnlyCollection<ClaimedNotificationDelivery>> LoadClaimedDeliveriesAsync(
        IReadOnlyCollection<NotificationDelivery> deliveries,
        IReadOnlyCollection<NotificationDeliveryAttempt> attempts,
        CancellationToken cancellationToken)
    {
        var notificationIds = deliveries
            .Select(delivery => delivery.NotificationId)
            .Distinct()
            .ToArray();

        var routeIds = deliveries
            .Select(delivery => delivery.RouteId)
            .Distinct()
            .ToArray();

        var notifications = await _context.Notifications
            .AsNoTracking()
            .Where(notification => notificationIds.Contains(notification.Id))
            .ToDictionaryAsync(notification => notification.Id, cancellationToken);

        var routes = await _context.NotificationRoutes
            .AsNoTracking()
            .Where(route => routeIds.Contains(route.Id))
            .ToDictionaryAsync(route => route.Id, cancellationToken);

        var attemptsByDeliveryId = attempts.ToDictionary(attempt => attempt.NotificationDeliveryId);

        return deliveries
            .Select(delivery => new ClaimedNotificationDelivery(
                attemptsByDeliveryId[delivery.Id].Id,
                new NotificationProviderRequest(
                    delivery,
                    notifications[delivery.NotificationId],
                    routes[delivery.RouteId])))
            .ToArray();
    }

}
