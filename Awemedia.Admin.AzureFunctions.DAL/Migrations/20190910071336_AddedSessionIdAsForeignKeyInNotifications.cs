using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedSessionIdAsForeignKeyInNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsumedDateTime",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "IsConsumed",
                table: "Notification");

            migrationBuilder.AddColumn<Guid>(
                name: "UserSessionId",
                table: "Notification",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserSessionId",
                table: "Notification",
                column: "UserSessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSession_Notifications",
                table: "Notification",
                column: "UserSessionId",
                principalTable: "UserSession",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSession_Notifications",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_UserSessionId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "UserSessionId",
                table: "Notification");

            migrationBuilder.AddColumn<DateTime>(
                name: "ConsumedDateTime",
                table: "Notification",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsConsumed",
                table: "Notification",
                nullable: false,
                defaultValue: false);
        }
    }
}
