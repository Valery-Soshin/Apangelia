using Apangelia.Persistence.Postgres.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apangelia.Persistence.Postgres.Configurations;

internal sealed class NotificationInboxMessageConfiguration : IEntityTypeConfiguration<NotificationInboxMessage>
{
    public void Configure(EntityTypeBuilder<NotificationInboxMessage> builder)
    {
        builder.ToTable("NotificationInboxMessages");
        builder.HasKey(message => message.Id);
        builder.Property(message => message.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<PostgresGuidV7ValueGenerator>();
        builder.Property(message => message.Source).HasMaxLength(64).IsRequired();
        builder.Property(message => message.EventType).HasMaxLength(128).IsRequired();
        builder.Property(message => message.ExternalEventId).HasMaxLength(128).IsRequired();
        builder.Property(message => message.RawPayloadJson).HasColumnType("jsonb").IsRequired();
        builder.Property(message => message.OccurredAt).IsRequired();
        builder.Property(message => message.ReceivedAt).IsRequired();
        builder.HasIndex(message => new { message.Source, message.ExternalEventId }).IsUnique();
    }
}
