using Apangelia.Core;
using Apangelia.Persistence.Postgres.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apangelia.Persistence.Postgres.Configurations;

internal sealed class NotificationDeliveryConfiguration : IEntityTypeConfiguration<NotificationDelivery>
{
    public void Configure(EntityTypeBuilder<NotificationDelivery> builder)
    {
        builder.ToTable("NotificationDeliveries");
        builder.HasKey(delivery => delivery.Id);
        builder.Property(delivery => delivery.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<PostgresGuidV7ValueGenerator>();
        builder.Property(delivery => delivery.NotificationId).IsRequired();
        builder.Property(delivery => delivery.RouteId).IsRequired();
        builder.Property(delivery => delivery.Status)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();
        builder.Property(delivery => delivery.NextAttemptAt);
        builder.Property(delivery => delivery.LastAttemptAt);
        builder.Property(delivery => delivery.DeliveredAt);
        builder.Property(delivery => delivery.FailedAt);
        builder.Property(delivery => delivery.AttemptCount).IsRequired();
        builder.Property(delivery => delivery.MaxAttempts).IsRequired();
        builder.Property(delivery => delivery.CreatedAt).IsRequired();
        builder.Property(delivery => delivery.UpdatedAt);
        builder.HasOne<Notification>()
            .WithMany()
            .HasForeignKey(delivery => delivery.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<NotificationRoute>()
            .WithMany()
            .HasForeignKey(delivery => delivery.RouteId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(delivery => delivery.NotificationId);
        builder.HasIndex(delivery => delivery.RouteId);
        builder.HasIndex(delivery => new { delivery.Status, delivery.NextAttemptAt });
    }
}
