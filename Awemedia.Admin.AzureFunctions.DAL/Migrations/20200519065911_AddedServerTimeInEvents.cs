using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Awemedia.Admin.AzureFunctions.DAL.Migrations
{
    public partial class AddedServerTimeInEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ServerTimeStamp",
                table: "Events",
                type: "datetime",
                nullable: true,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServerTimeStamp",
                table: "Events");
        }
    }
}
