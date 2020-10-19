using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedStartTimeAndEndTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Promotion",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Promotion",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Promotion",
                type: "time(0)",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Promotion",
                type: "time(0)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Promotion");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Promotion");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Promotion",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "Promotion",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);
        }
    }
}
