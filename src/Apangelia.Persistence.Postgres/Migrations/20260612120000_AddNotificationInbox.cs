#nullable disable

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Apangelia.Persistence.Postgres.Migrations;

/// <inheritdoc />
public partial class AddNotificationInbox : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "NotificationInboxMessages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Source = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                EventType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                ExternalEventId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                RawPayloadJson = table.Column<string>(type: "jsonb", nullable: false),
                OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                ReceivedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_NotificationInboxMessages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Notifications",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Source = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                EventType = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                ExternalEventId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                Title = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                Message = table.Column<string>(type: "text", nullable: true),
                OccurredAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Notifications", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_NotificationInboxMessages_Source_ExternalEventId",
            table: "NotificationInboxMessages",
            columns: new[] { "Source", "ExternalEventId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_Source_ExternalEventId",
            table: "Notifications",
            columns: new[] { "Source", "ExternalEventId" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "NotificationInboxMessages");

        migrationBuilder.DropTable(name: "Notifications");
    }
}
