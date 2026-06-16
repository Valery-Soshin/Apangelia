using Apangelia.Core;
using Apangelia.Persistence.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apangelia.Persistence.Postgres;

/// <summary>
/// Контекст PostgreSQL для хранения уведомлений и входящих сообщений.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Созданные уведомления.
    /// </summary>
    public DbSet<Notification> Notifications { get; set; }

    /// <summary>
    /// Пользовательские маршруты доставки уведомлений.
    /// </summary>
    public DbSet<NotificationRoute> NotificationRoutes { get; set; }

    /// <summary>
    /// Задачи доставки уведомлений.
    /// </summary>
    public DbSet<NotificationDelivery> NotificationDeliveries { get; set; }

    /// <summary>
    /// История попыток доставки уведомлений.
    /// </summary>
    public DbSet<NotificationDeliveryAttempt> NotificationDeliveryAttempts { get; set; }

    /// <summary>
    /// Входящие сообщения уведомлений для идемпотентной обработки.
    /// </summary>
    public DbSet<NotificationInboxEntity> NotificationInboxMessages { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
