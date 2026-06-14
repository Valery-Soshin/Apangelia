using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Apangelia.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotificationInboxReceivedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceivedAt",
                table: "NotificationInboxMessages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReceivedAt",
                table: "NotificationInboxMessages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }
    }
}
