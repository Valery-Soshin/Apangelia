using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apangelia.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotificationDeliveryScheduledAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotificationDeliveries_Status_NextAttemptAt_ScheduledAt",
                table: "NotificationDeliveries");

            migrationBuilder.DropColumn(
                name: "ScheduledAt",
                table: "NotificationDeliveries");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDeliveries_Status_NextAttemptAt",
                table: "NotificationDeliveries",
                columns: new[] { "Status", "NextAttemptAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotificationDeliveries_Status_NextAttemptAt",
                table: "NotificationDeliveries");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ScheduledAt",
                table: "NotificationDeliveries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(
                    new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                    new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_NotificationDeliveries_Status_NextAttemptAt_ScheduledAt",
                table: "NotificationDeliveries",
                columns: new[] { "Status", "NextAttemptAt", "ScheduledAt" });
        }
    }
}
