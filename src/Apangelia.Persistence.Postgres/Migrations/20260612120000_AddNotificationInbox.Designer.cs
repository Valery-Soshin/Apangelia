#nullable disable

using System;
using Apangelia.Persistence.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Apangelia.Persistence.Postgres.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260612120000_AddNotificationInbox")]
partial class AddNotificationInbox
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "10.0.9")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        modelBuilder.Entity("Apangelia.Core.Notification", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<DateTimeOffset>("CreatedAt")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("EventType")
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("character varying(128)");

            b.Property<string>("ExternalEventId")
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("character varying(128)");

            b.Property<string>("Message")
                .HasColumnType("text");

            b.Property<DateTimeOffset>("OccurredAt")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Source")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("character varying(64)");

            b.Property<string>("Title")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("character varying(256)");

            b.HasKey("Id");

            b.HasIndex("Source", "ExternalEventId");

            b.ToTable("Notifications");
        });

        modelBuilder.Entity("Apangelia.Persistence.Postgres.NotificationInboxMessage", b =>
        {
            b.Property<Guid>("Id")
                .ValueGeneratedOnAdd()
                .HasColumnType("uuid");

            b.Property<string>("EventType")
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("character varying(128)");

            b.Property<string>("ExternalEventId")
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnType("character varying(128)");

            b.Property<DateTimeOffset>("OccurredAt")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("RawPayloadJson")
                .IsRequired()
                .HasColumnType("jsonb");

            b.Property<DateTimeOffset>("ReceivedAt")
                .HasColumnType("timestamp with time zone");

            b.Property<string>("Source")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("character varying(64)");

            b.HasKey("Id");

            b.HasIndex("Source", "ExternalEventId")
                .IsUnique();

            b.ToTable("NotificationInboxMessages");
        });
#pragma warning restore 612, 618
    }
}
