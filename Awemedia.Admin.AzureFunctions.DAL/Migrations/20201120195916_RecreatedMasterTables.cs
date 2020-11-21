using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class RecreatedMasterTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "EventType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IndustryType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndustryType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SessionStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
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
                    Id = table.Column<int>(nullable: false),
                    Type = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
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

            migrationBuilder.InsertData(table: "SessionType",
                columns: new[] { "Id", "IsActive", "Type" },
                values: new object[,] {
                { 1,true,"Promotion" },
                { 2,true,"Paid"  } });

            migrationBuilder.InsertData(table: "SessionStatus",
               columns: new[] { "Id", "Status", "IsActive" },
               values: new object[,] {
                { 1,"New",true },
                { 2,"AuthorizationSuccessful",true },
                { 3,"AuthorizationFailure",true },
                { 4,"PaymentRequestSent",true },
                { 5,"PaymentCompleted",true },
                { 6,"PaymentFailure",true },
                { 7,"PaymentTimeout",true },
                { 8,"PaymentCanceledByUser",true },
                { 9,"Charging",true },
                { 10,"ChargingCompleted",true },
                { 11,"ChargeFailed",true },
                { 12,"SessionTimeout",true },
                { 13,"AuthorizationTimeout",true },
                { 14,"PaymentRequestTimeout",true },
                { 15,"ServerTimeOut",true }
               });
            migrationBuilder.InsertData(table: "IndustryType",
                columns: new[] { "Id", "Name", "IsActive", "CreatedDate", "ModifiedDate" },
                values: new object[,] {
                { 1,"F&b (food and beverage)",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 2,"Karaoke",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 3,"Service",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 4,"Event",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },

                });
            migrationBuilder.InsertData(table: "EventType",
               columns: new[] { "Id", "Name", "IsActive", "CreatedDate", "ModifiedDate" },
               values: new object[,] {
                { 1,"app started",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 2,"charge session started",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 3,"charge session ended",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 7,"charging session failed",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 8,"charging station connection failed",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 9,"charge station not returning the battery level",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 12,"video started",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 13,"video ended",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },
                { 14,"video failed",true,DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") },

               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_SessionStatusNavigationId",
                table: "UserSession",
                column: "SessionStatusNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSession_SessionTypeNavigationId",
                table: "UserSession",
                column: "SessionTypeNavigationId");
        }
    }
}
