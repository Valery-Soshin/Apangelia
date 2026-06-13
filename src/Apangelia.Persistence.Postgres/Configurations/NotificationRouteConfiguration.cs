using Apangelia.Core;
using Apangelia.Persistence.Postgres.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Apangelia.Persistence.Postgres.Configurations;

internal sealed class NotificationRouteConfiguration : IEntityTypeConfiguration<NotificationRoute>
{
    public void Configure(EntityTypeBuilder<NotificationRoute> builder)
    {
        builder.ToTable("NotificationRoutes");
        builder.HasKey(route => route.Id);
        builder.Property(route => route.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<PostgresGuidV7ValueGenerator>();
        builder.Property(route => route.UserId).IsRequired();
        builder.Property(route => route.InputProviderId).HasMaxLength(64).IsRequired();
        builder.Property(route => route.OutputProviderId).HasMaxLength(64).IsRequired();
        builder.Property(route => route.DestinationId).HasMaxLength(256).IsRequired();
        builder.Property(route => route.ConditionsJson).HasColumnType("jsonb");
        builder.Property(route => route.CreatedAt).IsRequired();
        builder.HasIndex(route => new { route.UserId, route.InputProviderId });
    }
}
