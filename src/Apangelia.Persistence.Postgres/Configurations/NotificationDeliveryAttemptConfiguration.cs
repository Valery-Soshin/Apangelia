using Apangelia.Core;
using Apangelia.Persistence.Postgres.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apangelia.Persistence.Postgres.Configurations;

internal sealed class NotificationDeliveryAttemptConfiguration : IEntityTypeConfiguration<NotificationDeliveryAttempt>
{
    public void Configure(EntityTypeBuilder<NotificationDeliveryAttempt> builder)
    {
        builder.ToTable("NotificationDeliveryAttempts");
        builder.HasKey(attempt => attempt.Id);
        builder.Property(attempt => attempt.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<PostgresGuidV7ValueGenerator>();
        builder.Property(attempt => attempt.NotificationDeliveryId).IsRequired();
        builder.Property(attempt => attempt.StartedAt).IsRequired();
        builder.Property(attempt => attempt.FinishedAt);
        builder.Property(attempt => attempt.ErrorCode).HasMaxLength(128);
        builder.Property(attempt => attempt.ErrorMessage);
        builder.HasOne<NotificationDelivery>()
            .WithMany()
            .HasForeignKey(attempt => attempt.NotificationDeliveryId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(attempt => new { attempt.NotificationDeliveryId, attempt.StartedAt });
    }
}
