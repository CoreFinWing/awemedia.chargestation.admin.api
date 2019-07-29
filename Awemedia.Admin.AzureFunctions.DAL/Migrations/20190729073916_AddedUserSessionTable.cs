using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedUserSessionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChargeStations_DeviceId",
                table: "ChargeStation");

            migrationBuilder.DropIndex(
                name: "IX_ChargeStations_UID",
                table: "ChargeStation");

            migrationBuilder.DropIndex(
                name: "IX_ChargeOptions",
                table: "ChargeOptions");

            migrationBuilder.RenameIndex(
                name: "fkIdx_Events_EventType",
                table: "Events",
                newName: "IX_Events_EventTypeId");

            migrationBuilder.CreateTable(
                name: "SessionStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Status = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
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
                    Type = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSession",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ChargeRentalRevnue = table.Column<decimal>(type: "money", nullable: true),
                    InvoiceNo = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    DeviceId = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    TransactionId = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    SessionStartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    SessionEndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ApplicationId = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    AppKey = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    UserAccountId = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Email = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    SessionType = table.Column<int>(nullable: true),
                    SessionStatus = table.Column<int>(nullable: true),
                    ChargeParams = table.Column<string>(unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionStatus_UserSession",
                        column: x => x.SessionStatus,
                        principalTable: "SessionStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SessionType_UserSession",
                        column: x => x.SessionType,
                        principalTable: "SessionType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "fkIdx_SessionStatus",
                table: "UserSession",
                column: "SessionStatus");

            migrationBuilder.CreateIndex(
                name: "fkIdx_SessionType",
                table: "UserSession",
                column: "SessionType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSession");

            migrationBuilder.DropTable(
                name: "SessionStatus");

            migrationBuilder.DropTable(
                name: "SessionType");

            migrationBuilder.RenameIndex(
                name: "IX_Events_EventTypeId",
                table: "Events",
                newName: "fkIdx_Events_EventType");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeStations_DeviceId",
                table: "ChargeStation",
                column: "DeviceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChargeStations_UID",
                table: "ChargeStation",
                column: "UID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChargeOptions",
                table: "ChargeOptions",
                columns: new[] { "ChargeDuration", "Price", "Currency" },
                unique: true);
        }
    }
}
