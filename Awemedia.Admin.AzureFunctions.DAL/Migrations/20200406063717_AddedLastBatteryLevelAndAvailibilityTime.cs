using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedLastBatteryLevelAndAvailibilityTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastBatteryLevel",
                table: "ChargeStation",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastBatteryLevelAvailablityTime",
                table: "ChargeStation",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastBatteryLevel",
                table: "ChargeStation");

            migrationBuilder.DropColumn(
                name: "LastBatteryLevelAvailablityTime",
                table: "ChargeStation");
        }
    }
}
