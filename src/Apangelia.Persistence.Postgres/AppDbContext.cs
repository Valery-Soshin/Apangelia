using Apangelia.Core;
using Microsoft.EntityFrameworkCore;

namespace Apangelia.Persistence.Postgres;

public class AppDbContext : DbContext
{
    public DbSet<Notification> Notifications { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}