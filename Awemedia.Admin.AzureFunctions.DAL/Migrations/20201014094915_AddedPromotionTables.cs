using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedPromotionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Payload",
                table: "Notification",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "NotificationResult",
                table: "Notification",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LoggedDateTime",
                table: "Notification",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceToken",
                table: "Notification",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "Notification",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerDateTime",
                table: "Events",
                type: "datetime",
                nullable: true,
                defaultValueSql: "('0001-01-01T00:00:00.0000000')",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "EventData",
                table: "Events",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.CreateTable(
                name: "PromotionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PromotionType = table.Column<string>(unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PromotionDesc = table.Column<string>(unicode: false, nullable: false),
                    PromotionTypeId = table.Column<int>(nullable: true),
                    BranchId = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Promotion_Branch",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotion_PromotionType",
                        column: x => x.PromotionTypeId,
                        principalTable: "PromotionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_BranchId",
                table: "Promotion",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_PromotionTypeId",
                table: "Promotion",
                column: "PromotionTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "PromotionType");

            migrationBuilder.AlterColumn<string>(
                name: "Payload",
                table: "Notification",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotificationResult",
                table: "Notification",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LoggedDateTime",
                table: "Notification",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceToken",
                table: "Notification",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceId",
                table: "Notification",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerDateTime",
                table: "Events",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "('0001-01-01T00:00:00.0000000')");

            migrationBuilder.AlterColumn<string>(
                name: "EventData",
                table: "Events",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false);
        }
    }
}
