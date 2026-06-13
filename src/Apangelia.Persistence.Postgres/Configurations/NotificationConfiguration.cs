using Apangelia.Core;
using Apangelia.Persistence.Postgres.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apangelia.Persistence.Postgres.Configurations;

internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(notification => notification.Id);
        builder.Property(notification => notification.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<PostgresGuidV7ValueGenerator>();
        builder.Property(notification => notification.Source).HasMaxLength(64).IsRequired();
        builder.Property(notification => notification.EventType).HasMaxLength(128).IsRequired();
        builder.Property(notification => notification.ExternalEventId).HasMaxLength(128).IsRequired();
        builder.Property(notification => notification.Title).HasMaxLength(256).IsRequired();
        builder.Property(notification => notification.Message);
        builder.Property(notification => notification.OccurredAt).IsRequired();
        builder.Property(notification => notification.CreatedAt).IsRequired();
        builder.HasIndex(notification => new { notification.Source, notification.ExternalEventId });
    }
}
