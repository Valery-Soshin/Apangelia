using Apangelia.Core;
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
    /// Входящие сообщения уведомлений для идемпотентной обработки.
    /// </summary>
    public DbSet<NotificationInboxMessage> NotificationInboxMessages { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
