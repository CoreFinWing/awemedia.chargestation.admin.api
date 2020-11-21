using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class DroppedMasterTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventID_EventTypeId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Merchant_IndustryType",
                table: "Merchant");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionStatus_UserSession",
                table: "UserSession");

            migrationBuilder.DropForeignKey(
                name: "FK_SessionType_UserSession",
                table: "UserSession");

            migrationBuilder.DropTable(
                name: "EventType");

            migrationBuilder.DropTable(
                name: "IndustryType");

            migrationBuilder.DropTable(
                name: "SessionStatus");

            migrationBuilder.DropTable(
                name: "SessionType");

            migrationBuilder.AddColumn<int>(
                name: "SessionStatusNavigationId",
                table: "UserSession",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SessionTypeNavigationId",
                table: "UserSession",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerDateTime",
                table: "Events",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldDefaultValueSql: "('0001-01-01T00:00:00.0000000')");

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_SessionStatusNavigationId",
                table: "UserSession",
                column: "SessionStatusNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_SessionTypeNavigationId",
                table: "UserSession",
                column: "SessionTypeNavigationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSession_SessionStatusNavigationId",
                table: "UserSession");

            migrationBuilder.DropIndex(
                name: "IX_UserSession_SessionTypeNavigationId",
                table: "UserSession");

            migrationBuilder.DropColumn(
                name: "SessionStatusNavigationId",
                table: "UserSession");

            migrationBuilder.DropColumn(
                name: "SessionTypeNavigationId",
                table: "UserSession");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ServerDateTime",
                table: "Events",
                type: "datetime",
                nullable: true,
                defaultValueSql: "('0001-01-01T00:00:00.0000000')",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "EventType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndustryType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsActive = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsActive = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionType", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_EventID_EventTypeId",
                table: "Events",
                column: "EventTypeId",
                principalTable: "EventType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Merchant_IndustryType",
                table: "Merchant",
                column: "IndustryTypeId",
                principalTable: "IndustryType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionStatus_UserSession",
                table: "UserSession",
                column: "SessionStatus",
                principalTable: "SessionStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SessionType_UserSession",
                table: "UserSession",
                column: "SessionType",
                principalTable: "SessionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
