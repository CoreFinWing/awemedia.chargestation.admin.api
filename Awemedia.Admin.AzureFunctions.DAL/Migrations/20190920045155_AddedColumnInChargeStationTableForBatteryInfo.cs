using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedColumnInChargeStationTableForBatteryInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BatteryLevel",
                table: "ChargeStation",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "ChargeStation",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPingTimeStamp",
                table: "ChargeStation",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatteryLevel",
                table: "ChargeStation");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "ChargeStation");

            migrationBuilder.DropColumn(
                name: "LastPingTimeStamp",
                table: "ChargeStation");
        }
    }
}
